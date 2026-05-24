using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

public class ZombieHitHandler : MonoBehaviour
{
    public static ZombieHitHandler Instance { get; private set; }

    private EntityManager entityManager;
    private EntityQuery zombieQuery;

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
        zombieQuery = entityManager.CreateEntityQuery(
            ComponentType.ReadWrite<Health>(),
            ComponentType.ReadOnly<LocalTransform>(),
            ComponentType.ReadOnly<Zombie>(),
            ComponentType.ReadOnly<ZombieTypeIndex>());
    }

    public bool DamageNearestZombie(Vector3 hitPosition, float damage, float radius = 0.5f)
    {
        if (zombieQuery.CalculateEntityCount() == 0) return false;

        float3 hitPos = new float3(hitPosition.x, hitPosition.y, 0f);
        float closestDistance = radius;
        Entity closestEntity = Entity.Null;

        var entities = zombieQuery.ToEntityArray(
            Unity.Collections.Allocator.Temp);
        var transforms = zombieQuery.ToComponentDataArray<LocalTransform>(
            Unity.Collections.Allocator.Temp);

        for (int i = 0; i < entities.Length; i++)
        {
            float distance = math.distance(hitPos, transforms[i].Position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestEntity = entities[i];
            }
        }

        entities.Dispose();
        transforms.Dispose();

        if (closestEntity == Entity.Null) return false;

        // deal damage
        Health health = entityManager
            .GetComponentData<Health>(closestEntity);
        health.currentHealth -= damage;
        entityManager.SetComponentData(closestEntity, health);

        // hit flash effect
        if (entityManager.HasComponent<SpriteRenderer>(closestEntity))
        {
            SpriteRenderer sr = entityManager
                .GetComponentObject<SpriteRenderer>(closestEntity);
            sr.GetComponent<ZombieHitFlash>()?.Flash();
        }

        if (health.currentHealth <= 0)
        {
            AudioManager.Instance?.PlayZombieDeath();
            ZombieTypeIndex typeIndex = entityManager
                .GetComponentData<ZombieTypeIndex>(closestEntity);

            Vector3 deathPos = new Vector3(hitPos.x, hitPos.y, 0f);

            // death effect
            DeathEffect.Instance?.PlayDeathEffect(
                deathPos, typeIndex.index);

            entityManager.DestroyEntity(closestEntity);
            WaveManager.Instance.OnZombieDied();

            ZombieDropBridge.Instance.TrySpawnDrop(
                deathPos, typeIndex.index);
        }

        return true;
    }
}