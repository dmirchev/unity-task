using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UnityTask
{
    public class LevelUI : MonoBehaviour
    {
        [Header("Level Objects Scroll View")]
        [SerializeField] private RectTransform levelObjectsScrollViewContentetRectTransform;

        [Header("Grid")]
        private List<FlexibleGridLayoutHolder> gridHolders;
        private List<FlexibleGridHolder> gridSeparators;

        [Header("Grid Prefabs")]
        [SerializeField] private FlexibleGridLayoutHolder gridHolderPrefab;
        [SerializeField] private FlexibleGridHolder gridSeparatorPrefab;

        [SerializeField] private FlexibleGridElement gridElementPrefab;

        public void CreateUI()
        {
            int levelObjectTypeCount = (int)LevelObjectType.Count;
            
            gridHolders = new List<FlexibleGridLayoutHolder>(levelObjectTypeCount);
            gridSeparators = new List<FlexibleGridHolder>(levelObjectTypeCount-1);

            for (int i = 0; i < levelObjectTypeCount; i++)
            {
                gridHolders.Add(Instantiate(gridHolderPrefab, Vector3.zero, Quaternion.identity, levelObjectsScrollViewContentetRectTransform));

                if (i+1 < levelObjectTypeCount)
                    gridSeparators.Add(Instantiate(gridSeparatorPrefab, Vector3.zero, Quaternion.identity, levelObjectsScrollViewContentetRectTransform));
            }

            LevelObject levelObject;
            FlexibleGridElement gridElementCopy;
            for (int i = 0; i < LevelManager.Instance.LevelObjectsPrefabs.Count; i++)
            {
                levelObject = LevelManager.Instance.LevelObjectsPrefabs[i];

                gridElementCopy = Instantiate(gridElementPrefab);
                gridElementCopy.SetGridElement(levelObject.uiButtonName);

                gridHolders[(int)levelObject.GetLevelObjectType()].AddElementToGrid(gridElementCopy.transform);
            }

            float scrollViewContenetHeight = 0;

            for (int i = 0; i < gridHolders.Count; i++)
                scrollViewContenetHeight += gridHolders[i].Height;

            for (int i = 0; i < gridSeparators.Count; i++)
                scrollViewContenetHeight += gridSeparators[i].Height;

            levelObjectsScrollViewContentetRectTransform.sizeDelta = new Vector2(
                levelObjectsScrollViewContentetRectTransform.sizeDelta.x,
                scrollViewContenetHeight
            );

            for (int i = 0; i < gridHolders.Count; i++)
                gridHolders[i].SetFlexibleHeight(scrollViewContenetHeight);

            for (int i = 0; i < gridSeparators.Count; i++)
                gridSeparators[i].SetFlexibleHeight(scrollViewContenetHeight);
        }
    }
}