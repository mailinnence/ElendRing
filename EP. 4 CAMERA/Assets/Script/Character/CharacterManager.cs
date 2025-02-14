using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

namespace SG
{
    public class CharacterManager : NetworkBehaviour
    {

        public CharacterController characterController; 

        CharacterNetworkManager characterNetworkManager; 

        protected virtual void Awake()
        {

            DontDestroyOnLoad(this);

            characterController = GetComponent<CharacterController>();

            characterNetworkManager = GetComponent<CharacterNetworkManager>();
        }
    
        protected virtual void Update()
        {
            // 21:00 설명 중요
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



