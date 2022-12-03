using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityTask
{
    public class GridManager : Singleton<GridManager>
    {
        [SerializeField] [Range(1, 50)] private int gridSize = 1;
        [SerializeField] [Range(1, 50)] private int gridCells = 1;

        public static int GRIDMINVALUE = 1;
        public static int GRIDMAXVALUE = 50;

        public int GridSize { get { return gridSize; } }
        public int GridCells { get { return gridCells; } }

        public float HalfGridSize { get { return gridSize * 0.5f; } }
        public Vector3 GridScale { get { return new Vector3(gridSize, 1, gridSize); } }

        public Vector3 BottomLeftCorner { get { return new Vector3(-HalfGridSize, 0, -HalfGridSize); } }
        public Vector3 TopRightCorner { get { return new Vector3(HalfGridSize, 0, HalfGridSize); } }

        public float GridCellSize { get { return (float)gridSize / gridCells; } }

        int oldGridSize;
        int oldGridCells;

        public static float DEFAILTGROUNDSIZE = 20.0f;

        public void CreateManager()
        {
            gridSize = PlayerDataManager.Instance.playerData.levelSize;
            gridCells = PlayerDataManager.Instance.playerData.level.Count;

            SetOldValues();
        }

        void SetOldValues()
        {
            oldGridSize = gridSize;
            oldGridCells = gridCells;

            UpdateCamera();
        }

        public void UpdateGridSize(int newGridSize)
        {
            gridSize = newGridSize;

            UpdateLevel();

            UpdateCamera();
        }

        public void UpdateGridCells(int newGridCells)
        {
            gridCells = newGridCells;

            UpdateLevel();
        }

        void UpdateLevel()
        {
            LevelManager.Instance.UpdateList(gridSize, gridCells > oldGridCells, oldGridCells, gridCells);

            SetOldValues();
        }

        void UpdateCamera()
        {
            CameraManager.Instance.UpdateCamera(GridManager.Instance.GetGridSizeRatio());
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

        public float GetGridSizeRatio()
        {
            return gridSize / DEFAILTGROUNDSIZE;
        }

        public float GetGridSizeRatioOpposite()
        {
            return DEFAILTGROUNDSIZE / gridSize;
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