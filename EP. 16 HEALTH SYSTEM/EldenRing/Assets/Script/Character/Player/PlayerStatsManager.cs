using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace SG
{
    public class PlayerStatsManager : CharacterStatsManager
    {
        PlayerManager player;

        public NetworkVariable<FixedString64Bytes> characterName = new NetworkVariable<FixedString64Bytes>("Character",  NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);


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
            CalculateHealthBasedOnVialityLevel(player.playerNetworkManager.viality.Value);
            CalculateStaminaBasedOnEnduranceLevel(player.playerNetworkManager.endurance.Value);

        }


    }
}
