using SpaceShooter.Game.Collision;
using SpaceShooter.Game.Models;

namespace SpaceShooter.Game.Components.Player;

public class PlayerViewModel : ImageGameObject, IRectCollider
{
    public override Type ViewType => typeof(PlayerView);
    public override string ImageName => "images/player.png";
    public override Size Size => new(98, 75);

    private readonly Polygon _colliderPolygon = new();
    public Polygon ColliderPolygon => _colliderPolygon;

    public bool IsColliderActive => true;

    public PlayerViewModel()
    {
        Position = new(500, 500);

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
    }

    public override void Update()
    {

        Position.X = MousePosition.MouseX - (Size.Width / 2);
        Position.Y = MousePosition.MouseY - (Size.Height / 2);

        _colliderPolygon.Offset(Position.X, Position.Y);

    }

    public void CollideWith(RenderGameObject renderGameObject)
    {

    }
}
