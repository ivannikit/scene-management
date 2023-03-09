#nullable enable
using System;
using Cysharp.Threading.Tasks;

namespace TeamZero.SceneManagement.ScreenManagement
{
    public class LoadFistTransition : IScreenTransition
    {
        private readonly IAnimatorFactory _factory;
        
        public static LoadFistTransition Create(Func<SceneView, IViewAnimator> animatorFactory)
        {
            IAnimatorFactory factory = CustomAnimatorFactory.Create(animatorFactory);
            return new LoadFistTransition(factory);
        }

        private LoadFistTransition(IAnimatorFactory factory)
        {
            _factory = factory;
        }
        
        public async UniTask<AnimateView> Switch(SceneView inView, AnimateView? outView)
        {
            UniTask? unload = null;
            if (outView != null && outView.Loaded())
                unload = outView.UnloadAsync();

            IViewAnimator animator = _factory.CreateAnimator(inView);
            AnimateView view = AnimateView.Create(inView, animator);
            await view.LoadAsync();
            await view.ShowAsync();

            if (unload.HasValue)
                await unload.Value;

            return view;
        }
    }
}
