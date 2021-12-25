using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using SpaceShooter.Game.Collision;
using SpaceShooter.Game.Models;

namespace SpaceShooter.Game.Components.Shoot
{
    public class ShotViewModel : ImageGameObject, IColliderAgent
    {
        public override Type ViewType => typeof(ShotView);
        public override string ImageName => "images/shot.png";
        public override Size Size => new(13, 54);

        private readonly Polygon _colliderPolygon = new();
        public Polygon ColliderPolygon => _colliderPolygon;

        public bool IsColliderActive => true;

        private double _posX = 0;
        private double _posY = 0;


        public ShotViewModel(int spawnPositionX = 0, int spawnPositionY = 0)
        {
            _posX = spawnPositionX - (Size.Width / 2);
            _posY = spawnPositionY;

            _colliderPolygon.Points.Add(new Vector(0, 0));
            _colliderPolygon.Points.Add(new Vector(13, 0));
            _colliderPolygon.Points.Add(new Vector(13, 54));
            _colliderPolygon.Points.Add(new Vector(0, 54));
            GameEnvironment.Instance.PlaySound("laser2.wav", 0.3f);
        }

        public override void Update()
        {
            _posY -= 15;
            _colliderPolygon.Offset((float)_posX, (float)_posY);
            Position.X = (int)_posX;
            Position.Y = (int)_posY;

            if (_posY < -50)
            {
                DestroyGameObject = true;
            }
        }

        public void CollideWith(RenderGameObject renderGameObject)
        {
            DestroyGameObject = true;
        }
    }
}
