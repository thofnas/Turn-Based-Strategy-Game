using System.Collections.Generic;
using Grid;
using UnityEngine;

public class MoveAction : MonoBehaviour
{
    private const float TOLERANCE = 0.1f;
    private const float MOVE_SPEED = 4f;
    private const float ROTATE_SPEED = 10f;

    
    private static readonly int IsWalking = Animator.StringToHash(nameof(IsWalking));
    
    [SerializeField] private int _maxMoveDistance = 4;
    [SerializeField] private Animator _unitAnimator;
    private Vector3 _targetPosition;
    private Unit _unit;

    private void Awake()
    {
        _targetPosition = transform.position;
        _unit = GetComponent<Unit>();
    }

    private void Update()
    {
        if (Vector3.Distance(transform.position, _targetPosition) <= TOLERANCE)
        {
            transform.position = _targetPosition;
            _unitAnimator.SetBool(IsWalking, false);
            return;
        }

        Vector3 moveDirection = (_targetPosition - transform.position).normalized;
        transform.position += moveDirection * (Time.deltaTime * MOVE_SPEED);
        transform.forward = Vector3.Lerp(transform.forward, moveDirection, Time.deltaTime * ROTATE_SPEED);
        
        _unitAnimator.SetBool(IsWalking, true);
    }
    
    public void Move(GridPosition gridPosition) => 
        _targetPosition = LevelGrid.Instance.GetWorldPosition(gridPosition);

    public bool IsValidActionGridPosition(GridPosition gridPosition) => 
        GetValidActionGridPositionList().Contains(gridPosition);

    public List<GridPosition> GetValidActionGridPositionList()
    {
        List<GridPosition> validGridPositionList = new();

        GridPosition unitGridPosition = _unit.GetGridPosition();

        for (int x = -_maxMoveDistance; x <= _maxMoveDistance; x++)
        {
            for (int z = -_maxMoveDistance; z <= _maxMoveDistance; z++)
            {
                GridPosition offsetGridPosition = new(x, z);
                GridPosition testGridPosition = unitGridPosition + offsetGridPosition;

                // if not inside the grid bounds
                if (!LevelGrid.Instance.IsValidGridPosition(testGridPosition)) continue;
                
                // if same position as where unit is standing
                if (unitGridPosition == testGridPosition) continue;
                
                // if gridPosition already occupied by any unit
                if (LevelGrid.Instance.HasAnyUnitOnGridPosition(testGridPosition)) continue;
                
                validGridPositionList.Add(testGridPosition);
            }
        }
        
        return validGridPositionList;
    }
}