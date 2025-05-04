using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace SG
{
    public class ResetJumping : StateMachineBehaviour
    {
        CharacterManager character;

        // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
        override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            if (character == null)
            {
                character = animator.GetComponent<CharacterManager>();
            }

            // THIS IS CALLED WHEN AN ACTION ENDS, AND THE STATE RETURNS TO "EMPTY"
            character.isPerformingAction = false;
            character.applyRootMotion = false;
            character.canRotate = true;
            character.canMove = true;
            character.isJumping = false;
        }
    }

}

