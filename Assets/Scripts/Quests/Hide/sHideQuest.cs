using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sHideQuest : Quest
{
    [SerializeField]
    private qHideAgent agent;

    [SerializeField]
    private GameObject questObj;

    [SerializeField]
    private qHideCheckPoint[] points;

    [SerializeField]
    private float amplitude = 20, scoreMax = 100;

    private float timeStart;

    private int indexPoint = 0;

    protected override void Start()
    {
        base.Start();
        questObj.SetActive(false);
    }

    /// <summary>
    /// atualiza qual é o próximo pathPoint e verifica se a missão acabou.
    /// </summary>
    public void NextPoint()
    {
        indexPoint++;
        if(indexPoint > points.Length - 1)
        {
            indexPoint = 0;
            State = QuestState.DONE;
        }

    }

    /// <summary>
    /// retorna o qHideCheckPoint atual.
    /// </summary>
    /// <returns></returns>
    public qHideCheckPoint ActualPoint()
    {
        return this.points[this.indexPoint];
    }

    public override void Restart()
    {
        indexPoint = 0;
        qHideObjs[] obj = FindObjectsOfType<qHideObjs>();

        for (int i = 0; i < obj.Length; i++)
        {
            obj[i].restart();
        }
        base.Restart();
    }

    protected override void OnActive()
    {
        base.OnActive();
        Player p = FindObjectOfType<Player>();
        p.Controller = this.controller;
        timeStart = Time.timeSinceLevelLoad;

        questObj.SetActive(true);
    }

    protected override void OnDone()
    {
        base.OnDone();
        Player p = FindObjectOfType<Player>();
        p.ResetToDefaultController();
        questObj.SetActive(false);
        AddScore((int)(scoreMax - (Time.timeSinceLevelLoad - timeStart)));
    }

    public override void LoadState()
    {
        base.LoadState();
        throw new NotImplementedException();
    }

    public qHideAgent Agent
    {
        get
        {
            return agent;
        }
    }

    public float Amplitude
    {
        get
        {
            return amplitude;
        }
    }
}
