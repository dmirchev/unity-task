using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace UnityTask
{
    public class PlayerDataManager : Singleton<PlayerDataManager>
    {
        public PlayerData playerData;

        static readonly string jsonSaveFile = "save.json";
        static string jsonPath = "";

        public void Create(Action onCompleteAction = null)
        {
            playerData = new PlayerData();

            jsonPath = Path.Combine(Application.persistentDataPath, jsonSaveFile);

            if(File.Exists(jsonPath))
            {
                Read(onCompleteAction);
            }
            else
            {
                NewSave(() => 
                    Read(onCompleteAction)
                );
            }
        }

        private void NewSave(Action onCompleteAction = null)
        {
            playerData.New();

            Save(onCompleteAction);
        }

        public void Save(Action onCompleteAction = null)
        {
            string json = JsonUtility.ToJson(playerData, true);

            //Serializing
            File.WriteAllText(jsonPath, json);

            // Completed
            onCompleteAction?.Invoke();
        }

        public void Read(Action onCompleteAction = null)
        {
            string jsonFromFile = File.ReadAllText(jsonPath);

            //Deserializing
            playerData = JsonUtility.FromJson<PlayerData>(jsonFromFile);

            // Completed
            onCompleteAction?.Invoke();
        }

        // Level
        public void SetLevelSize(int gridSize)
        {
            playerData.levelSize = gridSize;
        }

        public void AddCellsToLevel(int oldCellsCount, int newCellsCount)
        {
            playerData.AddCellsToLevel(oldCellsCount, newCellsCount);

            Save();
        }

        public void RemoveCellsFromLevel(int oldCellsCount, int newCellsCount)
        {
            playerData.RemoveCellsFromLevel(oldCellsCount, newCellsCount);

            Save();
        }

        public bool HasObjectIndex(int xIndex, int yIndex)
        {
            return playerData.level[xIndex].row[yIndex] > -1;
        }

        public void SetObjectIndex(int xIndex, int yIndex, int objectIndex)
        {
            playerData.level[xIndex].row[yIndex] = objectIndex;

            Save();
        }

        public void RemoveObjectIndex(int xIndex, int yIndex)
        {
            playerData.level[xIndex].row[yIndex] = -1;

            Save();
        }

        public int GetObjectIndex(int xIndex, int yIndex)
        {
            return playerData.level[xIndex].row[yIndex];
        }
    }
}