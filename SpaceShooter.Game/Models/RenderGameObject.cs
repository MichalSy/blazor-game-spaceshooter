using SpaceShooter.Game.Components;

namespace SpaceShooter.Game.Models;
public class RenderGameObject : GameObject
{
    public virtual Type ViewType => typeof(EmptyView);
    public virtual Position Position { get; set; } = new(0, 0);
}
