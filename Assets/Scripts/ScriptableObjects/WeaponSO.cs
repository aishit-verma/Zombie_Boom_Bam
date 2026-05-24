using UnityEngine;

[CreateAssetMenu(menuName = "Weapons/WeaponData")]
public class WeaponSO : ScriptableObject
{
    [Header("Info")]
    public string weaponName;
    public Sprite weaponIcon;
    public Color weaponColor;        // placeholder color until sprites ready

    [Header("Firing")]
    public float fireRate;           // shots per second
    public float damage;
    public float bulletSpeed;
    public int magazineSize;         // bullets before reload
    public float reloadTime;         // seconds to reload

    [Header("Bullet")]
    public GameObject bulletPrefab;
    public int bulletsPerShot;       // 1 = pistol, 8 = shotgun spread
    public float spreadAngle;        // 0 = accurate, 30 = shotgun spread

    [Header("Range")]
    public float bulletLifetime;     // how far bullet travels before dying

    [Header("Shop")]
    public int cost;                 // credits to buy from shop

    [Header("Recoil")]
    public float recoilForce;           // pushes player back on shoot
    public float recoilRecoverySpeed;   // how fast player returns to position

    [Header("Camera Shake")]
    public float shakeIntensity;        // how strong the shake is
    public float shakeDuration;         // how long it lasts
    public CameraShakePattern shakePattern; // what pattern

    [Header("Audio")]
    public AudioClip shootSound;    // ← add this
}

public enum CameraShakePattern
{
    Single,      // one quick shake — pistol
    Burst,       // multiple small shakes — SMG
    Heavy,       // one big shake — shotgun, sniper
}
