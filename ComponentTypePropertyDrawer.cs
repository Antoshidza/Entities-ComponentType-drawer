using System;
using System.Collections.Generic;
using System.Linq;
using Unity.Entities;
using UnityEditor;
using UnityEngine;

namespace TonyMax.Entities.Extensions.ComponentTypeDrawer
{
    [CustomPropertyDrawer(typeof(ComponentTypeContainer))]
    internal class ComponentTypePropertyDrawer : PropertyDrawer
    {
        //cached type list here, because searching and filteting whole types in unity project (and editor) may be havy
        private List<Type> _typeList;

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);
            EditorGUI.PrefixLabel(position, new GUIContent(property.displayName));

            //Lazy type list initialization
            if(_typeList == null)
            {
                if(TryGetTypesFromCustomTypeProvider(out var customTypes))
                    _typeList = customTypes.ToList();
                else
                    _typeList = DefaultTypesProvider.GetDefaultTypes().ToList();
            }

            //If _typeList still null OR it is zero size then it seems there is no types to assign to ComponentType
            if(_typeList == null || _typeList.Count == 0)
                EditorGUI.HelpBox(position, "There is no types for ComponentType in project", MessageType.Warning);

            //Get TypeIndex field and restore type from TypeManager
            var typeIndexProperty = property.FindPropertyRelative("TypeIndex");
            var currentType = TypeManager.GetType(typeIndexProperty.intValue);

            //if _typeList doesn't contain this type then just add it (let's not be picky)
            var currentTypeIndex = _typeList.IndexOf(currentType);
            if(currentTypeIndex == -1)
            {
                _typeList.Add(currentType);
                currentTypeIndex = _typeList.Count - 1;
            }
            string[] GetTypeNames()
            {
                var names = new string[_typeList.Count];
                for(int i = 0; i < names.Length; i++)
                    names[i] = _typeList[i]?.Name;
                return names;
            }

            //expose all types as selectable dropdown menu (which is being called popup in EditorGUI)
            var popupRect = new Rect(position.x, position.y + 20, position.width, position.height);
            currentTypeIndex = EditorGUI.Popup(popupRect, currentTypeIndex, GetTypeNames());

            //reassign TypeIndex value back to our serialized property
            typeIndexProperty.intValue = TypeManager.GetTypeIndex(_typeList[currentTypeIndex]);

            property.serializedObject.ApplyModifiedProperties();

            EditorGUI.EndProperty();
        }
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return 40f;
        }

        private static bool TryGetTypesFromCustomTypeProvider(out IEnumerable<Type> resultTypes)
        {
            var customProviders = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(assembly => assembly.GetTypes())
                .Where(type =>
                {
                    //Filter only types that belongs to DOTS.ECS workflow
                    return type.GetInterfaces().Contains(typeof(ITypesProvider));
                }).ToArray();

            if(customProviders != null && customProviders.Length != 0)
            {
                resultTypes = (Activator.CreateInstance(customProviders[0]) as ITypesProvider).GetTypes();
                return true;
            }
            resultTypes = null;
            return false;
        }
    }
}