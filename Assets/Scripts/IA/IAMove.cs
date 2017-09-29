using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class IAMove : MonoBehaviour {

    [SerializeField]
    protected IAPath path;

    [SerializeField]
    protected int chosenStreet, countStreet;

    protected void moveUpdate()
    {
        if (Vector3.Distance(actualPosition(), transform.position) < 1)
        {
            countStreet++;
            if (countStreet > path.getPath(chosenStreet).Length - 1)
            {
                onChange();
                newPath();
            }
        }
    }

    protected abstract void onChange();

    protected Vector3 actualPosition()
    {
        return path.getPath(chosenStreet)[countStreet].position;
    }

    private void newPath()
    {
        countStreet = 0;
        path = path.getNextPath(chosenStreet);
        //chosenStreet = chosenStreet == 0 ? 1 : 0;
        chosenStreet = UnityEngine.Random.Range(0, 2);        
    }
}
