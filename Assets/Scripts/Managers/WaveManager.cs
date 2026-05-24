using UnityEngine;
using System.Collections;

public class WaveManager : MonoBehaviour
{
    public static WaveManager Instance { get; private set; }

    [SerializeField] private WaveConfigSO[] waves;
    [SerializeField] private float timeBetweenWaves = 8f;
    [SerializeField] private float collectTime = 5f;



    private int currentWaveIndex = 0;
    private int zombiesAlive = 0;

    // Other systems listen to these
    public System.Action OnWaveStart;
    public System.Action OnWaveEnd;
    public System.Action OnShopOpen;


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
        // keep main scene active so all instantiated objects go here
        UnityEngine.SceneManagement.SceneManager.SetActiveScene(
            gameObject.scene);

        StartCoroutine(StartNextWave());
    }

    private IEnumerator StartNextWave()
    {
        // Delay before wave starts
        yield return new WaitForSeconds(3f);

        if (currentWaveIndex >= waves.Length)
        {
           
            yield break;
        }

        WaveConfigSO wave = waves[currentWaveIndex];
        zombiesAlive = wave.zombieCount;

   
        OnWaveStart?.Invoke();

        ZombieSpawner.Instance.SpawnWave(wave);
    }

    // Called by every zombie when it dies
    public void OnZombieDied()
    {
        zombiesAlive--;
        

        if (zombiesAlive <= 0)
            StartCoroutine(EndWave());
    }

    private IEnumerator EndWave()
    {
        OnWaveEnd?.Invoke();  // credits awarded, wave complete UI shows

        // give player time to collect drops first
        yield return new WaitForSeconds(collectTime);

        // NOW open shop
        OnShopOpen?.Invoke();  // add this new event

        // wait remaining time then start next wave
        yield return new WaitForSeconds(timeBetweenWaves - collectTime);

        currentWaveIndex++;
        StartCoroutine(StartNextWave());
    }

    public int GetCurrentWaveReward()
    {
        // currentWaveIndex already incremented in EndWave
        // so we use index - 1 to get the wave that just ended
        int completedIndex = currentWaveIndex - 1;
        if (completedIndex >= 0 && completedIndex < waves.Length)
            return waves[completedIndex].creditsReward;
        return 0;
    }

    public int GetCurrentWaveNumber() => currentWaveIndex + 1;
}