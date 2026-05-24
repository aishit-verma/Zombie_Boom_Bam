using UnityEngine;
using Unity.Cinemachine;
using System.Collections;

public class CameraShake : MonoBehaviour
{
    public static CameraShake Instance { get; private set; }

    private CinemachineImpulseSource impulseSource;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        impulseSource = GetComponent<CinemachineImpulseSource>();
    }

    public void Shake(float intensity, float duration, 
        CameraShakePattern pattern)
    {
        StopAllCoroutines();
        StartCoroutine(ShakeRoutine(intensity, duration, pattern));
    }

    private IEnumerator ShakeRoutine(float intensity, float duration,
        CameraShakePattern pattern)
    {
        switch (pattern)
        {
            case CameraShakePattern.Single:
                impulseSource.GenerateImpulse(intensity);
                break;

            case CameraShakePattern.Heavy:
                impulseSource.GenerateImpulse(intensity);
                yield return new WaitForSeconds(duration * 0.5f);
                impulseSource.GenerateImpulse(intensity * 0.3f);
                break;

            case CameraShakePattern.Burst:
                int bursts = 3;
                float burstIntensity = intensity * 0.5f;
                for (int i = 0; i < bursts; i++)
                {
                    impulseSource.GenerateImpulse(burstIntensity);
                    yield return new WaitForSeconds(duration / bursts);
                }
                break;
        }
    }

    // simple overload for grenade
    public void Shake(float intensity)
    {
        impulseSource.GenerateImpulse(intensity);
    }
}