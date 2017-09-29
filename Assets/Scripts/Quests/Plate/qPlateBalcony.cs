using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class qPlateBalcony : qPlateObjs {

    [SerializeField]
    private GameObject plateObj;

    [SerializeField]
    private PlateType plateType;

    protected override void Start()
    {
        base.Start();
        UI.GetComponentInChildren<Text>().text = "Meu prato: " + plateType;
    }

    public override void IsInteractible(bool b)
    {
        if (Quest.Run)
        {
            UI.enabled = b;
            if (!b)
            {
                Spawn();
            }
        }
        else
        {
            UI.enabled = false;
        }
    }

    public override void OnInteract()
    {
        if (Quest.Run)
        {
            cPlateQuest c = (cPlateQuest)Quest.Player.Controller;
            if (c.PlateType == PlateType.NULL)
            {
                c.GetPlate(plateType, plateObj);
                plateObj.SetActive(false);
            }
        }
    }

    public void Spawn()
    {
        plateObj.SetActive(true);
    }   

    public PlateType PlateType
    {
        get
        {
            return plateType;
        }

        set
        {
            plateType = value;
        }
    }

    public override void onDone()
    {
        plateObj.SetActive(false);
    }
}
