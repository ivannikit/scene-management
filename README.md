# Scene Management SDK for UnityEngine

### Sample:
```csharp
ScreenSystem gameplayScreen = ScreenSystem.CreateOnUnityUI(order: 0);
ScreenSystem menuScreen = ScreenSystem.CreateOnUnityUI(order: 1);
IScreenTransition transition = ...

await menuScreen.SwitchToAsync("menu scene name", transition);
...

UniTask closeMenuTask = menuScreen.CloseCurrentView();
UniTask showGameplayTask = gameplayScreen.SwitchToAsync("gameplay scene name", transition);
await UniTask.WhenAll(closeMenuTask, showGameplayTask);
```