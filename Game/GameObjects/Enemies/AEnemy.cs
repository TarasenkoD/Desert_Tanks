using SharpDX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PGZ_Desert_Battle
{
    abstract class AEnemy : Game3DObject
    {
        private bool isPlayerDetected;
        public bool IsPlayerDetected { get => isPlayerDetected; }

        private bool isDead;
        public bool IsDead { get => isDead; }

        protected float detectionRadius;
        protected float shootTimeout;
        protected float shootInterval;
        protected float shootHeight;

        private MeshObject tankTower;
        private MeshObject tankCase;
        private Vector3 rotationPosition;

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
                    isDead = true;
                }
                else
                {
                    health = value;
                }
            }
        }

        public AEnemy(Vector3 position, Vector3 rotation, float detectionRadius)
            : base(position, rotation)
        {
            health = 100;
            isDead = false;
            shootTimeout = 0;
            shootInterval = 1;
            shootHeight = 1.4f;
            isPlayerDetected = false;
            this.detectionRadius = detectionRadius;
            rotationPosition = new Vector3(0, 0, detectionRadius) + position;
        }

        public override void AddMeshObjects(params MeshObject[] meshes)
        {
            if (meshes.Length == 2)
            {
                base.AddMeshObjects(meshes);
                tankCase = meshes[0];
                tankTower = meshes[1];
                CreateCollider(meshes[0]);
            }
            else
            {
                throw new ArgumentException();
            }
        }

        public override void Update(float delta)
        {
            base.Update(delta);
            shootTimeout += delta;
        }

        public void UpdatePlayerDetector(Player player)
        {
            if (DistanceTo(player.Position) <= detectionRadius)
            {
                isPlayerDetected = true;
            }
            else
            {
                isPlayerDetected = false;
            }
        }

        public void RotateTower(Vector3 position)
        {
            var a = (GetDistance(Position, position) * GetDistance(Position, position) + GetDistance(Position, rotationPosition) * GetDistance(Position, rotationPosition) - GetDistance(position, rotationPosition) * GetDistance(position, rotationPosition))
                / (2 * GetDistance(Position, position) * GetDistance(Position, rotationPosition));

            var b = Math.Acos(a);

            var p1 = Vector3.Distance(position, rotationPosition);

            var newX = (float)Math.Cos(b) * (rotationPosition.X - Position.X) - (float)Math.Sin(b) * (rotationPosition.Z - Position.Z) + Position.X;
            var newZ = (float)Math.Sin(b) * (rotationPosition.X - Position.X) + (float)Math.Cos(b) * (rotationPosition.Z - Position.Z) + Position.Z;
            rotationPosition.X = newX;
            rotationPosition.Z = newZ;

            var p2 = Vector3.Distance(position, rotationPosition);

            if (p1 < p2)
            {
                tankTower.RotateZ((float)b * 180 / pi);
            }
            else
            {
                tankTower.RotateZ(-(float)b * 180 / pi);
            }
            rotationPosition = position;
        }

        private double GetDistance(Vector3 pos1, Vector3 pos2)
        {
            return Math.Sqrt((pos1.X - pos2.X) * (pos1.X - pos2.X) + (pos1.Z - pos2.Z) * (pos1.Z - pos2.Z));
        }

        public override void MoveBy(Vector3 delta)
        {
            base.MoveBy(delta);

            rotationPosition += delta;
        }

        public override void RotateZ(float deltaZ)
        {
            base.RotateZ(deltaZ);

            tankCase.RotateZ(deltaZ);
        }

        public void Shoot(GameScene scene, MeshObject mesh)
        {
            if (shootTimeout >= shootInterval)
            {
                Projectile projectile = new Projectile(mesh, 5f, 1.5f, Position + new Vector3(0, shootHeight, 0), (scene.Player.Position - Position), this);
                scene.Meshes.Add(mesh);
                scene.Projectiles.Add(projectile);
                shootTimeout = 0;
            }
        }
    }
}
