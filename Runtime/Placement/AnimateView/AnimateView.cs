using Cysharp.Threading.Tasks;
using UnityEngine;

#nullable enable

namespace TeamZero.SceneManagement
{
    public class AnimateView : ISceneView
    {
        private readonly SceneView _view;
        private readonly IViewAnimator _animator;
        
        public static AnimateView CreateNotAnimated(SceneView view) 
            => new AnimateView(view, DummyAnimator.Create(view));
        
        public static AnimateView Create(SceneView view, IViewAnimator animator) 
            => new AnimateView(view, animator);

        private AnimateView(SceneView view, IViewAnimator animator)
        {
            _view = view;
            _animator = animator;
        }

        public async UniTask ShowAsync(bool force = false) 
            => await _animator.ShowAsync(force);
        public async UniTask HideAsync(bool force = false) 
            => await _animator.HideAsync(force);

        public bool Loaded() => _view.Loaded();

        public async UniTask LoadAsync() => await _view.LoadAsync();

        public async UniTask UnloadAsync() => await _view.UnloadAsync();

        public bool GetRootObject(out GameObject? root) => _view.GetRootObject(out root);

        public bool Interactable() => _view.Interactable();

        public void SetInteractable(bool value) => _view.SetInteractable(value);

        public bool Visible() => _view.Visible();

        public void SetVisible(bool value) => _view.SetVisible(value);
    }
}