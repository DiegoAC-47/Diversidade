using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class qSquareBase : qSquareObjs {

    [SerializeField]
    private float life;

    private float lifeMax;

    [SerializeField]
    private Text text;

    protected override void Start()
    {
        base.Start();
        text.text = "Vida: " + life;
        lifeMax = life;
    }

    void OnTriggerEnter(Collider c)
    {
        if (c.gameObject.GetComponent<qSquareTarget>())
        {
            c.gameObject.GetComponent<qSquareTarget>().ArriveBase = true;
            life--;
            text.text = "Vida: " + life;
            if (life < 1)
            {
                Quest.defeat();
            }
        }
    }

    public override void reset()
    {
        life = lifeMax;
    }
}
