using PGZ_Desert_Battle;
using SharpDX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PGZ_Desert_Battle
{
    class Player : Game3DObject
    {
        private MeshObject tankTower;
        private MeshObject tankCase;

        public Vector3 LastMovement;

        private float health;
        public float Health
        {
            get
            {
                return health;
            }

            set
            {
                if (value <= 0)
                {
                    health = 0;
                }
                else if (value > 100)
                {
                    health = 100;
                }
                else
                {
                    health = value;
                }
            }
        }

        public Player(Vector3 postiton, Vector3 rotation) 
            : base(postiton, rotation)
        {
            health = 100;
        }

        public override void AddMeshObjects(params MeshObject[] meshes)
        {
            if (meshes.Length == 2)
            {
                base.AddMeshObjects(meshes);
                tankCase = meshes[0];
                tankTower = meshes[1];
                CreateCollider(tankCase);
                Collider.UpdateCoordinates(new Vector3(0, 1, 0));
            }
            else
            {
                throw new ArgumentException();
            }
        }

        public void Shoot(GameScene scene, MeshObject mesh, Vector3 direction)
        {
            scene.Audio["shoot"].Stop();
            scene.Audio["shoot"].Play();

            Projectile projectile = new Projectile(mesh, 10f, 80f, Position + new Vector3(0, 1.4f, 0), direction, this);
            scene.Meshes.Add(mesh);
            scene.Projectiles.Add(projectile);
        }

        public void RollBack()
        {
            MoveBy(new Vector3(-LastMovement.X, -LastMovement.Y, -LastMovement.Z));
        }

        public void RotateTower(float delta)
        {
            tankTower.RotateZ(delta);
        }

        public void RotateCase(float delta)
        {
            tankCase.RotateZ(delta);
        }

        public override void RotateZ(float deltaZ)
        {
            base.RotateZ(deltaZ);
            RotateCase(deltaZ);
        }

        public override void MoveBy(Vector3 delta)
        {
            base.MoveBy(delta);
            LastMovement = delta;
        }
    }
}
