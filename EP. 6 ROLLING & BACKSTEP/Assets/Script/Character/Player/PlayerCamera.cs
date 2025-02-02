using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

namespace SG
{
    public class PlayerCamera : MonoBehaviour
    {
        public static PlayerCamera instance;                        // 싱글톤 패톤
        public PlayerManager player;                                // 타깃할 플레이어
        public Camera cameraObject;                                 // 카메라 오브젝트를 받는다.
        [SerializeField] Transform cameraPivotTransform;

    
        [Header("Camera Settings")]
        public float cameraSmoothSpeed = 1f;                        // 캐릭터가 카메라가 있는 곳에 따라오는 시간 : 커질수록 카메라가 따라오는 시간이 느려진다.
        [SerializeField] float leftAndRightRotationSpeed = 140f;
        [SerializeField] float upAndDownRotationSpeed = 30f;
        [SerializeField] float minimumPivot = -30f;                 // 당신이 아래로 볼 수 있는 가장 낮은 지점
        [SerializeField] float maximumPivot = 60f;                  //  당신이 위로 볼 수 있는 가장 높은 지점
        [SerializeField] float cameraCollisionRadius = 0.2f;        // 카메라가 벽을 통과하지 않게 하도록 막아줄 변수
        [SerializeField] LayerMask collideWithLayers;               // 카메라가 벽을 통과하지 않게 하도록 막아줄 변수


        [Header("Camera Values")]
        public Vector3 cameraVelocity;                              // THE HIGHEST POINT YOU ARE ABLE TO LOOK UP
        public Vector3 cameraObjectPosition;                        // 카메라 충돌에 사용됨 (충돌 시 카메라 오브젝트를 이 위치로 이동시킴)
        [SerializeField] float leftAndRightLookAngle;               // 카메라가 벽을 통과하지 않게 하도록 막아줄 기본 위치 변수
        [SerializeField] float upAndDownLookAngle;                  // 카메라가 벽을 통과하지 않게 하도록 막아줄 타겟 위치 변수
        private float cameraZPosition;
        private float targetCameraZPosition;
        


        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
            else
            {
                Destroy(gameObject);
            }


        }

        private void Start()
        {
            DontDestroyOnLoad(gameObject);
            cameraZPosition = cameraObject.transform.localPosition.z;
        }

        
        public void HandleAllCameraActions()
        {
            if (player != null)
            {
                HandleFollowTarget();
                HandleRotations();
                HandleCollisons();
            }
        }


        private void HandleFollowTarget()
        {
            Vector3 targetCameraZPosition = Vector3.SmoothDamp(transform.position, player.transform.position, ref cameraVelocity, cameraSmoothSpeed * Time.deltaTime);
            transform.position = targetCameraZPosition;
        }


        private void HandleRotations()
        {
            // 락온시에는 타겟을 바라봐야 한다.
            // 그밖에는 정상적으로 카메라 전환

            // 평상시 카메라 전환
            // 좌우 전환
            leftAndRightLookAngle += (PlayerIputManager.instance.cameraHorizontalInput * leftAndRightRotationSpeed) * Time.deltaTime;
            // 상하 전환
            upAndDownLookAngle -= (PlayerIputManager.instance.cameraVerticalInput * upAndDownRotationSpeed) * Time.deltaTime;
            // 전환 제한
            upAndDownLookAngle = Mathf.Clamp(upAndDownLookAngle, minimumPivot, maximumPivot);
            
            Vector3 cameraRotation = Vector3.zero;
            Quaternion targetRotation;

            // ROTATE THIS GAMEOBJECT LEFT AND RIGHT
            cameraRotation.y = leftAndRightLookAngle;
            targetRotation = Quaternion.Euler(cameraRotation);
            transform.rotation = targetRotation;

            // ROTATE TJE PIVOT GAMEOBJECT UP AND DOWN 
            cameraRotation = Vector3.zero;
            cameraRotation.x = upAndDownLookAngle;
            targetRotation = Quaternion.Euler(cameraRotation);
            cameraPivotTransform.localRotation = targetRotation;

        }


        private void HandleCollisons()
        {
            targetCameraZPosition = cameraZPosition;

            RaycastHit hit;
            // DIRECTION DOR COLLISION CHECK
            Vector3 direction = cameraObject.transform.position - cameraPivotTransform.position;
            direction.Normalize();
            
            // WE CHECK IF THERE IS AN OBJECT IN FRONT OF OUR DESIRED DIRECTION ^ (SEE ABOVE)
            if (Physics.SphereCast(cameraPivotTransform.position, cameraCollisionRadius, direction, out hit, Mathf.Abs(targetCameraZPosition), collideWithLayers))
            {  
                // IF THERE IS , WE GET OUR DISTANCE FROM IT
                float distanceFromHitObject = Vector3.Distance(cameraPivotTransform.position, hit.point);
                targetCameraZPosition = -(distanceFromHitObject - cameraCollisionRadius);
            }
            // IF OUR TARGET POSITION IS LESS TJAN OUR COLLISION RADIUS, WE SUBTRACT OUR COLLISION RADIUS (MAKING IT SNAP BACK)
            if (Mathf.Abs(targetCameraZPosition) < cameraCollisionRadius)
            {
                targetCameraZPosition = -cameraCollisionRadius;
            }

            // WE THEN APPLY OUR FINAL POSITION USING A LERP OVER A TIME OF 0.2F
            cameraObjectPosition.z = Mathf.Lerp(cameraObject.transform.localPosition.z, targetCameraZPosition, 0.3f);
            cameraObject.transform.localPosition = cameraObjectPosition;
        }


    }
}