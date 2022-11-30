using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityTask
{
    [System.Serializable]
    public class LevelRow
    {
        public List<int> row;
    }

    [System.Serializable]
    public class PlayerData
    {
        public List<LevelRow> level;

        public void New()
        {
            level = new List<LevelRow>();
        }

        public void CreateLevel(int gridCells)
        {
            level = new List<LevelRow>(gridCells);

            int nextLevelRowIndex;
            for (int i = 0; i < level.Capacity; i++)
            {
                level.Add(new LevelRow() {
                    row = new List<int>(gridCells)
                });
                
                nextLevelRowIndex = level.Count-1;
                for (int j = 0; j < level[nextLevelRowIndex].row.Capacity; j++)
                    level[nextLevelRowIndex].row.Add(-1);
            }

            Display();
        }

        public void AddCellsToLevel(int cellsDifference)
        {
            for (int i = 0; i < cellsDifference; i++)
            {
                level.Add(new LevelRow() {
                    row = new List<int>(level.Count + 1)
                });

                int nextLevelRowIndex = level.Count-1;
                for (int j = 0; j < nextLevelRowIndex; j++)
                    level[j].row.Add(-1);

                for (int j = 0; j < level[nextLevelRowIndex].row.Capacity; j++)
                    level[nextLevelRowIndex].row.Add(-1);
            }

            Display();
        }

        public void RemoveCellsFromLevel(int cellsDifference)
        {
            for (int i = 0; i < cellsDifference; i++)
            {
                level.RemoveAt(level.Count-1);

                int rowsCount = level.Count-1;
                for (int j = 0; j < level.Count; j++)
                    level[j].row.RemoveAt(rowsCount);
            }

            Display();
        }

        public void Display()
        {
            string output = "";
            for (int i = 0; i < level.Count; i++)
            {
                for (int j = 0; j < level[i].row.Count; j++)
                    output += level[i].row[j] > -1 ? "+" : "-";
                
                output += "\n";
            }

            Debug.Log(output);
        }
    }
}