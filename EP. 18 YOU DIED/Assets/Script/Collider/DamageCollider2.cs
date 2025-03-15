using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SG
{

    public class DamageCollider2 : MonoBehaviour
    {
        [Header("Damage")]
        public float physicalDamage = 0; // (TO DO, SPLIT INTO "Standard", "Strike", "Slash" and "Pierce")
        public float magicDamage = 0;
        public float fireDamage = 0;
        public float lightningDamage = 0;
        public float holyDamage = 0;


        [Header("Contact Point")]
        protected Vector3 contactPoint;


        [Header("Damage")]
        protected List<CharacterManager> charactersDamaged = new List<CharacterManager>();  


        private float lastDamageTime = 0f;
        private float damageInterval = 3f; // 3초마다 데미지 적용

        private void OnTriggerStay(Collider other)
        {
            CharacterManager damageTarget = other.GetComponent<CharacterManager>();

            if (damageTarget != null)
            {
                Debug.Log(3333);
                contactPoint = other.ClosestPoint(transform.position);

                lastDamageTime += Time.deltaTime;

                // 3초마다 데미지 적용
                if (damageInterval <= lastDamageTime)
                {
                    lastDamageTime = Time.time;
                    DamageTarget(damageTarget);
                }
            }
      
        }


        protected virtual void DamageTarget(CharacterManager damageTarget)
        {
            // WE DON'T WANT TO DAMAGE THE SAME TARGER ORE THAN ONCE IN A SINGLE ATTACK
            // SO WE ADD THEN TO A LIST THAT CHECKS BEFORE APPLYING DAMAGE

            if (charactersDamaged.Contains(damageTarget))
            {
                return;
            }

            
            charactersDamaged.Add(damageTarget);

            TakeDamageEffect damageEffect = Instantiate(WorldCharacterEffectsManager.instance.takeDamageEffect);
            damageEffect.physicalDamage = physicalDamage;
            damageEffect.magicDamage = magicDamage;
            damageEffect.fireDamage = fireDamage;
            damageEffect.holyDamage = holyDamage;
            damageEffect.contactPoint = contactPoint;

            damageTarget.characterEffectManager.ProcessInstantEffect(damageEffect);


        }



    }


}

