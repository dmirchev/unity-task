using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityTask
{
    public class LevelManager : Singleton<LevelManager>
    {
        [SerializeField] private List<Transform> levelTransformPrefabs;
        [SerializeField] Transform levelParent;

        private List<List<Transform>> levelTransformsList;

        private static Vector3 LEVELTRANSFORMSCALEVECTOR = new Vector3(1, 0, 1);

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

        public void UpdateList(bool levelCellsCreationDirection)
        {
            int cellsDifference;
            if (levelCellsCreationDirection)
            {
                cellsDifference = GridManager.Instance.gridCells - levelTransformsList.Count;

                PlayerDataManager.Instance.AddCellsToLevel(cellsDifference);
                AddCells(cellsDifference);
            }
            else
            {
                cellsDifference = levelTransformsList.Count - GridManager.Instance.gridCells;

                PlayerDataManager.Instance.RemoveCellsFromLevel(cellsDifference);
                RemoveCells(cellsDifference);
            }

            // for (int i = 0; i < )
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
            if (index > -1 && index < levelTransformPrefabs.Count)
            {
                Transform objectCopy = Transform.Instantiate(
                    levelTransformPrefabs[index],
                    GridManager.Instance.GetPosition(xIndex, yIndex),
                    Quaternion.identity,
                    levelParent
                );

                objectCopy.localScale = GridManager.Instance.GridCellScale;

                levelTransformsList[xIndex][yIndex] = objectCopy;
            }
        }

        public void DestroyLevelObject(int xIndex, int yIndex)
        {
            if (levelTransformsList[xIndex][yIndex] == null)
                return;
            else
                Destroy(levelTransformsList[xIndex][yIndex].gameObject);
        }

        public void AddCells(int cellsDifference)
        {
            /* for (int i = 0; i < levelTransformsList.Count; i++)
            {
                for (int j = levelTransformsList[i].Count; j < GridManager.Instance.gridCells; j++)
                    levelTransformsList[i].Add(null);
            }

            int gridCells = GridManager.Instance.gridCells;

            int lastListIndex;
            for (int i = levelTransformsList.Count; i < gridCells; i++)
            {
                levelTransformsList.Add(new List<Transform>(gridCells));
                
                lastListIndex = levelTransformsList.Count-1;
                for (int j = 0; j < levelTransformsList[lastListIndex].Capacity; j++)
                {
                    levelTransformsList[lastListIndex].Add(null);
                }
            } */

            /* for (int i = 0; i < cellsDifference; i++)
            {
                levelTransformsList.Add(new List<Transform>(levelTransformsList.Count + 1));

                int nextLevelRowIndex = levelTransformsList.Count-1;
                for (int j = 0; j < nextLevelRowIndex; j++)
                    levelTransformsList[j].Add(null);

                for (int j = 0; j < levelTransformsList[nextLevelRowIndex].Capacity; j++)
                    levelTransformsList[nextLevelRowIndex].Add(null);
            } */

            Display();
        }

        public void RemoveCells(int cellsDifference)
        {
            /* for (int i = GridManager.Instance.gridCells; i < levelTransformsList.Count; i++)
            {
                for (int j = 0; j < levelTransformsList[i].Count; j++)
                    DestroyLevelObject(i, j);

                levelTransformsList.RemoveAt(i);
                i--;
            }

            for (int i = 0; i < levelTransformsList.Count; i++)
            {
                for (int j = GridManager.Instance.gridCells; j < levelTransformsList[i].Count-1; j++)
                {
                    DestroyLevelObject(i, j);

                    levelTransformsList[i].RemoveAt(j);
                    j--;
                }
            } */

            /* for (int i = 0; i < cellsDifference; i++)
            {
                for (int j = 0; j < levelTransformsList[i].Count; j++)
                    DestroyLevelObject(i, j);
                
                levelTransformsList.RemoveAt(levelTransformsList.Count-1);

                int rowsCount = levelTransformsList.Count-1;
                for (int j = 0; j < levelTransformsList.Count; j++)
                {
                    DestroyLevelObject(i, j);
                    levelTransformsList[j].RemoveAt(rowsCount);
                }
            } */

            Display();
        }

        public void Display()
        {
            string output = "";
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