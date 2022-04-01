#nullable enable
using System;

namespace TeamZero.SceneManagement
{
    public class CustomAnimatorFactory : IAnimatorFactory
    {
        private readonly Func<SceneView, IViewAnimator> _func;

        public static CustomAnimatorFactory Create(Func<SceneView, IViewAnimator> func)
            => new CustomAnimatorFactory(func);

        private CustomAnimatorFactory(Func<SceneView, IViewAnimator> func)
        {
            _func = func;
        }
        
        public IViewAnimator CreateAnimator(SceneView view) => _func.Invoke(view);
    }
}
