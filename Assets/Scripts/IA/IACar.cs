using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class IACar : IAMove
{
    private NavMeshAgent agent;

    private ManagerCars manager;

    [SerializeField]
    private Renderer rend;

    private float speed, distAux;

    private int index;

    [SerializeField]
    private bool stop;

    void Start()
    {
        stop = false;
        agent = GetComponent<NavMeshAgent>();
        manager = FindObjectOfType<ManagerCars>();        
        speed = agent.speed;
        countStreet = 0;
        
        if (path == null)
        {
            stop = true;
        }
        agent.destination = path.getPath(chosenStreet)[countStreet].position;
    }


    void Update()
    {
        distAux = 1000;
        index = -1;
        for (int i = 0; i < manager.Centers.Length; i++)
        {
            if(Vector3.Distance(manager.Centers[i].Center.gameObject.transform.position, transform.position) < distAux)
            {
                distAux = Vector3.Distance(manager.Centers[i].Center.gameObject.transform.position, transform.position);
                index = i;
            }
        }
        
        if (index > -1)
        {
            rend.material.SetVector("_Center",
                            new Vector4(manager.Centers[index].Center.gameObject.transform.position.x, transform.position.y, manager.Centers[index].gameObject.transform.position.z));

            rend.material.SetFloat("_Radius", Vector3.Distance(manager.Centers[index].Ref.position, manager.Centers[index].Center.gameObject.transform.position));
        }


        stop = false;
            if (countStreet == 0)
            {
                if (!((StreetPath)(path)).IsOpen)
                {
                    stop = true;
                    agent.destination = path.getPath(chosenStreet)[0].position;
                }
                else
                {
                    agent.destination = path.getPath(chosenStreet)[countStreet].position;
                }
            }
       

        if (!stop)
        {
            moveUpdate();
            agent.destination = path.getPath(chosenStreet)[countStreet].position;
        }
    }

    protected override void onChange()
    {
        
    }

    public void closeCar()
    {
        agent.speed = 0;
    }

    public void awayCar()
    {
        agent.speed = speed;
    }



    public Renderer Rend
    {
        get
        {
            return rend;
        }

        set
        {
            rend = value;
        }
    }
}
