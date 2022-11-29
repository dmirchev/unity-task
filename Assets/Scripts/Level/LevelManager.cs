using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityTask
{
    public class LevelManager : Singleton<LevelManager>
    {
        [SerializeField] private List<List<int>> levelObjectsList = new List<List<int>>();

        void Start()
        {
            UpdateList(GridManager.Instance.gridCells);
        }

        public void UpdateList(int gridCells)
        {
            levelObjectsList = new List<List<int>>(gridCells);
            
            for (int i = 0; i < levelObjectsList.Capacity; i++)
            {
                levelObjectsList.Add(new List<int>(gridCells));
                
                for (int j = 0; j <levelObjectsList[levelObjectsList.Count-1].Capacity; j++)
                    levelObjectsList[levelObjectsList.Count-1].Add(0);
            }

            for (int i = 0; i < levelObjectsList.Count; i++)
            {
                for (int j = 0; j < levelObjectsList[i].Count; j++)
                {
                    Debug.DrawRay(GridManager.Instance.GetPosition(i, j), Vector3.up * 2, Color.red);
                }
            }
        }

        public void SetObject(Vector3 hitPosition)
        {
            int xIndex, yIndex;

            GridManager.Instance.GetGridIndices(hitPosition, out xIndex, out yIndex);

            levelObjectsList[xIndex][yIndex] = 1;
        }

        void Update()
        {
            for (int i = 0; i < levelObjectsList.Count; i++)
            {
                for (int j = 0; j < levelObjectsList[i].Count; j++)
                {
                    if (levelObjectsList[i][j] != 0)
                        Debug.DrawRay(GridManager.Instance.GetPosition(i, j), Vector3.up * 2, Color.red);
                }
            }
        }
    }
}