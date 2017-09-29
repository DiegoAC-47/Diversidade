using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class CollectItens : Quest {

    [SerializeField]
    protected GameObject item, circle;

    protected QuestItem[] itens;
    protected int count = 0;

    public event Predicate<QuestItem> Collect;

    protected override void Start ()
    {
        base.Start();
	}
	
    protected override void OnActive()
    {
        base.OnActive();
        GameObject item = Instantiate(this.item);
        this.itens = item.GetComponentsInChildren<QuestItem>();
        GameObject g;
        foreach (QuestItem i in this.itens)
        {
            g = Instantiate(circle, i.transform);
            i.Collected += Item_Collected;
        }
        this.Label.Counter = this.FeedBack();
    }

    private bool Item_Collected(QuestItem obj)
    {
        this.AddScore(1);
        this.count++;
        this.OnCollect(obj);
        if (this.count >= this.itens.Length)
        {
            this.State = QuestState.DONE;
        }
        Destroy(obj.gameObject);
        return true;
    }

    public string FeedBack()
    {
        return (this.count.ToString() + "/" + this.itens.Length.ToString());
    }

    private bool OnCollect(QuestItem obj)
    {
        this.Label.Counter = this.FeedBack();
        if (this.Collect != null)
            this.Collect(obj);
        else
            return false;
        return true;
    }

    public override void Restart()
    {
        foreach (QuestItem i in this.itens)
        {
            if (i != null)
            {
                i.Collected -= Item_Collected;
                Destroy(i.gameObject);
            }
        }
        this.count = 0;
        this.Label.Counter = this.FeedBack();
        base.Restart();
    }

    public override void LoadState()
    {
        base.LoadState();
    }
}
