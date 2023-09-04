using System;
using System.Collections.Generic;
using Actions;
using Grid;
using UnityEngine;

[RequireComponent(typeof(MoveAction), typeof(HealthSystem))]
public class Unit : MonoBehaviour
{
    private const int ACTION_POINTS_MAX = 2;

    public static event EventHandler OnAnyActionPointsChanged;
    public static event EventHandler OnAnyUnitSpawned;
    public static event EventHandler OnAnyUnitDied;

    [SerializeField] private bool _isEnemy;
    private GridPosition _gridPosition;
    private HealthSystem _healthSystem;
    private MoveAction _moveAction;
    private SpinAction _spinAction;
    private BaseAction[] _actionsArray;
    private int _actionPoints = ACTION_POINTS_MAX;

    private void Awake()
    {
        _healthSystem = GetComponent<HealthSystem>();
        _moveAction = GetComponent<MoveAction>();
        _spinAction = GetComponent<SpinAction>();
        _actionsArray = GetComponents<BaseAction>();
    }

    private void Start()
    {
        _gridPosition = LevelGrid.Instance.GetGridPosition(transform.position);

        LevelGrid.Instance.AddUnitAtGridPosition(_gridPosition, this);
        
        TurnSystem.Instance.OnTurnChanged += TurnSystem_OnTurnChanged;
        _healthSystem.OnUnitDied += HealthSystem_OnUnitDied;
        
        OnAnyUnitSpawned?.Invoke(this, EventArgs.Empty);
    }

    private void OnDestroy()
    {
        TurnSystem.Instance.OnTurnChanged -= TurnSystem_OnTurnChanged;
        _healthSystem.OnUnitDied -= HealthSystem_OnUnitDied;
    }

    private void Update()
    {
        GridPosition newGridPosition = LevelGrid.Instance.GetGridPosition(transform.position);

        if (newGridPosition != _gridPosition)
        {
            GridPosition oldGridPos = _gridPosition;
            _gridPosition = newGridPosition;
            
            LevelGrid.Instance.UnitMovedGridPosition(this, oldGridPos, newGridPosition);
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

    public int GetActionPoints() => _actionPoints;

    public bool IsEnemy() => _isEnemy;

    public void Damage(int amount, Vector3 damageDealerPosition)
    {
        _healthSystem.Damage(amount, damageDealerPosition);
    }

    public Vector3 GetWorldPosition() => transform.position;

    private void SpendActionPoints(int amount)
    {
        _actionPoints -= amount;
    
        OnAnyActionPointsChanged?.Invoke(this, EventArgs.Empty);
    }
    
    private void TurnSystem_OnTurnChanged(object sender, EventArgs e)
    {
        if ((!IsEnemy() || TurnSystem.Instance.IsPlayerTurn()) && (IsEnemy() || !TurnSystem.Instance.IsPlayerTurn())) return;
        
        _actionPoints = ACTION_POINTS_MAX;
        
        OnAnyActionPointsChanged?.Invoke(this, EventArgs.Empty);
    }
    
    private void HealthSystem_OnUnitDied(object sender, Vector3 vector3)
    {
        LevelGrid.Instance.ClearUnitAtGridPosition(_gridPosition, this);
        
        Destroy(gameObject);
        
        OnAnyUnitDied?.Invoke(this, EventArgs.Empty);
    }
}
