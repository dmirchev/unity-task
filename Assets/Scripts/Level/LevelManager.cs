using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityTask
{
    public class LevelManager : Singleton<LevelManager>
    {
        [SerializeField] private List<LevelObject> levelObjectsPrefabs;
        [SerializeField] Transform levelParent;
        [SerializeField] Transform levelGround;
        [SerializeField] private List<Collider> levelBoundColliders;

        [Header("Level Objects Lists")]
        private List<List<LevelObject>> levelObjectList;
        private List<DynamicLevelObject> dynamicLevelObjectList;
        private Transform playerLevelObjectTransform;
        public Transform Player { get { return playerLevelObjectTransform; } }

        public bool hasPlayer { get { return playerLevelObjectTransform != null; } }

        private static Vector3 LEVELTRANSFORMSCALEVECTOR = new Vector3(1, 0, 1);

        public List<LevelObject> LevelObjectsPrefabs { get { return levelObjectsPrefabs; } }

        public int selectedLevelObjectIndex;

        public static int GROUNDLAYER = 6;

        public void CreateManager()
        {
            SetLayers();

            int gridCells = GridManager.Instance.GridCells;

            levelObjectList = new List<List<LevelObject>>(gridCells);
            dynamicLevelObjectList = new List<DynamicLevelObject>();

            int lastListIndex;
            for (int i = 0; i < levelObjectList.Capacity; i++)
            {
                levelObjectList.Add(new List<LevelObject>(gridCells));
                
                lastListIndex = levelObjectList.Count-1;
                for (int j = 0; j < levelObjectList[lastListIndex].Capacity; j++)
                {
                    levelObjectList[lastListIndex].Add(null);
                    CreateLevelObject(i, j, PlayerDataManager.Instance.GetObjectIndex(i, j));
                }
            }
        }

        public void InitManager()
        {
            selectedLevelObjectIndex = -2;

            InitLevelObjects();
            UpdateLevelGround();

            GameUI.Instance.UpdateLevelUI();
        }

        public void UpdateList(int levelSize, bool levelCellsCreationDirection, int oldCellsCount, int newCellsCount)
        {
            if (levelCellsCreationDirection)
            {
                PlayerDataManager.Instance.AddCellsToLevel(oldCellsCount, newCellsCount);
                AddCells(oldCellsCount, newCellsCount);
            }
            else
            {
                PlayerDataManager.Instance.RemoveCellsFromLevel(oldCellsCount, newCellsCount);
                RemoveCells(oldCellsCount, newCellsCount);
            }

            PlayerDataManager.Instance.SetLevelSize(levelSize);
            UpdateLevelGround();

            InitLevelObjects();

            GameUI.Instance.UpdateLevelUI();
        }

        void InitLevelObjects()
        {
            for (int i = 0; i < levelObjectList.Count; i++)
                for (int j = 0; j < levelObjectList.Count; j++)
                    InitLevelObject(i, j);
        }

        public void SetObject(Vector3 hitPosition)
        {
            if (selectedLevelObjectIndex == -2) return;

            int xIndex, yIndex;

            GridManager.Instance.GetGridIndices(hitPosition, out xIndex, out yIndex);

            if (PlayerDataManager.Instance.HasObjectIndex(xIndex, yIndex))
            {
                if (selectedLevelObjectIndex != -1) return;
                
                PlayerDataManager.Instance.RemoveObjectIndex(xIndex, yIndex);

                DestroyLevelObject(xIndex, yIndex);
            }
            else
            {
                if (selectedLevelObjectIndex == -1) return;

                PlayerDataManager.Instance.SetObjectIndex(xIndex, yIndex, selectedLevelObjectIndex);

                CreateLevelObject(xIndex, yIndex, selectedLevelObjectIndex);
            }

            GameUI.Instance.UpdateLevelUI();
        }

        public void CreateLevelObject(int xIndex, int yIndex, int index)
        {
            if (index > -1 && index < levelObjectsPrefabs.Count)
            {
                if (levelObjectsPrefabs[index].GetLevelObjectType() == LevelObjectType.Player && playerLevelObjectTransform != null) return;

                LevelObject levelObjectCopy = Instantiate(
                    levelObjectsPrefabs[index],
                    levelParent
                );

                levelObjectList[xIndex][yIndex] = levelObjectCopy;

                if (levelObjectCopy.GetLevelObjectVariant() == LevelObjectVariant.Dynamic)
                {
                    dynamicLevelObjectList.Add((DynamicLevelObject) levelObjectCopy);

                    if (levelObjectCopy.GetLevelObjectType() == LevelObjectType.Player)
                        playerLevelObjectTransform = levelObjectCopy.transform;
                }

                InitLevelObject(xIndex, yIndex);
            }
        }

        void UpdateLevelGround()
        {
            levelGround.localScale = new Vector3(
                GridManager.Instance.GridSize, 
                levelGround.localScale.y, 
                GridManager.Instance.GridSize
            );
        }

        void InitLevelObject(int xIndex, int yIndex)
        {
            if (levelObjectList[xIndex][yIndex] != null)
                levelObjectList[xIndex][yIndex].InitLevelObject(
                    GridManager.Instance.GetPosition(xIndex, yIndex)
                );
        }

        public void DestroyLevelObject(int xIndex, int yIndex)
        {
            if (levelObjectList[xIndex][yIndex] == null)
            {
                return;
            }
            else
            {
                LevelObject levelObject = levelObjectList[xIndex][yIndex];

                if (levelObject.GetLevelObjectVariant() == LevelObjectVariant.Dynamic)
                    dynamicLevelObjectList.Remove((DynamicLevelObject) levelObject);
                
                if (levelObject.GetLevelObjectType() == LevelObjectType.Player)
                    playerLevelObjectTransform = null;
                
                Destroy(levelObjectList[xIndex][yIndex].gameObject);

                levelObjectList[xIndex][yIndex] = null;
            }
        }

        public void AddCells(int oldCellsCount, int newCellsCount)
        {
            for (int i = oldCellsCount; i < newCellsCount; i++)
            {
                levelObjectList.Add(new List<LevelObject>(newCellsCount));
                
                for (int j = 0; j < levelObjectList[i].Capacity; j++)
                    levelObjectList[i].Add(null);
            }

            for (int i = 0; i < oldCellsCount; i++)
            {
                for (int j = oldCellsCount; j < newCellsCount; j++)
                    levelObjectList[i].Add(null);
            }

            Display();
        }

        public void RemoveCells(int oldCellsCount, int newCellsCount)
        {
            for (int i = oldCellsCount-1; i >= newCellsCount; i--)
            {
                for (int j = 0; j < levelObjectList[i].Count; j++)
                    DestroyLevelObject(i, j);

                levelObjectList.RemoveAt(i);
            }
            
            for (int i = newCellsCount-1; i >= 0; i--)
            {
                for (int j = oldCellsCount-1; j >= newCellsCount; j--)
                {
                    DestroyLevelObject(i, j);

                    levelObjectList[i].RemoveAt(j);
                }
            }

            Display();
        }

        public void Display()
        {
            string output = "\n";
            for (int i = 0; i < levelObjectList.Count; i++)
            {
                for (int j = 0; j < levelObjectList[i].Count; j++)
                    output += levelObjectList[i][j] != null ? "+" : "-";
                
                output += "\n";
            }

            // Debug.Log(output);
        }

        public void UpdateManager()
        {
            for (int i = 0; i < PlayerDataManager.Instance.playerData.level.Count; i++)
            {
                for (int j = 0; j < PlayerDataManager.Instance.playerData.level[i].row.Count; j++)
                {
                    if (PlayerDataManager.Instance.playerData.level[i].row[j] > -1)
                        Debug.DrawRay(GridManager.Instance.GetPosition(i, j), Vector3.up * 2, Color.red);
                }
            }
            
            for (int i = 0; i < dynamicLevelObjectList.Count; i++)
                dynamicLevelObjectList[i].UpdateLevelObject();
        }

        public void FixedUpdateManager()
        {
            for (int i = 0; i < dynamicLevelObjectList.Count; i++)
                dynamicLevelObjectList[i].FixedUpdateLevelObject();
        }

        public int playerLayer;
        public int obstacleLayer;
        public int npcLayer;
        public int collectableLayer;

        void SetLayers()
        {
            playerLayer = GetLayerFromLevelObjectType(LevelObjectType.Player);
            obstacleLayer = GetLayerFromLevelObjectType(LevelObjectType.Obstacle);
            npcLayer = GetLayerFromLevelObjectType(LevelObjectType.NPC);
            collectableLayer = GetLayerFromLevelObjectType(LevelObjectType.Collectable);
        }

        public int GetLayerFromLevelObjectType(LevelObjectType levelObjectType)
        {
            return (int)levelObjectType + GROUNDLAYER;
        }

        public LevelObjectType GetLevelObjectTypeFromLayer(int layer)
        {
            return (LevelObjectType)(layer - GROUNDLAYER);
        }
    }
}