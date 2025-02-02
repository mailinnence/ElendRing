using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

namespace SG
{
    public class PlayerManager : CharacterManager
    {

        PlayerLocomotionManager playerLocomotionManager;    

        protected override void Awake()
        {
            base.Awake();

            // DO MORE STUFF , ONLY FOR THE PLAYER
        
            playerLocomotionManager = GetComponent<PlayerLocomotionManager>();
        
        }

        protected override void Update()
        {
            base.Update();
            // IF WE DO NOT OWN THIS GAMEOBJECT , WE DO NOT CONTROL OR EDIT IT
            if(!IsOwner)
            {
                return;

            }

            // HANDLE MOVE
            playerLocomotionManager.HandleAllMovement();
        }

        protected override void LateUpdate()
        {
            if (!IsOwner) return;
            base.LateUpdate();
            PlayerCamera.instance.HandleAllCameraActions();
        }


        // 상속한 characterMagaer 가 가지고 상속받은 네트워크 기능을 가져온다.
        // OnNetworkSpawn은 네트워크 관련 게임 개발에서 사용되는 메서드로, 네트워크 오브젝트가 "스폰"(생성)되었을 때 자동으로 호출되는 메서드
        public override void OnNetworkSpawn()
        {
            base.OnNetworkSpawn();

            // 자신의 객체만 카메라가 이동해야 하므로 자신의 객체인지 확인하는 조건문
            // IsOwner : 네트워크 프레임워크(예: Netcode for GameObjects(옛 MLAPI) 또는 Photon, Mirror)에서 제공하는 속성
            // 소유자가 자신이라면 PlayerCamera 의 player 는 this 즉 현 스크립트를 가진 객체가 된다.
            if (IsOwner)
            {
                PlayerCamera.instance.player = this;
            }

        }

    }

}


