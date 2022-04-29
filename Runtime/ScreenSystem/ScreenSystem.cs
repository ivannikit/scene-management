#nullable enable
using Cysharp.Threading.Tasks;

namespace TeamZero.SceneManagement.ScreenManagement
{
    public class ScreenSystem
    {
        private readonly SceneLayer _layer;
        private AnimateView? _view;
        
        public static ScreenSystem CreateOnUnityUI(int order)
        {
            var placement = ScenePlacement.CreateOnUnityUI();
            var layer = placement.CreateLayer(order);
            return new ScreenSystem(layer);
        }

        private ScreenSystem(SceneLayer layer)
        {
            _layer = layer;
        }

        public bool ActiveTransition() => _activeTransition;
        private bool _activeTransition = false;

        public async UniTask<ISceneView?> SwitchToAsync(string sceneName, IScreenTransition transition)
        {
            if (!_activeTransition)
            {
                _activeTransition = true;
                
                SceneView next = _layer.CreateView(sceneName);
                _view = await transition.Switch(next, _view);
                _activeTransition = false;
                return _view;
            }
            
            LogSystem.Main.Error($"Switch transition in progress, ignore switch to {sceneName}");
            return null;
        }
    }
}
