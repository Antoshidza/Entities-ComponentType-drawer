using System;
using System.Collections.Generic;

namespace TonyMax.Entities.Extensions.ComponentTypeDrawer
{
    public interface ITypesProvider
    {
        public IEnumerable<Type> GetTypes();
    }
}
