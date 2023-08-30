using System;
using Grid;
using UnityEngine;

public class UnitActionSystem : Singleton<UnitActionSystem>
{
    public event EventHandler OnSelectedUnitChanged;
    
    [SerializeField] private Unit _selectedUnit;
    [SerializeField] private LayerMask _unitLayerMask;
    private bool _isBusy;
    
    private void Update()
    {
        if (_isBusy) return;
        
        if (Input.GetMouseButtonDown(0))
        {
            if (TryHandleUnitSelection()) return;

            GridPosition mouseGridPosition = LevelGrid.Instance.GetGridPosition(MouseWorld.GetPosition());
            
            if (!_selectedUnit.GetMoveAction().IsValidActionGridPosition(mouseGridPosition)) return;
            
            SetBusy();

            _selectedUnit.GetMoveAction().Move(mouseGridPosition, UnsetBusy);
        }
        
        if (Input.GetMouseButtonDown(1))
        {
            SetBusy();
            
            _selectedUnit.GetSpinAction().Spin(UnsetBusy);
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

    private void SetBusy() => _isBusy = true;
    
    private void UnsetBusy() => _isBusy = false;
}
