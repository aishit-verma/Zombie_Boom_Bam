using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

public partial struct ZombieSeparationSystem : ISystem
{
    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<PlayerPosition>();
    }

    public void OnUpdate(ref SystemState state)
    {
        float deltaTime = SystemAPI.Time.DeltaTime;
        float3 playerPos =
            SystemAPI.GetSingleton<PlayerPosition>().position;

        float zombieRadius = 1f;
        float playerRadius = 1f;
        float separationForce = 5f;

        // get all zombie positions first
        var query = SystemAPI.QueryBuilder()
            .WithAll<Zombie, LocalTransform>()
            .Build();

        var transforms = query.ToComponentDataArray<LocalTransform>(
            Unity.Collections.Allocator.Temp);
        var entities = query.ToEntityArray(
            Unity.Collections.Allocator.Temp);

        for (int i = 0; i < entities.Length; i++)
        {
            float3 pos = transforms[i].Position;
            float3 separation = float3.zero;

            // push away from player
            float distToPlayer = math.distance(pos, playerPos);
            if (distToPlayer < playerRadius && distToPlayer > 0f)
            {
                float3 pushDir = math.normalize(pos - playerPos);
                float pushStrength = (playerRadius - distToPlayer) 
                    / playerRadius;
                separation += pushDir * pushStrength * separationForce;
            }

            // push away from other zombies
            for (int j = 0; j < entities.Length; j++)
            {
                if (i == j) continue;

                float3 otherPos = transforms[j].Position;
                float dist = math.distance(pos, otherPos);

                if (dist < zombieRadius && dist > 0f)
                {
                    float3 pushDir = math.normalize(pos - otherPos);
                    float pushStrength = (zombieRadius - dist) 
                        / zombieRadius;
                    separation += pushDir * pushStrength * separationForce;
                }
            }

            // apply separation
            if (!separation.Equals(float3.zero))
            {
                LocalTransform t = transforms[i];
                t.Position += separation * deltaTime;
                SystemAPI.GetComponentRW<LocalTransform>(entities[i])
                    .ValueRW = t;
            }
        }

        transforms.Dispose();
        entities.Dispose();
    }

    public void OnDestroy(ref SystemState state) { }
}