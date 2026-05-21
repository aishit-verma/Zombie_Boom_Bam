using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using Unity.Burst;

[BurstCompile]
public partial struct ZombieMoveSystem : ISystem
{
    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<PlayerPosition>();
    }

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        float deltaTime = SystemAPI.Time.DeltaTime;

        float3 playerPosition =
            SystemAPI.GetSingleton<PlayerPosition>().position;

        foreach (var (localTransform, moveSpeed) in
            SystemAPI.Query<
                RefRW<LocalTransform>,
                RefRO<MoveSpeed>>()
            .WithAll<Zombie>())
        {
            float3 direction = math.normalize(
                playerPosition - localTransform.ValueRO.Position);

            localTransform.ValueRW.Position +=
                direction * moveSpeed.ValueRO.moveSpeed * deltaTime;
        }
    }

    [BurstCompile]
    public void OnDestroy(ref SystemState state) { }
}