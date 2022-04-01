using Cysharp.Threading.Tasks;

#nullable enable

namespace TeamZero.SceneManagement
{
    public class DummyAnimator : IViewAnimator
    {
        private readonly SceneView _view;

        public static DummyAnimator Create(SceneView view) => new DummyAnimator(view);
        
        private DummyAnimator(SceneView view)
        {
            _view = view;
        }
        
#pragma warning disable 1998
        public async UniTask ShowAsync(bool force = false)
#pragma warning restore 1998
        {
            //TODO
            _view.SetInteractable(true);
            _view.SetVisible(true);
        }

#pragma warning disable 1998
        public async UniTask HideAsync(bool force = false)
#pragma warning restore 1998
        {
            //TODO
            _view.SetInteractable(false);
            _view.SetVisible(false);
        }
    }
}
