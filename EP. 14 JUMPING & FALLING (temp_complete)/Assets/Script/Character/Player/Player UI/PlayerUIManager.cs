using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

namespace SG
{
    public class PlayerUIManager : MonoBehaviour
    {
        public static PlayerUIManager instance;

        [Header("NETWORK JOIN")]
        [SerializeField] bool startGameAsClient;

        [HideInInspector] public PlayerHudUiManager playerHudUiManager;

        private void Awake()
        {
            if(instance == null)
            {
                instance  = this;
            }
            else
            {    
                Destroy(gameObject);
            }

            playerHudUiManager = GetComponentInChildren<PlayerHudUiManager>();
        }

        private void Start()
        {
            mouse.instance.mouseOff();      // 마우스 안 보이기
            DontDestroyOnLoad(gameObject);
        }

        private void Update()
        {
            if(startGameAsClient)
            {
                startGameAsClient = false;

                // WE MUST FIRST SHUT DOWN , BECAUSE WE HAVE STARTED AS A HOST DURING THE TITLE SCREEN
                NetworkManager.Singleton.Shutdown();

                // WE THEN RESTART , AS A CLIENT
                NetworkManager.Singleton.StartClient();
            }
        }

    }






}

