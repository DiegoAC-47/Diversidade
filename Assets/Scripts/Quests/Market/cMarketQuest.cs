using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cMarketQuest : PlayerControllerDefault {

    private bool hasObject = false;

    public bool HasObject
    {
        get
        {
            return hasObject;
        }

        set
        {
            hasObject = value;
        }
    }
}
