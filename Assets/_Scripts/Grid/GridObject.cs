using System.Collections.Generic;

namespace Grid
{
    public class GridObject
    {
        private GridSystem _gridSystem;
        private GridPosition _gridPosition;
        private List<Unit> _unitsList;

        public GridObject(GridSystem gridSystem, GridPosition gridPosition)
        {
            _gridSystem = gridSystem;
            _gridPosition = gridPosition;
            _unitsList = new List<Unit>();
        }

        public void AddUnit(Unit unit) => _unitsList.Add(unit);

        public List<Unit> GetUnitList() => _unitsList;

        public void RemoveUnit(Unit unit) => _unitsList.Remove(unit);

        public bool HasAnyUnit() => _unitsList.Count > 0;

        public Unit GetUnit() => HasAnyUnit() ? _unitsList[0] : null;

        public override string ToString()
        {
            string unitsString = "";
            
            foreach (Unit unit in _unitsList)
            {
                unitsString += unit + "\n";
            }
            
            return $"{_gridPosition}\n{unitsString}";
        }
    }
}
