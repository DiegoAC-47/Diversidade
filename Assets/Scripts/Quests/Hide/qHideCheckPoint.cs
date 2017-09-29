using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class qHideCheckPoint : MonoBehaviour {

    [SerializeField]
    private Transform[] pathPoints;
    
    public Transform[] PathPoints
    {
        get
        {
            return pathPoints;
        }

        set
        {
            pathPoints = value;
        }
    }
}
