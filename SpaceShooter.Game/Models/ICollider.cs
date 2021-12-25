using SpaceShooter.Game.Collision;

namespace SpaceShooter.Game.Models;
public interface ICollider
{
    Polygon ColliderPolygon { get; }

    bool IsColliderActive { get; }

    void CollideWith(RenderGameObject renderGameObject);
}