using SpaceShooter.Game.Collision;
using SpaceShooter.Game.Models;

namespace SpaceShooter.Game.Manager;

public class CollisionManager : GameObject
{
    public override void Update(float time)
    {
        // calculate edges for collision detection
        foreach (var currentView in GameEnvironment.GameObjects)
        {
            if (currentView is not ICollider rectCollider || !rectCollider.IsColliderActive)
                continue;

            rectCollider.ColliderPolygon.BuildEdges();
        }

        // check for collisions
        foreach (var currentView in GameEnvironment.GameObjects)
        {
            if (currentView is not IColliderAgent agent)
                continue;

            foreach (var checkView in GameEnvironment.GameObjects)
            {
                if (checkView is not ICollider rectCollider || checkView is not RenderGameObject renderGameObject || !rectCollider.IsColliderActive)
                    continue;

                if (checkView is IColliderAgent)
                    continue;

                var collisionCheck = CollisionCheck.PolygonCollision(agent.ColliderPolygon, rectCollider.ColliderPolygon);
                if (collisionCheck)
                {
                    agent.CollideWith(renderGameObject);
                    rectCollider.CollideWith((RenderGameObject)agent);
                }
            }
        }
    }
}

