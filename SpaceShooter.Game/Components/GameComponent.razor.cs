using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using SpaceShooter.Game.Components.Enemy;
using SpaceShooter.Game.Components.Player;
using SpaceShooter.Game.Components.Shoot;
using SpaceShooter.Game.Models;
using static SpaceShooter.Game.Collision.CollisionCheck;

namespace SpaceShooter.Game.Components
{
    partial class GameComponent : ComponentBase
    {
        private bool _isRunning;
        private int _width = 600;
        private int _height = 800;
        private int _fpsCount;
        private int _fpsCounter;
        private DateTime _lastFPSTime = DateTime.MinValue;

        private MousePoint _currentMousePosition = new(0, 0);

        private IList<RenderGameObject> _views = new List<RenderGameObject>();

        protected bool _showCollider = false;

        private PlayerViewModel? _currentPlayer;

        [Inject]
        public IJSRuntime? JSRuntime { get; set; }

        private void StopGame()
        {
            _isRunning = false;
            JSRuntime!.InvokeAsync<object>("removeAudios");
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                GameEnvironment.Instance.Init(JSRuntime!);
                await JSRuntime!.InvokeAsync<object>("initRenderJS", DotNetObjectReference.Create(this));
                await base.OnInitializedAsync();
            }
        }

        [JSInvokable]
        public void ResizeInBlazor(double width, double height)
        {
            _width = (int)width;
            _height = (int)height;
        }

        [JSInvokable]
        public void TouchMoveInBlazor(double posX, double posY)
        {
            _currentMousePosition = new(Math.Max((int)posX, 0), Math.Max((int)posY, 0));
        }

        [JSInvokable]
        public void RenderInBlazor(float _)
        {
            Update();

            Render();
        }


        public void StartGame()
        {
            _isRunning = true;

            _views.Clear();
            _currentPlayer = new PlayerViewModel();
            _views.Add(_currentPlayer);

            GameEnvironment.Instance.PlaySound("background.mp3", 1, true);
        }

        public void ToggleCollider()
        {
            _showCollider = !_showCollider;
        }

        private DateTime _lastAddEnemy = DateTime.MinValue;
        private DateTime _lastAddShot = DateTime.MinValue;

        private void Update()
        {
            if (!_isRunning)
                return;

            // cleanup dead game objects
            _views = _views.Where(v => !v.DestroyGameObject).ToList();

            if (_lastAddEnemy.AddMilliseconds(500) < DateTime.Now)
            {
                _views.Insert(0, new EnemyViewModel());
                _lastAddEnemy = DateTime.Now;
            }

            if (_currentPlayer != null && _lastAddShot.AddMilliseconds(400) < DateTime.Now)
            {
                _views.Insert(0, new ShotViewModel(_currentPlayer.Position.X + (_currentPlayer.Size.Width / 2), _currentPlayer.Position.Y));
                _lastAddShot = DateTime.Now;
            }

            // calculate edges for collision detection
            foreach (var currentView in _views)
            {
                if (currentView is not ICollider rectCollider || !rectCollider.IsColliderActive)
                    continue;

                rectCollider.ColliderPolygon.BuildEdges();
            }

            // check for collisions
            foreach (var currentView in _views)
            {
                if (currentView is not IColliderAgent agent)
                    continue;

                foreach (var checkView in _views)
                {
                    if (checkView is not ICollider rectCollider || !rectCollider.IsColliderActive)
                        continue;

                    if (checkView is IColliderAgent)
                        continue;

                    var collisionCheck = PolygonCollision(agent.ColliderPolygon, rectCollider.ColliderPolygon);
                    if (collisionCheck)
                    {
                        agent.CollideWith(checkView);
                        rectCollider.CollideWith((RenderGameObject)agent);
                    }
                }
            }

            // update game objects
            foreach (var item in _views)
            {
                item.WindowSize.Width = _width;
                item.WindowSize.Height = _height;
                item.MousePosition = _currentMousePosition;
                item.Update();
            }

            if (_lastFPSTime.AddSeconds(1) < DateTime.Now)
            {
                _fpsCount = _fpsCounter;
                _fpsCounter = 0;
                _lastFPSTime = DateTime.Now;
            }
        }

        private void MouseMove(MouseEventArgs mouseEventArgs)
        {
            _currentMousePosition = new(Math.Max((int)mouseEventArgs.OffsetX, 0), Math.Max((int)mouseEventArgs.OffsetY, 0));
        }

        private DateTime _lastUpdateTimer = DateTime.MinValue;
        private void Render()
        {
            if (_lastUpdateTimer.AddMilliseconds(10) < DateTime.Now)
            {
                StateHasChanged();
                _lastUpdateTimer = DateTime.Now;
            }
            _fpsCounter++;
        }

        private static RenderFragment RenderWidget(RenderGameObject gameObject) => builder =>
        {
            builder.OpenComponent(0, gameObject.ViewType);
            builder.AddAttribute(1, "Model", gameObject);
            builder.CloseComponent();
        };
    }
}
