using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

namespace SG
{
    public class CharacterManager : NetworkBehaviour
    {

        [Header("Status")]
        public NetworkVariable<bool> isDead = new NetworkVariable<bool>(false, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

        [HideInInspector] public CharacterController characterController; 
        [HideInInspector] public Animator animator;

        [HideInInspector] public CharacterNetworkManager characterNetworkManager; 
        [HideInInspector] public CharacterEffectManager characterEffectManager; 



        [Header("Flags")]
        public bool isPerformingAction = false;
        public bool isJumping = false;  // 점프 중과 지면 상태 변수 추가
        public bool isGrounded = true;
        public bool applyRootMotion = false;
        public bool canRotate = true;
        public bool canMove = true;
        
   
        protected virtual void Awake()
        {

            DontDestroyOnLoad(this);

            characterController = GetComponent<CharacterController>();
            characterNetworkManager = GetComponent<CharacterNetworkManager>();
            characterEffectManager = GetComponent<CharacterEffectManager>();
          
            animator = GetComponent<Animator>();
      
      
        }
    
        protected virtual void Update()
        {
            animator.SetBool("isGrounded", isGrounded);
            // IF THIS CHARACTER IS BEING CONTROLLED FROM OUR SIDE, THEN ASSIGN ITS NETWORK POSITION TO THE POSITION OF OUR TRANSFORM
            if(IsOwner)
            {
                characterNetworkManager.networkPosition.Value = transform.position;
                characterNetworkManager.networkRotation.Value = transform.rotation;
         
            }
            // IF THIS CHARACTER IS BEING CONTROLLED FROM ELSE WHERE, THEN ASSIGN ITS HERE LOCALLY BY THE POSITION OF ITS NETWORK TRANSFORM
            else
            {
                // Position
                transform.position = Vector3.SmoothDamp
                (
                transform.position, 
                characterNetworkManager.networkPosition.Value, 
                ref characterNetworkManager.networkPositionVelocity, 
                characterNetworkManager.networkPositionSmoothTime
                );

                // Rotation
                transform.rotation = Quaternion.Slerp
                (
                    transform.rotation,
                    characterNetworkManager.networkRotation.Value, 
                    characterNetworkManager.networkRotationSmoothTime
                );


            }
        }
    
        protected virtual void LateUpdate()
        {
            
        }
    




    }


}



