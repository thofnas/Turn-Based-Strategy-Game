using System;
using Actions;
using Grid;
using UnityEngine;
using UnityEngine.EventSystems;

public class UnitActionSystem : Singleton<UnitActionSystem>
{
    public event EventHandler OnSelectedUnitChanged;
    public event EventHandler OnSelectedActionChanged;
    
    [SerializeField] private Unit _selectedUnit;
    [SerializeField] private LayerMask _unitLayerMask;
    private BaseAction _selectedAction;
    private bool _isBusy;

    private void Start()
    {
        SetSelectedUnit(_selectedUnit);
    }

    private void Update()
    {
        if (_isBusy) return;
        
        if (EventSystem.current.IsPointerOverGameObject()) return;
        
        if (TryHandleUnitSelection()) return;
        
        HandleSelectedAction();
    }

    private void HandleSelectedAction()
    {
        if (!Input.GetMouseButtonDown(0)) return;
        
        GridPosition mouseGridPosition = LevelGrid.Instance.GetGridPosition(MouseWorld.GetPosition());

        if (!_selectedAction.IsValidActionGridPosition(mouseGridPosition)) return;
        
        SetBusy();
        _selectedAction.DoAction(mouseGridPosition, UnsetBusy);
    }

    private bool TryHandleUnitSelection()
    {
        if (!Input.GetMouseButtonDown(0)) return false;

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (!Physics.Raycast(ray, out RaycastHit raycastHit, float.MaxValue, _unitLayerMask))
            return false;

        if (!raycastHit.transform.TryGetComponent(out Unit unit))
            return false;

        if (unit == _selectedUnit) 
            return false;
        
        SetSelectedUnit(unit);
        return true;
    }

    private void SetSelectedUnit(Unit unit)
    {
        _selectedUnit = unit;
        
        SetSelectedAction(unit.GetMoveAction());
        
        OnSelectedUnitChanged?.Invoke(this, EventArgs.Empty);
    }

    public void SetSelectedAction(BaseAction action)
    {
        _selectedAction = action;
        
        OnSelectedActionChanged?.Invoke(this, EventArgs.Empty);
    }

    public Unit GetSelectedUnit() => _selectedUnit;

    private void SetBusy() => _isBusy = true;
    
    private void UnsetBusy() => _isBusy = false;

    public BaseAction GetSelectedAction() => _selectedAction;
}
