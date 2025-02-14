using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace SG
{
    public class CharacterAnimationManager : MonoBehaviour
    {
        CharacterManager character;
        
        float vertical;
        float horizontal;

        protected virtual void Awake()
        {
            character = GetComponent<CharacterManager>();
        }

        public void UpdateAnimatorMovementParameters(float horizontalValue, float verticalValue)
        {
            // OPTION 1
            character.animator.SetFloat("Horizontal", horizontalValue, 0.1f, Time.deltaTime);
            character.animator.SetFloat("Vertical", verticalValue, 0.1f, Time.deltaTime);
        }
    }
}
