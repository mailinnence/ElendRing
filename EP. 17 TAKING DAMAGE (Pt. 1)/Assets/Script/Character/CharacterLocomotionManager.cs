using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SG
{
    public class CharacterLocomotionManager : MonoBehaviour
    {
        CharacterManager character;

        [Header("Ground Check & Jumping")]    
        [SerializeField] protected float gravityForce = -20f;    
        [SerializeField] LayerMask groundLayer;                   // 지면 확인을 위한 레이어
        [SerializeField] float groundCheckSphereRadius = 1;       // 지면 확인 구체 반경
        [SerializeField] protected Vector3 yVelocity;             // 캐릭터의 상하 이동 속도 (점프나 낙하 시)
        [SerializeField] protected float groundedYVelocity = -20; // 캐릭터가 지면에 붙어있는 동안의 힘
        [SerializeField] protected float fallStartYVelocity = -5; // 캐릭터가 지면에서 떨어질 때 시작하는 힘
        protected bool fallingVelocityHasBeenSet = false;
        protected float inAirTimer = 0;

        protected virtual void Awake()
        {
            character = GetComponent<CharacterManager>();
        }

        protected virtual void Update()
        {

            if (Input.GetKeyDown(KeyCode.K))
            {
                Vector3 newPosition = transform.position;
                newPosition.y = 22f;
                transform.position = newPosition;
            }


            HandleGroundCheck();        // 구체에 닿았는가?

            if (character.isGrounded)
            {
                // 캐릭터가 지면에 붙어있는 경우
                if (yVelocity.y < 0)
                {
                    // 캐릭터가 점프나 상하 이동을 시도하지 않는 경우
                    inAirTimer = 0; // 공중에 있는 시간 초기화
                    fallingVelocityHasBeenSet = false; // 낙하 속도 설정되지 않음
                    yVelocity.y = groundedYVelocity; // 지면에 붙어있는 동안의 속도 설정
                }
            }
            else
            {
                // 캐릭터가 지면에서 떨어진 경우 - 점프를 했을경우(초반에는 속도가 조금 느려야 한다.)
                if (!character.isJumping && !fallingVelocityHasBeenSet)
                {
                    // 캐릭터가 점프하지 않고, 낙하 속도가 설정되지 않은 경우
                    fallingVelocityHasBeenSet = true; // 낙하 속도 설정됨
                    yVelocity.y = fallStartYVelocity; // 떨어질 때 시작하는 속도 설정
                }
           
                inAirTimer = inAirTimer + Time.deltaTime; // 공중에 있는 시간 업데이트
                character.animator.SetFloat("inAirTimer", inAirTimer);
                yVelocity.y += gravityForce * Time.deltaTime; // 중력 가속도 적용하여 속도 업데이트
               
            }

            // THERE SHOULD ALWAYS BE SOME FORCE APLLIED TO THE Y VELOCITY
            character.characterController.Move(yVelocity * Time.deltaTime);

        }

        protected void HandleGroundCheck()
        {
            character.isGrounded = Physics.CheckSphere(character.transform.position, groundCheckSphereRadius, groundLayer); // 지면 확인
        }

        // 장면 뷰(Scene View)에서 지면 확인 구체를 그립니다.
        protected void OnDrawGizmosSelected()
        {
            Gizmos.DrawSphere(character.transform.position, groundCheckSphereRadius); // 장면 보기에서 지면 확인 구체를 그림
        }
    }
}

