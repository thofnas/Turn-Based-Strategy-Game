using System;
using System.Collections.Generic;
using Actions;
using UnityEngine;

namespace Grid
{
    public class GridSystemVisual : Singleton<GridSystemVisual>
    {
        private const float RANGE_COLOR_ALPHA = 0.35f;
        
        [SerializeField] private GridSystemVisualSingle _gridSystemVisualSinglePrefab;
        private GridSystemVisualSingle[,] _gridSystemVisualSingleArray;

        protected override void Awake()
        {
            base.Awake();
            
            _gridSystemVisualSingleArray = new GridSystemVisualSingle[
                LevelGrid.Instance.GetWidth(), 
                LevelGrid.Instance.GetHeight()
                ];
            
            for (int x = 0; x < LevelGrid.Instance.GetWidth(); x++)
            {
                for (int z = 0; z < LevelGrid.Instance.GetHeight(); z++)
                {
                    GridSystemVisualSingle singleVisualTransform = Instantiate(_gridSystemVisualSinglePrefab,
                        LevelGrid.Instance.GetWorldPosition(new GridPosition(x, z)),
                        Quaternion.identity);

                    _gridSystemVisualSingleArray[x, z] = singleVisualTransform;
                }
            }
        }

        private void Start()
        {
            UnitActionSystem.Instance.OnSelectedActionChanged += UnitActionSystem_OnSelectedActionChanged;
            LevelGrid.Instance.OnAnyUnitChangedGridPosition += LevelGrid_OnAnyUnitChangedGridPosition;

            UpdateGridVisual();
        }

        private void OnDestroy()
        {
            UnitActionSystem.Instance.OnSelectedActionChanged -= UnitActionSystem_OnSelectedActionChanged;
            LevelGrid.Instance.OnAnyUnitChangedGridPosition -= LevelGrid_OnAnyUnitChangedGridPosition;
        }

        public void HideAllGridPositions()
        {
            foreach (GridSystemVisualSingle gridSystemVisualSingle in _gridSystemVisualSingleArray)
            {
                gridSystemVisualSingle.Hide();
            }  
        }
        
        public void ShowGridPositionList(List<GridPosition> gridPositions, float alpha = 1)
        {
            gridPositions.ForEach(gridPosition =>
            {
                Color currentActionColor = UnitActionSystem.Instance.GetSelectedAction().GetColorOfVisual();
                _gridSystemVisualSingleArray[gridPosition.x, gridPosition.z].Show(currentActionColor, alpha);
            });
        }
        
        private void UpdateGridVisual()
        {
            HideAllGridPositions();

            BaseAction selectedAction = UnitActionSystem.Instance.GetSelectedAction();

            if (selectedAction.HasRangeVisual())
                ShowGridPositionList(selectedAction.GetRangeGridPositionList(), RANGE_COLOR_ALPHA);

            ShowGridPositionList(selectedAction.GetValidActionGridPositionList());
        }
        
        private void UnitActionSystem_OnSelectedActionChanged(object sender, EventArgs e) => UpdateGridVisual();

        private void LevelGrid_OnAnyUnitChangedGridPosition(object sender, EventArgs e) => UpdateGridVisual();
    }
}
