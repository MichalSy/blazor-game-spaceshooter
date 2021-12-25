using Microsoft.JSInterop;

namespace SpaceShooter.Game.Models;

public class GameEnvironment
{
    private static readonly GameEnvironment _instance = new();
    public static GameEnvironment Instance => _instance;

    private IJSRuntime? _runtime;

    public void Init(IJSRuntime runtime)
    {
        _runtime = runtime;
    }

    public void PlaySound(string filename, float volume = 1, bool loop = false)
    {
        _runtime!.InvokeAsync<object>("playsound", filename, volume, loop);
    }

}