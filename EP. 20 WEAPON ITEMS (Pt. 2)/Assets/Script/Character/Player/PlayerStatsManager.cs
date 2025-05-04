using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace SG
{
    public class PlayerStatsManager : CharacterStatsManager
    {

        PlayerManager player;

        protected override void Awake()
        {
            base.Awake();

            player = GetComponent<PlayerManager>();
        }

        protected override void Start()
        {
            base.Start();

            // WHY CALCULATE THESE HERE?
            // WHEN WE MAKE A CHARACTER CREATION MENU, AND SET THE STATS DEPENDING ON THE CLASS, THIS WILL BE CALCULATED THERE
            // UNTIL THEN HOWEVER, STATS ARE NEVER CALCULATED, SO WE DO IT HERE ON START, IF A SAVE FILE EXISTS THEY WILL BE OVER WRITTEN WHEN LOADING INTO A SCENE
            int maxHealth = CalculateHealthBasedOnVitalityLevel(player.playerNetworkManager.vitality.Value);
            int maxStamina = CalculateStaminaBasedOnEnduranceLevel(player.playerNetworkManager.endurance.Value);
            
            // 소유자인 경우에만 값을 설정
            if (player.IsOwner)
            {
                // 초기 최대 값 설정
                player.playerNetworkManager.maxHealth.Value = maxHealth;
                player.playerNetworkManager.maxStamina.Value = maxStamina;
                

                player.playerNetworkManager.currentHealth.Value = maxHealth;
                player.playerNetworkManager.currentStamina.Value = maxStamina;
                // // 초기 현재 체력과 스태미나를 최대값으로 설정
                // if (player.playerNetworkManager.currentHealth.Value <= 0)
                //     player.playerNetworkManager.currentHealth.Value = maxHealth;
                
                // if (player.playerNetworkManager.currentStamina.Value <= 0)
                //     player.playerNetworkManager.currentStamina.Value = maxStamina;
            }
        }

    }
}
