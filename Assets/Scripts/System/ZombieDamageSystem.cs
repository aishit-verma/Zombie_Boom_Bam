using Unity.Entities;
using Unity.Burst;

[BurstCompile]
public partial struct ZombieDamageSystem : ISystem
{
    [BurstCompile]
    public void OnCreate(ref SystemState state) { }

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        // destroy zombies with 0 health
        EntityCommandBuffer ecb = new EntityCommandBuffer(
            Unity.Collections.Allocator.Temp);

        foreach (var (health, entity) in
            SystemAPI.Query<RefRO<Health>>()
            .WithAll<Zombie>()
            .WithEntityAccess())
        {
            if (health.ValueRO.currentHealth <= 0)
                ecb.DestroyEntity(entity);
        }

        ecb.Playback(state.EntityManager);
        ecb.Dispose();
    }

    [BurstCompile]
    public void OnDestroy(ref SystemState state) { }
}