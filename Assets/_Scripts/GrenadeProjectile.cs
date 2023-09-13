using System;
using Grid;
using UnityEngine;

public class GrenadeProjectile : MonoBehaviour
{
    private const float MOVE_SPEED = 15f;
    private const float DAMAGE_RADIUS = 4f;
    private const double TOLERANCE = 0.2f;
    private const float MAX_HEIGHT = 4f;

    public static event EventHandler OnAnyGrenadeExploded;

    [SerializeField] private Transform _grenadeExplodeParticles;
    [SerializeField] private TrailRenderer _trailRenderer;
    [SerializeField] private AnimationCurve _arcYAnimationCurve;
    private Action _onGrenadeBehaviorComplete;
    private Vector3 _targetPosition;
    private float _totalDistance;
    private Vector3 _positionXZ;

    private void Update()
    {
        Vector3 moveDir = (_targetPosition - _positionXZ).normalized;
        _positionXZ += moveDir * (MOVE_SPEED * Time.deltaTime);
        float distance = Vector3.Distance(_positionXZ, _targetPosition);
        float distanceNormalized = Mathf.InverseLerp(_totalDistance, 0f, distance);
        float positionY = _arcYAnimationCurve.Evaluate(distanceNormalized) * _totalDistance / MAX_HEIGHT;

        transform.position = new Vector3(_positionXZ.x, positionY, _positionXZ.z);
        
        if (!(Vector3.Distance(_positionXZ, _targetPosition) < TOLERANCE)) return;

        Collider[] colliders = Physics.OverlapSphere(_targetPosition, DAMAGE_RADIUS);

        foreach (Collider collider in colliders)
        {
            if (collider.TryGetComponent(out Unit targetUnit)) 
                targetUnit.Damage(100, transform.position);
            if (collider.TryGetComponent(out DestructibleCrate crate))
                crate.Damage(_targetPosition, DAMAGE_RADIUS);
        }

        _onGrenadeBehaviorComplete();

        Instantiate(_grenadeExplodeParticles, _targetPosition, Quaternion.identity);
        
        _trailRenderer.transform.SetParent(null);
        
        Destroy(gameObject);
        
        OnAnyGrenadeExploded?.Invoke(this, EventArgs.Empty);
    }

    public void Setup(GridPosition targetGridPosition, Action onGrenadeBehaviorComplete)
    {
        _onGrenadeBehaviorComplete = onGrenadeBehaviorComplete;
        _targetPosition = LevelGrid.Instance.GetWorldPosition(targetGridPosition);
        _positionXZ = transform.position;
        _positionXZ.y = 0;
        _totalDistance = Vector3.Distance(_positionXZ, _targetPosition);
    }
}
