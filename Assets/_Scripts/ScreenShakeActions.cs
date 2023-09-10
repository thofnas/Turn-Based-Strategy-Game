using System;
using Actions;
using UnityEngine;

public class ScreenShakeActions : MonoBehaviour
{
    private void Start()
    {
        ShootAction.OnAnyUnitShoot += ShootAction_OnAnyUnitShoot;
        GrenadeProjectile.OnAnyGrenadeExploded += GrenadeProjectile_OnAnyGrenadeExploded; 
    }

    private void OnDestroy()
    {
        ShootAction.OnAnyUnitShoot -= ShootAction_OnAnyUnitShoot;
        GrenadeProjectile.OnAnyGrenadeExploded -= GrenadeProjectile_OnAnyGrenadeExploded;
    }
    
    private void ShootAction_OnAnyUnitShoot(object sender, ShootAction.UnitShootEventArgs e)
    {
        ScreenShake.Instance.Shake(2f);
    }
    
    private void GrenadeProjectile_OnAnyGrenadeExploded(object sender, EventArgs e)
    {
        ScreenShake.Instance.Shake(5f);
    }
}
