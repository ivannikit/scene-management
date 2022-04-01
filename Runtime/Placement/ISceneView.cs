#nullable enable
using System.Diagnostics.CodeAnalysis;
using Cysharp.Threading.Tasks;
using UnityEngine;


namespace TeamZero.SceneManagement
{
    public interface ISceneView
    {
        bool Loaded();

        UniTask LoadAsync();

        UniTask UnloadAsync();

        bool GetRootObject([NotNullWhen(true)] out GameObject? root);

        bool Interactable();
        void SetInteractable(bool value);
        
        bool Visible();
        void SetVisible(bool value);
    }
}
