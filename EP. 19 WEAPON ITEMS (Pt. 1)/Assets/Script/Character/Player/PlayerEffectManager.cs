using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SG
{
    public class PlayerEffectManager : CharacterEffectManager
    {
        [Header("Debug Delete Later")]
        [SerializeField] InstantCharacterEffect effectToTest;
        [SerializeField] TakeStaminaDamageEffect effectToTest2;
        [SerializeField] bool processEffect = false;
        
        [Header("방식 : 인스터스(1), 코드(2), 코드(3)")]
        public int num;

        // Unity Message | 0 references
        private void Update()
        {
            method(num);
        }


        void method(int num)
        {
            // 방법.1 - 인스터스를 이용
            if (processEffect && num == 1)
            {
                processEffect = false;
                // WHY ARE WE INSTANTIATING A COPY OF THIS, INSTEAD OF JUST USING IT AS IT IS?
                InstantCharacterEffect effect = Instantiate(effectToTest);
                ProcessInstantEffect(effect);
            }

            // 방법.2 - 코드 이용
            if (processEffect && num == 2)
            {
                processEffect = false;
                // WHEN WE INSTANTIATE IT, THE ORIGINAL IS NOT EFFECTED
                TakeStaminaDamageEffect effect = Instantiate(effectToTest) as TakeStaminaDamageEffect;
                effect.staminaDamage = 55;
                
                // WHEN WE DONT INSTANTIATE IT, THE ORIGINAL IS CHANGED (YOU DO NOT WANT THIS IN MOST CASES)
                //TakeStaminaDamageEffect effect = (TakeStaminaDamageEffect) effectToTest;
                //effect.staminaDamage = 55;
                ProcessInstantEffect(effect);
            }

            // 방법.3 - 코드로 인스터스 값까지 변경
            if (processEffect && num == 3)
            {
                processEffect = false;
                effectToTest2.staminaDamage = 55;
                ProcessInstantEffect(effectToTest2);
            }
        }
    }
}
