using System.Collections.Generic;
using Grid;
using UnityEngine;

public class LevelGrid : Singleton<LevelGrid>
{
    [SerializeField] private Transform _gridDebugObjectPrefab;
    
    private GridSystem _gridSystem;

    protected override void Awake()
    {
        base.Awake();
        
        _gridSystem = new GridSystem(20, 20);
        _gridSystem.CreateDebugObjects(_gridDebugObjectPrefab);
    }

    public void UnitMovedGridPosition(Unit unit, GridPosition from, GridPosition to)
    {
        ClearUnitAtGridPosition(from, unit);
        AddUnitAtGridPosition(to, unit);
    }
    
    public void AddUnitAtGridPosition(GridPosition gridPosition, Unit unit) => _gridSystem.GetGridObject(gridPosition).AddUnit(unit);

    public List<Unit> GetUnitsListAtGridPosition(GridPosition gridPosition) => _gridSystem.GetGridObject(gridPosition).GetUnitList();

    public void ClearUnitAtGridPosition(GridPosition gridPosition, Unit unit) => _gridSystem.GetGridObject(gridPosition).RemoveUnit(unit);

    public GridPosition GetGridPosition(Vector3 worldPosition) => _gridSystem.GetGridPosition(worldPosition);
    
    public Vector3 GetWorldPosition(GridPosition gridPosition) => _gridSystem.GetWorldPosition(gridPosition);

    public bool IsValidGridPosition(GridPosition gridPosition) => _gridSystem.IsValidGridPosition(gridPosition);

    public bool HasAnyUnitOnGridPosition(GridPosition gridPosition) => _gridSystem.GetGridObject(gridPosition).HasAnyUnit();
    
    public Unit GetUnitAtGridPosition(GridPosition gridPosition) => _gridSystem.GetGridObject(gridPosition).GetUnit();
    
    public int GetWidth() => _gridSystem.GetWidth();

    public int GetHeight() => _gridSystem.GetHeight();
}
