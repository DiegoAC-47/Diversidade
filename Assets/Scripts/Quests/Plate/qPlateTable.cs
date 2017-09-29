using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;


public enum TableState
{
    COOLDOWN,
    WAIT_DELIVERY
}

public class qPlateTable : qPlateObjs
{
    [SerializeField]
    private GameObject plate;

    [SerializeField]
    private PlateType plateType;
    private TableState state;
    private float timer = -1, timeOutUI;

    /// <summary>
    /// Inicia corrotine da table
    /// </summary>
    /// <param name="startTime"></param>
    public void Setup(float startTime)
    {
        this.timer = startTime;
        state = TableState.COOLDOWN;
        StartCoroutine(_Update());
        this.plate.SetActive(false);
    }

    /// <summary>
    /// Maquina de estado
    /// </summary>
    /// <returns></returns>
    private IEnumerator _Update()
    {
        do
        {
            if (this.timer > 0)
            {
                timer--;
                if (State == TableState.COOLDOWN)
                {
                    if (timer < timeOutUI)
                    {
                        UI.enabled = false;
                    }                    
                }
                else
                {
                    UI.GetComponentInChildren<Image>().color = new Color(1,  (timer / Quest.Cooldown), (timer / Quest.Cooldown), 1);
                }
            }
            else
            {
                if (State == TableState.COOLDOWN)
                {
                    State = TableState.WAIT_DELIVERY;
                }
                else
                {
                    State = TableState.COOLDOWN;
                    Quest.CheckScore(ScoreType.MISS);
                    manageUI("Não quero mais");
                }
            }
            yield return new WaitForSeconds(1);
        }
        while (Quest.State == QuestState.ACTIVE);
    }
    /// <summary>
    /// Verifica se o prato entregue é o correto e atribui a pontuação
    /// </summary>
    /// <param name="plate"></param>
    public void CheckPlate(PlateType plate)
    {
        if (Quest.Run)
        {
            if(PlayerPlateType == plateType)
            {
                Quest.CheckScore(ScoreType.RIGHT);
                manageUI("Muito obrigado, até o diego ia gostar");
                timeOutUI = Quest.Cooldown / 2;
            }
            else
            {
                Quest.CheckScore(ScoreType.WRONG);
                manageUI("Não quero isso parece a comida do miguel");
                timeOutUI = Quest.Cooldown * 0.8f;
            }

            State = TableState.COOLDOWN;
            
        }
    }

    /// <summary>
    /// Sorteia um prato
    /// </summary>
    /// <returns></returns>
    public PlateType GetNextPlate()
    {
        PlateType p = plateType;
        int aux = 0, controll = 0;
        do
        {
            try
            {
                aux = UnityEngine.Random.Range(0, Enum.GetValues(typeof(PlateType)).Length);
                plateType = (PlateType)aux;
            }
            catch 
            {
                controll = 10;
            }
            controll++;
        }
        while  ((plateType == p || plateType == PlateType.NULL) & controll < 10);

        manageUI("Esperando: " + plateType.ToString());
        plate.SetActive(false);
        return plateType;
    }

    public override void IsInteractible(bool b)
    {
        if (State == TableState.WAIT_DELIVERY)
        {
            if (b)
            {
                manageUI("Da meu prato!!!");
            }
            else
            {
                manageUI("Esperando: " + plateType.ToString());
            }
        }
    }

    public override void OnInteract()
    {
        if (Quest.Run & State == TableState.WAIT_DELIVERY)
        {
            cPlateQuest c = (cPlateQuest)Quest.Player.Controller;
            if (c.PlateType != PlateType.NULL)
            {
                CheckPlate(PlayerPlateType);
                c.DeliveryPlate(this);
            }
        }
    }

    public TableState State
    {
        get
        {
            return state;
        }

        set
        {
            state = value;
            if (state == TableState.WAIT_DELIVERY)
            {
                this.plateType = GetNextPlate();
                this.timer = Quest.TimeLimit;
            }
            else
            {
                this.timer = Quest.Cooldown;
                timeOutUI = timer * 0.8f;
                UI.GetComponentInChildren<Image>().color = new Color(1, 1, 1, 1);
            }
        }
    }

    protected override void manageUI(string mensage)
    {
        UI.enabled = true;
        UI.GetComponentInChildren<Text>().text = mensage;
    }

    public override void onDone()
    {
        StopCoroutine(_Update());
    }

    public void DropPlate()
    { 
        this.plate.SetActive(true);
    }
}
