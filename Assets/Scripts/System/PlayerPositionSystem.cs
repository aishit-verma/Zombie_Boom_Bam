using Unity.Entities;
using Unity.Mathematics;
using Unity.Burst;
using UnityEngine;

[BurstCompile]
public partial struct PlayerPositionSystem : ISystem
{
    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<PlayerPosition>();
    }

    public void OnUpdate(ref SystemState state)
    {
        if (PlayerMovement.Instance == null) return;

        Vector3 pos = PlayerMovement.Instance.transform.position;

        RefRW<PlayerPosition> playerPosition =
            SystemAPI.GetSingletonRW<PlayerPosition>();

        playerPosition.ValueRW.position =
            new float3(pos.x, pos.y, pos.z);
    }

    [BurstCompile]
    public void OnDestroy(ref SystemState state) { }
}