using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class qPlateObjPlate : MonoBehaviour
{
    [SerializeField]
    private PlateType type;

    public PlateType Type
    {
        get
        {
            return type;
        }

        set
        {
            type = value;
        }
    }
    
}
