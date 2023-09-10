using System;
using Grid;
using UnityEngine;

public class GrenadeProjectile : MonoBehaviour
{
    private const float MOVE_SPEED = 15f;
    private const float DAMAGE_RADIUS = 4f;
    private const double TOLERANCE = 0.2f;
    
    private Action _onGrenadeBehaviorComplete;
    private Vector3 _targetPosition;

    private void Update()
    {
        Vector3 moveDir = (_targetPosition - transform.position).normalized;
        transform.position += moveDir * (MOVE_SPEED * Time.deltaTime);

        if (!(Vector3.Distance(transform.position, _targetPosition) < TOLERANCE)) return;

        Collider[] colliders = Physics.OverlapSphere(_targetPosition, DAMAGE_RADIUS);

        foreach (Collider collider in colliders)
        {
            if (!collider.TryGetComponent(out Unit targetUnit)) continue;
            
            targetUnit.Damage(100, transform.position);
        }

        _onGrenadeBehaviorComplete();
        
        Destroy(gameObject);
    }

    public void Setup(GridPosition targetGridPosition, Action onGrenadeBehaviorComplete)
    {
        _onGrenadeBehaviorComplete = onGrenadeBehaviorComplete;
        _targetPosition = LevelGrid.Instance.GetWorldPosition(targetGridPosition);
    }
}
