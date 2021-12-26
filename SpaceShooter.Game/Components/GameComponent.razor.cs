using System.Diagnostics;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using SpaceShooter.Game.Components.Player;
using SpaceShooter.Game.Manager;
using SpaceShooter.Game.Models;


namespace SpaceShooter.Game.Components;

partial class GameComponent : ComponentBase
{
    const float FRAME_CAP_MS = 1000.0f / 40.0f;
    private bool _isRunning;
    private int _width = 600;
    private int _height = 800;
    private int _fpsCount;
    private int _fpsCounter;
    private float _lastFPSTime = 0;

    protected bool _showCollider = false;


    [Inject]
    public IJSRuntime? JSRuntime { get; set; }


    protected override void OnInitialized()
    {
        GameEnvironment.Init(JSRuntime!, new Size(_width, _height));
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender && JSRuntime != null)
        {
            await JSRuntime.InvokeVoidAsync("initRenderJS", DotNetObjectReference.Create(this));
        }
        await base.OnAfterRenderAsync(firstRender);
    }

    [JSInvokable]
    public void ResizeInBlazor(double width, double height)
    {
        _width = (int)width;
        _height = (int)height;
        GameEnvironment.Init(JSRuntime!, new Size(_width, _height));
        StateHasChanged();
    }

    [JSInvokable("TouchMoveInBlazor")]
    public void TouchMoveInBlazor(double posX, double posY)
    {
        GameEnvironment.UpdateMouse(new Position(Math.Max((int)posX, 0), Math.Max((int)posY, 0)));
    }

    private static void MouseMove(MouseEventArgs mouseEventArgs)
    {
        GameEnvironment.UpdateMouse(new(Math.Max((int)mouseEventArgs.OffsetX, 0), Math.Max((int)mouseEventArgs.OffsetY, 0)));
    }

    public void ToggleCollider()
    {
        _showCollider = !_showCollider;
    }

    public async void StartGame()
    {
        CleanupGame();

        _isRunning = true;

        GameEnvironment.GameObjects.Clear();
        GameEnvironment.GameObjects.Add(new PlayerViewModel());
        GameEnvironment.GameObjects.Add(new EnemyManager());
        GameEnvironment.GameObjects.Add(new CollisionManager());

        GameEnvironment.PlaySound("background.mp3", 0.4f, true);

        var watch = Stopwatch.StartNew();
        while (_isRunning)
        {
            var frameStartTime = watch.ElapsedMilliseconds;

            UpdateGameFrame(frameStartTime);

            RenderGameFrame();

            // FPS Cap
            var frameTime = watch.ElapsedMilliseconds - frameStartTime;
            var waitTime = Math.Max((int)(FRAME_CAP_MS - frameTime), 1);

            await Task.Delay(waitTime);
        }
    }

    private void CleanupGame()
    {
        _isRunning = false;
        GameEnvironment.UpdateMouse(new Position());
        GameEnvironment.RemoveAllSounds();
    }

    private void UpdateGameFrame(float time)
    {
        if (!_isRunning)
            return;

        // cleanup dead game objects
        GameEnvironment.GameObjects.RemoveAll(o => o.DestroyGameObject);

        // update game objects
        foreach (var item in GameEnvironment.GameObjects.ToArray())
        {
            item.Update(time);
        }

        // calculate FPS
        if (_lastFPSTime + 1000 < time)
        {
            _fpsCount = _fpsCounter;
            _fpsCounter = 0;
            _lastFPSTime = time;
        }
    }

    private void RenderGameFrame()
    {
        _fpsCounter++;
        StateHasChanged();
    }

    private RenderFragment RenderWidget(RenderGameObject gameObject) => builder =>
    {
        builder.OpenComponent(0, gameObject.ViewType);
        builder.AddAttribute(1, "Model", gameObject);
        builder.CloseComponent();
    };
}