#nullable enable
using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;

namespace TeamZero.SceneManagement
{
    public class PopupSystem
    {
        private readonly SceneLayer _layer;
        private readonly Queue<AnimateView> _queue;
        private readonly Func<SceneView, IViewAnimator> _animatorFactory;
        private AnimateView? _view;

        public static PopupSystem CreateOnUnityUI(int order, int capacity = 2)
        {
            var placement = ScenePlacement.CreateOnUnityUI();
            var layer = placement.CreateLayer(order);
            return new PopupSystem(layer, DefaultAnimatorFactory, capacity);
        }

        private static IViewAnimator DefaultAnimatorFactory(SceneView view) => DummyAnimator.Create(view);

        private PopupSystem(SceneLayer layer, Func<SceneView, IViewAnimator> animatorFactory, int capacity)
        {
            _layer = layer;
            _animatorFactory = animatorFactory;
            _queue = new Queue<AnimateView>(capacity);
            _view = null;
        }

        public bool ViewActive() => _view != null;

        public async UniTask ShowAsync(string sceneName)
        {
            await EnqueueCurrentAsync();

            SceneView sceneView = _layer.CreateView(sceneName);
            IViewAnimator animator = _animatorFactory.Invoke(sceneView);
            _view = AnimateView.Create(sceneView, animator);
            await _view.LoadAsync();
            await _view.ShowAsync();
        }

        private async UniTask EnqueueCurrentAsync()
        {
            if (_view != null)
            {
                await _view.HideAsync();
                _queue.Enqueue(_view);
                _view = null;
            }
        }

        public async UniTask CloseAllAsync(bool force = false)
        {
            foreach (AnimateView view in _queue)
            {
#pragma warning disable 4014
                CloseAsync(view, true);
#pragma warning restore 4014
            }
            
            await CloseCurrentAsync(force);
        }

        public async UniTask CloseCurrentAsync(bool force = false)
        {
            if (_view != null)
            {
                await CloseAsync(_view, force);
                _view = null;
            }

            await ShowNextAsync(false);
        }

        private async UniTask CloseAsync(AnimateView view, bool force)
        {
            await view.HideAsync(force);
#pragma warning disable 4014
            view.UnloadAsync();
#pragma warning restore 4014
        }

        private async UniTask ShowNextAsync(bool force)
        {
            if (_view == null && _queue.Count != 0)
            {
                _view = _queue.Dequeue();
                await _view.ShowAsync(force);
            }
        }
    }
}
