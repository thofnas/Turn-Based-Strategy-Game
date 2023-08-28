using System;
using Grid;
using UnityEngine;

public class UnitActionSystem : Singleton<UnitActionSystem>
{
    public event EventHandler OnSelectedUnitChanged;
    
    [SerializeField] private Unit _selectedUnit;
    [SerializeField] private LayerMask _unitLayerMask;
    
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (TryHandleUnitSelection()) return;

            GridPosition mouseGridPosition = LevelGrid.Instance.GetGridPosition(MouseWorld.GetPosition());
            
            if (!GetSelectedUnit().GetMoveAction().IsValidActionGridPosition(mouseGridPosition)) return;
            
            GetSelectedUnit().GetMoveAction().Move(mouseGridPosition);
        }
    }

    private bool TryHandleUnitSelection()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (!Physics.Raycast(ray, out RaycastHit raycastHit, float.MaxValue, _unitLayerMask))
            return false;

        if (!raycastHit.transform.TryGetComponent(out Unit unit))
            return false;
        
        SetSelectedUnit(unit);
        return true;
    }

    private void SetSelectedUnit(Unit unit)
    {
        _selectedUnit = unit;
        OnSelectedUnitChanged?.Invoke(this, EventArgs.Empty);
    }

    public Unit GetSelectedUnit() => _selectedUnit;
}
