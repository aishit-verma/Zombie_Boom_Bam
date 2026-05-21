using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

public partial struct ZombieVisualSyncSystem : ISystem
{
    public void OnCreate(ref SystemState state) { }

    public void OnUpdate(ref SystemState state)
    {
        if (ZombieVisualRegistry.Instance == null) return;

        foreach (var (visual, entity) in
            SystemAPI.Query<RefRW<ZombieVisual>>()
            .WithAll<Zombie>()
            .WithEntityAccess())
        {
            if (!visual.ValueRO.needsUpdate) continue;

            if (state.EntityManager
                .HasComponent<SpriteRenderer>(entity))
            {
                SpriteRenderer sr = state.EntityManager
                    .GetComponentObject<SpriteRenderer>(entity);

                // // set color
                // float4 c = visual.ValueRO.color;
                // sr.color = new Color(c.x, c.y, c.z, c.w);

                // set sprite if available
                Sprite sprite = ZombieVisualRegistry.Instance
                    .GetSprite(visual.ValueRO.spriteIndex);
                if (sprite != null)
                    sr.sprite = sprite;
            }

            visual.ValueRW.needsUpdate = false;
        }
    }

    public void OnDestroy(ref SystemState state) { }
}