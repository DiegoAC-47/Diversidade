using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[RequireComponent(typeof(Collider))]
public class qSoccerIA : MonoBehaviour
{
    [SerializeField]
    protected qSoccerPosition startPosition;
    [SerializeField]
    protected qSoccerTeam team;
    [SerializeField]
    protected Transform foot;
    [SerializeField]
    private float kickForce = 15f;
    [SerializeField]
    protected float speed = 5f;
    [SerializeField]
    protected SpriteRenderer target;

    protected SpriteRenderer targetInstance;
    protected qSoccerPosition position;
    protected sSoccerQuest quest;
    protected Vector3 kickTarget;
    protected bool waitingBall = false;
    protected Vector2 rangeX, rangeZ;

    public event Action ReadyToKick;

    public virtual void Setup(sSoccerQuest quest)
    {
        this.quest = quest;
        this.position = this.startPosition;
        this.target.name = "Instance " + name;
        this.targetInstance = Instantiate(this.target, quest.MidField.position + Vector3.up * 2, this.target.transform.rotation, this.quest.transform);

        RaycastHit hit = new RaycastHit();
        Physics.Raycast(new Ray(this.targetInstance.transform.position, -this.quest.MidField.up), out hit);
        this.targetInstance.transform.position = new Vector3(this.targetInstance.transform.position.x, hit.point.y + 0.01f, this.targetInstance.transform.position.z);

        this.targetInstance.enabled = false;

        //calculo de range talvez tenha que otimizar
        this.rangeX = new Vector2();
        this.rangeZ = new Vector2();

        if (this.quest.FieldLength == qSoccerFieldSize.X)
        {
            this.rangeX.x = this.quest.MidField.position.x;
            this.rangeX.y = this.team == qSoccerTeam.Adversary ? this.quest.PlayerGoalKeeper.position.x : this.quest.AdversaryGoalKeeper.position.x;
            this.rangeZ.x = this.quest.MinFieldPosition.position.z;
            this.rangeZ.y = this.quest.MaxFieldPosition.position.z;
        }
        else
        {
            this.rangeX.x = this.quest.MinFieldPosition.position.x;
            this.rangeX.y = this.quest.MaxFieldPosition.position.x;
            this.rangeZ.x = this.quest.MidField.position.z;
            this.rangeZ.y = this.team == qSoccerTeam.Adversary ? this.quest.PlayerGoalKeeper.position.z : this.quest.AdversaryGoalKeeper.position.z;
        }

        if (this.quest.MaxFieldPosition.position.x > 0)
        {
            this.rangeX.x -= targetInstance.bounds.extents.x;
            this.rangeX.y += targetInstance.bounds.extents.x;
        }
        else
        {
            this.rangeX.x += targetInstance.bounds.extents.x;
            this.rangeX.y -= targetInstance.bounds.extents.x;
        }
        if (this.quest.MaxFieldPosition.position.z > 0)
        {
            this.rangeZ.x += targetInstance.bounds.extents.z;
            this.rangeZ.y -= targetInstance.bounds.extents.z;
        }
        else
        {
            this.rangeZ.x -= targetInstance.bounds.extents.z;
            this.rangeZ.y += targetInstance.bounds.extents.z;
        }
    }

    public virtual void Remove()
    {
    }

    public void StartPlayerRound()
    {
        switch (this.position)
        {
            case qSoccerPosition.GOAL_KEEPER:
                this.StartGoalKeeper();
                break;
            case qSoccerPosition.STRIKER:
                this.StartStriker();
                break;
        }
    }

    public void EndPlayerRound()
    {
        switch (this.position)
        {
            case qSoccerPosition.GOAL_KEEPER:
                this.EndGoalKeeper();
                break;
            case qSoccerPosition.STRIKER:
                this.EndStriker();
                break;
        }
    }

    public virtual void EndTurn()
    {
        if (this.quest.Turn == qSoccerTeam.Adversary)
        {
            if (this.position == qSoccerPosition.GOAL_KEEPER)
            {
                this.transform.position = this.quest.AdversaryGoalKeeper.position;
                this.transform.forward = this.quest.AdversaryGoalKeeper.forward;

                this.quest.BallInstance.RigidBody.velocity = Vector3.zero;
                this.quest.BallInstance.RigidBody.useGravity = false;
                this.quest.BallInstance.transform.position = this.transform.position + this.transform.forward;
            }
            else
            {
                this.RandomizeTarget();
            }
        }
    }

    protected virtual void StartGoalKeeper()
    {
        if (this.quest.Turn == qSoccerTeam.Adversary)
        {
            this.quest.FeedbackGoal.enabled = true;
            this.quest.BallInstance.Parable(this.quest.BullyInstance.TargetInstance.transform);
        }
    }

    protected virtual void StartStriker()
    {
        if (this.quest.Turn == qSoccerTeam.Adversary)
        {
            this.waitingBall = true;
            StartCoroutine(this.GotoTarget(this.targetInstance.transform, this.speed));
        }
    }

    protected virtual void EndGoalKeeper()
    {
        if (this.quest.PlayerSuccess)
        {
            this.quest.GirlInstance.ReadyToKick += Ia_ReadyToKick;
        }
    }

    protected virtual void EndStriker()
    {
        if (!this.quest.PlayerSuccess)
        {
            this.RandomizeTarget();
            this.waitingBall = true;

            StartCoroutine(this.GotoTarget(this.targetInstance.transform, this.speed));
        }
    }

    protected virtual void Ia_ReadyToKick()
    {
        Vector3 aux = this.transform.position;

        if (this.quest.FieldLength == qSoccerFieldSize.X)
            aux.z = this.quest.AdversaryGoal.GetOpositePosition(this.quest.GirlInstance.KickTarget).z;
        else
            aux.x = this.quest.AdversaryGoal.GetOpositePosition(this.quest.GirlInstance.KickTarget).x;

        this.quest.GirlInstance.ReadyToKick -= this.Ia_ReadyToKick;

        StartCoroutine(this.GotoTarget(aux, this.speed));
    }

    public void RandomizeTarget()
    {
        float x = UnityEngine.Random.Range(this.rangeX.x, this.rangeX.y);
        float z = UnityEngine.Random.Range(this.rangeZ.x, this.rangeZ.y);
        this.targetInstance.transform.position = new Vector3(x, this.targetInstance.transform.position.y, z);
        controllTarget(this.targetInstance);
    }

    /// <summary>
    /// Verifica se a targetInstance está fora do campo
    /// </summary>
    /// <param name="targetInstance"></param>
    private void controllTarget(SpriteRenderer targetInstance)
    {
        if (targetInstance.bounds.max.x > quest.MinFieldPosition.position.x)
        {
            Vector3 aux = targetInstance.transform.position;
            aux.x = quest.MinFieldPosition.position.x - targetInstance.bounds.extents.x;
            targetInstance.transform.position = aux;
        }
        else if (targetInstance.bounds.min.x < quest.MaxFieldPosition.position.x)
        {
            Vector3 aux = targetInstance.transform.position;
            aux.x = quest.MaxFieldPosition.position.x + targetInstance.bounds.extents.x;
            targetInstance.transform.position = aux;
        }

        if (targetInstance.bounds.max.z > quest.MinFieldPosition.position.z)
        {
            Vector3 aux = targetInstance.transform.position;
            aux.z = quest.MinFieldPosition.position.z - targetInstance.bounds.extents.z;
            targetInstance.transform.position = aux;
        }
        else if (targetInstance.bounds.min.z < quest.MaxFieldPosition.position.z)
        {
            Vector3 aux = targetInstance.transform.position;
            aux.z = quest.MaxFieldPosition.position.z + targetInstance.bounds.extents.z;
            targetInstance.transform.position = aux;
        }
    }

    public virtual void OnTriggerStay(Collider collider)
    {
        if (this.waitingBall && collider.GetComponent<qSoccerBall>() && quest.Turn == this.team)
        {
            this.waitingBall = false;
            if (this.position == qSoccerPosition.GOAL_KEEPER)
            {
                collider.GetComponent<qSoccerBall>().BallReady += QSoccerIA_BallReady;
                collider.GetComponent<qSoccerBall>().GoTo(this.foot);
                this.quest.EndTurn();
            }
            else
            {
                this.kickTarget = this.quest.PlayerGoal.GetPosition();
                collider.transform.position = this.transform.position + this.transform.forward;
                StartCoroutine(this.Kick(collider.GetComponent<qSoccerBall>(), this.speed));
            }
        }
    }

    protected virtual void QSoccerIA_BallReady(qSoccerBall obj)
    {
        print(name + " lançamento");
        obj.BallReady -= this.QSoccerIA_BallReady;
        obj.Parable(this.quest.BullyInstance.TargetInstance.transform);
        StartCoroutine(this.GotoTarget(quest.AdversaryGoalKeeper, this.speed));
    }

    protected IEnumerator GotoTarget(Transform target, float speed)
    {
        Vector3 initialPosition = this.transform.position;
        Vector3 finalPosition = target.position;
        finalPosition.y = initialPosition.y;
        float distance = Vector3.Distance(initialPosition, finalPosition);
        float time = Time.time;
        while (this.transform.position != finalPosition)
        {
            float fracJourney = ((Time.time - time) * speed) / distance;

            this.transform.position = Vector3.Lerp(initialPosition, finalPosition, fracJourney);
            yield return new WaitForFixedUpdate();
        }
    }

    protected IEnumerator GotoTarget(Vector3 target, float speed)
    {
        Vector3 initialPosition = this.transform.position;
        Vector3 finalPosition = target;
        finalPosition.y = initialPosition.y;
        float distance = Vector3.Distance(initialPosition, finalPosition);
        float time = Time.time;
        while (this.transform.position != finalPosition)
        {
            float fracJourney = ((Time.time - time) * speed) / distance;

            this.transform.position = Vector3.Lerp(initialPosition, finalPosition, fracJourney);
            yield return new WaitForFixedUpdate();
        }
    }

    public virtual void ResetPosition()
    {
        if (this.position == qSoccerPosition.STRIKER)
        {
            this.transform.position = this.quest.AdversaryStriker.position;
            this.transform.rotation = this.quest.AdversaryStriker.rotation;
        }
        else if (this.position == qSoccerPosition.GOAL_KEEPER)
        {
            this.transform.position = this.quest.AdversaryGoalKeeper.position;
            this.transform.rotation = this.quest.AdversaryGoalKeeper.rotation;
        }
    }

    protected IEnumerator Kick(qSoccerBall ball, float speed)
    {
        if (this.team == qSoccerTeam.Adversary)
        {
            this.quest.FeedbackIA.enabled = true;
            ((sSoccerQuest)quest).FeedbackIA.sprite = this.quest.PlayerGoal.GetIndex(kickTarget) > -1 & this.quest.PlayerGoal.GetIndex(kickTarget) < ((sSoccerQuest)quest).SpriteFeedback.Length - 1 ? ((sSoccerQuest)quest).SpriteFeedback[this.quest.PlayerGoal.GetIndex(kickTarget)] : ((sSoccerQuest)quest).SpriteFeedback[0];

            //((sSoccerQuest)quest).SetFeedbackIA(true, "Ele vai chutar: " + ( + 1));
            yield return new WaitForSeconds(1f);
        }

        ball.transform.parent = this.transform;

        Vector3 aux = this.kickTarget - this.transform.position;
        aux.y = this.transform.position.y;
        Quaternion finalRotation = Quaternion.LookRotation(aux);
        Vector3 initialRotation = this.transform.rotation.eulerAngles;
        float time = Time.time;

        while (Vector3.Distance(this.transform.rotation.eulerAngles, finalRotation.eulerAngles) > 0.1f)
        {
            this.transform.rotation = Quaternion.Lerp(transform.rotation, finalRotation, speed / 4 * (Time.time - time));
            yield return new WaitForFixedUpdate();
        }

        if (this.ReadyToKick != null)
        {
            this.ReadyToKick();
        }

        ball.transform.parent = this.quest.transform;
        ball.transform.forward = this.transform.forward;
        ball.RigidBody.useGravity = true;
        ball.RigidBody.AddForce(ball.transform.forward * this.kickForce, ForceMode.Impulse);
    }

    public qSoccerPosition Position
    {
        get
        {
            return position;
        }
    }

    public Vector3 KickTarget
    {
        get
        {
            return kickTarget;
        }
    }

    public SpriteRenderer TargetInstance
    {
        get
        {
            return targetInstance;
        }
    }

    public Vector2 RangeX
    {
        get
        {
            return rangeX;
        }
    }

    public Vector2 RangeZ
    {
        get
        {
            return rangeZ;
        }
    }
}
