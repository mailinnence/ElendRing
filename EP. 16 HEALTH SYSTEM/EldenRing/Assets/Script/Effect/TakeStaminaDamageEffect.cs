using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SG
{
    // Unity의 ScriptableObject를 쉽게 생성할 수 있도록 메뉴에 추가하는 역할을
    [CreateAssetMenu(menuName = "Character Effects/Instant Effects/Take Stamina Damage")]

    public class TakeStaminaDamageEffect : InstantCharacterEffect
    {
        public float staminaDamage;
        
        public override void ProcessEffect(CharacterManager character)
        {
            CalculateStaminaDamage(character);
        }
        
        // CharacterManager를 가져와서 자신의 소유면 스테미나를 내린다.
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