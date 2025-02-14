using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SG
{ 
    public class PlayerHudUiManager : MonoBehaviour
    {
        [SerializeField] UI_Stat_Bar staminaBar;

        public void SetNewStanminaValue(float oldValue, float newValue)
        {
            staminaBar.SetStat(Mathf.RoundToInt(newValue));
        }

        public void SetMaxStaminaValue(int maxStamina)
        {
            staminaBar.SetMaxStat(maxStamina);
        }

    }
}