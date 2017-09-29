using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class qHideObjs : MonoBehaviour {

    private sHideQuest quest;

    private Vector3 startPosi;

    private float distX, distZ;

    protected virtual void Start()
    {
        quest = FindObjectOfType<sHideQuest>();
        startPosi = transform.position;
    }

    public sHideQuest Quest
    {
        get
        {
            return quest;
        }
    }

    /// <summary>
    /// movimenta o objeto
    /// </summary>
    /// <param name="dest"></param>
    /// <param name="speed"></param>
    protected void translate(Transform dest, float speed)
    {
        speed *= -1;
        distX = transform.position.x > dest.position.x ? transform.position.x - dest.position.x : dest.position.x - transform.position.x;

        distZ = transform.position.z > dest.position.z ? transform.position.z - dest.position.z : dest.position.z - transform.position.z;

        if (distX > 0.2f)
        {
            if (transform.position.x > dest.position.x)
            {
                transform.position += Vector3.right * speed * Time.deltaTime;
            }
            else //if (transform.position.x > dest.position.x)
            {
                transform.position += Vector3.left * speed * Time.deltaTime;
            }
        }
        else
        {
            transform.position = new Vector3(dest.position.x, transform.position.y, transform.position.z);
        }

        if (distZ > 0.2f)
        {
            if (transform.position.z > dest.position.z)
            {
                transform.position += Vector3.forward * speed * Time.deltaTime;
            }
            else //if (transform.position.z > dest.position.z)
            {
                transform.position += Vector3.back * speed * Time.deltaTime;
            }
        }
        else
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, dest.position.z);
        }

        try
        {
            if (Vector3.Distance(dest.position, transform.position) > 1)
            {
                Quaternion targetRotation = Quaternion.LookRotation(new Vector3(dest.position.x, transform.position.y, dest.position.z) - transform.position);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 2 * Time.deltaTime);
            }
        }
        catch
        {

        }
        
    }

    /// <summary>
    /// é chamado no restart da quest
    /// </summary>
    public virtual void restart()
    {
        transform.position = startPosi;        
    }
}
