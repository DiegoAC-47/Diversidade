using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StreetPath : IAPath {

    [SerializeField]
    private TrafficSignal signal;

    protected override void Start()
    {
        base.Start();

        Transform[] t = new Transform[firstPath.Length];
        t[0] = transform;
        for (int i = 1; i < t.Length; i++)
        {
            t[i] = firstPath[i - 1];
        }
        firstPath = t;
        t = new Transform[secondPath.Length];
        t[0] = transform;
        for (int i = 1; i < t.Length; i++)
        {
            t[i] = secondPath[i - 1];
        }
        secondPath = t;
    }

    
    public bool IsOpen
    {
        get
        {
            if (signal != null)
            {
                return signal.Open;
            }
            else
            {
                return true;
            }
        }
    }

    
}
