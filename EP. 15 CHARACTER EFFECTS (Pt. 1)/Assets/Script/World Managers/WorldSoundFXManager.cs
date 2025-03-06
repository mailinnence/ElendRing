using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace SG
{
    public class WorldSoundFXManager : MonoBehaviour
    {

        public static WorldSoundFXManager instance;

        [Header("Action  Sounds")]
        public AudioClip rollSFX;
                
        [Header("Walk  Sounds")]        
        public AudioClip Walk1;
        public AudioClip Walk2;
        public AudioClip Walk3;

        private void Awake()
        {
            if(instance == null)
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


    }
}

