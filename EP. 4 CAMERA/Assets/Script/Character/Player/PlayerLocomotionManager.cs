using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

namespace SG 
{
    public class PlayerLocomotionManager : CharacterLocomotionManager
    {
        PlayerManager player;
   
        public float verticalMovement;
        public float horizontalMovement; 
        public float moveAmount; 

        private Vector3 moveDirection;
        private Vector3 targetRotationDirection; 

        [SerializeField] float walkingSpeed = 2;
        [SerializeField] float runningSpeed = 5;
        [SerializeField] float rotationSpeed = 15;

        protected override void Awake()
        {
            base.Awake();

            player = GetComponent<PlayerManager>();
        }


        public void HandleAllMovement()
        {
            // GROUNDED MOVEMENT
            HandleGroundedMovement();
            HandleRotation();
            // AERIAL MOVEMENT
        }


        private void GetVerticalAndHorizontalInputs()
        {
            verticalMovement = PlayerIputManager.instance.verticalInput;
            horizontalMovement = PlayerIputManager.instance.horizontalInput;

            // CLAMP THE MOVEMENTS
        }



        private void HandleGroundedMovement()
        {
            GetVerticalAndHorizontalInputs();
            // OUR MOVE DIRECTION IS BASED ON OUR CAMERAS FACING PERPECTIVE & OUR MOVEMENT INPUTS
            moveDirection = PlayerCamera.instance.transform.forward * verticalMovement;                     
            moveDirection = moveDirection + PlayerCamera.instance.transform.right * horizontalMovement;
            moveDirection.Normalize(); 
            moveDirection.y = 0;
        
            if (PlayerIputManager.instance.moveAmount > 0.5f)
            {
                // MOVE AT A RUNNING SPEED
                player.characterController.Move(moveDirection * runningSpeed * Time.deltaTime);
            }
            else if(PlayerIputManager.instance.moveAmount >= 0.5f)
            {
                // MOVE AT A WALKING SPEED
                player.characterController.Move(moveDirection * walkingSpeed * Time.deltaTime);
  
            }
        }


        private void HandleRotation()
        {
            targetRotationDirection = Vector3.zero;
            targetRotationDirection = PlayerCamera.instance.cameraObject.transform.forward * verticalMovement;
            targetRotationDirection = targetRotationDirection + PlayerCamera.instance.cameraObject.transform.right * horizontalMovement;
            targetRotationDirection.Normalize();
            targetRotationDirection.y = 0;

            if(targetRotationDirection == Vector3.zero)
            {
                targetRotationDirection = transform.forward;
            }

            Quaternion newRotation = Quaternion.LookRotation(targetRotationDirection);
            Quaternion targetRotation = Quaternion.Slerp(transform.rotation, newRotation, rotationSpeed * Time.deltaTime);
            transform.rotation = targetRotation;
        }



    }

}



