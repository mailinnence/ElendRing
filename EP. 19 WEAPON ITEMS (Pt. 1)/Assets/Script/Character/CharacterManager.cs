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
        [HideInInspector] public CharacterAnimationManager characterAnimatorManager; 


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

            animator = GetComponent<Animator>();
            characterController = GetComponent<CharacterController>();
            characterNetworkManager = GetComponent<CharacterNetworkManager>();
            characterEffectManager = GetComponent<CharacterEffectManager>();
            characterAnimatorManager = GetComponent<CharacterAnimationManager>();
            
      
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

        public virtual IEnumerator ProcessDeathEvent(bool manuallySelectDeathAnimation = false)
        {
                // 자신의 소유일때
                if (IsOwner)
                {
                        // 자신이 살아 있으면
                        characterNetworkManager.currentHealth.Value = 0;
                        isDead.Value = true;
                        
                        // 여기에서 재설정해야 할 플래그를 초기화
                        // 아직 없음
                        
                        // 만약 지면에 서 있지 않다면, 공중 사망 애니메이션을 재생
                        if (!manuallySelectDeathAnimation)
                        {
                            characterAnimatorManager.PlayerTargetActionAnimation("Death_01", true);
                        }
                }
                
                // 사망 효과음 재생
                
                yield return new WaitForSeconds(5);
                
                // 플레이어에게 룬 보상 지급
                
                // 캐릭터 비활성화
        }

        public virtual void ReviveCharacter()
        {
            
        }


    }


}



