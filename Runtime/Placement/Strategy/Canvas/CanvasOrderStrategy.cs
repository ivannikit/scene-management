#nullable enable
using UnityEngine;

namespace TeamZero.SceneManagement.UnityUI
{
    public class CanvasOrderStrategy : IOrderStrategy
    {
        private readonly SceneView _view;
        private Canvas? _canvas;

        public static CanvasOrderStrategy Create(SceneView view) 
            => new CanvasOrderStrategy(view);
        
        private CanvasOrderStrategy(SceneView view)
        {
            _view = view;
        }

        private int _order = 0;
        public int Order() => _order;
        
        public void SetOrder(int value)
        {
            _order = value;
            InitCanvas();
            if (_canvas is { })
                _canvas.sortingOrder = value;
        }
        
        private void InitCanvas()
        {
            if (_canvas is null && _view.Loaded())
            {
                if (_view.GetRootObject(out GameObject? root))
                {
                    _canvas = root.GetComponent<Canvas>();
                    if (_canvas is { })
                        _canvas.sortingOrder = _order;
                }
            }
        }
    }
}
