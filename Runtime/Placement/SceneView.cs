#nullable enable

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace TeamZero.SceneManagement
{
    public class SceneView : ISceneView
    {
        private readonly string _sceneName;
        private readonly int _order;
        private IOrderStrategy _orderStrategy = default!;
        private IVisibleStrategy _visibleStrategy = default!;
        private IUserInputStrategy _userInputStrategy = default!;

        public static SceneView Create(string sceneName, int order,
            IStrategyFactory<IOrderStrategy> orderFactory, 
            IStrategyFactory<IVisibleStrategy> visibleFactory, 
            IStrategyFactory<IUserInputStrategy> userInputFactory)
        {
            SceneView instance = new SceneView(sceneName, order);
            var orderStrategy = orderFactory.Create(instance);
            var visibleStrategy = visibleFactory.Create(instance);
            var userInputStrategy = userInputFactory.Create(instance);
            instance.Inject(orderStrategy, visibleStrategy, userInputStrategy);

            return instance;
        }

        private SceneView(string sceneName, int order)
        {
            if (string.IsNullOrEmpty(sceneName))
                throw new ArgumentException(nameof(sceneName));
            
            _sceneName = sceneName;
            _order = order;
        }

        private void Inject(IOrderStrategy orderStrategy, IVisibleStrategy visibleStrategy,
            IUserInputStrategy userInputStrategy)
        {
            _orderStrategy = orderStrategy;
            _visibleStrategy = visibleStrategy;
            _userInputStrategy = userInputStrategy;
            Rebuild();
        }

        private void Rebuild()
        {
            if (Loaded())
            {
                _orderStrategy.SetOrder(_order);
                _visibleStrategy.RefreshVisible();
                _userInputStrategy.RefreshInteractable();
            }
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
                
                _processLoad = SceneManager.LoadSceneAsync(_sceneName, LoadSceneMode.Additive).ToUniTask();
                await _processLoad.Value;
                
                _processLoad = null;
                FindScene();
                Rebuild();
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
                _root = null;
                _identifier = null;
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
                root = Root();
                result = root is { };
            }

            return result;
        }

        private GameObject? _root;
        private GameObject? Root()
        {
            if (Loaded())
            {
                if (_root is null)
                {
                    Scene scene = FindScene();
                    int rootCount = scene.rootCount;
                    if (rootCount != 0)
                    {
                        if(rootCount > 1)
                            LogSystem.Main.Warning("Scene have more than one root GameObject");
                        
                        List<GameObject> rootObjects = new(scene.rootCount);
                        scene.GetRootGameObjects(rootObjects);
                        _root = rootObjects[0];
                    }
                    else
                    {
                        LogSystem.Main.Error("Scene isn't have root GameObject");
                    }
                }
                
                return _root;
            }

            return null;
        }
        
        private SceneIdentifier? _identifier;
        private Scene FindScene()
        {
            int count = SceneManager.sceneCount;
            for (int i = count - 1; i >= 0; i--)
            {
                Scene scene = SceneManager.GetSceneAt(i);
                if (scene.name == _sceneName)
                {
                    int rootCount = scene.rootCount;
                    if (rootCount != 0)
                    {
                        if(rootCount > 1)
                            LogSystem.Main.Warning("Scene have more than one root GameObject");
                    
                        List<GameObject> rootObjects = new(scene.rootCount);
                        scene.GetRootGameObjects(rootObjects);
                        GameObject rootObject = rootObjects[0];
                        
                        SceneIdentifier identifierInScene = rootObject.GetComponent<SceneIdentifier>();
                        if (_identifier == null)
                        {
                            if (identifierInScene == null)
                            {
                                _identifier = rootObject.AddComponent<SceneIdentifier>();
                                return scene;
                            }
                        }
                        else if(_identifier == identifierInScene)
                        {
                            return scene;
                        }
                    }
                }
            }
            
            Debug.LogError($"loaded scene '{_sceneName}' not found");
            return SceneManager.GetSceneByName(_sceneName);
        }

        public bool Interactable() => _userInputStrategy.Interactable();
        public void SetInteractable(bool value) => _userInputStrategy.SetInteractable(value);
        
        public bool Visible() => _visibleStrategy.Visible();
        public void SetVisible(bool value) => _visibleStrategy.SetVisible(value);
    }
}
