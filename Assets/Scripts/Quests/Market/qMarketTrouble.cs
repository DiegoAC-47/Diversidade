using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


enum TroubleState
{
    SOLVE,
    BRING,
}

public class qMarketTrouble : qMarketObjs
{
    [SerializeField]
    private Collider areaSpawn;

    [SerializeField]
    private float TimeToCheckTrouble, TimeTroubleSolve, timeBetweenTroubles;
    
    private float TimeToCheck, timeNewTrouble,time;
        
    private TroubleState state;
    
    private bool near, busy;   

    protected override void Start()
    {
        base.Start();
        changeTrouble();     
    }

    void Update()
    {
        if (timeNewTrouble > 0)
        {
            timeNewTrouble -= Time.deltaTime;
            if (timeNewTrouble < 0)
            {
                if (State == TroubleState.BRING)
                {
                    State = TroubleState.SOLVE;
                }
                else
                {
                    State = TroubleState.BRING;
                    Quest.objecTrouble();
                }

                TimeToCheck = TimeToCheckTrouble;
                Busy = false;

            }
        }
        else
        {
            if (Input.GetAxis(Inputs.Interact.ToString()) > 0)
            {
                if (near & !busy)
                {
                    if (State == TroubleState.BRING)
                    {
                        interactionBring();
                    }
                    else
                    {
                        interactionSolve();
                    }
                }
            }
            else
            {
                if (TimeToCheck > 0)
                {
                    TimeToCheck -= Time.deltaTime;
                }
                else
                {
                    changeTrouble();
                    Quest.problem();
                }
            }
        }
    }

    private void interactionSolve()
    {
        time += Time.deltaTime;
        if (time > TimeTroubleSolve)
        {
            changeTrouble();
        }
    }

    private void interactionBring()
    {
        if(Player.HasObject)
        {
            changeTrouble();
        }
    }

    private void changeTrouble()
    {
        timeNewTrouble = timeBetweenTroubles;
        IsInteractible(false);
        Busy =  true;
        if (State == TroubleState.BRING)
        {
            Player.HasObject = false;
        }
        else
        {
            time = 0;
        }

        #region new position trouble
        int safe = 100;
        Vector3 newPosi = transform.position;
        do
        {
            safe--;
            newPosi.x = UnityEngine.Random.Range(areaSpawn.bounds.min.x, areaSpawn.bounds.max.x);
            newPosi.z = UnityEngine.Random.Range(areaSpawn.bounds.min.z, areaSpawn.bounds.max.z);
            if (Physics.CheckSphere(newPosi, GetComponent<Collider>().bounds.max.z))
            {
                transform.position = newPosi;
                safe = -1;
            }
        }
        while (safe > 0);
    #endregion
    }

    public override void IsInteractible(bool b)
    {
        if (!busy)
        {
            base.IsInteractible(b);
            near = b;
        }
        else
        {
            base.IsInteractible(false);
        }
    }

    public override void OnInteract()
    {
    }

    public bool Busy
    {
        get
        {
            return busy;
        }

        set
        {
            busy = value;
            GetComponent<Renderer>().enabled = !busy;
        }
    }

    internal TroubleState State
    {
        get
        {
            return state;
        }

        set
        {
            state = value;
            if(value == TroubleState.BRING)
            {
                Canvas.GetComponentInChildren<UnityEngine.UI.Image>().color = Color.green;
            }
            else
            {
                Canvas.GetComponentInChildren<UnityEngine.UI.Image>().color = Color.blue;
            }
        }
    }
}
