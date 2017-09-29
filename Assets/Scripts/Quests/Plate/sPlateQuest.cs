using UnityEngine;
using System.Collections;
using System;

public enum PlateType
{
    NULL,
    TYPE_1,
    TYPE_2,
    TYPE_3,
    TYPE_4
}

public enum ScoreType
{
    RIGHT,
    WRONG,
    MISS
}

public class sPlateQuest : Quest
{
    [SerializeField]
    private float cooldown, timeLimit, timeQuestMax, timeQuest;

    [SerializeField]
    private int scoreRight, scoreWrong, scoreMiss;

    private bool run = false;

    /// <summary>
    /// Altera a pontução de acordo com o ScoreType
    /// </summary>
    /// <param name="type"></param>
    public void CheckScore(ScoreType type)
    {
        if(type == ScoreType.MISS)
        {
            AddScore(scoreMiss);
        }
        else if(type == ScoreType.RIGHT)
        {
            AddScore(scoreRight);
        }
        else
        {
            AddScore(scoreWrong);
        }
    }

    public override void Restart()
    {
        this.timeQuest = this.timeQuestMax;
        base.Restart();
    }

    protected override void OnDone()
    {
        Final();
        base.OnDone();
    }

    protected override void OnActive()
    {
        base.OnActive();
        this.timeQuest = this.timeQuestMax;
        Player.Controller = this.controller;
        run = true;
        for (int i = 0; i < FindObjectsOfType<qPlateTable>().Length; i++)
        {
            FindObjectsOfType<qPlateTable>()[i].Setup(UnityEngine.Random.Range(0, 10));
        }

        StartCoroutine(controllTime());
    }

    private IEnumerator controllTime()
    {
        do
        {
            timeQuest -= 0.1f;
            yield return new WaitForSeconds(0.1f);
        }
        while (timeQuest > 0);
        State = QuestState.DONE;
    }

    public override void LoadState()
    {
        base.LoadState();
        Final();
    }

    private void Final()
    {
        run = false;

        for (int i = 0; i < FindObjectsOfType<qPlateObjs>().Length; i++)
        {
            FindObjectsOfType<qPlateObjs>()[i].onDone();
        }

        Player p = FindObjectOfType<Player>();
        p.ResetToDefaultController();
    }

    public float Cooldown
    {
        get
        {
            return cooldown;
        }

        set
        {
            cooldown = value;
        }
    }

    public float TimeLimit
    {
        get
        {
            return timeLimit;
        }

        set
        {
            timeLimit = value;
        }
    }

    public bool Run
    {
        get
        {
            return run;
        }
    }
}
