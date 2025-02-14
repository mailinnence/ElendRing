using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

namespace SG 
{
    public class PlayerLocomotionManager : CharacterLocomotionManager
    {
        PlayerManager player;
   
        [HideInInspector] public float verticalMovement;
        [HideInInspector] public float horizontalMovement; 
        [HideInInspector] public float moveAmount; 

        [Header("Movement Settings")]
        private Vector3 moveDirection;
        private Vector3 targetRotationDirection; 

        [SerializeField] float walkingSpeed = 2;
        [SerializeField] float runningSpeed = 5;
        [SerializeField] float sprintingSpeed = 8.5f;
        [SerializeField] float rotationSpeed = 15;
        [SerializeField] float sprintingStaminaCost = 6f;


        [Header("Dodge")]
        private Vector3 rollDirection;
        [SerializeField] float dodgeStaminaCost = 15;

        protected override void Awake()
        {
            base.Awake();

            player = GetComponent<PlayerManager>();
        }

        protected override void Update()
        {
            base.Update();
            if(player.IsOwner)
            {
                player.characterNetworkManager.verticalMovement.Value = verticalMovement;
                player.characterNetworkManager.horizontalMovement.Value = horizontalMovement;
                player.characterNetworkManager.moveAmount.Value = moveAmount;
            }
            else
            {
                verticalMovement = player.characterNetworkManager.verticalMovement.Value;
                horizontalMovement = player.characterNetworkManager.horizontalMovement.Value;
                moveAmount = player.characterNetworkManager.moveAmount.Value;
            
                // IF NOT LOCKED ON, PASS MOVE AMOUNT
                player.playerAnimationManager.UpdateAnimatorMovementParameters(0, moveAmount, player.playerNetworkManager.isSprinting.Value);
        
        
                // IF LOCKED ON, PASS MOVE AND VERT
            }
        }

        public void HandleAllMovement()
        {
            // GROUNDED MOVEMENT
            HandleGroundedMovement();
            HandleRotation();
            
 
            // AERIAL MOVEMENT
        }

        private void GetMovementValues()
        {
            verticalMovement = PlayerIputManager.instance.verticalInput;
            horizontalMovement = PlayerIputManager.instance.horizontalInput;
            moveAmount = PlayerIputManager.instance.moveAmount;

            // CLAMP THE MOVEMENTS
        }

        private void HandleGroundedMovement()
        {
            GetMovementValues();

            if(!player.canMove)
                return;

            // OUR MOVE DIRECTION IS BASED ON OUR CAMERAS FACING PERPECTIVE & OUR MOVEMENT INPUTS
            moveDirection = PlayerCamera.instance.transform.forward * verticalMovement;                     
            moveDirection = moveDirection + PlayerCamera.instance.transform.right * horizontalMovement;
            moveDirection.Normalize(); 
            moveDirection.y = 0;
        
            if(player.playerNetworkManager.isSprinting.Value)
            {
                player.characterController.Move(moveDirection * sprintingSpeed * Time.deltaTime);
            }
            else
            {

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


        }

        private void HandleRotation()
        {
            if (!player.canRotate)
                return;
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

        public void HandelSpringting()
        {
            if(player.isPerformingAction) // 액션 중에는 못함
            {
                player.playerNetworkManager.isSprinting.Value = false;
            }

            if(player.playerNetworkManager.currentStamina.Value <= 0)
            {
                player.playerNetworkManager.isSprinting.Value = false;
                return;
            }

            // IF WE ARE MOVING, SPRINTING IS TRUE
            if(moveAmount >= 0.5f) // 뛰고 있을때 
            {
                player.playerNetworkManager.isSprinting.Value = true;
            }
            // IF WE ARE STATIONARY/MOVING SLOWLY SPRINTING IS FALSE
            else // 서있을때
            {
                player.playerNetworkManager.isSprinting.Value = false;
            }

            if(player.playerNetworkManager.isSprinting.Value)
            {
                player.playerNetworkManager.currentStamina.Value -= sprintingStaminaCost * Time.deltaTime;
            }

        }


        public void AttempToPerformDodge()
        {
            if (player.isPerformingAction)
                return;    

            if (player.playerNetworkManager.currentStamina.Value <= 0 )
                return;    


            // IF WE ARE MOVING WHEN WE ATTEMPT TO DODGE, WE PERFORM A ROLL
            if (PlayerIputManager.instance.moveAmount > 0)
            {
                rollDirection = PlayerCamera.instance.cameraObject.transform.forward * PlayerIputManager.instance.verticalInput;
                rollDirection += PlayerCamera.instance.cameraObject.transform.right * PlayerIputManager.instance.horizontalInput;
                rollDirection.y = 0;
                rollDirection.Normalize();

                Quaternion playerRotation = Quaternion.LookRotation(rollDirection);
                player.transform.rotation = playerRotation;

                player.playerAnimationManager.PlayerTargetActionAnimation("Roll_Forward_01", true, true);
            }
            // IF WE ARE STATIONARY, WE PERFORM A BACKSTEP
            else
            {
                // player.playerAnimationManager.PlayerTargetActionAnimation("Back_Step_01", true, true);
            }

            player.playerNetworkManager.currentStamina.Value -= dodgeStaminaCost;

        }


    }

}



