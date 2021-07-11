using System;
using System.Collections.Generic;
using System.Linq;
using Unity.Entities;

namespace TonyMax.Entities.Extensions.ComponentTypeDrawer
{
    public static class DefaultTypesProvider
    {
        public static IEnumerable<Type> GetDefaultTypes()
        {
            return AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(assembly => assembly.GetTypes())
                .Where(type =>
                {
                    //Filter only types that belongs to DOTS.ECS workflow
                    var implementedInterfeces = type.GetInterfaces();
                    bool ContainsAnyInterface(params Type[] targetInterfaces)
                    {
                        foreach(var item in targetInterfaces)
                            if(implementedInterfeces.Contains(item))
                                return true;
                        return false;
                    }
                    return ContainsAnyInterface
                    (
                        typeof(IComponentData),
                        typeof(ISystemStateComponentData),
                        typeof(IBufferElementData),
                        typeof(ISystemStateBufferElementData),
                        typeof(ISharedComponentData),
                        typeof(ISystemStateSharedComponentData)
                    );
                });
        }
    }
}
