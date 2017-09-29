using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class IAPerson : IAMove
{
    private NavMeshAgent agent;

    private Vector3 oldPosi;

    private float speed;

    [SerializeField]
    private bool stop;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        speed = agent.speed;
        Stop = false;
    }

    void Update()
    {
        if (!Stop)
        {
            moveUpdate();
            agent.destination = path.getPath(chosenStreet)[countStreet].position;
        }        
    }

    protected override void onChange()
    {
        
    }

    public bool Stop
    {
        get
        {
            return stop;
        }

        set
        {
            stop = value;
            if (stop)
            {
                agent.speed = 0;
            }
            else
            {
                agent.speed = speed;
            }
        }
    }
}