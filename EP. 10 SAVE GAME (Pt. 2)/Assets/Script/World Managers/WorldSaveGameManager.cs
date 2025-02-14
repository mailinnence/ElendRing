using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


namespace SG
{

    public class WorldSaveGameManager : MonoBehaviour
    {
        
        public static WorldSaveGameManager instance;

        [SerializeField] PlayerManager player;

        [Header("SAVE/LOAD")]
        [SerializeField] bool saveGame;
        [SerializeField] bool loadGame;

        [Header("World Scene Index")]
        [SerializeField] int worldSceneIndex = 1;

        [Header("World Scene Index")]
        private SaveFileDataWriter saveFileDataWriter;

        [Header("Current Character Data")]
        public CharacterSlot currentCharacterSlotBeingUsed;
        public CharacterSaveData currentCharacterData;
        public string saveFileName;

        // 10개 제한하고 배열 쓰면 되지 않을까...
        [Header("Character Slots")]
        public CharacterSaveData CharacterSlot01;
        public CharacterSaveData CharacterSlot02;
        public CharacterSaveData CharacterSlot03;
        public CharacterSaveData CharacterSlot04;
        public CharacterSaveData CharacterSlot05;
        public CharacterSaveData CharacterSlot06;
        public CharacterSaveData CharacterSlot07;
        public CharacterSaveData CharacterSlot08;
        public CharacterSaveData CharacterSlot09;
        public CharacterSaveData CharacterSlot10;
         

        private void Awake()
        {

            // THERE CAN ONLY BE ONE INSTANCE OF THIS SCRIPT AT ONE TIME , IF ANOTHER EXISTS , DESTORY IT
            if (instance == null)
            {
                instance = this;
            }
            else
            {
                Destroy(gameObject);
            }

        }

        private void Start()
        {
            DontDestroyOnLoad(gameObject);
            LoadAllCharacterProfiles();
        }

        private void Update()
        {
            if(saveGame)
            {
                saveGame = false;
                SaveGame();
            }
            if(loadGame)
            {
                loadGame = false;
                LoadGame();
            }
        }

        public string DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(CharacterSlot characterSlot)
        {
            string filename = "";
            switch(characterSlot)
            {
                case CharacterSlot.CharacterSlot_01:
                    filename = "CharacterSlot_01";
                    break;
                case CharacterSlot.CharacterSlot_02:
                    filename = "CharacterSlot_02";
                    break;
                case CharacterSlot.CharacterSlot_03:
                    filename = "CharacterSlot_03";
                    break;
                case CharacterSlot.CharacterSlot_04:
                    filename = "CharacterSlot_04";
                    break;
                case CharacterSlot.CharacterSlot_05:
                    filename = "CharacterSlot_05";
                    break;
                case CharacterSlot.CharacterSlot_06:
                    filename = "CharacterSlot_06";
                    break; 
                case CharacterSlot.CharacterSlot_07:
                    filename = "CharacterSlot_07";
                    break;
                case CharacterSlot.CharacterSlot_08:
                    filename = "CharacterSlot_08";
                    break;
                case CharacterSlot.CharacterSlot_09:
                    filename = "CharacterSlot_09";
                    break;
                case CharacterSlot.CharacterSlot_10:
                    filename = "CharacterSlot_10";
                    break;
                default:
                    break;

            }

            return filename;
        }

        public void CreateNewGame()
        {
            // CREATE A NEW FILE, WITH A FILE NAME DEPENDING ON WHICH SLOT WE ARE USING
            saveFileName = DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(currentCharacterSlotBeingUsed);
            
            currentCharacterData = new CharacterSaveData();
        }


        public void LoadGame()
        {
            // LOAD A PREVIOUS FILE, WITH A FILE NAME DEPENDING ON WHICH SLOT WE ARE USING
            saveFileName = DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(currentCharacterSlotBeingUsed);

            saveFileDataWriter = new SaveFileDataWriter();
            // GENERALLY WORKS ON MULTIPLE MACHINE TYPES (Application.persistentDataPath)
            saveFileDataWriter.saveDataDirectoryPath = Application.persistentDataPath;
            saveFileDataWriter.saveFileName = saveFileName;
            currentCharacterData = saveFileDataWriter.LoadSaveFile();

            StartCoroutine(LoadWorldScene());
        }

        public void SaveGame()
        {
            // SAVE THE CURRENT FILE UNDER A FILE NAME DEPENDING ON WHICH SLOT WE ARE USING
            saveFileName = DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(currentCharacterSlotBeingUsed);

            saveFileDataWriter = new SaveFileDataWriter();
            // GENERALLY WORKS ON MULTIPLE MACHINE TYPES (Application.persistentDataPath)
            saveFileDataWriter.saveDataDirectoryPath = Application.persistentDataPath;
            saveFileDataWriter.saveFileName = saveFileName;

            // PASS THE PLAYERS INFO, FROM GAME, TO THEIR SAVE FILE
            player.SaveGameDataToCurrentCharacterData(ref currentCharacterData);
            
            // WRITE THAT INFO ONTO A JSON FILE, SAVED TO THIS MACHINE
            saveFileDataWriter.CreateNewCharacterSaveFile(currentCharacterData);
        }

        // 반복문으로 하면되지 않나....
        private void LoadAllCharacterProfiles()
        {
            saveFileDataWriter = new SaveFileDataWriter();
            saveFileDataWriter.saveDataDirectoryPath = Application.persistentDataPath;

            saveFileDataWriter.saveFileName = DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(CharacterSlot.CharacterSlot_01);
            CharacterSlot01 = saveFileDataWriter.LoadSaveFile();

            saveFileDataWriter.saveFileName = DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(CharacterSlot.CharacterSlot_02);
            CharacterSlot02 = saveFileDataWriter.LoadSaveFile();

            saveFileDataWriter.saveFileName = DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(CharacterSlot.CharacterSlot_03);
            CharacterSlot03 = saveFileDataWriter.LoadSaveFile();

            saveFileDataWriter.saveFileName = DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(CharacterSlot.CharacterSlot_04);
            CharacterSlot04 = saveFileDataWriter.LoadSaveFile();

            saveFileDataWriter.saveFileName = DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(CharacterSlot.CharacterSlot_05);
            CharacterSlot05 = saveFileDataWriter.LoadSaveFile();

            saveFileDataWriter.saveFileName = DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(CharacterSlot.CharacterSlot_06);
            CharacterSlot06 = saveFileDataWriter.LoadSaveFile();

            saveFileDataWriter.saveFileName = DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(CharacterSlot.CharacterSlot_07);
            CharacterSlot07 = saveFileDataWriter.LoadSaveFile();

            saveFileDataWriter.saveFileName = DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(CharacterSlot.CharacterSlot_08);
            CharacterSlot08 = saveFileDataWriter.LoadSaveFile();

            saveFileDataWriter.saveFileName = DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(CharacterSlot.CharacterSlot_09);
            CharacterSlot09 = saveFileDataWriter.LoadSaveFile();

            saveFileDataWriter.saveFileName = DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(CharacterSlot.CharacterSlot_10);
            CharacterSlot10 = saveFileDataWriter.LoadSaveFile();

        }


        public IEnumerator LoadWorldScene()
        {
            AsyncOperation loadOperation = SceneManager.LoadSceneAsync(worldSceneIndex);
       
            yield return null;
        }

        // 밖에서 불러오기 위한 함수
        public int GetWorldSceneIndex()
        {
            return worldSceneIndex;
        }



    }
}

