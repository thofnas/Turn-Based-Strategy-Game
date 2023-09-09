using Actions;
using UnityEngine;

public class ScreenShakeActions : MonoBehaviour
{
    private void Start()
    {
        ShootAction.OnAnyUnitShoot += ShootAction_OnAnyUnitShoot;
    }
    
    private void OnDestroy()
    {
        ShootAction.OnAnyUnitShoot -= ShootAction_OnAnyUnitShoot;
    }
    
    private void ShootAction_OnAnyUnitShoot(object sender, ShootAction.UnitShootEventArgs e)
    {
        ScreenShake.Instance.Shake(2f);
    }
}
