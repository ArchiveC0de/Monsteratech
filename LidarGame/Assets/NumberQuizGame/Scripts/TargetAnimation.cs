using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NumberQuizGame
{
    public class TargetAnimation : StateMachineBehaviour
    {
        override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            Destroy(animator.gameObject);
        }
    }
}