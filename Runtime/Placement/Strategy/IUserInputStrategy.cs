#nullable enable

namespace TeamZero.SceneManagement
{
    public interface IUserInputStrategy
    {
        bool Interactable();
        void SetInteractable(bool value);
        void RefreshInteractable();
    }
}
