using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class qSoccerGoal : MonoBehaviour
{
    private qSoccerTeam team;
    private sSoccerQuest quest;
    private Vector3[] positions;

    public void Setup(sSoccerQuest quest, qSoccerTeam team)
    {
        this.quest = quest;
        this.team = team;

        this.positions = new Vector3[3];
        Bounds bounds = this.GetComponent<Renderer>().bounds;
        float size = bounds.extents.x * 2 / 3;
        Vector3 aux = this.transform.position;
        aux.x = bounds.min.x + size / 2;
        this.positions[0] = aux;
        this.positions[1] = this.transform.position;
        aux.x = bounds.max.x - size / 2;
        this.positions[2] = aux;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.GetComponent<qSoccerBall>())
        {
            this.quest.Goal(this.team);
        }
    }

    public Vector3 GetPosition()
    {
        return this.positions[Random.Range(0, this.positions.Length)];
    }

    public int GetIndex(Vector3 posi)
    {
        for (int i = 0; i < this.positions.Length; i++)
        {
            if (Vector3.Distance(this.positions[i], posi) < 1)
            {
                return i;
            }
        }
        return -1;
    }

    public Vector3 GetOpositePosition(Vector3 position)
    {
        float dist = -10, aux;
        int index = -1;
        for (int i = 0; i < this.positions.Length; i++)
        {
            aux = Vector3.Distance(this.positions[i], position);
            if (aux > dist)
            {
                index = i;
                dist = aux;
            }
        }
        return this.positions[index];
    }

    public Vector3[] Positions
    {
        get
        {
            return positions;
        }
    }
}
