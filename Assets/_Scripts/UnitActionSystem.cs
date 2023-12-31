using System;
using Actions;
using Grid;
using UnityEngine;
using UnityEngine.EventSystems;

public class UnitActionSystem : Singleton<UnitActionSystem>
{
    public event EventHandler OnSelectedUnitChanged;
    public event EventHandler OnSelectedActionChanged;
    public event EventHandler<bool> OnBusyStateChanged;
    public event EventHandler OnActionStarted;
    
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
        if (_isBusy) 
            return;

        if (!TurnSystem.Instance.IsPlayerTurn()) 
            return;
        
        if (EventSystem.current.IsPointerOverGameObject())
            return;
        
        if (TryHandleUnitSelection()) 
            return;
        
        HandleSelectedAction();
    }

    public void SetSelectedAction(BaseAction action)
    {
        _selectedAction = action;
        
        OnSelectedActionChanged?.Invoke(this, EventArgs.Empty);
    }

    public BaseAction GetSelectedAction() => _selectedAction;

    public bool IsBusy() => _isBusy;
    
    public Unit GetSelectedUnit() => _selectedUnit;

    private void HandleSelectedAction()
    {
        if (!Input.GetMouseButtonDown(0)) return;
        
        GridPosition mouseGridPosition = LevelGrid.Instance.GetGridPosition(MouseWorld.GetPosition());

        if (!_selectedAction.IsValidActionGridPosition(mouseGridPosition)) return;

        if (!_selectedUnit.TrySpendActionPoints(_selectedAction)) return;
        
        SetBusy();
        _selectedAction.TakeAction(mouseGridPosition, UnsetBusy);
        OnActionStarted?.Invoke(this, EventArgs.Empty);
    }

    private bool TryHandleUnitSelection()
    {
        if (!Input.GetMouseButtonDown(0)) return false;

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (!Physics.Raycast(ray, out RaycastHit raycastHit, float.MaxValue, _unitLayerMask))
            return false;

        if (!raycastHit.transform.TryGetComponent(out Unit unit))
            return false;

        if (unit.IsEnemy()) 
            return false;

        if (unit == _selectedUnit) 
            return false;
        
        SetSelectedUnit(unit);
        return true;
    }

    private void SetBusy()
    {
        _isBusy = true;
        OnBusyStateChanged?.Invoke(this, _isBusy);
    }

    private void UnsetBusy()
    {
        _isBusy = false;
        OnBusyStateChanged?.Invoke(this, _isBusy);
    }

    private void SetSelectedUnit(Unit unit)
    {
        _selectedUnit = unit;
        
        SetSelectedAction(unit.GetAction<MoveAction>());
        
        OnSelectedUnitChanged?.Invoke(this, EventArgs.Empty);
    }
}
