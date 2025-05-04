using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SG
{
    public class PlayerInventoryManager : CharacterEquipmentManager
    {

        public WeaponItem currentRightHandWeapon;
        public WeaponItem currentLeftHandWeapon;


        [Header("Quick Slots")]
        public WeaponItem[] WeaponInRightHandSlots = new WeaponItem[3];
        public int rightHandWeaponIndex = 0;
        public WeaponItem[] WeaponInLeftHandSlots = new WeaponItem[3];
        public int leftHandWeaponIndex = 0;

    }
}


