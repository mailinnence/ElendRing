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
                // 코드에서 Instantiate는 **effectToTest**라는 InstantCharacterEffect의 인스턴스를 복사본으로 만든다.
                // as 키워드를 사용하여 InstantCharacterEffect를 TakeStaminaDamageEffect 타입으로 안전하게 변환
                // 상속받았기에 생길 수 있는 오류 방지 
                TakeStaminaDamageEffect effect = Instantiate(effectToTest) as TakeStaminaDamageEffect;
                effect.staminaDamage = 55;
                ProcessInstantEffect(effect);
            }

            // 방법.3 - 코드로 인스터스 값까지 변경 
            if (processEffect && num == 3)
            {
                processEffect = false;
                effectToTest2.staminaDamage = 55;
                ProcessInstantEffect(effectToTest2); // 직접 변경
            }
        }
    }
}
