using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class qHideAgent : qHideObjs
{
    [SerializeField]
    private float speed;

    private int index;

    private bool moving, run;

    protected override void Start()
    {
        base.Start();
        moving = false;
        run = false;
    }

    /// <summary>
    /// começa a corrotine de se mover
    /// </summary>
    public void GoTo()
    {
        if (!moving)
        {
            StartCoroutine(move());
        }
    }

    /// <summary>
    /// move-se para o próximo ponto
    /// </summary>
    /// <returns></returns>
    private IEnumerator move()
    {
        index = 0;
        moving = true;
        run = true;
        Transform dest;
        while (Quest.ActualPoint().PathPoints.Length - 1 >= index)
        {
            dest = Quest.ActualPoint().PathPoints[index].transform;
            do
            {
                translate(dest, speed);
                yield return null;
            }
            while (Vector2.Distance(new Vector2(dest.position.x, dest.position.z), new Vector2(transform.position.x, transform.position.z)) > 0.1f & run);

            index++;
        }
        if (run)
        {
            Quest.NextPoint();
            moving = false;
        }
    }
        
    public override void restart()
    {
        moving = false;
        run = false;
        StopCoroutine(move());
        base.restart();
    }

}
