using Microsoft.JSInterop;

namespace SpaceShooter.Game.Models;

public class GameEnvironment
{
    public static readonly Size WindowSize = new();
    public static readonly Position MousePosition = new();
    private static IJSRuntime? _runtime;

    public static readonly List<GameObject> GameObjects = new();

    public static void Init(IJSRuntime runtime, Size windowSize)
    {
        _runtime = runtime;
        WindowSize.Width = windowSize.Width;
        WindowSize.Height = windowSize.Height;
    }

    public static void UpdateMouse(Position position)
    {
        MousePosition.X = position.X;
        MousePosition.Y = position.Y;
    }

    public static async void RemoveAllSounds()
    {
        if (_runtime != null)
        {
            await _runtime.InvokeVoidAsync("removeAudios");
        }
    }

    public static async void PlaySound(string filename, float volume = 1, bool loop = false)
    {
        if (_runtime != null)
        {
            await _runtime.InvokeVoidAsync("playsound", filename, volume, loop);
        }
    }

}