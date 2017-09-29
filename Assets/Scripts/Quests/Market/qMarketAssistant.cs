using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public enum AssistantType
{
    NONE,
    FOOD,
    MATERIAL,
    MONEY,
}

public class qMarketAssistant : qMarketObjs
{
    private NavMeshAgent agent;

    private qMarketTent tent;

    private Transform player;

    private Vector3 startPosi;

    [SerializeField]
    private AssistantType type;

    private int stateHelping;

    protected override void Start()
    {
        base.Start();
        startPosi = transform.position;
        agent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
       if (stateHelping == 2)
        {
            if(tent.Type == AssistantType.NONE)
            {
                stateHelping = 0;
            }            
        }
        else if (stateHelping == 0)
        {
            //moveToPlayer();
            agent.destination = player.position;
        }
        else if (stateHelping == 1)
        {
            if (Vector3.Distance(transform.position, agent.destination) < 2)
            {
                tent.Type = this.type;
                stateHelping = 2;
            }

        }
    }

    private void moveToPlayer()
    {
        if (agent.enabled)
        {
            if (Vector3.Distance(Quest.Destiny, transform.position) > 2)
            {
                agent.destination = Quest.Destiny;
            }
            else
            {
                Quest.nextDestiny();
            }
        }
    }

    

    public override void IsInteractible(bool b)
    {
        //base.IsInteractible(b);
    }

    public override void OnInteract()
    {
        
    }

    public void help(qMarketTent tent)
    {
        if (tent.Type == AssistantType.NONE)
        {
            if(this.tent != null)
            {
                this.tent.Type = AssistantType.NONE;
            }
            stateHelping = 1;
            this.tent = tent;
            agent.destination = this.tent.gameObject.transform.position;
        }
    }

    public override void restart()
    {
        transform.position = startPosi;
        agent.destination = startPosi;
    }

    public void setup(Transform player)
    {
        this.player = player;
    }

    public AssistantType Type
    {
        get
        {
            return type;
        }
    }
}