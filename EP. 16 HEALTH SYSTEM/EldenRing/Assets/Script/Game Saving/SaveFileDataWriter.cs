using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;

namespace SG
{

    public class SaveFileDataWriter
    {
        public string saveDataDirectoryPath = "";
        public string saveFileName = "";

        // 새로운 저장 파일을 생성하기 전에, 해당 캐릭터 슬롯이 이미 존재하는지 확인해야 합니다 (최대 10개의 캐릭터 슬롯).
        public bool CheckToSeeIfFileExists()
        {
            if (File.Exists(Path.Combine(saveDataDirectoryPath, saveFileName)))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        // 캐릭터의 저장 파일을 삭제하는 데 사용
        public void DeleteSaveFile()
        {
            File.Delete(Path.Combine(saveDataDirectoryPath,saveFileName));
        }

        // 새로운 캐릭터 저장 파일 만들기
        public void CreateNewCharacterSaveFile(CharacterSaveData characterData)
        {
            // 파일을 저장할 경로를 생성합니다 (컴퓨터의 특정 위치).
            string savePath = Path.Combine(saveDataDirectoryPath, saveFileName);

            try
            {
                // 파일이 저장될 디렉터리가 존재하지 않는다면 생성합니다.
                Directory.CreateDirectory(Path.GetDirectoryName(savePath));
                Debug.Log("CREATING SAVE FILE, AT SAVE PATH: " + savePath);

                // C# 게임 데이터 객체를 JSON으로 직렬화합니다.
                string dataToStore = JsonUtility.ToJson(characterData, true);

                // JSON 파일을 시스템에 저장합니다.
                using (FileStream stream = new FileStream(savePath, FileMode.Create))
                {
                    using (StreamWriter fileWriter = new StreamWriter(stream))
                    {
                        fileWriter.Write(dataToStore);
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.LogError("ERROR WHILST TRYING TO SAVE CHARACTER DATA, GAME NOT SAVED" + savePath + "\n" + ex);
            }
        }

        // 이전 게임을 불러올 때 저장 파일을 로드하는 데 사용됨
        public CharacterSaveData LoadSaveFile()
        {
            CharacterSaveData characterData = null;

            // MAKE A PATH TO LOAD THE FILE (A LOCATION ON THE MACHINE)
            string loadPath = Path.Combine(saveDataDirectoryPath, saveFileName);

            if (File.Exists(loadPath))
            {
                try
                {
                    string dataToLoad = "";

                    using (FileStream stream = new FileStream(loadPath, FileMode.Open))
                    {
                        using (StreamReader reader = new StreamReader(stream))
                        {
                            dataToLoad = reader.ReadToEnd();
                        }
                    }

                    // DESERIALIZE THE DATA FROM JSON BACK TO UNITY
                    characterData = JsonUtility.FromJson<CharacterSaveData>(dataToLoad);
                }
                catch (Exception ex)
                {
                    Debug.LogError("Error loading character data: " + ex.Message);
                }

            }

            return characterData;
        }

    }

}