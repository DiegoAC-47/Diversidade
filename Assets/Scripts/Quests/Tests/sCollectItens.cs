using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sCollectItens : CollectItens {

    [SerializeField]
    private GameObject limiters;

    private GameObject instance;

    protected override void Start()
    {
        base.Start();
        this.IsStatic = true;
	}

    protected override void OnActive()
    {
        base.OnActive();
        if(this.limiters != null)
            this.instance = Instantiate(this.limiters);
    }

    protected override void RealDone()
    {
        base.RealDone();
        if(this.instance != null)
            Destroy(this.instance);
    }

    public override void Restart()
    {
        if (this.instance != null)
            Destroy(this.instance);
        base.Restart();
    }
}
