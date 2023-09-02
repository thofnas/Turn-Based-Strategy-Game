using UnityEngine;

public class UnitRagdoll : MonoBehaviour
{
    [SerializeField] private Transform _ragdollRootBone;
    private Vector3 _shooterPosition;

    public void Setup(Transform originalRootBone, Vector3 damageDealerPosition)
    {
        MathAllChildTransforms(originalRootBone, _ragdollRootBone);

        float offset = 0.5f;
        Vector3 explosionPosition = (damageDealerPosition - transform.position).normalized * offset + transform.position;
        print(damageDealerPosition);
        
        ApplyExplosionForce(_ragdollRootBone, 400f, explosionPosition, 10f);
    }

    private void MathAllChildTransforms(Transform root, Transform clone)
    {
        foreach (Transform child in root)
        {
            Transform cloneChild = clone.Find(child.name);
            
            if (cloneChild == null) continue;

            cloneChild.position = child.position;
            cloneChild.rotation = child.rotation;
            
            MathAllChildTransforms(child, cloneChild);
        }
    }

    private void ApplyExplosionForce(Transform root, float explosionForce, Vector3 explosionPosition, float explosionRange)
    {
        foreach (Transform child in root)
        {
            if (child.TryGetComponent(out Rigidbody childRb))
            {
                print("force fart");
                childRb.AddExplosionForce(explosionForce, explosionPosition, explosionRange);
            }


            ApplyExplosionForce(child, explosionForce, explosionPosition, explosionRange);
        }
    }
}
