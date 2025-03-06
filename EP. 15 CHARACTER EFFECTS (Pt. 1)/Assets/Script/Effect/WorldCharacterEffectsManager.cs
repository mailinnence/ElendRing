using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SG
{
    // Unity Script | 1 reference
    public class WorldCharacterEffectsManager : MonoBehaviour
    {
        public static WorldCharacterEffectsManager instance;
        
        [SerializeField] List<InstantCharacterEffect> instantEffects;
        
        // Unity Message | 0 references
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
        
        // 1 reference
        private void GenerateEffectIDs()
        {
            for (int i = 0; i < instantEffects.Count; i++)
            {
                instantEffects[i].instantEffectID = i;
            }
        }
    }
}