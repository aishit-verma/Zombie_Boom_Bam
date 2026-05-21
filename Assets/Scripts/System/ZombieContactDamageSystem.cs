using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

public partial struct ZombieContactDamageSystem : ISystem
{
    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<PlayerPosition>();
    }

    public void OnUpdate(ref SystemState state)
    {
        if (ZombieContactBridge.Instance == null) return;
        if (PlayerHealth.Instance == null) return;

        float3 playerPos =
            SystemAPI.GetSingleton<PlayerPosition>().position;

        float contactRadius = 0.6f;
        float damagePerSecond = 10f;
        float deltaTime = SystemAPI.Time.DeltaTime;

        foreach (var (localTransform, moveSpeed) in
            SystemAPI.Query<
                RefRO<LocalTransform>,
                RefRO < MoveSpeed >> ()
            .WithAll<Zombie>())
        {
            float distance = math.distance(
                localTransform.ValueRO.Position, playerPos);

            if (distance < contactRadius)
            {
                ZombieContactBridge.Instance.DealDamage(
                    damagePerSecond * deltaTime);
                break;
            }
        }
    }

    public void OnDestroy(ref SystemState state) { }
}