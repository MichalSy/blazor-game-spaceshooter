using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using SpaceShooter.Game.Components.Enemy;
using SpaceShooter.Game.Components.Player;
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

        private string _collisionTest = string.Empty;

        protected bool _showCollider = false;

        [Inject]
        public IJSRuntime? JSRuntime { get; set; }

        private void StopGame()
        {
            _isRunning = false;
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
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
        public void RenderInBlazor(float _)
        {
            Update();

            Render();
        }


        public void StartGame()
        {
            _isRunning = true;

            _views.Clear();
            _views.Add(new PlayerViewModel());
        }

        public void ToggleCollider()
        {
            _showCollider = !_showCollider;
        }

        private DateTime _lastAddEnemy = DateTime.MinValue;

        private void Update()
        {
            // cleanup dead game objects
            _views = _views.Where(v => !v.DestroyGameObject).ToList();

            if (_lastAddEnemy.AddMilliseconds(500) < DateTime.Now)
            {
                _views.Insert(0, new EnemyViewModel());
                _lastAddEnemy = DateTime.Now;
            }

            var colliderObjects = _views.Where(v => v is IRectCollider rectCollider && rectCollider.IsColliderActive).Select(colliderGameObject =>
            {
                var colliderPolygon = (colliderGameObject as IRectCollider)!.ColliderPolygon;
                colliderPolygon.BuildEdges();

                return (colliderGameObject, colliderPolygon);
            });

            _collisionTest = "NO";
            foreach (var (colliderGameObject, colliderPolygon) in colliderObjects.Where(c => c.colliderGameObject is PlayerViewModel))
            {
                foreach (var item2 in colliderObjects)
                {
                    if (colliderGameObject == item2.colliderGameObject)
                        continue;

                    var collisionCheck = PolygonCollision(colliderPolygon, item2.colliderPolygon);
                    if (collisionCheck)
                    {
                        (colliderGameObject as IRectCollider)!.CollideWith(item2.colliderGameObject);
                        (item2.colliderGameObject as IRectCollider)!.CollideWith(colliderGameObject);
                        _collisionTest = "YES";
                    }
                }
            }

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

        private void TouceMove(TouchEventArgs touchEventArgs)
        {
            _currentMousePosition = new(Math.Max((int)touchEventArgs.Touches[0].ClientX, 0), Math.Max((int)touchEventArgs.Touches[0].ClientY, 0));
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
