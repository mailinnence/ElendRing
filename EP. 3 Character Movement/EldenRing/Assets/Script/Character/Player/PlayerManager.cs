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


    }

}


