using System.Collections.Generic;
using UnityEngine;

namespace Grid
{
    public class GridSystemVisual : Singleton<GridSystemVisual>
    {
        [SerializeField] private GridSystemVisualSingle _gridSystemVisualSinglePrefab;

        private GridSystemVisualSingle[,] _gridSystemVisualSingleArray;

        private void Start()
        {
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

        private void Update()
        {
            UpdateGridVisual();
        }

        public void HideAllGridPositions()
        {
            foreach (GridSystemVisualSingle gridSystemVisualSingle in _gridSystemVisualSingleArray)
            {
                gridSystemVisualSingle.Hide();
            }  
        }
        
        public void ShowGridPositionList(List<GridPosition> gridPositions)
        {
            gridPositions.ForEach(gridPosition => _gridSystemVisualSingleArray[gridPosition.x, gridPosition.z].Show());
        }

        private void UpdateGridVisual()
        {
            HideAllGridPositions();
            ShowGridPositionList(UnitActionSystem.Instance.GetSelectedAction().GetValidActionGridPositionList());
        }
    }
}
