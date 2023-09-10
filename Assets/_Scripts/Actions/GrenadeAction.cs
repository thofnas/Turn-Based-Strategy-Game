using System;
using System.Collections.Generic;
using Grid;
using UnityEngine;

namespace Actions
{
    public class GrenadeAction : BaseAction
    {
        [SerializeField] private GrenadeProjectile _grenadeProjectilePrefab;
        [SerializeField] private LayerMask _obstaclesLayerMask;

        private void Update()
        {
            if (!IsActive) return;
            
        }

        public override void DoAction(GridPosition targetGridPosition, Action onActionComplete)
        {
            GrenadeProjectile grenadeProjectile = Instantiate(_grenadeProjectilePrefab, Unit.GetWorldPosition(), Quaternion.identity);
            
            grenadeProjectile.Setup(targetGridPosition, OnGrenadeBehaviorComplete);
            
            ActionStart(onActionComplete);
        }
        
        public override EnemyAIAction GetEnemyAIAction(GridPosition gridPosition) => 
            new() {
                EnemyGridPosition = gridPosition,
                ActionValue = 0 
            };
        
        public override int GetActionPointsCost() => 2;
        
        public override string GetActionName() => "Grenade";


        protected override List<GridPosition> GetGridPositions(bool _)
        {
            const float unitShoulderHeight = 1.7f;
            GridPosition unitGridPosition = Unit.GetGridPosition();
            List<GridPosition> validGridPositionList = new();
            Vector3 unitWorldPosition = LevelGrid.Instance.GetWorldPosition(unitGridPosition);
            int maxDistance = GetMaxDistance();

            for (int x = -maxDistance; x <= maxDistance; x++)
            {
                for (int z = -maxDistance; z <= maxDistance; z++)
                {
                    GridPosition offsetGridPosition = new(x, z);
                    GridPosition testGridPosition = unitGridPosition + offsetGridPosition;

                    if (!LevelGrid.Instance.IsValidGridPosition(testGridPosition))
                        continue;

                    float testDistance = Mathf.Sqrt(x * x + z * z);

                    if (Mathf.Round(testDistance) > maxDistance)
                        continue;
                    
                    Unit targetUnit = LevelGrid.Instance.GetUnitAtGridPosition(testGridPosition);
                    
                    if (targetUnit != null)
                    {
                        if (targetUnit.IsEnemy() == Unit.IsEnemy())
                            continue;
                    }
                    
                    validGridPositionList.Add(testGridPosition);
                }
            }

            return validGridPositionList;
        }
        

        private void OnGrenadeBehaviorComplete() => ActionComplete();
    }
}
