using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum Status
{
    OPEN,
    YELLOW,
    CLOSE,
}

public class TrafficSignal : MonoBehaviour {

    [SerializeField]
    private Renderer[] rend;

    [SerializeField]
    private float time, timeOpen, timeClose;
    [SerializeField]
    private bool open;

    void Start()
    {
        setStatus(Status.CLOSE);
    }

    void Update()
    {
        time += Time.deltaTime;

        if (open)
        {
            if(time > timeOpen)
            {
                time = 0;
                setStatus(Status.CLOSE);
            }
            else if(time > timeOpen * 0.8f)
            {
                setStatus(Status.YELLOW);
            }
        }
        else
        {
            if (time > timeClose)
            {
                time = 0;
                setStatus(Status.OPEN);
            }

        }
    }

    private void setStatus(Status status)
    {
        open = status == Status.CLOSE ? false : true;
        for (int i = 0; i < rend.Length; i++)
        {
            if(i== (int)status)
            {
                rend[i].material.color = status == Status.OPEN ? Color.green : Color.yellow;
                if (status == Status.CLOSE)
                {
                    rend[i].material.color = Color.red;
                }
            }
            else
            {
                rend[i].material.color = Color.black;
            }
        }
    }    

    public bool Open
    {
        get
        {
            return open;
        }
    }
}
