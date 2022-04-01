#nullable enable

namespace TeamZero.SceneManagement
{
    public interface IAnimatorFactory
    {
        IViewAnimator CreateAnimator(SceneView view);
    }
}
