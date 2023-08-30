using System.Collections.Generic;
using Actions;
using Grid;
using UnityEngine;

[RequireComponent(typeof(MoveAction))]
public class Unit : MonoBehaviour
{
    private GridPosition _gridPosition;
    private MoveAction _moveAction;
    private SpinAction _spinAction;
    private BaseAction[] _actionsArray;
    private int _actionPoints = 2;

    private void Awake()
    {
        _moveAction = GetComponent<MoveAction>();
        _spinAction = GetComponent<SpinAction>();
        _actionsArray = GetComponents<BaseAction>();
    }

    private void Start()
    {
        _gridPosition = LevelGrid.Instance.GetGridPosition(transform.position);

        LevelGrid.Instance.AddUnitAtGridPosition(_gridPosition, this);
    }

    private void Update()
    {
        GridPosition newGridPosition = LevelGrid.Instance.GetGridPosition(transform.position);

        if (newGridPosition != _gridPosition)
        {
            LevelGrid.Instance.UnitMovedGridPosition(this, _gridPosition, newGridPosition);
            _gridPosition = newGridPosition;
        }
    }
    
    public MoveAction GetMoveAction() => _moveAction;

    public SpinAction GetSpinAction() => _spinAction;

    public GridPosition GetGridPosition() => _gridPosition;

    public IEnumerable<BaseAction> GetActionsArray() => _actionsArray;

    public bool TrySpendActionPoints(BaseAction action)
    {
        if (!CanSpendActionPointsOnAction(action)) return false;
        
        SpendActionPoints(action.GetActionPointsCost());
        return true;
    }

    public bool CanSpendActionPointsOnAction(BaseAction action) => _actionPoints >= action.GetActionPointsCost();

    private void SpendActionPoints(int amount) => _actionPoints -= amount;

    public int GetActionPoints() => _actionPoints;
}
