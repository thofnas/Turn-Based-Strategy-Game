using System;
using Actions;
using UnityEngine;

public class UnitAnimator : MonoBehaviour
{
    private static readonly int IsWalking = Animator.StringToHash(nameof(IsWalking));
    private static readonly int Shoot = Animator.StringToHash(nameof(Shoot));
    private static readonly int SwordSlash = Animator.StringToHash(nameof(SwordSlash));

    [SerializeField] private Animator _animator;
    [SerializeField] private BulletProjectile _bulletProjectilePrefab;
    [SerializeField] private Transform _rifleShootPointTransform;
    [SerializeField] private Transform _rifleTransform;
    [SerializeField] private Transform _swordTransform;

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

       if (TryGetComponent(out MeleeAction meleeAction))
       {
           meleeAction.OnMeleeActionStarted += MeleeAction_OnMeleeActionStarted;
           meleeAction.OnMeleeActionCompleted += MeleeAction_OnMeleeActionCompleted;
       }
    }

    private void Start()
    {
        EquipRifle();
    }

    private void EquipSword()
    {
        _swordTransform.gameObject.SetActive(true);
        _rifleTransform.gameObject.SetActive(false);
    }

    private void EquipRifle()
    {
        _swordTransform.gameObject.SetActive(false);
        _rifleTransform.gameObject.SetActive(true);
    }

    private void MoveAction_OnUnitStartMoving(object sender, EventArgs e) => 
        _animator.SetBool(IsWalking, true);

    private void MoveAction_OnUnitStopMoving(object sender, EventArgs e) => 
        _animator.SetBool(IsWalking, false);
    
    private void ShootAction_OnUnitShoot(object sender, ShootAction.UnitShootEventArgs e)
    {
        _animator.SetTrigger(Shoot);

        BulletProjectile bulletProjectile = 
            Instantiate(_bulletProjectilePrefab, _rifleShootPointTransform.position, Quaternion.identity);
        Vector3 targetUnitShootAtPosition = e.TargetUnit.GetWorldPosition();
        targetUnitShootAtPosition.y = _rifleShootPointTransform.position.y;
        
        bulletProjectile.Setup(targetUnitShootAtPosition);
    }
    
    private void MeleeAction_OnMeleeActionStarted(object sender, EventArgs e)
    {
        _animator.SetTrigger(SwordSlash);
        EquipSword();
    }
    
    private void MeleeAction_OnMeleeActionCompleted(object sender, EventArgs e)
    {
        EquipRifle();
    }
}
