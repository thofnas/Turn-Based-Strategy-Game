using Actions;
using Grid;
using UnityEngine;

[RequireComponent(typeof(MoveAction), typeof(SpinAction))]
public class Unit : MonoBehaviour
{
    private GridPosition _gridPosition;
    private MoveAction _moveAction;
    private SpinAction _spinAction;

    private void Start()
    {
        _gridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
        _moveAction = GetComponent<MoveAction>();
        _spinAction = GetComponent<SpinAction>();

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
}
