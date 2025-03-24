using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SG
{
    public class WeaponItem : Item
    {
      // 애니메이터 컨트롤러 오버라이드 (현재 사용 중인 무기에 따라 공격 애니메이션을 변경)

        [Header("Weapon Model")]
        public GameObject weaponModel; // 무기 모델

        [Header("Weapon Requirements")]
        public int strengthREQ = 0; // 필요 힘 스탯
        public int dexREQ = 0;      // 필요 민첩 스탯
        public int intREQ = 0;      // 필요 지능 스탯
        public int faithREQ = 0;    // 필요 신앙 스탯

        [Header("Weapon Base Damage")]
        public int physicalDamage = 0;   // 물리 피해
        public int magicDamage = 0;      // 마법 피해
        public int fireDamage = 0;       // 화염 피해
        public int holyDamage = 0;       // 신성 피해
        public int lightningDamage = 0;  // 번개 피해


        // 무기 가드 흡수율 (막을 때 방어력)
        [Header("Weapon Poise")]
        public float poiseDamage = 10; // 슈퍼아머 관통 데미지
        // 공격 시 포이즈 보너스 (공격할 때의 슈퍼아머 보너스)

        // 무기 보정치
        // 경공격 보정치
        // 강공격 보정치
        // 치명타 피해 보정치 등등

        [Header("Stamina Costs")]
        public int baseStaminaCost = 20; // 기본 스태미나 소모
        // 달리며 공격 시 스태미나 소모 보정치
        // 경공격 시 스태미나 소모 보정치
        // 강공격 시 스태미나 소모 보정치 등등

        // 아이템 기반 액션들 (RB, RT, LB, LT)
        // 전쟁의 재 (Ash of War)
        // 막기 사운드
    }
}