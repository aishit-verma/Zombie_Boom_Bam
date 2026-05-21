using UnityEngine;

public class WeaponController : MonoBehaviour
{
    public static WeaponController Instance { get; private set; }

    // currently equipped weapons in 3 slots
    private WeaponSO[] equippedWeapons = new WeaponSO[3];
    private int[] currentAmmo = new int[3];
    private int activeSlot = 0;
    private bool isReloading = false;
    private float reloadTimer = 0f;
    private float fireTimer = 0f;

    [SerializeField] private Transform firePoint;

    [SerializeField] private WeaponSO startingWeapon;

    void Start()
    {
        if (startingWeapon != null)
            EquipWeapon(startingWeapon, 0);
    }

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    void Update()
    {
        HandleWeaponSwitch();
        HandleReload();
        HandleShooting();
    }

    // ── Weapon Switching ────────────────────────────
    private void HandleWeaponSwitch()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1)) SwitchSlot(0);
        if (Input.GetKeyDown(KeyCode.Alpha2)) SwitchSlot(1);
        if (Input.GetKeyDown(KeyCode.Alpha3)) SwitchSlot(2);

        // scroll wheel switching
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (scroll > 0f) SwitchSlot((activeSlot + 1) % 3);
        if (scroll < 0f) SwitchSlot((activeSlot + 2) % 3);
    }

    private void SwitchSlot(int slot)
    {
        if (slot == activeSlot) return;
        if (equippedWeapons[slot] == null) return; // empty slot

        activeSlot = slot;
        isReloading = false;
        reloadTimer = 0f;

        GameplayUI.Instance.SetActiveSlot(activeSlot);
        UpdateAmmoUI();
    }

    // ── Shooting ────────────────────────────────────
    private void HandleShooting()
    {
        if (isReloading) return;
        if (equippedWeapons[activeSlot] == null) return;

        fireTimer -= Time.deltaTime;

        if (Input.GetMouseButton(0) && fireTimer <= 0f)
        {
            if (currentAmmo[activeSlot] <= 0)
            {
                StartReload();
                return;
            }

            Shoot();
            fireTimer = equippedWeapons[activeSlot].fireRate;
        }
    }

    private void Shoot()
    {
        WeaponSO weapon = equippedWeapons[activeSlot];

        for (int i = 0; i < weapon.bulletsPerShot; i++)
        {
            float angle = Random.Range(
                -weapon.spreadAngle / 2f,
                weapon.spreadAngle / 2f);

            Quaternion spreadRotation =
                firePoint.rotation * Quaternion.Euler(0, 0, angle);

            GameObject bulletObj =
                Instantiate(weapon.bulletPrefab, firePoint.position, spreadRotation);

            Bullet bullet = bulletObj.GetComponent<Bullet>();
            bullet.Initialize(weapon.damage, weapon.bulletSpeed, weapon.bulletLifetime);
        }

        currentAmmo[activeSlot]--;
        UpdateAmmoUI();

        if (currentAmmo[activeSlot] <= 0)
            StartReload();
    }

    // ── Reload ──────────────────────────────────────
    private void HandleReload()
    {
        // manual reload input
        if (Input.GetKeyDown(KeyCode.R) && !isReloading)
        {
            StartReload();
            return;
        }

        if (!isReloading) return;

        reloadTimer -= Time.deltaTime;

        if (reloadTimer <= 0f)
            FinishReload();
    }

    private void StartReload()
    {
        if (isReloading) return;
        if (equippedWeapons[activeSlot] == null) return;
        if (currentAmmo[activeSlot] ==
            equippedWeapons[activeSlot].magazineSize) return;

        isReloading = true;
        reloadTimer = equippedWeapons[activeSlot].reloadTime;

        // show reloading in ammo text
        GameplayUI.Instance.UpdateAmmo(-1,
            equippedWeapons[activeSlot].magazineSize);

        Debug.Log("Reloading...");
    }

    private void FinishReload()
    {
        isReloading = false;
        currentAmmo[activeSlot] =
            equippedWeapons[activeSlot].magazineSize;

        UpdateAmmoUI();
        Debug.Log("Reload complete!");
    }

    // ── Equip ───────────────────────────────────────
    public bool EquipWeapon(WeaponSO weapon, int slot)
    {
        if (slot < 0 || slot >= 3) return false;

        equippedWeapons[slot] = weapon;
        currentAmmo[slot] = weapon.magazineSize;

        GameplayUI.Instance.SetWeaponInSlot(
            slot, weapon.weaponIcon, weapon.weaponColor);

        if (slot == activeSlot)
            UpdateAmmoUI();

        Debug.Log($"{weapon.weaponName} equipped in slot {slot + 1}");
        return true;
    }

    public void ClearSlot(int slot)
    {
        if (slot < 0 || slot >= 3) return;
        equippedWeapons[slot] = null;
        currentAmmo[slot] = 0;
        GameplayUI.Instance.ClearWeaponSlot(slot);

        if (slot == activeSlot)
            GameplayUI.Instance.UpdateAmmo(0, 0);
    }

    // ── Helpers ─────────────────────────────────────
    private void UpdateAmmoUI()
    {
        if (equippedWeapons[activeSlot] == null)
        {
            GameplayUI.Instance.UpdateAmmo(0, 0);
            return;
        }
        GameplayUI.Instance.UpdateAmmo(
            currentAmmo[activeSlot],
            equippedWeapons[activeSlot].magazineSize);
    }
    public int GetFirstEmptySlot()
    {
        for (int i = 0; i < equippedWeapons.Length; i++)
        {
            if (equippedWeapons[i] == null)
                return i;
        }
        return -1; // all slots full
    }
    public WeaponSO GetWeaponInSlot(int slot)
    {
        if (slot < 0 || slot >= equippedWeapons.Length) return null;
        return equippedWeapons[slot];
    }

    public WeaponSO GetActiveWeapon() => equippedWeapons[activeSlot];
    public int GetActiveSlot() => activeSlot;
}