using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

public struct PlayerPosition : IComponentData
{
    public float3 position;
}

public class PlayerPositionAuthoring : MonoBehaviour
{
    public class Baker : Baker<PlayerPositionAuthoring>
    {
        public override void Bake(PlayerPositionAuthoring authoring)
        {
            Entity entity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent(entity, new PlayerPosition());
        }
    }
}