# Entities ComponentType drawer
 
This package lets you easy authorize ComponentType from inspector.

Since ComponentType isn't serializable struct there is no possible to use custom property drawer. But ComponentType is just 2 simple fields: int TypeIndex and enum AccessMode. To be able to serialize this data package uses ComponentTypeContainer which is just struct holding int TypeIndex and can be implicetelly converted to ComponentType. So speaking truth package provides property drawer for ComponentTypContainer.

By default you can select any type which implements any of these interfaces
* `IComponentData`
* `IBufferElementData`
* `ISharedComponentData`
* `ISystemStateComponentData`
* `ISystemStateBufferElementData`
* `ISystemStateSharedComponentData`

But you can define your own list of types you want to expose in editor. To do it you need simply define non-static class which implements ITypesProvider interface. For example:
```cs
public class CompTypeDrawerTypeProvider : ITypesProvider
{
   public IEnumerable<Type> GetTypes()
   {
      return new Type[]
      {
         typeof(Health),
         typeof(Mana)
      }
   }
}
```

Using example
```cs
public class AddComponentOnTimerAuthoring : Monobehaviour, IConvertGameObjectToEntity
{
   [SerializeField]
   private float _timerDuration;
   [SerializeField]
   private ComponentTypeContainer _componentType;

   public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
   {
      dstManager.AddComponentData(entity, new Timer { value = _timerDuration });
      dstManager.AddComponentData(entity, new AddComponetData { componentType = _componentType });
   }
}
```
![image](https://user-images.githubusercontent.com/19982288/125196363-5c692880-e262-11eb-98c6-8f06f174e73e.png)
