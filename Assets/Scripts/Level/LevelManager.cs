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

        private List<List<Transform>> levelTransformsList;

        private static Vector3 LEVELTRANSFORMSCALEVECTOR = new Vector3(1, 0, 1);

        public List<LevelObject> LevelObjectsPrefabs { get { return levelObjectsPrefabs; } }

        public void InitManager()
        {
            int gridCells = GridManager.Instance.gridCells;

            levelTransformsList = new List<List<Transform>>(gridCells);

            int lastListIndex;
            for (int i = 0; i < levelTransformsList.Capacity; i++)
            {
                levelTransformsList.Add(new List<Transform>(gridCells));
                
                lastListIndex = levelTransformsList.Count-1;
                for (int j = 0; j < levelTransformsList[lastListIndex].Capacity; j++)
                {
                    levelTransformsList[lastListIndex].Add(null);
                    CreateLevelObject(i, j, PlayerDataManager.Instance.GetObjectIndex(i, j));
                }
            }
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

            for (int i = 0; i < levelTransformsList.Count; i++)
                for (int j = 0; j < levelTransformsList.Count; j++)
                    UpdateLevelObjectTransform(i, j);
        }

        public void SetObject(Vector3 hitPosition)
        {
            int xIndex, yIndex;

            GridManager.Instance.GetGridIndices(hitPosition, out xIndex, out yIndex);

            if (PlayerDataManager.Instance.HasObjectIndex(xIndex, yIndex))
            {
                PlayerDataManager.Instance.RemoveObjectIndex(xIndex, yIndex);

                DestroyLevelObject(xIndex, yIndex);
            }
            else
            {
                PlayerDataManager.Instance.SetObjectIndex(xIndex, yIndex, 0);

                CreateLevelObject(xIndex, yIndex, 0);
            }
        }

        public void CreateLevelObject(int xIndex, int yIndex, int index)
        {
            if (index > -1 && index < levelObjectsPrefabs.Count)
            {
                /* levelTransformsList[xIndex][yIndex] = Transform.Instantiate(
                    levelObjectsPrefabs[index],
                    levelParent
                ); */

                UpdateLevelObjectTransform(xIndex, yIndex);
            }
        }

        void UpdateLevelGround()
        {
            levelGround.localScale = GridManager.Instance.GridScale;


        }

        void UpdateLevelObjectTransform(int xIndex, int yIndex)
        {
            Transform levelObjectTransform = levelTransformsList[xIndex][yIndex];

            if (levelObjectTransform == null) return;

            levelObjectTransform.localPosition = GridManager.Instance.GetPosition(xIndex, yIndex);
            levelObjectTransform.localRotation = Quaternion.identity;
            levelObjectTransform.localScale = GridManager.Instance.GridCellScale;
        }

        public void DestroyLevelObject(int xIndex, int yIndex)
        {
            if (levelTransformsList[xIndex][yIndex] == null)
                return;
            else
                Destroy(levelTransformsList[xIndex][yIndex].gameObject);
        }

        public void AddCells(int oldCellsCount, int newCellsCount)
        {
            for (int i = oldCellsCount; i < newCellsCount; i++)
            {
                levelTransformsList.Add(new List<Transform>(newCellsCount));
                
                for (int j = 0; j < levelTransformsList[i].Capacity; j++)
                    levelTransformsList[i].Add(null);
            }

            for (int i = 0; i < oldCellsCount; i++)
            {
                for (int j = oldCellsCount; j < newCellsCount; j++)
                    levelTransformsList[i].Add(null);
            }

            Display();
        }

        public void RemoveCells(int oldCellsCount, int newCellsCount)
        {
            for (int i = oldCellsCount-1; i >= newCellsCount; i--)
            {
                for (int j = 0; j < levelTransformsList[i].Count; j++)
                    DestroyLevelObject(i, j);

                levelTransformsList.RemoveAt(i);
            }
            
            for (int i = newCellsCount-1; i >= 0; i--)
            {
                for (int j = oldCellsCount-1; j >= newCellsCount; j--)
                {
                    DestroyLevelObject(i, j);

                    levelTransformsList[i].RemoveAt(j);
                }
            }

            Display();
        }

        public void Display()
        {
            string output = "\n";
            for (int i = 0; i < levelTransformsList.Count; i++)
            {
                for (int j = 0; j < levelTransformsList[i].Count; j++)
                    output += levelTransformsList[i][j] != null ? "+" : "-";
                
                output += "\n";
            }

            Debug.Log(output);
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
        }
    }
}