using Microsoft.AspNetCore.Components;

namespace SpaceShooter.Game.Models;
public class GameObject : ComponentBase
{
    public Size WindowSize { get; set; } = new();
    public MousePoint MousePosition { get; set; } = new();

    public bool DestroyGameObject { get; set; } = false;

    public virtual void Update()
    {
    }
}

