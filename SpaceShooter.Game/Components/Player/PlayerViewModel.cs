using SpaceShooter.Game.Collision;
using SpaceShooter.Game.Components.Shoot;
using SpaceShooter.Game.Models;

namespace SpaceShooter.Game.Components.Player;

public class PlayerViewModel : ImageGameObject, IColliderAgent
{
    public override Type ViewType => typeof(PlayerView);
    public override string ImageName => "images/player.png";
    public override Size Size => new(98, 75);

    private readonly Polygon _colliderPolygon = new();
    public Polygon ColliderPolygon => _colliderPolygon;

    public bool IsColliderActive => _position.X > 0;

    private readonly Position _position = new (-500, -500);


    private float _lastAddShot = 0;

    public PlayerViewModel()
    {
        _position.X = (GameEnvironment.WindowSize.Width / 2) - (Size.Width / 2);
        _position.Y = (GameEnvironment.WindowSize.Height - 200);

        _colliderPolygon.Points.Add(new Vector(45, 0));
        _colliderPolygon.Points.Add(new Vector(53, 0));
        _colliderPolygon.Points.Add(new Vector(60, 15));
        _colliderPolygon.Points.Add(new Vector(98, 65));
        _colliderPolygon.Points.Add(new Vector(70, 65));
        _colliderPolygon.Points.Add(new Vector(62, 75));
        _colliderPolygon.Points.Add(new Vector(36, 75));
        _colliderPolygon.Points.Add(new Vector(28, 65));
        _colliderPolygon.Points.Add(new Vector(0, 65));
        _colliderPolygon.Points.Add(new Vector(38, 15));

        _lastAddShot = 0;
    }

    public override void Update(float time)
    {
        if (GameEnvironment.MousePosition.X != 0)
        {
            _position.X = GameEnvironment.MousePosition.X - (Size.Width / 2);
            _position.Y = GameEnvironment.MousePosition.Y - Size.Height - 30;
        }

        Position.X = _position.X;
        Position.Y = _position.Y;
        _colliderPolygon.Offset(Position.X, Position.Y);

        if (_lastAddShot + 300 < time)
        {
            GameEnvironment.GameObjects.Insert(0, new ShotViewModel(Position.X + (Size.Width / 2), Position.Y));
            _lastAddShot = time;
        }
    }

    public void CollideWith(RenderGameObject renderGameObject)
    {

    }
}
