#nullable enable
using Cysharp.Threading.Tasks;

namespace TeamZero.SceneManagement.ScreenManagement
{
    public interface IScreenTransition
    {
        UniTask<AnimateView> Switch(SceneView inView, AnimateView? outView);
    }
}
