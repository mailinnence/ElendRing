using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SG
{
    [CreateAssetMenu(menuName = "Character Effects/Instant Effects/Take Stamina Damage")]
    // Unity Script | 0 references
    public class TakeStaminaDamageEffect : InstantCharacterEffect
    {
        public float staminaDamage;
        
        // 1 reference
        public override void ProcessEffect(CharacterManager character)
        {
            CalculateStaminaDamage(character);
        }
        
        // 1 reference
        public void CalculateStaminaDamage(CharacterManager character)
        {
            if (character.IsOwner)
            {
                Debug.Log("CHARACTER IS TAKING : " +staminaDamage + " STAMINA DAMAGE");
                character.characterNetworkManager.currentStamina.Value -= staminaDamage;
            }
        }
    }
}