using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace SG
{
    public class UI_Stat_Bar : MonoBehaviour
    {
        private Slider slider;
        private RectTransform rectTransform;

        [Header("Bar Options")]
        [SerializeField] protected bool scaleBarLengthWithStats = true;
        [SerializeField] protected float widthScaleMultiplier = 1;

        private float smoothTime = 0.2f; // 부드럽게 변하는 속도 조절

        private void Awake()
        {
            slider = GetComponent<Slider>();
            rectTransform = GetComponent<RectTransform>();
        }

        public virtual void SetStat(int newValue)
        {
            StopAllCoroutines(); // 기존에 진행 중이던 보간 중지
            StartCoroutine(SmoothChangeStat(newValue));
            slider.value = newValue;
        }

        public virtual void SetMaxStat(int maxValue)
        {
            slider.maxValue = maxValue;
            slider.value = maxValue;
            

            if(scaleBarLengthWithStats)
            {
                rectTransform.sizeDelta = new Vector2(maxValue * widthScaleMultiplier, rectTransform.sizeDelta.y);
                // RESETS THE POSITION OF THE BARS BASED ON THEIR LAYOUT GROUPLS SETTING
                PlayerUIManager.instance.playerHudUiManager.RefeshHUD();

            }

            StopAllCoroutines();
            StartCoroutine(SmoothChangeStat(maxValue));

        }

        private IEnumerator SmoothChangeStat(float targetValue)
        {
            float currentValue = slider.value;
            float elapsedTime = 0f;

            while (elapsedTime < smoothTime)
            {
                elapsedTime += Time.deltaTime;
                slider.value = Mathf.Lerp(currentValue, targetValue, elapsedTime / smoothTime);
                yield return null;
            }

            slider.value = targetValue; // 최종적으로 목표값 설정
        }



    }
}
