using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IAPath : MonoBehaviour
{
    [SerializeField]
    protected Transform[] firstPath;

    [SerializeField]
    protected IAPath firstStreet;

    [SerializeField]
    protected Transform[] secondPath;

    [SerializeField]
    protected IAPath secondStreet;

    protected virtual void Start()
    {
        if (secondPath == null || secondPath.Length < 1)
        {
            secondPath = firstPath;
        }

        if (secondStreet == null)
        {
            secondStreet = firstStreet;
        }

        for (int i = 0; i < GetComponentsInChildren<Renderer>().Length; i++)
        {
            GetComponentsInChildren<Renderer>()[i].enabled = false;
        }
    }

    public IAPath getNextPath(int chosenStreet)
    {
        if (chosenStreet == 1)
        {
            return secondStreet;
        }
        else
        {
            return firstStreet;
        }
    }

    public Transform[] getPath(int chosenStreet)
    {
        if (chosenStreet == 1)
        {
            return secondPath;
        }
        else
        {
            return firstPath;
        }
    }

}
