#nullable enable

namespace TeamZero.SceneManagement.UnityUI
{
    public class CanvasUserInputFactory : IStrategyFactory<IUserInputStrategy>
    {
        public IUserInputStrategy Create(SceneView view) 
            => CanvasUserInputStrategy.Create(view);
        
        public static CanvasUserInputFactory Create() => new CanvasUserInputFactory();
        private CanvasUserInputFactory()
        {
        }
    }
}
