using System;
using System.Collections.Generic;
using UnityEngine;

namespace Grid
{
    public class GridSystemVisual : Singleton<GridSystemVisual>
    {
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

        private void ShowGridPositionRange(GridPosition gridPosition, int range)
        {
            List<GridPosition> gridPositions = new();
            
            for (int x = -range; x < range; x++)
            {
                for (int z = -range; z < range; z++)
                {
                    GridPosition testGridPosition = gridPosition + new GridPosition(x, z);

                    if (!LevelGrid.Instance.IsValidGridPosition(testGridPosition)) 
                        continue;
                    
                    float testDistance = Mathf.Sqrt(x * x + z * z);

                    if (Mathf.Floor(testDistance) > range)
                        continue;
                    
                    gridPositions.Add(testGridPosition);
                }
            }

            ShowGridPositionList(gridPositions, 0.5f);
        }
        
        private void UpdateGridVisual()
        {
            HideAllGridPositions();

            switch (UnitActionSystem.Instance.GetSelectedAction())
            {
                case ShootAction shootAction:
                    ShowGridPositionRange(UnitActionSystem.Instance.GetSelectedUnit().GetGridPosition(),shootAction.GetMaxShootDistance());
                    break;
            }
            
            ShowGridPositionList(UnitActionSystem.Instance.GetSelectedAction().GetValidActionGridPositionList());
        }
        
        private void UnitActionSystem_OnSelectedActionChanged(object sender, EventArgs e) => UpdateGridVisual();

        private void LevelGrid_OnAnyUnitChangedGridPosition(object sender, EventArgs e) => UpdateGridVisual();
    }
}
