using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioBehaviour : StateMachineBehaviour {

    [SerializeField]
    private AudioClip audio;
    [SerializeField]
    private float repeatInterval;

    private float timer;

    //OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        this.timer = this.repeatInterval;
        this.PlayAudio(animator.gameObject);
    }

    //OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (this.repeatInterval > 0)
        {
            if (this.timer > 0)
            {
                this.timer -= Time.deltaTime;
            }
            else
            {
                this.PlayAudio(animator.gameObject);
                this.timer = this.repeatInterval;
            }
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    //override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
    //
    //}

    // OnStateMove is called right after Animator.OnAnimatorMove(). Code that processes and affects root motion should be implemented here
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
    //
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK(). Code that sets up animation IK (inverse kinematics) should be implemented here.
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
    //
    //}

    private void PlayAudio(GameObject go)
    {
        if (go.GetComponentInParent<AudioSource>().clip != this.audio)
            go.GetComponentInParent<AudioSource>().clip = this.audio;
        go.GetComponentInParent<AudioSource>().Play();
    }
}
