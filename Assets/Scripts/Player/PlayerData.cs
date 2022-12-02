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
        public int levelSize;
        public List<LevelRow> level;

        public void New()
        {
            levelSize = 20;
            
            int cellsCount = 10;
            level = new List<LevelRow>(cellsCount);

            int nextLevelRowIndex;
            for (int i = 0; i < level.Capacity; i++)
            {
                level.Add(new LevelRow() {
                    row = new List<int>(cellsCount)
                });
                
                nextLevelRowIndex = level.Count-1;
                for (int j = 0; j < level[nextLevelRowIndex].row.Capacity; j++)
                    level[nextLevelRowIndex].row.Add(-1);
            }
        }

        public void AddCellsToLevel(int oldCellsCount, int newCellsCount)
        {
            for (int i = oldCellsCount; i < newCellsCount; i++)
            {
                level.Add(new LevelRow() {
                    row = new List<int>(newCellsCount)
                });
                
                for (int j = 0; j < level[i].row.Capacity; j++)
                    level[i].row.Add(-1);
            }

            for (int i = 0; i < oldCellsCount; i++)
            {
                for (int j = oldCellsCount; j < newCellsCount; j++)
                    level[i].row.Add(-1);
            }

            Display();
        }

        public void RemoveCellsFromLevel(int oldCellsCount, int newCellsCount)
        {
            for (int i = oldCellsCount-1; i >= newCellsCount; i--)
            {
                level.RemoveAt(i);
            }
            
            for (int i = newCellsCount-1; i >= 0; i--)
            {
                for (int j = oldCellsCount-1; j >= newCellsCount; j--)
                {
                    level[i].row.RemoveAt(j);
                }
            }

            Display();
        }

        public void Display()
        {
            string output = "\n";
            for (int i = 0; i < level.Count; i++)
            {
                for (int j = 0; j < level[i].row.Count; j++)
                    output += level[i].row[j] > -1 ? "+" : "-";
                
                output += "\n";
            }

            // Debug.Log(output);
        }
    }
}