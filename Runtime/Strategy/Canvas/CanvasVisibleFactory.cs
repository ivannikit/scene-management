#nullable enable

namespace TeamZero.SceneManagement.UnityUI
{
    public class CanvasVisibleFactory : IStrategyFactory<IVisibleStrategy>
    {
        public IVisibleStrategy Create(SceneView view) 
            => CanvasVisibleStrategy.Create(view);
        
        public static CanvasVisibleFactory Create() => new CanvasVisibleFactory();
        private CanvasVisibleFactory()
        {
        }
    }
}
