using Unity.Entities;

namespace TonyMax.Entities.Extensions.ComponentTypeDrawer
{
    [System.Serializable]
    public struct ComponentTypeContainer
    {
        public int TypeIndex;

        public static implicit operator ComponentType(ComponentTypeContainer container)
        {
            return new ComponentType(TypeManager.GetType(container.TypeIndex));
        }
    }
}
