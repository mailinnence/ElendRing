using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SG
{
    // Unity Script | 0 references
    public class MeleeWeaponDamageCollider : DamageCollider
    {
        [Header("Attacking Character")] // 공격한 캐릭터의 정보를 담는 변수
        public CharacterManager characterCausingDamage; // (When calculating damage this is used to check for attackers damage modifiers, effects etc)
    }
}
