using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class qHidePursuer : qHideObjs
{

    [SerializeField]
    private Renderer render;

    [SerializeField]
    private Transform[] pathPoints;

    [SerializeField]
    private float speed;

    private float angGirl, angPlayer;

    /// <summary>
    /// indice do pathPoints atual.
    /// </summary>
    private int index;

    private bool run = false;

    protected override void Start()
    {
        base.Start();
        index = -1;
        nextIndex();
        run = true;
    }

    private void FixedUpdate()
    {
        //render.material.SetFloat("_Amplitude", Quest.Amplitude);
        //render.material.SetVector("_Center", new Vector4(transform.position.x, transform.position.y, transform.position.z, 1));

        if (run)
        {
            translate(pathPoints[index].transform, speed);
            if (Vector3.Distance(pathPoints[index].transform.position, transform.position) < 2)
            {
                nextIndex();
            }

            angGirl = Ang(Quest.Agent.gameObject.transform);


            catched(angGirl > transform.localEulerAngles.y + Quest.Amplitude || angGirl < transform.localEulerAngles.y - Quest.Amplitude);

            angPlayer = Ang(Quest.Player.transform);
            catched(angPlayer > transform.localEulerAngles.y + Quest.Amplitude || angPlayer < transform.localEulerAngles.y - Quest.Amplitude); 
        }
    }

    /// <summary>
    /// trata o que acontece se a garota ou o jogador foi visto.
    /// </summary>
    /// <param name="free"></param>
    private void catched(bool free)
    {
        if (free)
        {
            GetComponent<Renderer>().material.color = Color.green;
        }
        else
        {
            GetComponent<Renderer>().material.color = Color.red;
            run = false;
            Quest.Restart();
        }
    }

    /// <summary>
    /// próximo pathPoints
    /// </summary>
    private void nextIndex()
    {
        index++;
        if (index > pathPoints.Length - 1)
        {
            index = 0;
        }
    }    

    /// <summary>
    /// calcula o angulo que o transform está.
    /// </summary>
    /// <param name="target"></param>
    /// <returns></returns>
    private float Ang(Transform target)
    {
        float f = (Mathf.Atan2((target.position - transform.position).x, (target.position - transform.position).z) * Mathf.Rad2Deg);// + transform.localEulerAngles.y;

        f = f < 0 ? f + 360 : f;

        //print("Pos : " + (transform.localEulerAngles.y + Quest.Amplitude));
        //print("Neg : " + (transform.localEulerAngles.y - Quest.Amplitude));
        //print(target.name + " -> ang: " + f);

        return f;
    }

    public override void restart()
    {
        Start();
        run = true;
        base.restart();
    }

    void OnTriggerEnter(Collider c)
    {
        if (c.GetComponent<qHideAgent>())
        {
            catched(false);
        }
    }
}
