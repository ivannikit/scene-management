#nullable enable
using System;
using System.Diagnostics.CodeAnalysis;
using UnityEngine;


namespace TeamZero.SceneManagement
{
    public class SceneView
    {
        private readonly string _sceneName;
        private readonly int _order;
        private IOrderStrategy _orderStrategy = default!;
        private IVisibleStrategy _visibleStrategy = default!;
        private IUserInputStrategy _userInputStrategy = default!;
        
        private GameObject? _root;

        public static SceneView Create(string sceneName, int order,
            IStrategyFactory<IOrderStrategy> orderFactory, 
            IStrategyFactory<IVisibleStrategy> visibleFactory, 
            IStrategyFactory<IUserInputStrategy> userInputFactory)
        {
            SceneView instance = new SceneView(sceneName, order);
            instance._orderStrategy = orderFactory.Create(instance);
            instance._visibleStrategy = visibleFactory.Create(instance);
            instance._userInputStrategy = userInputFactory.Create(instance);

            return instance;
        }

        private SceneView(string sceneName, int order)
        {
            if (string.IsNullOrEmpty(sceneName))
                throw new ArgumentException(nameof(sceneName));
            
            _sceneName = sceneName;
            _order = order;
        }

        public bool Loaded() => false;

        public bool GetRootObject([NotNullWhen(true)] out GameObject? root)
        {
            bool result = false;
            root = null;
            if (Loaded())
            {
                root = _root;
            }

            return result;
        }
    }
}
