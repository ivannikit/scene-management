#nullable enable

namespace TeamZero.SceneManagement.UnityUI
{
    public class CanvasOrderFactory : IStrategyFactory<IOrderStrategy>
    {
        public IOrderStrategy Create(SceneView view) 
            => CanvasOrderStrategy.Create(view);

        public static CanvasOrderFactory Create() => new CanvasOrderFactory();
        private CanvasOrderFactory()
        {
        }
    }
}
