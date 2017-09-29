using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersonSignal : MonoBehaviour
{

    [SerializeField]
    private bool needPass;

    public bool NeedPass
    {
        get
        {
            return needPass;
        }

    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.GetComponent<IAPerson>())
        {
            needPass = true;
            GetComponent<Renderer>().material.color = Color.green;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.GetComponent<IAPerson>())
        {
            needPass = false;
            GetComponent<Renderer>().material.color = Color.red;
        }
    }
}
