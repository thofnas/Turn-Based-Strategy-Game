using Cinemachine;
using UnityEngine;

[RequireComponent(typeof(CinemachineImpulseSource))]
public class ScreenShake : Singleton<ScreenShake>
{
    private CinemachineImpulseSource _cinemachineImpulseSource;

    protected override void Awake()
    {
        base.Awake();
        
        _cinemachineImpulseSource = GetComponent<CinemachineImpulseSource>();
    }

    public void Shake(float intensity)
    {
        _cinemachineImpulseSource.GenerateImpulse(intensity);
    }
}
