using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

public class ZombieDotsSpawner : MonoBehaviour
{
    public static ZombieDotsSpawner Instance { get; private set; }

    private EntityManager entityManager;
    private Entity zombiePrefabEntity;
    private bool isInitialized = false;

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
        entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
    }

    private void Initialize()
    {
        if (isInitialized) return;

        // query for ZombiePrefab singleton entity
        EntityQuery query = entityManager.CreateEntityQuery(
            ComponentType.ReadOnly<ZombiePrefab>());

        if (query.CalculateEntityCount() == 0) return;

        ZombiePrefab prefabSingleton = query.GetSingleton<ZombiePrefab>();
        zombiePrefabEntity = prefabSingleton.zombiePrefab;
        isInitialized = true;
    }

    public void SpawnZombie(ZombieTypeSO type, Vector3 position, int typeIndex)
    {
        Initialize();
        if (!isInitialized) return;

        Entity zombie = entityManager.Instantiate(zombiePrefabEntity);

        // position
        entityManager.SetComponentData(zombie, new LocalTransform
        {
            Position = new float3(position.x, position.y, 0f),
            Rotation = quaternion.identity,
            Scale = 1f
        });

        // movement
        entityManager.SetComponentData(zombie, new MoveSpeed
        {
            moveSpeed = type.moveSpeed
        });

        // health
        entityManager.SetComponentData(zombie, new Health
        {
            currentHealth = type.health,
            maxHealth = type.health
        });

        // contact damage
        entityManager.SetComponentData(zombie, new ZombieDamage
        {
            damagePerSecond = type.contactDamage
        });

        // visuals — color + sprite
        // Color c = type.zombieColor;
        entityManager.SetComponentData(zombie, new ZombieVisual
        {
            // color = new float4(c.r, c.g, c.b, c.a),
            spriteIndex = typeIndex,
            needsUpdate = true
        });

        // type index for drops
        entityManager.SetComponentData(zombie, new ZombieTypeIndex
        {
            index = typeIndex
        });
    }
}