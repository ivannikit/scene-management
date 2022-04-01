#nullable enable
using UnityEngine;
using UnityEngine.UI;

namespace TeamZero.SceneManagement.UnityUI
{
    public class CanvasUserInputStrategy : IUserInputStrategy
    {
        private readonly SceneView _view;
        private GraphicRaycaster? _raycaster;

        public static CanvasUserInputStrategy Create(SceneView view) 
            => new CanvasUserInputStrategy(view);
        
        private CanvasUserInputStrategy(SceneView view)
        {
            _view = view;
        }

        private bool _interactable = false;
        public bool Interactable() => _interactable;

        public void SetInteractable(bool value)
        {
            _interactable = value;
            InitRaycaster();
            if (_raycaster is { })
                _raycaster.enabled = value;
        }

        private void InitRaycaster()
        {
            if (_raycaster is null && _view.Loaded())
            {
                if (_view.GetRootObject(out GameObject? root))
                {
                    _raycaster = root.GetComponent<GraphicRaycaster>();
                    if (_raycaster is { })
                        _raycaster.enabled = _interactable;
                }
            }
        }
    }
}
