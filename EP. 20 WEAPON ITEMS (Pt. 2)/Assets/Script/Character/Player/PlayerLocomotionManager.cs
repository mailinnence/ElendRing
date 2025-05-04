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
        [SerializeField] float walkingSpeed = 2;
        [SerializeField] float runningSpeed = 5;
        [SerializeField] float sprintingSpeed = 8.5f;
        [SerializeField] float rotationSpeed = 15;
        [SerializeField] float sprintingStaminaCost = 6f;
        private Vector3 moveDirection;
        private Vector3 targetRotationDirection; 

        [Header("Jump")]
        [SerializeField] float jumpHeight = 4;
        [SerializeField] float jumpStaminaCost = 15;
        [SerializeField] float jumpForwardSpeed = 5;
        [SerializeField] float fireFallSpeed = 2;
        private Vector3 jumpDirection;
        

        [Header("Dodge")]
        [SerializeField] float dodgeStaminaCost = 15;
        private Vector3 rollDirection;




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
            HandleGroundedMovement();
            HandleRotation();
            HandleJumpingMovement();
            HandleFreeFallMovement();
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
        
            if(player.playerNetworkManager.isSprinting.Value && !player.isJumping)
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

        private void HandleJumpingMovement()
        {
            if (player.isJumping)
            {
                player.characterController.Move(jumpDirection * jumpForwardSpeed * Time.deltaTime);
            }
        }

        private void HandleFreeFallMovement()
        {
            if (!player.isGrounded)
            {
                Vector3 freeFallDirection;

                freeFallDirection = PlayerCamera.instance.transform.forward * PlayerIputManager.instance.verticalInput;
                freeFallDirection = freeFallDirection + PlayerCamera.instance.transform.right * PlayerIputManager.instance.horizontalInput;
                freeFallDirection.y = 0;

                player.characterController.Move(freeFallDirection * jumpForwardSpeed * Time.deltaTime);
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

        public void AttempToPerformJump()
        {
            // IF WE ARE PERFORMING A GENERAL ACTION, WE DO NOT WANT TO ALLOW A JUMP (WILL CHANGE WHEN COMBAT IS ADDED)
            if (player.isPerformingAction)
                return;    

            // IF WE ARE OUT OF STAMINA, WE DO NOT WISH TO ALLOW A JUMP
            if (player.playerNetworkManager.currentStamina.Value <= 0 )
                return;  

            // IF WE ARE ALREADY IN A JUMP, WE DO NOT WANT TO ALLOW A JUMP AGAIN UNTIL THE CURRENT JUMP HAS FINISHED
            if (player.isJumping)
                return;    

            // IF WE ARE NOT GROUNDED, WE DO NOT WANT TO ALLOW A JUMP
            if (!player.isGrounded)
                return;    

            // 만약 우리가 무기를 양손으로 들고 있다면, 양손 점프 애니메이션을 재생하고, 그렇지 않다면 한손 점프 애니메이션을 재생한다. (추후 구현 예정)
            
            // 움직이지 않을때
            if (player.playerNetworkManager.moveAmount.Value == 0 && !player.isJumping)
            {
                player.playerAnimationManager.PlayerTargetActionAnimation("Jump_Stand", false); 
            }
            // Run 상태
            if (player.playerNetworkManager.moveAmount.Value != 0 && !player.playerNetworkManager.isSprinting.Value)
            {
                player.playerAnimationManager.PlayerTargetActionAnimation("Jump_Stand", false);
            }
            // Sprint 상태
            if (player.playerNetworkManager.moveAmount.Value != 0 && player.playerNetworkManager.isSprinting.Value)
            {
                player.playerAnimationManager.PlayerTargetActionAnimation("Jump_Sprint", false);
                JumpVector();
            }
        }
    


        public void JumpVector()
        {
            player.isJumping = true;

            player.playerNetworkManager.currentStamina.Value -= jumpStaminaCost;


            jumpDirection = PlayerCamera.instance.cameraObject.transform.forward * PlayerIputManager.instance.verticalInput;
            jumpDirection += PlayerCamera.instance.cameraObject.transform.right * PlayerIputManager.instance.horizontalInput;
            jumpDirection.y = 0;

            if (jumpDirection != Vector3.zero)
            {
                // 만약 전력 질주(Sprinting) 중이라면, 점프 방향(jumpDirection)은 전체 거리의 1/4로 조정된다.
                if (player.playerNetworkManager.isSprinting.Value)
                {
                    jumpDirection *= 0.25f;
                }
                // 만약 달리기(Running) 중이라면, 점프 방향은 전체 거리의 1/4로 조정된다.
                else if (PlayerIputManager.instance.moveAmount > 0.5)
                {
                    jumpDirection *= 0.25f;
                }
                // 만약 걷기(Walking) 중이라면, 점프 방향은 전체 거리의 1/4로 조정된다.
                else if (PlayerIputManager.instance.moveAmount <= 0.5)
                {
                    jumpDirection *= 0.25f;
                }
            }
        }



        public void AttempJumpingVelocity()
        {
            yVelocity.y = Mathf.Sqrt(jumpHeight * -2 * gravityForce);
        }

    }

}



