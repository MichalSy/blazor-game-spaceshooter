using SpaceShooter.Game.Collision;
using SpaceShooter.Game.Models;

namespace SpaceShooter.Game.Components.Enemy;

public class EnemyViewModel : ImageGameObject, ICollider
{
    public override Type ViewType => typeof(EnemyView);
    public override string ImageName => "images/enemy.png";
    public override Size Size => new(58, 68);


    private readonly Polygon _colliderPolygon = new();
    public Polygon ColliderPolygon => _colliderPolygon;

    private bool _isColliderActive = true;
    public bool IsColliderActive => _isColliderActive && _posX > -Size.Height;

    private double _posX = 0;
    private double _posY = 0;

    private bool _isDestroyed = false;
    public float Opacity { get; set; } = 1;

    private float _speed;

    private static Random _random = new();

    public EnemyViewModel()
    {
        _posX = new Random().Next(50, GameEnvironment.WindowSize.Width - 100);
        _posY = -Size.Height;
        Position.X = (int)_posX;
        Position.Y = (int)_posY;
        
        _colliderPolygon.Points.Add(new Vector(0, 0));
        _colliderPolygon.Points.Add(new Vector(58, 0));
        _colliderPolygon.Points.Add(new Vector(47, 40));
        _colliderPolygon.Points.Add(new Vector(47, 68));
        _colliderPolygon.Points.Add(new Vector(12, 68));
        _colliderPolygon.Points.Add(new Vector(12, 40));
        _colliderPolygon.Offset((float)_posX, (float)_posY);

        _speed = (_random.NextSingle() * 3) + 3;
    }

    public override void Update(float time)
    {
        if (!_isDestroyed)
        {
            _posY += _speed;
            _colliderPolygon.Offset((float)_posX, (float)_posY);
            Position.X = (int)_posX;
            Position.Y = (int)_posY;
        }
        else
        {
            Opacity -= .05f;
            Opacity = Math.Max(Opacity, 0);

            if (Opacity == 0)
            {
                DestroyGameObject = true;
            }
        }

        if (_posY > GameEnvironment.WindowSize.Height + 10)
        {
            DestroyGameObject = true;
        }
    }

    public void CollideWith(RenderGameObject renderGameObject)
    {
        if (renderGameObject is IColliderAgent)
        {
            GameEnvironment.PlaySound("zap.ogg");
            _isColliderActive = false;
            _isDestroyed = true;
        }
    }
}
