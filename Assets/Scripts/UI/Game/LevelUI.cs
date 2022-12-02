using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UnityTask
{
    public class LevelUI : MonoBehaviour
    {
        [Header("Content")]
        [SerializeField] private GameObject contentGameObject;

        [Header("Level Objects Scroll View")]
        [SerializeField] private RectTransform levelObjectsScrollViewContentetRectTransform;

        [Header("Grid")]
        private List<FlexibleGridLayoutHolder> gridHolders;
        private List<FlexibleGridHolder> gridSeparators;
        public List<FlexibleGridElement> gridElements;

        [Header("Grid Prefabs")]
        [SerializeField] private FlexibleGridLayoutHolder gridHolderPrefab;
        [SerializeField] private FlexibleGridHolder gridSeparatorPrefab;

        [SerializeField] private FlexibleGridElement gridElementPrefab;

        [Header("Buttons")]
        [SerializeField] private FlexibleGridElement deleteButton;

        FlexibleGridElement lastSelectedFlexibleGridElement;

        [SerializeField] private Color[] buttonNotInteractableStateColors;

        [Header("Sliders")]
        [SerializeField] private Slider gridSizeSlider;
        [SerializeField] private Slider gridCellsSlider;

        public void CreateUI()
        {
            int levelObjectTypeCount = (int)LevelObjectType.Count;
            
            gridHolders = new List<FlexibleGridLayoutHolder>(levelObjectTypeCount);
            gridSeparators = new List<FlexibleGridHolder>(levelObjectTypeCount-1);
            gridElements = new List<FlexibleGridElement>(LevelManager.Instance.LevelObjectsPrefabs.Count);

            for (int i = 0; i < levelObjectTypeCount; i++)
            {
                gridHolders.Add(Instantiate(gridHolderPrefab, Vector3.zero, Quaternion.identity, levelObjectsScrollViewContentetRectTransform));

                if (i+1 < levelObjectTypeCount)
                    gridSeparators.Add(Instantiate(gridSeparatorPrefab, Vector3.zero, Quaternion.identity, levelObjectsScrollViewContentetRectTransform));
            }

            LevelObject levelObject;
            LevelObjectType levelObjectType;
            FlexibleGridElement gridElementCopy;
            for (int i = 0; i < LevelManager.Instance.LevelObjectsPrefabs.Count; i++)
            {
                levelObject = LevelManager.Instance.LevelObjectsPrefabs[i];
                levelObjectType = levelObject.GetLevelObjectType();

                gridElementCopy = Instantiate(gridElementPrefab);
                gridElementCopy.SetGridElement(levelObject.label);
                int index = i;
                gridElementCopy.SetButtonOnClick(
                    () => OnClickLevelUIButton(index), 
                    levelObjectType
                );

                gridElements.Add(gridElementCopy);

                gridHolders[(int)levelObjectType].AddElementToGrid(gridElementCopy.transform);
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

            deleteButton.SetButtonOnClick(
                () => OnClickLevelUIButton(-1),
                LevelObjectType.None
            );

            lastSelectedFlexibleGridElement = null;
        }

        public void SetContentState(bool state)
        {
            contentGameObject.SetActive(state);
        }

        public void InitUI()
        {
            InitSliders();

            UpdateUI();
        }

        void InitSliders()
        {
            gridSizeSlider.minValue = GridManager.GRIDMINVALUE;
            gridSizeSlider.maxValue = GridManager.GRIDMAXVALUE;

            gridCellsSlider.minValue = GridManager.GRIDMINVALUE;
            gridCellsSlider.maxValue = GridManager.GRIDMAXVALUE;

            gridSizeSlider.value = GridManager.Instance.GridSize;
            gridCellsSlider.value = GridManager.Instance.GridCells;

            gridSizeSlider.onValueChanged.AddListener(OnValueChangedGridSize);
            gridCellsSlider.onValueChanged.AddListener(OnValueChangedGridCells);
        }

        public void OnClickLevelUIButton(int buttonIndex)
        {
            LevelManager.Instance.selectedButtonIndex = buttonIndex;

            SetButtonsState(buttonIndex);
        }

        private void SetButtonsState(int buttonIndex)
        {
            if (lastSelectedFlexibleGridElement != null) SetButtonInteractableState(lastSelectedFlexibleGridElement, true);

            lastSelectedFlexibleGridElement = buttonIndex == -1 ? deleteButton : gridElements[buttonIndex];

            SetButtonInteractableState(lastSelectedFlexibleGridElement, false);
        }

        public void UpdateUI()
        {
            for (int i = 0; i < gridElements.Count; i++)
                SetButtonInteractableState(gridElements[i]);
            
            SetButtonInteractableState(deleteButton);

            if (lastSelectedFlexibleGridElement != null)
                SetButtonInteractableState(lastSelectedFlexibleGridElement, false);
        }

        void SetButtonInteractableState(FlexibleGridElement flexibleGridElement, bool state = true)
        {
            if (flexibleGridElement.LevelObjectType == LevelObjectType.Player)
            {
                bool hasPlayer = LevelManager.Instance.hasPlayer;
                if (hasPlayer)
                    state = false;

                if (!state)
                    flexibleGridElement.SetButtonDisabledColor(
                        GetButtonInteractableStateColor(
                            hasPlayer ? ButtonInteractableState.Disabled : ButtonInteractableState.Selected
                        )
                    );
            }

            flexibleGridElement.SetButtonState(state);
        }

        Color GetButtonInteractableStateColor(ButtonInteractableState buttonInteractableState)
        {
            return buttonNotInteractableStateColors[(int)buttonInteractableState];
        }

        enum ButtonInteractableState
        {
            Selected,
            Disabled
        }

        void OnValueChangedGridSize(float value)
        {
            GridManager.Instance.GridSize = (int)value;
        }

        void OnValueChangedGridCells(float value)
        {
            GridManager.Instance.GridCells = (int)value;
        }
    }
}