using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


namespace SG
{

    public class WorldSaveGameManager : MonoBehaviour
    {
        
        public static WorldSaveGameManager instance;

        [SerializeField] int worldSceneIndex = 1;

        private void Awake()
        {

            // THERE CAN ONLY BE ONE INSTANCE OF THIS SCRIPT AT ONE TIME , IF ANOTHER EXISTS , DESTORY IT
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
        }


        public IEnumerator LoadNewGame()
        {
            AsyncOperation loadOperation = SceneManager.LoadSceneAsync(worldSceneIndex);
       
            yield return null;
        }




        // 밖에서 불러오기 위한 함수
        public int GetWorldSceneIndex()
        {
            return worldSceneIndex;
        }
    }
}

