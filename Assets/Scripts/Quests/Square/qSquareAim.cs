using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


enum State
{
    COLD,
    HOT,
}

public class qSquareAim : qSquareObjs
{

    private Vector3 speed;

    private Rigidbody rb;

    [SerializeField]
    private Slider slider;

    private State state;

    [SerializeField]
    private float speedX, speedZ, damage, pressureMax, AmountPressureRemove, timeToCold;
    
    private float pressure, controll, toCold;

    private bool canShot, run;

    protected override void Start()
    {
        base.Start();
        rb = GetComponent<Rigidbody>();
        slider.minValue = 0;
        slider.maxValue = pressureMax;
    }

    void Update()
    {
        toCold = AmountPressureRemove * Time.deltaTime;
        if (State == State.COLD)
        {
            if (Input.GetAxis(Inputs.Interact.ToString()) > 0)
            {
                pressure += Time.deltaTime;

               
                    shot();
                

                if (pressure > pressureMax)
                {
                    State = State.HOT;
                }
            }
            else
            {
                pressure = pressure - toCold < 0 ? 0 : pressure - toCold;
            }

        }
        else
        {
            pressure = pressure - toCold < 0 ? 0 : pressure - toCold;

            if (pressure <= pressureMax - ((AmountPressureRemove) * timeToCold))
            {
                State = State.COLD;
            }

        }

        slider.value = pressure;
    }

    private void FixedUpdate()
    {
        move();
    }

    private void shot()
    {
        for (int i = 0; i < FindObjectsOfType<qSquareTarget>().Length; i++)
        {
            FindObjectsOfType<qSquareTarget>()[i].damage(damage * Time.deltaTime);
        }
    }

    public void setup(bool b)
    {
        run = b;

        if (b)
        {
            State = State.COLD;
        }
    }

    private void move()
    {
        rb.velocity = new Vector3(0, rb.velocity.y, 0);
        speed = Vector3.zero;
        if (Input.GetAxis(Inputs.Vertical.ToString()) > 0)
        {
            speed.z += speedZ;
        }
        if (Input.GetAxis(Inputs.Vertical.ToString()) < 0)
        {
            speed.z -= speedZ;
        }

        if (Input.GetAxis(Inputs.Horizontal.ToString()) > 0)
        {
            speed.x += speedX;
        }
        if (Input.GetAxis(Inputs.Horizontal.ToString()) < 0)
        {
            speed.x -= speedX;
        }

        rb.AddForce(speed);

    }

    void OnTriggerEnter(Collider c)
    {
        if (c.gameObject.GetComponent<qSquareTarget>())
        {
            c.gameObject.GetComponent<qSquareTarget>().CanDamage = true;
        }
    }

    void OnTriggerExit(Collider c)
    {
        if (c.gameObject.GetComponent<qSquareTarget>())
        {
            c.gameObject.GetComponent<qSquareTarget>().CanDamage = false;
        }
    }

    public override void reset()
    {
        pressure = 0;
    }

    State State
    {
        get
        {
            return state;
        }

        set
        {
            state = value;
            if(state == State.COLD)
            {
                GetComponent<MeshRenderer>().material.color = Color.blue;
            }
            else
            {
                GetComponent<MeshRenderer>().material.color = Color.red;
            }
        }
    }
}
