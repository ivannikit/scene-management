#nullable enable
using System;
using System.Diagnostics.CodeAnalysis;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;


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

        public bool Loaded() => _loading && !_processUnload.HasValue;
        private bool _loading = false;

        private UniTask? _processLoad;
        public async UniTask LoadAsync()
        {
            if (!_loading)
            {
                _loading = true;
                
                if (_processUnload.HasValue)
                    await _processUnload.Value;

                _processLoad = SceneManager.LoadSceneAsync(_sceneName).ToUniTask();
                await _processLoad.Value;
                _processLoad = null;
            }
            else
            {
                LogSystem.Main.Warning($"{nameof(SceneView)} already loaded or in process loading");
            }
        }
        
        private UniTask? _processUnload;
        public async UniTask UnloadAsync()
        {
            if (_loading)
            {
                _loading = false;
                
                if (_processLoad.HasValue)
                    await _processLoad.Value;

                _processUnload = SceneManager.UnloadSceneAsync(_sceneName).ToUniTask();
                await _processUnload.Value;
                _processUnload = null;
            }
            else
            {
                LogSystem.Main.Warning($"{nameof(SceneView)} already unloaded or in process unloading");
            }
        }

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
