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
        }


        private void Start()
        {
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



    // private void Update()
    // {
    //     if (startGameAsClient)
    //     {
    //         startGameAsClient = false;

    //         // 현재 호스트가 실행 중인 경우에만 종료
    //         if (NetworkManager.Singleton.IsHost)
    //         {
    //             // 현재 호스트 종료
    //             NetworkManager.Singleton.Shutdown();
    //         }

    //         // 클라이언트로 시작
    //         NetworkManager.Singleton.StartClient();
    //     }
    // }




}