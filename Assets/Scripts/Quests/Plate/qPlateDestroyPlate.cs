using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class qPlateDestroyPlate : MonoBehaviour {


    float time = 0;

    private void Update()
    {
        if(time > 0)
        {
            time -= Time.deltaTime;
            if (time < 0)
            {
                Destroy(gameObject);
            }
        }
    }

    public void Setup()
    {
        this.time = 2;
    }


}
