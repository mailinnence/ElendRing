using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


namespace SG
{
    public class UI_Character_Save_Slot : MonoBehaviour
    {

        SaveFileDataWriter saveFileWriter;

        [Header("Game Slot")]
        public CharacterSlot characterSlot;

        [Header("Character Info")]
        public TextMeshProUGUI characterName;
        public TextMeshProUGUI timedPlayed;

        private void OnEnable()
        {
            LoadSaveSlots();
        }

        private void LoadSaveSlots()
        {
            saveFileWriter = new SaveFileDataWriter();
            saveFileWriter.saveDataDirectoryPath = Application.persistentDataPath;

            // SAVE SLOT 01
            if (characterSlot == CharacterSlot.CharacterSlot_01)
            {
                saveFileWriter.saveFileName = WorldSaveGameManager.instance.DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(characterSlot);

                // IF THE FILE EXISTS, GET INFORMATION FROM IT
                if (saveFileWriter.CheckToSeeIfFileExists())
                {
                    characterName.text = WorldSaveGameManager.instance.CharacterSlot01.characterName;
                }
                // IF IT DOES NOT, DISABLE THIS GAMEOBJECT
                else
                {
                    gameObject.SetActive(false);
                }
            }

            // SAVE SLOT 02
            else if (characterSlot == CharacterSlot.CharacterSlot_02)
            {
                saveFileWriter.saveFileName = WorldSaveGameManager.instance.DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(characterSlot);

                // IF THE FILE EXISTS, GET INFORMATION FROM IT
                if (saveFileWriter.CheckToSeeIfFileExists())
                {
                    characterName.text = WorldSaveGameManager.instance.CharacterSlot02.characterName;
                }
                // IF IT DOES NOT, DISABLE THIS GAMEOBJECT
                else
                {
                    gameObject.SetActive(false);
                }
            }

            // SAVE SLOT 03
            else if (characterSlot == CharacterSlot.CharacterSlot_03)
            {
                saveFileWriter.saveFileName = WorldSaveGameManager.instance.DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(characterSlot);

                // IF THE FILE EXISTS, GET INFORMATION FROM IT
                if (saveFileWriter.CheckToSeeIfFileExists())
                {
                    characterName.text = WorldSaveGameManager.instance.CharacterSlot03.characterName;
                }
                // IF IT DOES NOT, DISABLE THIS GAMEOBJECT
                else
                {
                    gameObject.SetActive(false);
                }
            }

            // SAVE SLOT 04
            else if (characterSlot == CharacterSlot.CharacterSlot_04)
            {
                saveFileWriter.saveFileName = WorldSaveGameManager.instance.DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(characterSlot);

                // IF THE FILE EXISTS, GET INFORMATION FROM IT
                if (saveFileWriter.CheckToSeeIfFileExists())
                {
                    characterName.text = WorldSaveGameManager.instance.CharacterSlot04.characterName;
                }
                // IF IT DOES NOT, DISABLE THIS GAMEOBJECT
                else
                {
                    gameObject.SetActive(false);
                }
            }

            // SAVE SLOT 05
            else if (characterSlot == CharacterSlot.CharacterSlot_05)
            {
                saveFileWriter.saveFileName = WorldSaveGameManager.instance.DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(characterSlot);

                // IF THE FILE EXISTS, GET INFORMATION FROM IT
                if (saveFileWriter.CheckToSeeIfFileExists())
                {
                    characterName.text = WorldSaveGameManager.instance.CharacterSlot05.characterName;
                }
                // IF IT DOES NOT, DISABLE THIS GAMEOBJECT
                else
                {
                    gameObject.SetActive(false);
                }
            }

            // SAVE SLOT 06
            else if (characterSlot == CharacterSlot.CharacterSlot_06)
            {
                saveFileWriter.saveFileName = WorldSaveGameManager.instance.DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(characterSlot);

                // IF THE FILE EXISTS, GET INFORMATION FROM IT
                if (saveFileWriter.CheckToSeeIfFileExists())
                {
                    characterName.text = WorldSaveGameManager.instance.CharacterSlot06.characterName;
                }
                // IF IT DOES NOT, DISABLE THIS GAMEOBJECT
                else
                {
                    gameObject.SetActive(false);
                }
            }

            // SAVE SLOT 07
            else if (characterSlot == CharacterSlot.CharacterSlot_07)
            {
                saveFileWriter.saveFileName = WorldSaveGameManager.instance.DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(characterSlot);

                // IF THE FILE EXISTS, GET INFORMATION FROM IT
                if (saveFileWriter.CheckToSeeIfFileExists())
                {
                    characterName.text = WorldSaveGameManager.instance.CharacterSlot07.characterName;
                }
                // IF IT DOES NOT, DISABLE THIS GAMEOBJECT
                else
                {
                    gameObject.SetActive(false);
                }
            }

            // SAVE SLOT 08
            else if (characterSlot == CharacterSlot.CharacterSlot_08)
            {
                saveFileWriter.saveFileName = WorldSaveGameManager.instance.DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(characterSlot);

                // IF THE FILE EXISTS, GET INFORMATION FROM IT
                if (saveFileWriter.CheckToSeeIfFileExists())
                {
                    characterName.text = WorldSaveGameManager.instance.CharacterSlot08.characterName;
                }
                // IF IT DOES NOT, DISABLE THIS GAMEOBJECT
                else
                {
                    gameObject.SetActive(false);
                }
            }

            // SAVE SLOT 09
            else if (characterSlot == CharacterSlot.CharacterSlot_09)
            {
                saveFileWriter.saveFileName = WorldSaveGameManager.instance.DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(characterSlot);

                // IF THE FILE EXISTS, GET INFORMATION FROM IT
                if (saveFileWriter.CheckToSeeIfFileExists())
                {
                    characterName.text = WorldSaveGameManager.instance.CharacterSlot09.characterName;
                }
                // IF IT DOES NOT, DISABLE THIS GAMEOBJECT
                else
                {
                    gameObject.SetActive(false);
                }
            }

            // SAVE SLOT 10
            else if (characterSlot == CharacterSlot.CharacterSlot_10)
            {
                saveFileWriter.saveFileName = WorldSaveGameManager.instance.DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(characterSlot);

                // IF THE FILE EXISTS, GET INFORMATION FROM IT
                if (saveFileWriter.CheckToSeeIfFileExists())
                {
                    characterName.text = WorldSaveGameManager.instance.CharacterSlot10.characterName;
                }
                // IF IT DOES NOT, DISABLE THIS GAMEOBJECT
                else
                {
                    gameObject.SetActive(false);
                }
            }

        }



    }
}
