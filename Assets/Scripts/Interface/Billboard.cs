using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Billboard : MonoBehaviour {

    private void Update()
    {
        if(FindObjectOfType<Player>())
            this.transform.LookAt(FindObjectOfType<CameraManager>().Camera.transform);
    }
}
