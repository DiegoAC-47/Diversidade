using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sMarketQuest : Quest
{
    [SerializeField]
    private GameObject[] objScene, disabled;

    private qMarketTent[] tents;

    [SerializeField]
    private qMarketAssistant[] assistants;

    private List<Vector3> oldPositions;

    [SerializeField]
    private float timeUpdate, people;

    private float time;

    private bool runing;

    protected override void Start()
    {
        base.Start();
        oldPositions = new List<Vector3>();
        assistants = new qMarketAssistant[3];
        setup(false);
    }

    void Update()
    {
        if (runing)
        {
            if (Vector3.Distance(Player.transform.position, oldPositions.Count > 0 ? oldPositions[oldPositions.Count - 1] : Player.transform.position) > 2f)
            {
                oldPositions.Add(Player.transform.position);
            }

            if (time > timeUpdate)
            {
                time = 0;
                for (int i = 0; i < tents.Length; i++)
                {
                    tents[i]._Update();
                }
            }
            else
            {
                time += Time.deltaTime;
            }

            if (Input.GetAxis(Inputs.qMarket1B.ToString()) > 0)
            {
                help(AssistantType.FOOD);
            }
            else if (Input.GetAxis(Inputs.qMarket2X.ToString()) > 0)
            {
                help(AssistantType.MATERIAL);
            }
            else if (Input.GetAxis(Inputs.qMarket3Y.ToString()) > 0)
            {
                help(AssistantType.MONEY);
            }
        }
    }

    public void problem()
    {
        people -= 10;

        if(people < 0)
        {
            lose();
        }
    }

    private void help(AssistantType type)
    {
        try
        {
            if (Player.Interactive.gameObject.GetComponent<qMarketTent>())
            {
                assistants[(int)type - 1].help(Player.Interactive.gameObject.GetComponent<qMarketTent>());
            }
        }
        catch 
        {

        }
    }

    private void lose()
    {
        Restart();
        people = 100;
        runing = false;
        for (int i = 0; i < FindObjectsOfType<qMarketObjs>().Length; i++)
        {
            FindObjectsOfType<qMarketObjs>()[i].restart();
        }
    }

    public void nextDestiny()
    {
        if (oldPositions.Count > 1)
        {
            oldPositions.RemoveAt(0);
        }
    }

    protected override void OnActive()
    {
        base.OnActive();
        setup(true);
        oldPositions.Add(Player.transform.position);
        tents = FindObjectsOfType<qMarketTent>();
        //assistants[(int)AssistantType.FOOD]
        qMarketAssistant[] t = FindObjectsOfType<qMarketAssistant>();

        GameObject g = new GameObject();
        g.name = "ref";
        g.transform.parent = Player.transform;
        g.transform.position = Player.transform.position - Vector3.forward;

        for (int i = 0; i < t.Length; i++)
        {
            assistants[(int)t[i].Type - 1] = t[i];
            t[i].setup(g.transform);
        }
          

    }

    public void objecTrouble()
    {
        tents[UnityEngine.Random.Range(0, tents.Length)].HasObject = true;
    }

    public override void Restart()
    {
        base.Restart();
        oldPositions.Clear();
    }

    protected override void OnDone()
    {
        base.OnDone();
        setup(false);
    }

    private void setup(bool b)
    {
        runing = b;
        for (int i = 0; i < objScene.Length; i++)
        {
            objScene[i].SetActive(b);
        }

        for (int i = 0; i < disabled.Length; i++)
        {
            disabled[i].SetActive(!b);
        }
    }

    public override void LoadState()
    {
        base.LoadState();
        throw new System.NotImplementedException();
    }

    public Vector3 Destiny
    {
        get
        {
            if (oldPositions.Count > 0)
            {
                return oldPositions[0];
            }
            else
            {
                return Vector3.zero;
            }
        }
    }
}
