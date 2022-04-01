using Cysharp.Threading.Tasks;

#nullable enable

namespace TeamZero.SceneManagement
{
    public interface IViewAnimator
    {
        UniTask ShowAsync(bool force);
        UniTask HideAsync(bool force);
    }
}
