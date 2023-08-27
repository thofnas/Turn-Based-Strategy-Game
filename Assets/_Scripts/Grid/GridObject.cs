namespace Grid
{
    public class GridObject
    {
        private GridSystem _gridSystem;
        public GridPosition _gridPosition;

        public GridObject(GridSystem gridSystem, GridPosition gridPosition)
        {
            _gridSystem = gridSystem;
            _gridPosition = gridPosition;
        }

        public override string ToString() => $"x: {_gridPosition.X}, z: {_gridPosition.Z}";
    }
}
