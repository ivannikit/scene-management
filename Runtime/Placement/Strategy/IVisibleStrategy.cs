#nullable enable

namespace TeamZero.SceneManagement
{
    public interface IVisibleStrategy
    {
        bool Visible();
        void SetVisible(bool value);
        void RefreshVisible();
    }
}
