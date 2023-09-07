using System;
using System.Collections.Generic;
using Grid;
using UnityEngine;

public class LevelGrid : Singleton<LevelGrid>
{
    public event EventHandler OnAnyUnitChangedGridPosition;
    
    [SerializeField] private Transform _gridDebugObjectPrefab;
    [SerializeField] private int _width = 10;
    [SerializeField] private int _height = 10;
    [SerializeField] private float _cellSize = 2f;
    private GridSystem<GridObject> _gridSystem;

    protected override void Awake()
    {
        base.Awake();
        
        _gridSystem = new GridSystem<GridObject>(_width, _height, _cellSize, 
            (gridSystem, gridPosition) => new GridObject(gridSystem, gridPosition));
        //_gridSystem.CreateDebugObjects(_gridDebugObjectPrefab);
    }

    private void Start()
    {
        Pathfinding.Instance.Setup(_width, _width, _cellSize);
    }

    public void UnitMovedGridPosition(Unit unit, GridPosition from, GridPosition to)
    {
        ClearUnitAtGridPosition(from, unit);
        AddUnitAtGridPosition(to, unit);
        
        OnAnyUnitChangedGridPosition?.Invoke(this, EventArgs.Empty);
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
