using System;
using System.Collections.Generic;
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
        [Header("Visuals")]
        [SerializeField] private Color _color = Color.white;
        [SerializeField] private bool _showVisual;
        private Action _onActionComplete;

        protected virtual void Awake()
        {
            Unit = GetComponent<Unit>();
        }

        public abstract string GetActionName();

        public abstract void DoAction(GridPosition gridPosition, Action onActionComplete);

        protected abstract List<GridPosition> GetGridPositions(bool filterByUnitPresence);
        
        public virtual List<GridPosition> GetValidActionGridPositionList() => 
            GetGridPositions(true);

        public virtual List<GridPosition> GetRangeGridPositionList() => 
            GetGridPositions(false);
        
        public virtual bool IsValidActionGridPosition(GridPosition gridPosition) => 
            GetValidActionGridPositionList().Contains(gridPosition);
        
        public virtual int GetActionPointsCost() => 1;

        public Unit GetUnit() => Unit;

        public int GetMaxDistance() => _maxDistance;
        
        public Color GetColorOfVisual() => _color;

        public bool HasRangeVisual() => _showVisual;

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
