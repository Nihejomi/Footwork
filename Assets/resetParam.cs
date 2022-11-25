using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class resetParam : StateMachineBehaviour
{
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (stateInfo.IsName("vasenAstuu")|| stateInfo.IsName("oikeaAstuu"))
        {
            animator.SetBool("vasenEteen", false);
            animator.SetBool("oikeaEteen", false);
          

        }
        if (stateInfo.IsName("VasenEdessa") || stateInfo.IsName("oikeaEdessa")) {
            animator.SetBool("AskelVasemmalleVasenEdessa", false);
            animator.SetBool("AskelOikeaanVasenEdessa", false);
            animator.SetBool("SivuaskelOikeaanOikeaEdella", false);
            animator.SetBool("SivuaskelVasempaanOikeaEdella", false);
        }

        if (stateInfo.IsName("YlavartalonIdle")) {
            animator.SetBool("VasenLyonti", false);
        }

    }
    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    //override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    //override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}
}
