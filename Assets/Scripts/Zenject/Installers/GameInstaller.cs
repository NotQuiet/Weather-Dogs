using System;
using Custom;
using MVC.Abstract;
using UnityEngine;

namespace Zenject.Installers
{
    public class GameInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<EventBus>().AsSingle().NonLazy();
            
            ViewBase[] views = FindObjectsByType<ViewBase>((FindObjectsSortMode)FindObjectsInactive.Include);
            
            Debug.Log("Starting binding process for views...");

            foreach (var view in views)
            {
                Type viewType = view.GetType();
                Debug.Log($"Processing view: {view.name} (Type: {viewType.Name})");

                Type genericViewType = FindGenericViewType(viewType);
                Type[] genericArguments = genericViewType.GetGenericArguments();
                Type modelType = genericArguments[0];    
                Type viewBaseType = genericArguments[1]; 
                Type controllerType = genericArguments[2]; 

                Debug.Log($"Found generic View<TM, TV, TC>: TM = {modelType.Name}, TV = {viewBaseType.Name}, TC = {controllerType.Name}");

                Debug.Log($"Binding model: {modelType.Name} as singleton");
                Container.Bind(modelType).AsSingle();

                Debug.Log($"Binding view: {viewType.Name} from hierarchy object '{view.name}'");
                Container.Bind(viewType)
                    .FromComponentInHierarchy(view.gameObject)
                    .AsSingle();

                Debug.Log($"Binding controller: {controllerType.Name} as singleton");
                Container.Bind(controllerType).AsSingle();
            }

            Debug.Log("Binding process completed.");
        }

        private Type FindGenericViewType(Type type)
        {
            Type currentType = type;
            while (currentType != null && currentType != typeof(object))
            {
                if (currentType.IsGenericType && currentType.GetGenericTypeDefinition() == typeof(View<,,>))
                {
                    return currentType;
                }
                currentType = currentType.BaseType;
            }
            throw new InvalidOperationException($"Type {type.Name} does not inherit from View<TM, TV, TC>");
        }
    }
}