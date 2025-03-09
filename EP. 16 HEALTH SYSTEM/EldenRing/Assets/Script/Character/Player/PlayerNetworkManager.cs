using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using Unity.Collections;

namespace SG
{
    public class PlayerNetworkManager : CharacterNetworkManager
    {
        PlayerManager player;

        public NetworkVariable<FixedString64Bytes> characterName = new NetworkVariable<FixedString64Bytes>("Character",  NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);


        protected override void Awake()
        {
            base.Awake();

            player = GetComponent<PlayerManager>();
        }
   

        public void SetNewHealthValue(int oldVitality, int newVitality)
        {
            maxHealth.Value = player.playerStatsManager.CalculateHealthBasedOnVialityLevel(newVitality);
            PlayerUIManager.instance.playerHudUiManager.SetMaxHealthValue(maxHealth.Value);
            player.playerNetworkManager.currentHealth.Value = maxHealth.Value;
            currentHealth.Value = maxHealth.Value;
        }


        public void SetMaxStaminaValue(int oldEndurance, int newEndurance)
        {
            maxStamina.Value = player.playerStatsManager.CalculateStaminaBasedOnEnduranceLevel(newEndurance);
            PlayerUIManager.instance.playerHudUiManager.SetMaxStaminaValue(maxStamina.Value);
            currentStamina.Value = maxStamina.Value;
        }

    }
}
