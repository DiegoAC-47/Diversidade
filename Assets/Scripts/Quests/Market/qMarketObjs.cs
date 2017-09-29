using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class qMarketObjs : Interactible
{

    private sMarketQuest quest;

    private Canvas myCanvas;

    protected virtual void Start()
    {
        myCanvas = GetComponentInChildren<Canvas>();
        quest = FindObjectOfType<sMarketQuest>().GetComponent<sMarketQuest>();
       
    }

    public override void IsInteractible(bool b)
    {
        Canvas.enabled = b;
    }

    public override void OnInteract()
    {
        throw new NotImplementedException();
    }


    protected Canvas Canvas
    {
        get
        {
            return myCanvas;
        }
    }

    protected sMarketQuest Quest
    {
        get
        {
            return quest;
        }
    }

    public cMarketQuest Player
    {
        get
        {
            return (cMarketQuest)quest.Player.Controller;
        }
    }

    public virtual void restart()
    {
        throw new NotImplementedException();
    }
}