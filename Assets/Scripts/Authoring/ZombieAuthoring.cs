using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

// ── Components ──────────────────────────────────────
public struct Zombie : IComponentData { }

public struct MoveSpeed : IComponentData
{
    public float moveSpeed;
}

public struct Health : IComponentData
{
    public float currentHealth;
    public float maxHealth;
}

public struct ZombieDamage : IComponentData
{
    public float damagePerSecond;
}

public struct ZombieTypeIndex : IComponentData
{
    public int index;
}

public struct ZombieVisual : IComponentData
{
    public int spriteIndex;
    public bool needsUpdate;
}

public struct ZombieSteering : IComponentData
{
    public float3 currentDirection;
    public float steeringSpeed;
}

// ── Authoring ────────────────────────────────────────
public class ZombieAuthoring : MonoBehaviour
{
    public float moveSpeed = 2f;
    public float maxHealth = 100f;
    public float damagePerSecond = 10f;
    public float steeringSpeed = 5f;

    public class Baker : Baker<ZombieAuthoring>
    {
        public override void Bake(ZombieAuthoring authoring)
        {
            Entity entity = GetEntity(TransformUsageFlags.Dynamic);

            AddComponent(entity, new Zombie());

            AddComponent(entity, new MoveSpeed
            {
                moveSpeed = authoring.moveSpeed
            });

            AddComponent(entity, new Health
            {
                currentHealth = authoring.maxHealth,
                maxHealth = authoring.maxHealth
            });

            AddComponent(entity, new ZombieDamage
            {
                damagePerSecond = authoring.damagePerSecond
            });

            AddComponent(entity, new ZombieTypeIndex
            {
                index = 0
            });

            AddComponent(entity, new ZombieVisual
            {
                spriteIndex = 0,
                needsUpdate = false
            });

            AddComponent(entity, new ZombieSteering
            {
                currentDirection = float3.zero,
                steeringSpeed = authoring.steeringSpeed
            });
        }
    }
}