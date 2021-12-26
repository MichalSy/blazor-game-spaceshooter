using Microsoft.AspNetCore.Components;

namespace SpaceShooter.Game.Models;
public class GameObject : ComponentBase
{
    public bool DestroyGameObject { get; set; } = false;

    public virtual void Update(float time)
    {
    }
}

