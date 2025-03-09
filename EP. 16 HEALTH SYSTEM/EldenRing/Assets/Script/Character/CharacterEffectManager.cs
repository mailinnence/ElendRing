using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SG
{
    // Unity Script | 0 references
    public class CharacterEffectManager : MonoBehaviour
    {
        // PROCESS INSTANT EFFECTS (TAKE DAMAGE, HEAL)
        
        // PROCESS TIMED EFFECTS (POISON, BUILD UPS)
        
        // PROCESS STATIC EFFECTS (ADDING/REMOVING BUFFS FROM TALISMANS ECT)
        
        CharacterManager character;

        protected virtual void Awake()
        {
            character = GetComponent<CharacterManager>();
        }

        // 0 references
        public virtual void ProcessInstantEffect(InstantCharacterEffect effect)
        {
            effect.ProcessEffect(character);
        }
    }
}