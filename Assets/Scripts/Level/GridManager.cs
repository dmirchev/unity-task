using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityTask
{
    public class GridManager : Singleton<GridManager>
    {
        [SerializeField] [Range(1, 50)] private int gridSize = 1;
        [SerializeField] [Range(1, 50)] public int gridCells = 1;

        public float HalfGridSize { get { return gridSize * 0.5f; } }

        public Vector3 BottomLeftCorner { get { return new Vector3(-HalfGridSize, 0, -HalfGridSize); } }
        public Vector3 TopRightCorner { get { return new Vector3(HalfGridSize, 0, HalfGridSize); } }

        public float GridCellSize { get { return (float)gridSize / gridCells; } }
        public Vector3 GridCellScale { get { return new Vector3(GridCellSize, 1, GridCellSize); } }

        int oldGridSize;
        int oldGridCells;

        public void InitManager()
        {
            gridCells = PlayerDataManager.Instance.playerData.level.Count;

            SetOldValues();
        }

        void SetOldValues()
        {
            oldGridSize = gridSize;
            oldGridCells = gridCells;
        }

        public void UpdateManager()
        {
            Vector3 pos = new Vector3(4, 0, 4);

            pos *= GridCellSize;
            pos += new Vector3(0.5f, 0, 0.5f) * GridCellSize;
            pos += BottomLeftCorner;

            Debug.DrawRay(pos, Vector3.up * 5, Color.red);

            if (gridSize != oldGridSize || gridCells != oldGridCells)
            {
                LevelManager.Instance.UpdateList(gridCells > oldGridCells, oldGridCells, gridCells);

                SetOldValues();
            }
        }

        public Vector3 GetPosition(int xIndex, int yIndex)
        {
            Vector3 centerPosition = new Vector3(xIndex, 0, yIndex);
            centerPosition *= GridCellSize;
            centerPosition += new Vector3(0.5f, 0, 0.5f) * GridCellSize;
            centerPosition += BottomLeftCorner;

            return centerPosition;
        }

        public void GetGridIndices(Vector3 worldPosition, out int xIndex, out int yIndex)
        {
            xIndex = Mathf.FloorToInt((worldPosition.x - BottomLeftCorner.x) / GridCellSize);
            yIndex = Mathf.FloorToInt((worldPosition.z - BottomLeftCorner.z) / GridCellSize);
        }

        void OnDrawGizmos()
        {
            // Borders
            Debug.DrawRay(BottomLeftCorner, Vector3.forward * gridSize, Color.green);
            Debug.DrawRay(BottomLeftCorner, Vector3.right * gridSize, Color.green);

            Debug.DrawRay(TopRightCorner, Vector3.back * gridSize, Color.green);
            Debug.DrawRay(TopRightCorner, Vector3.left * gridSize, Color.green);

            // Gird
            for (int i = 0; i < gridCells-1; i++)
            {
                Debug.DrawRay(BottomLeftCorner + (Vector3.right * GridCellSize * (i+1)), Vector3.forward * gridSize, Color.green);
                Debug.DrawRay(BottomLeftCorner + (Vector3.forward * GridCellSize * (i+1)), Vector3.right * gridSize, Color.green);
            }
        }
    }
}