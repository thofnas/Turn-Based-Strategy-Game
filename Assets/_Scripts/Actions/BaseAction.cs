using System;
using System.Collections.Generic;
using System.Linq;
using Grid;
using UnityEngine;

namespace Actions
{
    public abstract class BaseAction : MonoBehaviour
    {
        public static event EventHandler OnAnyActionStarted;
        public static event EventHandler OnAnyActionCompleted;

        protected Unit Unit;
        protected bool IsActive;

        [SerializeField, Min(0)] private int _maxDistance;
        [Header("Visuals")] [SerializeField] private Color _color = Color.white;
        [SerializeField] private bool _showRange;
        private Action _onActionComplete;

        protected virtual void Awake()
        {
            Unit = GetComponent<Unit>();
        }
        
        public abstract void TakeAction(GridPosition targetGridPosition, Action onActionComplete);

        public abstract EnemyAIAction GetEnemyAIAction(GridPosition gridPosition);

        public abstract string GetActionName();

        protected abstract List<GridPosition> GetGridPositions(bool filterByUnitPresence);

        public List<GridPosition> GetValidActionGridPositionList() => GetGridPositions(true);

        public List<GridPosition> GetRangeGridPositionList() => GetGridPositions(false);

        public virtual bool IsValidActionGridPosition(GridPosition gridPosition) =>
            GetValidActionGridPositionList().Contains(gridPosition);

        public virtual int GetActionPointsCost() => 1;

        public Unit GetUnit() => Unit;

        public int GetMaxDistance() => _maxDistance;

        public Color GetColorOfVisual() => _color;

        public bool HasRangeVisual() => _showRange;

        public EnemyAIAction GetBestEnemyAIAction()
        {
            List<EnemyAIAction> enemyAIActions = new();
            List<GridPosition> validActionGridPositionList = GetValidActionGridPositionList();

            foreach (GridPosition gridPosition in validActionGridPositionList)
            {
                EnemyAIAction enemyAIAction = GetEnemyAIAction(gridPosition);
                enemyAIActions.Add(enemyAIAction);
            }

            if (enemyAIActions.Count <= 0) return null;

            enemyAIActions.Sort((actionA, actionB) => actionB.ActionValue - actionA.ActionValue);

            return enemyAIActions.First();
        }

        protected void ActionStart(Action onActionComplete)
        {
            IsActive = true;
            _onActionComplete = onActionComplete;

            OnAnyActionStarted?.Invoke(this, EventArgs.Empty);
        }

        protected void ActionComplete()
        {
            IsActive = false;
            _onActionComplete();

            OnAnyActionCompleted?.Invoke(this, EventArgs.Empty);
        }
    }
}
