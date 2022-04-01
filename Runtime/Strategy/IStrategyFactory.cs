#nullable enable

namespace TeamZero.SceneManagement
{
    public interface IStrategyFactory<out T>
    {
        T Create(SceneView view);
    }
}
