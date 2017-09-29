using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class qPlateNPC1 : MonoBehaviour
{

    delegate void Behaviour();

    private Behaviour actualBehaviour;

    [SerializeField]
    private NavMeshAgent agent;

    [SerializeField]
    private Animator animator;

    [SerializeField]
    private string nameLayer, triggerAnimation, nameAnimation;

    [SerializeField]
    private float timeAnimaiton;

    private bool free;

    void Start()
    {
        free = true;
        actualBehaviour = null;
        timeAnimaiton = 0;
        if (agent == null)
        {
            agent = GetComponent<NavMeshAgent>();
        }
    }

    private void Update()
    {
        if (actualBehaviour != null)
        {
            actualBehaviour();
        }
    }

    public void Goto(Vector3 dest, string triggerAnimation, string nameAnimation)
    {
        Goto(dest, triggerAnimation, nameAnimation, 0);
    }

    public void Goto(Vector3 dest, string triggerAnimation, string nameAnimation, float timeAnimation)
    {
        if (actualBehaviour == null)
        {
            free = false;
            this.triggerAnimation = triggerAnimation;
            this.nameAnimation = nameAnimation;
            this.timeAnimaiton = timeAnimation;
            agent.destination = dest;
            actualBehaviour = WaitPosition;
        }
    }

    private void WaitPosition()
    {
        if (Vector3.Distance(agent.destination, transform.position) < 1)
        {
            actualBehaviour = WaitAnimaiton;
        }
    }

    private void WaitAnimaiton()
    {
        if (timeAnimaiton > 0)
        {
            timeAnimaiton -= Time.deltaTime;
        }
        else
        {
            if (animator.GetCurrentAnimatorStateInfo(animator.GetLayerIndex(nameLayer)).normalizedTime > 1)
            {
                this.actualBehaviour = null;
                free = true;
            }
        }
    }
    
    public bool Free
    {
        get
        {
            return free;
        }
    }
}