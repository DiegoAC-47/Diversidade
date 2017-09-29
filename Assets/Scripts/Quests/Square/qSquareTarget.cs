using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class qSquareTarget : qSquareObjs
{
    [SerializeField]
    private Slider slider;

    [SerializeField]
    private Vector3 dest;

    [SerializeField]
    private float life, speed;

    private float distX, distZ, dif;

    [SerializeField]
    private bool canDamage, arriveBase;

    protected override void Start()
    {
        base.Start();
        canDamage = false;
        slider.minValue = 0;
        slider.maxValue = life;
        dest = Quest.Base;
        ArriveBase = false;
    }

    void Update()
    {
        if (!arriveBase)
        {
            translate();

            slider.value = slider.maxValue - life;
        }
    }

    public void damage(float damage)
    {
        if (canDamage & !ArriveBase)
        {
            life -= damage;

            if (life < 1)
            {
                dead();
            }
        }
    }

    private void dead()
    {
        Destroy(gameObject);
    }

    private void translate()
    {
        distX = transform.position.x > dest.x ? transform.position.x - dest.x : dest.x - transform.position.x;

        distZ = transform.position.z > dest.z ? transform.position.z - dest.z : dest.z - transform.position.z;

        if (distX > 0.2f)
        {
            if (transform.position.x > dest.x)
            {
                transform.position += Vector3.left * speed * Time.deltaTime;
            }
            else //if (transform.position.x > dest.x)
            {
                transform.position += Vector3.right * speed * Time.deltaTime;
            }
        }
        else
        {
            transform.position = new Vector3(dest.x, transform.position.y, transform.position.z);
        }

        if (distZ > 0.2f)
        {
            if (transform.position.z > dest.z)
            {
                transform.position += Vector3.back * speed * Time.deltaTime;
            }
            else //if (transform.position.z > dest.z)
            {
                transform.position += Vector3.forward * speed * Time.deltaTime;
            }
        }
        else
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, dest.z);
        }

        try
        {
            if (Vector3.Distance(dest, transform.position) > 1)
            {
                Quaternion targetRotation = Quaternion.LookRotation(new Vector3(dest.x, transform.position.y, dest.z) - transform.position);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 2 * Time.deltaTime);
            }
        }
        catch
        {

        }

    }

    public bool CanDamage
    {
        get
        {
            return canDamage;
        }

        set
        {
            canDamage = value;
        }
    }
    
    public bool ArriveBase
    {
        get
        {
            return arriveBase;
        }

        set
        {
            arriveBase = value;

        }
    }

    public override void reset()
    {
        
    }
}
