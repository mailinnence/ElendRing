using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SG
{

    public class WorldCharacterEffectsManager : MonoBehaviour
    {
        // 싱글톤
        public static WorldCharacterEffectsManager instance;
        
        // 즉시 발생하는
        [SerializeField] List<InstantCharacterEffect> instantEffects;
        
   
        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
            
            GenerateEffectIDs();
        }
        
        
        private void GenerateEffectIDs()
        {
            for (int i = 0; i < instantEffects.Count; i++)
            {
                instantEffects[i].instantEffectID = i;
            }
        }
    }
}