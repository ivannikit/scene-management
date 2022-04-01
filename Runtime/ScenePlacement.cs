using TeamZero.Core.Logging;
using TeamZero.SceneManagement.UnityUI;

#nullable enable

namespace TeamZero.SceneManagement
{
    public class ScenePlacement
    {
        private readonly IStrategyFactory<IOrderStrategy> _orderFactory;
        private readonly IStrategyFactory<IVisibleStrategy> _visibleFactory;
        private readonly IStrategyFactory<IUserInputStrategy> _userInputFactory;

        public static ScenePlacement CreateOnUnityUI(Log log) => new ScenePlacement(
            CanvasOrderFactory.Create(), CanvasVisibleFactory.Create(), CanvasUserInputFactory.Create());
        
        private ScenePlacement(
            IStrategyFactory<IOrderStrategy> orderFactory, 
            IStrategyFactory<IVisibleStrategy> visibleFactory, 
            IStrategyFactory<IUserInputStrategy> userInputFactory)
        {
            _orderFactory = orderFactory;
            _visibleFactory = visibleFactory;
            _userInputFactory = userInputFactory;
        }

        public SceneLayer CreateLayer(int order) 
            => SceneLayer.Create(order, _orderFactory, _visibleFactory, _userInputFactory);
    }
}
