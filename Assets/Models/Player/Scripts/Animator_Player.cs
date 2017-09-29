using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Animator_Player : MonoBehaviour {

    Animator anim;

	// Use this for initialization
	void Start () {

        anim = GetComponent<Animator>();

        

    }
	
	// Update is called once per frame
	void Update () {

        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            anim.SetTrigger("Jump");
        }

        if (anim.GetCurrentAnimatorStateInfo(1).IsName("Throw Meat") && anim.GetCurrentAnimatorStateInfo(1).normalizedTime > 1)
        {
            anim.SetLayerWeight(1, 0);
        }

        if (anim.GetCurrentAnimatorStateInfo(2).IsName("Indignado Running") && anim.GetCurrentAnimatorStateInfo(2).normalizedTime > 1)
        {
            anim.SetLayerWeight(2, 0);
        }
       


        if (Input.GetKeyDown(KeyCode.I))
        {
            anim.SetLayerWeight(2, 1);
            anim.Play("Indignado Running", 2, 0f);
        }

        if (Input.GetKeyDown(KeyCode.K))
        {
            anim.SetLayerWeight(3, 1);
            anim.Play("Knocking", 3, 0f);
        }

        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            anim.SetLayerWeight(1,1);
            anim.Play("Throw Meat", 1, 0f);
        }

        if (Input.GetKey(KeyCode.LeftShift))
        {
            anim.SetBool("Running", true);
        }

        else
        {
            //anim.SetBool("Running", false);
        }
    }
}
