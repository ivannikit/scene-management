#nullable enable
using UnityEngine;

namespace TeamZero.SceneManagement.UnityUI
{
    public class CanvasVisibleStrategy : IVisibleStrategy
    {
        private readonly SceneView _view;

        public static CanvasVisibleStrategy Create(SceneView view) 
            => new CanvasVisibleStrategy(view);
        
        private CanvasVisibleStrategy(SceneView view)
        {
            _view = view;
        }

        private bool _visible = false;
        public bool Visible() => _visible;

        public void SetVisible(bool value)
        {
            _visible = value;
            Rebuild();
        }

        private void Rebuild()
        {
            if (_view.GetRootObject(out GameObject? root))
                root.SetActive(_visible);
        }
    }
}
