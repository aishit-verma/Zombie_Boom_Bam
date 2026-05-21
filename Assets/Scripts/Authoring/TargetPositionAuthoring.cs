using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

public struct TargetPosition : IComponentData
{
    public float3 targetPosition;
}

public class TargetPositionAuthoring : MonoBehaviour
{
    public class Baker : Baker<TargetPositionAuthoring>
    {
        public override void Bake(TargetPositionAuthoring authoring)
        {
            Entity entity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent(entity, new TargetPosition
            {
                targetPosition = float3.zero
            });
        }
    }
}