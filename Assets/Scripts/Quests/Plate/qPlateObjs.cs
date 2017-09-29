using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class qPlateObjs : Interactible {

    private sPlateQuest quest;

    [SerializeField]
    private Canvas ui;

    protected virtual void Start()
    {
        quest = FindObjectOfType<sPlateQuest>();
        ui.enabled = false;
    }

    protected sPlateQuest Quest
    {
        get
        {
            if (quest != null)
            {
                return quest;
            }
            else
            {
                return FindObjectOfType<sPlateQuest>();
            }
        }
    }

    protected PlateType PlayerPlateType
    {
        get
        {
            cPlateQuest c = (cPlateQuest)Quest.Player.Controller;
            return c.PlateType;
        }
    }

    public Canvas UI
    {
        get
        {
            return ui;
        }
    }

    public override void IsInteractible(bool b)
    {
        throw new NotImplementedException();
    }

    public override void OnInteract()
    {
        throw new NotImplementedException();
    }

    protected virtual void manageUI(string mensage)
    {

    }

    public virtual void onDone()
    {
        throw new NotImplementedException();
    }
}
