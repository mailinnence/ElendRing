using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SG
{
    // Unity Script | 0 references
    public class InstantCharacterEffect : ScriptableObject
    {
        [Header("Effect ID")]
        public int instantEffectID;
        
        public virtual void ProcessEffect(CharacterManager character)
        {
            
        }
    }
}