using System;
using System.Collections.Generic;
using MVC.Abstract;
using UnityEngine;

namespace Zenject.Installers
{
    public class GameInstaller : MonoInstaller
    {
        [SerializeField] private List<ViewBase> views;

        public override void InstallBindings()
        {
            Debug.Log("Starting binding process for views...");

            foreach (var view in views)
            {
                Type viewType = view.GetType();
                Debug.Log($"Processing view: {view.name} (Type: {viewType.Name})");

                // Находим generic-тип View<TM, TV, TC> в цепочке наследования
                Type genericViewType = FindGenericViewType(viewType);
                Type[] genericArguments = genericViewType.GetGenericArguments();
                Type modelType = genericArguments[0];     // TM (например, ButtonsModel)
                Type viewBaseType = genericArguments[1];  // TV (например, ButtonsView)
                Type controllerType = genericArguments[2]; // TC (например, ButtonsController)

                Debug.Log($"Found generic View<TM, TV, TC>: TM = {modelType.Name}, TV = {viewBaseType.Name}, TC = {controllerType.Name}");

                // Привязываем модель как синглтон
                Debug.Log($"Binding model: {modelType.Name} as singleton");
                Container.Bind(modelType).AsSingle();

                // Привязываем вью из иерархии
                Debug.Log($"Binding view: {viewType.Name} from hierarchy object '{view.name}'");
                Container.Bind(viewType)
                    .FromComponentInHierarchy(view.gameObject)
                    .AsSingle();

                // Привязываем контроллер, используя TC из View<TM, TV, TC>
                Debug.Log($"Binding controller: {controllerType.Name} as singleton");
                Container.Bind(controllerType).AsSingle();
            }

            Debug.Log("Binding process completed.");
        }

        // Метод для поиска generic-типа View<TM, TV, TC> в цепочке наследования
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