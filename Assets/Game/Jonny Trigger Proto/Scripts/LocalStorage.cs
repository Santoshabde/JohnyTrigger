using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SNGames.JonnyTriggerProto
{
    public class LocalStorage : BaseService
    {
        private const string GAME_SAVE_POINT_DATA = "GameSavePointData";

        public GameSavePoint gameSavePoint
        {
            get
            {
                return GetSavedData<GameSavePoint>(GAME_SAVE_POINT_DATA);
            }
            set
            {
                SetSaveData(value, GAME_SAVE_POINT_DATA);
            }
        }

        #region Helper Functions
        private T GetSavedData<T>(string playerPrefStoredString) where T: class
        {
            string savedData = PlayerPrefs.GetString(playerPrefStoredString, string.Empty);

            if (string.IsNullOrEmpty(savedData))
            {
                return null;
            }
            else
            {
                return DeserializeData<T>(savedData);
            }
        }

        private void SetSaveData<T>(T dataToSave, string playerPrefStoredString)
        {
            string saveData = SerializeData<T>(dataToSave);
            PlayerPrefs.SetString(playerPrefStoredString, saveData);
        }

        private string SerializeData<T>(T dataToSerialize)
        {
            string serializeOutput = JsonUtility.ToJson(dataToSerialize);
            return serializeOutput;
        }

        public T DeserializeData<T>(string data)
        {
            return JsonUtility.FromJson<T>(data);
        }
        #endregion
    }

    [System.Serializable]
    public class GameSavePoint
    {
        public int worldNumber;
        public int regionNumber;
    }
}