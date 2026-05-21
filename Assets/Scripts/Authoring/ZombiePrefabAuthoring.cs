using Unity.Entities;
using UnityEngine;

public struct ZombiePrefab : IComponentData
{
    public Entity zombiePrefab;
}

public class ZombiePrefabAuthoring : MonoBehaviour
{
    public GameObject zombiePrefab;

    public class Baker : Baker<ZombiePrefabAuthoring>
    {
        public override void Bake(ZombiePrefabAuthoring authoring)
        {
            Entity entity = GetEntity(TransformUsageFlags.None);
            AddComponent(entity, new ZombiePrefab
            {
                zombiePrefab = GetEntity(authoring.zombiePrefab,
                    TransformUsageFlags.Dynamic)
            });
        }
    }
}