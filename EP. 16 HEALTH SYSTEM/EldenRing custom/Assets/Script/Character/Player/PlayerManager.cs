using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using UnityEngine.SceneManagement;

namespace SG
{
    public class PlayerManager : CharacterManager
    {

        [HideInInspector] public PlayerAnimationManager playerAnimationManager;
        [HideInInspector] public PlayerLocomotionManager playerLocomotionManager;    
        [HideInInspector] public PlayerNetworkManager playerNetworkManager;
        [HideInInspector] public PlayerStatsManager playerStatsManager;


        protected override void Awake()
        {
            base.Awake();

            // DO MORE STUFF , ONLY FOR THE PLAYER
        
            playerLocomotionManager = GetComponent<PlayerLocomotionManager>();
            playerAnimationManager = GetComponent<PlayerAnimationManager>();
            playerNetworkManager = GetComponent<PlayerNetworkManager>();
            playerStatsManager = GetComponent<PlayerStatsManager>();
        }

        protected override void Update()
        {

            if (Input.GetKeyDown(KeyCode.Escape)) { ExitGame(); }


            base.Update();
            // IF WE DO NOT OWN THIS GAMEOBJECT , WE DO NOT CONTROL OR EDIT IT
            if(!IsOwner)
            {
                return;

            }

            // HANDLE MOVE
            playerLocomotionManager.HandleAllMovement();

            // REGEN STAMINA
            playerStatsManager.RegenerateStamina();
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
                PlayerIputManager.instance.player = this;
                WorldSaveGameManager.instance.player = this;
                
                playerNetworkManager.vitality.OnValueChanged += playerNetworkManager.SetNewMaxHealthValue;
                playerNetworkManager.endurance.OnValueChanged += playerNetworkManager.SetNewMaxStaminaValue;

                playerNetworkManager.currentHealth.OnValueChanged += PlayerUIManager.instance.playerHudUiManager.SetNewHealthValue;
                playerNetworkManager.currentStamina.OnValueChanged += PlayerUIManager.instance.playerHudUiManager.SetNewStanminaValue;
                playerNetworkManager.currentStamina.OnValueChanged += playerStatsManager.ResetStaminaRegenTimer;

            }

        }

        public void SaveGameDataToCurrentCharacterData(ref CharacterSaveData currentCharacterData)
        {
            currentCharacterData.sceneIndex = SceneManager.GetActiveScene().buildIndex;

            currentCharacterData.characterName = playerNetworkManager.characterName.Value.ToString();
            currentCharacterData.xPosition = transform.position.x;
            currentCharacterData.yPosition = transform.position.y;
            currentCharacterData.zPosition = transform.position.z;

            // 저장시 캐릭터의 데이터를 저장한다.
            currentCharacterData.currentHealth = playerNetworkManager.currentHealth.Value;
            currentCharacterData.currentStamina = playerNetworkManager.currentStamina.Value;

            currentCharacterData.vitality = playerNetworkManager.vitality.Value;
            currentCharacterData.endurance = playerNetworkManager.endurance.Value;

        }

        public void LoadGaneDataFromCurrentCharacterData(ref CharacterSaveData currentCharacterData)
        {
            playerNetworkManager.characterName.Value = currentCharacterData.characterName;
            Vector3 myPosition = new Vector3(currentCharacterData.xPosition, currentCharacterData.yPosition, currentCharacterData.zPosition);
            transform.position = myPosition;
            
            playerNetworkManager.vitality.Value = currentCharacterData.vitality;
            playerNetworkManager.endurance.Value = currentCharacterData.endurance;

            // THIS WILL BE MOVED WHEN SAVING AND LOADING IS ADDED
            playerNetworkManager.maxHealth.Value = playerStatsManager.CalculateHealthBasedOnVitalityLevel(playerNetworkManager.vitality.Value);
            playerNetworkManager.maxStamina.Value = playerStatsManager.CalculateStaminaBasedOnEnduranceLevel(playerNetworkManager.endurance.Value);
            
            // 이 부분을 수정 - currentHealth와 currentStamina를 올바르게 설정
            playerNetworkManager.currentHealth.Value = currentCharacterData.currentHealth;
            playerNetworkManager.currentStamina.Value = currentCharacterData.currentStamina;
            
            // 체력바와 스태미나바의 최대값을 UI에 적용
            PlayerUIManager.instance.playerHudUiManager.SetMaxHealthValue(playerNetworkManager.maxHealth.Value);
            PlayerUIManager.instance.playerHudUiManager.SetMaxStaminaValue(playerNetworkManager.maxStamina.Value);
            
            // UI 강제 새로고침
            PlayerUIManager.instance.playerHudUiManager.RefeshHUD();
      
            // 최대값을 먼저 설정
            playerNetworkManager.maxHealth.Value = playerStatsManager.CalculateHealthBasedOnVitalityLevel(playerNetworkManager.vitality.Value);
            playerNetworkManager.maxStamina.Value = playerStatsManager.CalculateStaminaBasedOnEnduranceLevel(playerNetworkManager.endurance.Value);
            
            // UI에 최대값 설정
            PlayerUIManager.instance.playerHudUiManager.SetMaxHealthValue(playerNetworkManager.maxHealth.Value);
            PlayerUIManager.instance.playerHudUiManager.SetMaxStaminaValue(playerNetworkManager.maxStamina.Value);
            
            // 잠시 대기 후 현재 값 설정 (중요: 최대값 설정 후 현재 값 설정)
            playerNetworkManager.currentHealth.Value = currentCharacterData.currentHealth;
            playerNetworkManager.currentStamina.Value = currentCharacterData.currentStamina;
            
            // 현재 값을 UI에 강제 적용
            PlayerUIManager.instance.playerHudUiManager.SetNewHealthValue(playerNetworkManager.maxHealth.Value, playerNetworkManager.currentHealth.Value);
            PlayerUIManager.instance.playerHudUiManager.SetNewStanminaValue(playerNetworkManager.maxStamina.Value, playerNetworkManager.currentStamina.Value);

        }



        public void ExitGame()
        {
            #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false; // 유니티 에디터에서 실행 중이면 플레이 모드 종료
            #else
                Application.Quit(); // 빌드된 게임에서 실행 중이면 게임 종료
            #endif
        }


    }

}


