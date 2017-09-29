using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class qSquareObjs : MonoBehaviour
{

    private sSquareQuest quest;

    virtual protected void Start()
    {
        quest = FindObjectOfType<sSquareQuest>();
    }

    public sSquareQuest Quest
    {
        get
        {
            return quest;
        }
    }

    public abstract void reset();
}
