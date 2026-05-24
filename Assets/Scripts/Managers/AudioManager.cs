using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [Header("SFX Sources")]
    [SerializeField] private AudioSource sfxSource;
    [SerializeField] private AudioSource musicSource;

    [Header("Weapon SFX")]
    [SerializeField] private AudioClip pistolShot;
    [SerializeField] private AudioClip shotgunBlast;
    [SerializeField] private AudioClip smgShot;
    [SerializeField] private AudioClip reloadSound;

    [Header("Zombie SFX")]
    [SerializeField] private AudioClip zombieDeath;
    [SerializeField] private AudioClip grenadeExplosion;

    [Header("Pickup SFX")]
    [SerializeField] private AudioClip creditPickup;
    [SerializeField] private AudioClip healthPickup;

    [Header("UI SFX")]
    [SerializeField] private AudioClip waveStart;
    [SerializeField] private AudioClip waveComplete;
    [SerializeField] private AudioClip shopOpen;
    [SerializeField] private AudioClip buttonClick;

    [Header("Music")]
    [SerializeField] private AudioClip gameplayMusic;
    [SerializeField] private AudioClip gameOverMusic;

    [Header("Settings")]
    [SerializeField] private float sfxVolume = 1f;
    [SerializeField] private float musicVolume = 0.5f;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    void Start()
    {
        // subscribe to events
        WaveManager.Instance.OnWaveStart += PlayWaveStart;
        WaveManager.Instance.OnWaveEnd += PlayWaveComplete;
        WaveManager.Instance.OnShopOpen += PlayShopOpen;

        // start music
        PlayMusic(gameplayMusic);
    }

    void OnDestroy()
    {
        WaveManager.Instance.OnWaveStart -= PlayWaveStart;
        WaveManager.Instance.OnWaveEnd -= PlayWaveComplete;
        WaveManager.Instance.OnShopOpen -= PlayShopOpen;
    }

    // ── SFX ─────────────────────────────────────────
    public void PlaySFX(AudioClip clip, float volume = 1f)
    {
        if (clip == null) return;
        sfxSource.PlayOneShot(clip, volume * sfxVolume);
    }

    // ── Weapon Sounds ────────────────────────────────
    public void PlayWeaponSound(WeaponSO weapon)
    {
        if (weapon == null) return;
        PlaySFX(GetWeaponClip(weapon));
    }

    private AudioClip GetWeaponClip(WeaponSO weapon)
    {
        return weapon.shootSound;
    }

    public void PlayReload() => PlaySFX(reloadSound);

    // ── Zombie Sounds ────────────────────────────────
    public void PlayZombieDeath() =>
        PlaySFX(zombieDeath, Random.Range(0.8f, 1.2f));

    public void PlayGrenadeExplosion() =>
        PlaySFX(grenadeExplosion);

    // ── Pickup Sounds ────────────────────────────────
    public void PlayCreditPickup() => PlaySFX(creditPickup);
    public void PlayHealthPickup() => PlaySFX(healthPickup);

    // ── UI Sounds ────────────────────────────────────
    private void PlayWaveStart() => PlaySFX(waveStart);
    private void PlayWaveComplete() => PlaySFX(waveComplete);
    private void PlayShopOpen() => PlaySFX(shopOpen);
    public void PlayButtonClick() => PlaySFX(buttonClick);

    // ── Music ────────────────────────────────────────
    public void PlayMusic(AudioClip clip)
    {
        if (clip == null) return;
        musicSource.clip = clip;
        musicSource.loop = true;
        musicSource.volume = musicVolume;
        musicSource.Play();
    }

    public void PlayGameOverMusic()
    {
        PlayMusic(gameOverMusic);
    }
}