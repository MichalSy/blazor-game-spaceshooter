namespace SpaceShooter.Game.Models;

public class ImageGameObject : RenderGameObject
{
    public virtual string ImageName => "";
    public virtual Size Size { get; set; } = new();
}

