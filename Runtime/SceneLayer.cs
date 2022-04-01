using TeamZero.Core.Logging;

#nullable enable

namespace TeamZero.SceneManagement
{
    public class SceneLayer
    {
        private readonly int _order;
        private readonly IStrategyFactory<IOrderStrategy> _orderFactory;
        private readonly IStrategyFactory<IVisibleStrategy> _visibleFactory;
        private readonly IStrategyFactory<IUserInputStrategy> _userInputFactory;

        internal static SceneLayer Create(int order,
            IStrategyFactory<IOrderStrategy> orderFactory,
            IStrategyFactory<IVisibleStrategy> visibleFactory,
            IStrategyFactory<IUserInputStrategy> userInputFactory)
            => new SceneLayer(order, orderFactory, visibleFactory, userInputFactory);

        private SceneLayer(int order,
            IStrategyFactory<IOrderStrategy> orderFactory, 
            IStrategyFactory<IVisibleStrategy> visibleFactory, 
            IStrategyFactory<IUserInputStrategy> userInputFactory)
        {
            _order = order;
            _orderFactory = orderFactory;
            _visibleFactory = visibleFactory;
            _userInputFactory = userInputFactory;
        }

        public SceneView CreateView(string sceneName)
        {
            if (string.IsNullOrEmpty(sceneName))
            {
                
            }
            
            return SceneView.Create(sceneName, _order, _orderFactory, _visibleFactory, _userInputFactory);
        }
    }
}
