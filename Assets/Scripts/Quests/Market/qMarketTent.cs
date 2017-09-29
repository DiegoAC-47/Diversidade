using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class qMarketTent : qMarketObjs
{
    private Text text;

    private int[] atributes;

    private AssistantType type;

    private bool[] canLose;

    private bool near, hasObject;

   

    protected override void Start()
    {
        base.Start();
        type = AssistantType.NONE;
        Canvas.enabled = true;
        atributes = new int[3];
        canLose = new bool[atributes.Length];        

        for (int i = 0; i < atributes.Length; i++)
        {
            atributes[i] = 50;
            canLose[i] = true;
        }
        HasObject = false;
        text = Canvas.GetComponentInChildren<Text>();
    }

    public void _Update()
    {
        if (type == AssistantType.NONE)
        {
            for (int i = 0; i < atributes.Length; i++)
            {
                atributes[i] -= UnityEngine.Random.Range(1, 6);
                if (atributes[i] < 1)
                {
                    atributes[i] = 0;
                    if (canLose[i])
                    {
                        canLose[i] = false;
                        problem();
                    }
                }
                else
                {
                    canLose[i] = true;
                }
            }
        }
        else
        {
            atributes[(int)type - 1] += 10;
            if (atributes[(int)type - 1] > 50)
            {
                atributes[(int)type - 1] = 50;
                type = AssistantType.NONE;
            }
        }      

        text.text = "Food: " + atributes[(int)AssistantType.FOOD - 1] + "\nMaterial: " + atributes[(int)AssistantType.MATERIAL - 1] + "\nMoney: " + atributes[(int)AssistantType.MONEY - 1];
    }

    private void problem()
    {
        Quest.problem();
    }
     
    public override void IsInteractible(bool b)
    {     
        near = b;
    }

    public override void OnInteract()
    {
        if (HasObject)
        {            
            Player.HasObject = true;
            HasObject = false;
        }
    }

    public override void restart()
    {
        for (int i = 0; i < atributes.Length; i++)
        {
            atributes[i] = 50;
        }

        text.text = "Food: " + atributes[(int)AssistantType.FOOD - 1] + "\nMaterial: " + atributes[(int)AssistantType.MATERIAL - 1] + "\nMoney: " + atributes[(int)AssistantType.MONEY - 1];
    }

    public AssistantType Type
    {
        get
        {
            return type;
        }

        set
        {
            type = value;
        }
    }

    public bool HasObject
    {
        get
        {
            return hasObject;
        }

        set
        {
            hasObject = value;

            if (hasObject)
            {
                Canvas.GetComponentInChildren<Image>().color = Color.green;
            }
            else
            {
                Canvas.GetComponentInChildren<Image>().color = Color.white;
            }
        }
    }
}