using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public delegate void UpdateDelegate();
    public static event UpdateDelegate OnUpdate;
    public delegate void FixedUpdateDelegate();
    public static event FixedUpdateDelegate OnFixedUpdate;
    public delegate void LateUpdateDelegate();
    public static event LateUpdateDelegate OnLateUpdate;

    public List<Enemy> AllEnemies;

    [Header("Enemy Spawn Setting")]
    [Tooltip("Mininum time between enemies weaves")]
    public float minSpawnTime;

    [Tooltip("Maximum time between enemies weaves")]
    public float  maxSpawnTime;

    [Tooltip("Mininum number of enemies to spawn")]
    public int minEnemySpawn;

    [Tooltip("Maximum number of enemies to spawn")]
    public int maxEnemySpawn;

    [Tooltip("Time to reduce between enemy weaves")]
    public float spawnTimeRate;

    [Tooltip("Number of enemies to add in next weaves")]
    public int spawnEnemyRate;
    [Tooltip("Minimum spawn radius for enemies")]
    public float minSpawnRadius;

    [Tooltip("Maximum spawn radius for enemies")]
    public float maxSpawnRadius;

    [Header("Popup Setting")]
    [SerializeField]
    private  DamagePopup damagePopupPrefab;

    [Header("Projectiles")]
    [SerializeField]
    private BarrageProjecttile barrageProjectilePrefab;
    [SerializeField]
    private FireballProjectile fireballProjectilePrefab;
    
    private int currentEnemyRate;               // Current number of enemies to spawn.
    private bool isGameStarted = false;         // Is game started flag.
    private float currentSpawnWait;             // Current time to wait before spawning new weave of enemies.
    private float currentSpawnRate;             // Next weave time to spawn enemies.

    private List<Enemy> enemyPool = new List<Enemy>();
    private List<DamagePopup> damagePopupPool = new List<DamagePopup>();
    private List<BarrageProjecttile> barrageProjectilePool = new List<BarrageProjecttile>();
    private List<FireballProjectile> fireballProjectilePool = new List<FireballProjectile>();

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else if(Instance != this)
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        if(!isGameStarted)
            return;
        OnUpdate?.Invoke();
        currentSpawnWait -= Time.deltaTime;
        if(currentSpawnWait <= 0)
        {
            currentSpawnRate -= spawnTimeRate;
            currentSpawnRate = Mathf.Clamp(currentSpawnRate, minSpawnTime, maxEnemySpawn);
            currentSpawnWait = currentSpawnRate;
            for(int i = 0; i < currentEnemyRate; i++)
            {
                var enemyToSpawn = Random.Range(0, AllEnemies.Count);
                var enemy = GetEnemy(AllEnemies[enemyToSpawn].EnemyData.Id);
                float randomAngle = Random.Range(0f, Mathf.PI * 2f);

            // Generate a random distance within the specified range
                float randomDistance = Random.Range(minSpawnRadius, maxSpawnRadius);

            // Calculate the spawn position based on polar coordinates
            Vector3 position = new Vector3(
                transform.position.x + randomDistance * Mathf.Cos(randomAngle),
                transform.position.y,
                transform.position.z + randomDistance * Mathf.Sin(randomAngle)
            );
                enemy.transform.position = position;
                enemy.Initiate();
            }
            currentEnemyRate += spawnEnemyRate; 
            currentEnemyRate = Mathf.Clamp(currentEnemyRate, minEnemySpawn, maxEnemySpawn);
        }
    }

    private void FixedUpdate()
    {
        if(!isGameStarted)
            return;
        OnFixedUpdate?.Invoke();
    }

    private void LateUpdate()
    {
        if(!isGameStarted)
            return;
        OnLateUpdate?.Invoke();
    }

    private Enemy GetEnemy(int enemyId)
    {
        var e = enemyPool.Find(x => x.EnemyData.Id == enemyId && !x.gameObject.activeSelf);
        if(e == null)
        {
            e = Instantiate(AllEnemies.Find(x => x.EnemyData.Id == enemyId));
            enemyPool.Add(e);
        }
        return e;
    }

    public DamagePopup GetDamagePopup()
    {
        var p = damagePopupPool.Find(x => !x.gameObject.activeSelf);
        if(p == null)
        {
            p = Instantiate(damagePopupPrefab);
            damagePopupPool.Add(p);
        }
        return p;
    }
    public BarrageProjecttile GetBarrageProjectile()
    {
        var b = barrageProjectilePool.Find(x => !x.gameObject.activeSelf);
        if(b == null)
        {
            b = Instantiate(barrageProjectilePrefab);
            barrageProjectilePool.Add(b);
        }
        return b;
    }

    public FireballProjectile GetFireballProjectile()
    {
        var f = fireballProjectilePool.Find(x => !x.gameObject.activeSelf);
        if(f == null)
        {
            f = Instantiate(fireballProjectilePrefab);
            fireballProjectilePool.Add(f);
        }
        return f;
    }

    //[ContextMenu("Start Game")]
    public void StartGame()
    {
        //currentSpawnRate = maxSpawnTime;
        TowerController.Instance.Initiate();
        isGameStarted = true;
    }

    public void SetGameOver()
    {
        isGameStarted = false;
    }
}
