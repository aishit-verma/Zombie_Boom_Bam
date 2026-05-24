using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using Unity.Burst;
using UnityEngine;

public partial struct ZombieSteeringSystem : ISystem
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

        foreach (var (localTransform, steering, moveSpeed) in
            SystemAPI.Query<
                RefRW<LocalTransform>,
                RefRW<ZombieSteering>,
                RefRO<MoveSpeed>>()
            .WithAll<Zombie>())
        {
            float3 zombiePos = localTransform.ValueRO.Position;

            // get best direction using context steering
            float3 bestDirection = GetBestDirection(
                zombiePos, playerPos);

            // smoothly rotate toward best direction
            steering.ValueRW.currentDirection = math.normalize(
                math.lerp(
                    steering.ValueRO.currentDirection,
                    bestDirection,
                    deltaTime * steering.ValueRO.steeringSpeed));

            // move in steered direction
            localTransform.ValueRW.Position +=
                steering.ValueRO.currentDirection *
                moveSpeed.ValueRO.moveSpeed * deltaTime;
        }
    }

    private float3 GetBestDirection(float3 zombiePos, float3 playerPos)
    {
        // 8 directions to check
        float3[] directions = new float3[]
        {
            new float3(1, 0, 0),
            new float3(-1, 0, 0),
            new float3(0, 1, 0),
            new float3(0, -1, 0),
            new float3(0.707f, 0.707f, 0),
            new float3(-0.707f, 0.707f, 0),
            new float3(0.707f, -0.707f, 0),
            new float3(-0.707f, -0.707f, 0)
        };

        float3 toPlayer = math.normalize(playerPos - zombiePos);
        float bestScore = float.MinValue;
        float3 bestDir = toPlayer;

        foreach (float3 dir in directions)
        {
            // score based on alignment with player direction
            float score = math.dot(dir, toPlayer);

            // cast ray to check for obstacles
            Vector2 origin = new Vector2(zombiePos.x, zombiePos.y);
            Vector2 direction = new Vector2(dir.x, dir.y);
            RaycastHit2D hit = Physics2D.Raycast(
                origin, direction, 3f);

            // reduce score if obstacle in this direction
            if (hit.collider != null &&
                !hit.collider.CompareTag("Player") &&
                !hit.collider.CompareTag("Enemy"))
            {
                score -= 5f;
            }

            if (score > bestScore)
            {
                bestScore = score;
                bestDir = dir;
            }
        }

        return bestDir;
    }

    public void OnDestroy(ref SystemState state) { }
}