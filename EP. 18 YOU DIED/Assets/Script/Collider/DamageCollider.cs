using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SG
{

    public class DamageCollider : MonoBehaviour
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


        private void OnTriggerEnter(Collider other)
        {
            CharacterManager damageTarget = other.GetComponent<CharacterManager>();

            if (damageTarget != null)
            {
                contactPoint = other.gameObject.GetComponent<Collider>().ClosestPointOnBounds(transform.position);

                // CHECK IF WE CAN DAMAGE THIS TARGET BASED ON FRIENDLY FIRE

                // CHECK IF TARGET IS BLOCKING

                // CHECK IF TARGET IS INVULNERABLE

                DamageTarget(damageTarget);
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

