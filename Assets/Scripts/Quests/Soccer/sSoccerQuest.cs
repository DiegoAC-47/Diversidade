using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum qSoccerPosition
{
    GOAL_KEEPER, STRIKER
}

public enum qSoccerTeam
{
    Player, Adversary
}

public enum qSoccerFieldSize
{
    X,Z
}

public class sSoccerQuest : Quest
{
    [SerializeField]
    private float timerTurn = 3f, playerActionTime = 9f, timerTarget = 3f;
    [SerializeField]
    private short maxPoints = 3;
    [SerializeField]
    private qSoccerIA bully, boy;
    [SerializeField]
    private qSoccerGirl girl;
    [SerializeField]
    private Transform adversaryStriker, adversaryGoalKeeper, playerStriker, playerGoalKeeper;
    [SerializeField]
    private Transform minFieldPosition, maxFieldPosition;
    [SerializeField]
    private qSoccerBall ball;
    [SerializeField]
    private qSoccerGoal playerGoal, adversaryGoal;

    [SerializeField]
    private Sprite[] spriteFeedback;

    [SerializeField]
    private Image imageBackGround, feedbackIA;

    [SerializeField]
    private SpriteRenderer feedbackGoal;

    [SerializeField]
    private Text pointIA, pointPlayer;

    private qSoccerFieldSize fieldLength;
    private Transform midField;
    private qSoccerIA bullyInstance, boyInstance;
    private qSoccerGirl girlInstance;
    private qSoccerBall ballInstance;    

    private short playerPoints, adversaryPoints;

    private qSoccerTeam turn = qSoccerTeam.Player;
    private bool playerTurn = false, playerSuccess = false;

    protected override void Start()
    {
        base.Start();
        imageBackGround.enabled = false;
        feedbackGoal.enabled = false;
        pointIA.text = "0";
        pointPlayer.text = "0";
        this.CreateField();
        GetComponentInChildren<Canvas>().enabled = false;
    }

    #region OnActive OnDone Restart
    protected override void OnActive()
    {
        base.OnActive();

        GetComponentInChildren<Canvas>().enabled = true;
        this.girlInstance = Instantiate(this.girl, this.playerStriker.position, this.playerStriker.rotation, this.transform);
        this.bullyInstance = Instantiate(this.bully, this.adversaryStriker.position, this.adversaryStriker.rotation, this.transform);
        this.boyInstance = Instantiate(this.boy, this.adversaryGoalKeeper.position, this.adversaryGoalKeeper.rotation, this.transform);
        this.ballInstance = Instantiate(this.ball, this.Player.transform.position + this.Player.transform.forward, this.Player.transform.rotation);
        this.feedbackIA = bullyInstance.GetComponentInChildren<Image>();
        this.FeedbackIA.enabled = false;
        this.Player.transform.position = this.playerGoalKeeper.position;
        this.Player.transform.rotation = this.playerGoalKeeper.rotation;

        this.girlInstance.Setup(this);
        this.bullyInstance.Setup(this);
        this.boyInstance.Setup(this);
        this.playerGoal.Setup(this, qSoccerTeam.Player);
        this.adversaryGoal.Setup(this, qSoccerTeam.Adversary);

        this.AddScore(3);
        StartCoroutine(this.CountDown(this.timerTurn));
    }

    protected override void OnDone()
    {
        base.OnDone();
        GetComponentInChildren<Canvas>().enabled = false;
        Destroy(this.girlInstance.gameObject);
        Destroy(this.bullyInstance.gameObject);
        Destroy(this.boyInstance.gameObject);
        Destroy(this.ballInstance.gameObject);
    }

    public override void Restart()
    {
        this.playerPoints = 0;
        this.adversaryPoints = 0;

        base.Restart();
    }
    #endregion

    /// <summary>
    /// reset the players, make fade out and start the round
    /// </summary>
    /// <param name="time">time before call start the round</param>
    /// <returns></returns>
    private IEnumerator CountDown(float time)
    {
        ((cSoccerQuest)this.Player.Controller).ResetPosition();
        this.bullyInstance.ResetPosition();
        this.girlInstance.ResetPosition();
        this.boyInstance.ResetPosition();

        Color color = new Color(0, 0, 0, 1);
        imageBackGround.color = color;
        while (color.a > 0)
        {
            color.a -= (1 / 30f);
            imageBackGround.color = color;
            //print("CountDown timeScreen: " + color.a);
            yield return new WaitForSeconds(0.01f);
        }
        imageBackGround.enabled = false;

        
        #region count down desable
        //while (time > 0)
        //{
        //    print("CountDown: " + time);
        //    time--;
            
        //    yield return new WaitForSeconds(1f);
        //}
        #endregion

        this.StartPlayerRound();
    }
    /// <summary>
    /// make fade in, change the turn, call the  players' function end Turn and start count down
    /// </summary>
    /// <returns></returns>
    private IEnumerator Fadein()
    {
        imageBackGround.enabled = true;
        Image image = imageBackGround.GetComponentInChildren<Image>();
        Color color = new Color(0, 0, 0, 0);
        image.color = color;
        while (color.a < 1)
        {
            color.a += (1 / 30f);
            image.color = color;
            yield return new WaitForSeconds(0.01f);
        }

        #region setup end turn
        
        if (this.turn == qSoccerTeam.Player)
            this.turn = qSoccerTeam.Adversary;
        else
            this.turn = qSoccerTeam.Player;

        this.bullyInstance.EndTurn();
        this.boyInstance.EndTurn();
        this.girlInstance.EndTurn();
        ((cSoccerQuest)this.Player.Controller).EndTurn();
        #endregion

        StartCoroutine(this.CountDown(this.timerTurn));
    }

    public void EndTurn()
    {
        FeedbackGoal.enabled = false;
        FeedbackIA.enabled = false;
        StartCoroutine(Fadein());
    }

    private void StartPlayerRound()
    {
        this.playerTurn = true;

        this.boyInstance.StartPlayerRound();
        this.bullyInstance.StartPlayerRound();
        this.girlInstance.StartPlayerRound();
    }

    public void EndPlayerRound()
    {
        this.playerTurn = false;

        this.girlInstance.EndPlayerRound();
        this.bullyInstance.EndPlayerRound();
        this.boyInstance.EndPlayerRound();
    }

    public void Goal(qSoccerTeam team)
    {
        if (team == qSoccerTeam.Adversary)
        {
            this.playerPoints++;
            if (this.playerPoints >= this.maxPoints)
            {
                this.State = QuestState.DONE;
                return;
            }
        }
        else if (team == qSoccerTeam.Player)
        {
            this.adversaryPoints++;
            this.AddScore(-1);
            if (this.adversaryPoints >= this.maxPoints)
            {
                this.Defeated();
                return;
            }
        }

        StartCoroutine(commemoration(team));
        this.PointIA.text = this.adversaryPoints.ToString();
        this.PointPlayer.text = this.playerPoints.ToString();
        //print(this.playerPoints + " x " + this.adversaryPoints + " - Score: " + this.Score);
    }

    public void Defense()
    {
        if (this.Turn != qSoccerTeam.Player)
        {
            StartCoroutine(commemoration(qSoccerTeam.Adversary));
        }
    }

    private IEnumerator commemoration(qSoccerTeam team)
    {
        if (team == qSoccerTeam.Adversary)
        {
            //print("Call Player Animation");
        }
        else if (team == qSoccerTeam.Player)
        {
            //print("Call Adversary Animation");
        }

        yield return new WaitForSeconds(2);
        this.EndTurn();
    }

    private void CreateField()
    {
        float zDistance = Mathf.Abs(this.maxFieldPosition.position.z - this.minFieldPosition.position.z);
        float xDistance = Mathf.Abs(this.maxFieldPosition.position.x - this.minFieldPosition.position.x);

        if (zDistance < xDistance)
            this.fieldLength = qSoccerFieldSize.X;
        else
            this.fieldLength = qSoccerFieldSize.Z;

        this.midField = new GameObject("Mid Field").transform;
        this.midField.SetParent(this.transform);
        this.midField.forward = this.playerGoalKeeper.forward;
        Vector3 mid = new Vector3((this.minFieldPosition.position.x + this.maxFieldPosition.position.x) / 2, 
                                  this.minFieldPosition.position.y + 2, 
                                  (this.minFieldPosition.position.z + this.maxFieldPosition.position.z) / 2);

        this.midField.position = mid;

        Vector3 aux;
        if (this.fieldLength == qSoccerFieldSize.X)
        {
            aux = this.playerGoal.transform.position;
            aux.z = this.midField.position.z;
            this.playerGoal.transform.position = aux;
            aux = this.adversaryGoal.transform.position;
            aux.z = this.midField.position.z;
            this.adversaryGoal.transform.position = aux;

            aux = this.playerGoalKeeper.position;
            aux.z = this.midField.position.z;
            this.playerGoalKeeper.position = aux;

            aux = this.adversaryGoalKeeper.position;
            aux.z = this.midField.position.z;
            this.adversaryGoalKeeper.position = aux;

            aux = this.playerStriker.position;
            aux.z = this.midField.position.z;
            this.playerStriker.position = aux;

            aux = this.adversaryStriker.position;
            aux.z = this.midField.position.z;
            this.adversaryStriker.position = aux;
        }
        else
        {
            aux = this.playerGoal.transform.position;
            aux.x = this.midField.position.x;
            this.playerGoal.transform.position = aux;
            aux = this.adversaryGoal.transform.position;
            aux.x = this.midField.position.x;
            this.adversaryGoal.transform.position = aux;

            aux = this.playerGoalKeeper.position;
            aux.x = this.midField.position.x;
            this.playerGoalKeeper.position = aux;

            aux = this.adversaryGoalKeeper.position;
            aux.x = this.midField.position.x;
            this.adversaryGoalKeeper.position = aux;

            aux = this.playerStriker.position;
            aux.x = this.midField.position.x;
            this.playerStriker.position = aux;

            aux = this.adversaryStriker.position;
            aux.x = this.midField.position.x;
            this.adversaryStriker.position = aux;
        }
    }

    public override void LoadState()
    {
        base.LoadState();
        throw new System.NotImplementedException();
    }

    #region get/set
    public bool PlayTurn
    {
        get
        {
            return playerTurn;
        }
    }

    public qSoccerTeam Turn
    {
        get
        {
            return turn;
        }
    }

    public Transform MidField
    {
        get
        {
            return midField;
        }
    }

    public Transform MinFieldPosition
    {
        get
        {
            return minFieldPosition;
        }
    }

    public Transform MaxFieldPosition
    {
        get
        {
            return maxFieldPosition;
        }
    }

    public Transform AdversaryStriker
    {
        get
        {
            return adversaryStriker;
        }
    }

    public Transform AdversaryGoalKeeper
    {
        get
        {
            return adversaryGoalKeeper;
        }
    }

    public Transform PlayerStriker
    {
        get
        {
            return playerStriker;
        }
    }

    public Transform PlayerGoalKeeper
    {
        get
        {
            return playerGoalKeeper;
        }
    }

    public bool PlayerSuccess
    {
        get
        {
            return playerSuccess;
        }

        set
        {
            playerSuccess = value;
        }
    }

    public qSoccerBall BallInstance
    {
        get
        {
            return ballInstance;
        }
    }

    public float PlayerActionTime
    {
        get
        {
            return playerActionTime;
        }
    }

    public qSoccerGirl GirlInstance
    {
        get
        {
            return girlInstance;
        }
    }

    public qSoccerFieldSize FieldLength
    {
        get
        {
            return fieldLength;
        }
    }

    public qSoccerGoal PlayerGoal
    {
        get
        {
            return playerGoal;
        }
    }

    public qSoccerGoal AdversaryGoal
    {
        get
        {
            return adversaryGoal;
        }
    }

    public qSoccerIA BullyInstance
    {
        get
        {
            return bullyInstance;
        }
    }

    public qSoccerIA BoyInstance
    {
        get
        {
            return boyInstance;
        }
    }

    public float TimerTarget
    {
        get
        {
            return timerTarget;
        }
    }

    public Text PointIA
    {
        get
        {
            return pointIA;
        }
    }

    public Text PointPlayer
    {
        get
        {
            return pointPlayer;
        }        
    }

    public Image FeedbackIA
    {
        get
        {
            return feedbackIA;
        }
    }

    public Sprite[] SpriteFeedback
    {
        get
        {
            return spriteFeedback;
        }
    }

    public SpriteRenderer FeedbackGoal
    {
        get
        {
            return feedbackGoal;
        }
    }



    #endregion


}
