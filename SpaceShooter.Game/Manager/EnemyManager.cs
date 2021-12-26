using SpaceShooter.Game.Components.Enemy;
using SpaceShooter.Game.Models;

namespace SpaceShooter.Game.Manager;

public class EnemyManager : GameObject
{
    private float _lastAddEnemy = 0;

    public EnemyManager()
    {
        _lastAddEnemy = 0;
    }

    public override void Update(float time)
    {
        if (_lastAddEnemy + 500 < time)
        {
            GameEnvironment.GameObjects.Insert(0, new EnemyViewModel());
            _lastAddEnemy = time;
        }
    }
}
