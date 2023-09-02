using System;
using Actions;
using UnityEngine;

public class UnitAnimator : MonoBehaviour
{
    private static readonly int IsWalking = Animator.StringToHash(nameof(IsWalking));
    private static readonly int Shoot = Animator.StringToHash(nameof(Shoot));

    [SerializeField] private Animator _animator;
    [SerializeField] private BulletProjectile _bulletProjectilePrefab;
    [SerializeField] private Transform _rifleShootPointTransform;

    private void Awake()
    {
       if (TryGetComponent(out MoveAction moveAction))
       {
           moveAction.OnUnitStartMoving += MoveAction_OnUnitStartMoving;
           moveAction.OnUnitStopMoving += MoveAction_OnUnitStopMoving;
       }

       if (TryGetComponent(out ShootAction shootAction))
       {
           shootAction.OnUnitShoot += ShootAction_OnUnitShoot;
       }
    }

    private void MoveAction_OnUnitStartMoving(object sender, EventArgs e) => 
        _animator.SetBool(IsWalking, true);

    private void MoveAction_OnUnitStopMoving(object sender, EventArgs e) => 
        _animator.SetBool(IsWalking, false);
    
    private void ShootAction_OnUnitShoot(object sender, ShootAction.OnUnitShootEventArgs e)
    {
        _animator.SetTrigger(Shoot);

        BulletProjectile bulletProjectile = 
            Instantiate(_bulletProjectilePrefab, _rifleShootPointTransform.position, Quaternion.identity);
        Vector3 targetUnitShootAtPosition = e.TargetUnit.GetWorldPosition();
        targetUnitShootAtPosition.y = _rifleShootPointTransform.position.y;
        
        bulletProjectile.Setup(targetUnitShootAtPosition);
    }
}
