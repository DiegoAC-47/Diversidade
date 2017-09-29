using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class sSquareQuest : Quest
{
    [SerializeField]
    private GameObject[] objScene, disable, targets;

    [SerializeField]
    private Transform basePosition;

    [SerializeField]
    private Transform[] spawn;

    [SerializeField]
    private float time, questTime;

    private bool run, shooting;

    protected override void Start()
    {
        base.Start();
        setup(false);
        shooting = false;
    }

    void Update()
    {
        if (run)
        {
            if(questTime < 0 )
            {
                State = QuestState.DONE;
            }
            else
            {
                questTime -= Time.deltaTime;
            }

            if (time < 0)
            {
                int aux = UnityEngine.Random.Range(0, spawn.Length);
                int aux2 = UnityEngine.Random.Range(0, targets.Length);

                Instantiate(targets[aux2], spawn[aux].position, Quaternion.identity);
                time = 5;
            }
            else
            {
                time -= Time.deltaTime;
            }
        }
    }

    public override void Restart()
    {
        qSquareTarget[] targets = FindObjectsOfType<qSquareTarget>();

        for (int i = 0; i <targets.Length; i++)
        {
            Destroy(targets[i].gameObject);
        }

        qSquareObjs[] objs = FindObjectsOfType<qSquareObjs>();

        for (int i = 0; i < objs.Length; i++)
        {
            objs[i].reset();
        }

        base.Restart();
    }

    protected override void OnActive()
    {
        base.OnActive();
        setup(true);
    }

    protected override void OnDone()
    {
        base.OnDone();
        setup(false);

        qSquareTarget[] targets = FindObjectsOfType<qSquareTarget>();

        for (int i = 0; i < targets.Length; i++)
        {
            Destroy(targets[i].gameObject); 
        }

    }

    private void setup(bool b)
    {
        for (int i = 0; i < objScene.Length; i++)
        {
            objScene[i].SetActive(b);
        }

        for (int i = 0; i < disable.Length; i++)
        {
            disable[i].SetActive(!b);
        }

        if (b)
        {
            FindObjectOfType<qSquareAim>().setup(b);
        }

        run = b;
    }

    public void defeat()
    {
        Restart();
    }

    public override void LoadState()
    {
        base.LoadState();
        throw new System.NotImplementedException();
    }

    public Vector3 Base
    {
        get
        {
            return basePosition.position;
        }
    }
}
