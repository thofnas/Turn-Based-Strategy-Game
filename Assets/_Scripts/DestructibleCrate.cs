using System;
using Grid;
using UnityEngine;

public class DestructibleCrate : MonoBehaviour
{
    public static event EventHandler OnAnyDestroyed;

    [SerializeField] private Transform _destructibleCratePrefab;
    private GridPosition _gridPosition;
    
    private void Start()
    {
        _gridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
    }

    public GridPosition GetGridPosition() => _gridPosition;

    public void Damage(Vector3 explosionPosition, float range)
    {
        Destroy(gameObject);

        Transform destroyedCrate = Instantiate(_destructibleCratePrefab, transform.position, transform.rotation);
        
        ApplyExplosionForceToChildren(destroyedCrate, 1000f, explosionPosition, range);
        
        OnAnyDestroyed?.Invoke(this, EventArgs.Empty);
    }
    
    private void ApplyExplosionForceToChildren(Transform root, float explosionForce, Vector3 explosionPosition, float explosionRange)
    {
        foreach (Transform child in root)
        {
            if (child.TryGetComponent(out Rigidbody childRb))
                childRb.AddExplosionForce(explosionForce, explosionPosition, explosionRange);
            
            ApplyExplosionForceToChildren(child, explosionForce, explosionPosition, explosionRange);
        }
    }
}
