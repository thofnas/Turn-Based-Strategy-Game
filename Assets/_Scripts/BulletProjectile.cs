using Unity.Mathematics;
using UnityEngine;

public class BulletProjectile : MonoBehaviour
{
    private const float MOVE_SPEED = 200f;

    [SerializeField] private TrailRenderer _trailRenderer;
    [SerializeField] private Transform _bulletHitParticles;
    private Vector3 _targetPosition;
    
    public void Setup(Vector3 targetPosition)
    {
        _targetPosition = targetPosition;
        Destroy(gameObject, 2f);
    }

    private void Update()
    {
        Vector3 moveDir = (_targetPosition - transform.position).normalized;
        float distanceBeforeMoving = Vector3.Distance(transform.position, _targetPosition);
        transform.position += moveDir * (MOVE_SPEED * Time.deltaTime);
        float distanceAfterMoving = Vector3.Distance(transform.position, _targetPosition);

        if (!(distanceBeforeMoving < distanceAfterMoving)) return;
        
        transform.position = _targetPosition;
            
        _trailRenderer.transform.SetParent(null);

        Instantiate(_bulletHitParticles, _targetPosition, Quaternion.identity);
        
        Destroy(gameObject);
    }
}
