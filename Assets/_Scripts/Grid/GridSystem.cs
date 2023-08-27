using UnityEngine;

namespace Grid
{
    public class GridSystem
    {
        private readonly int _width;
        private readonly int _height;
        private readonly float _cellSize;
        private readonly GridObject[,] _gridObjectArray;
    
        public GridSystem(int width, int height, float cellSize = 2f)
        {
            _width = width;
            _height = height;
            _cellSize = cellSize;

            _gridObjectArray = new GridObject[width, height];
        
            for (int x = 0; x < width; x++)
            {
                for (int z = 0; z < height; z++)
                {
                    GridPosition gridPosition = new(x, z);
                    _gridObjectArray[x, z] = new GridObject(this, gridPosition);
                }
            }
        }

        public Vector3 GetWorldPosition(GridPosition gridPosition) =>
            new Vector3(gridPosition.X, 0, gridPosition.Z) * _cellSize;

        public GridPosition GetGridPosition(Vector3 worldPosition) => new(
            Mathf.RoundToInt(worldPosition.x / _cellSize),
            Mathf.RoundToInt(worldPosition.z / _cellSize));

        public void CreateDebugObjects(Transform debugPrefab)
        {
            for (int x = 0; x < _width; x++)
            {
                for (int z = 0; z < _height; z++)
                {
                    var gridPosition = new GridPosition(x, z);
                    Transform debugTransform = Object.Instantiate(debugPrefab, GetWorldPosition(gridPosition), Quaternion.identity);
                    var gridDebugObject = debugTransform.GetComponent<GridDebugObject>();
                    
                    gridDebugObject.SetGridObject(GetGridObject(gridPosition));
                }
            }
        }

        public GridObject GetGridObject(GridPosition gridPosition) => 
            _gridObjectArray[gridPosition.X, gridPosition.Z];
    }
}
