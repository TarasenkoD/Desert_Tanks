using SharpDX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PGZ_Desert_Battle
{
    class Projectile : Game3DObject
    {
        private bool isActive;
        public bool IsActive { get => isActive; }

        private float damage;
        public float Damage { get => damage; }

        private Game3DObject creator;
        public Game3DObject Creator { get => creator; }

        private float speed;
        private Vector3 direction;
        private float lifetime;

        public Projectile(MeshObject mesh, float damage, float speed, Vector3 position, Vector3 direction, Game3DObject creator)
        : base(position, Vector3.Zero)
        {
            lifetime = 1f;
            isActive = true;
            this.speed = speed;
            this.damage = damage;
            this.creator = creator;
            this.direction = direction;

            AddMeshObjects(mesh);
            CreateCollider(mesh);
        }

        public MeshObject GetMesh()
        {
            return meshes[0];
        }

        public void SetSpeed(float speed)
        {
            this.speed = speed;
        }

        public override void Update(float delta)
        {
            base.Update(delta);
            lifetime -= delta;
            MoveBy(direction * speed * delta);

            if (lifetime <= 0)
            {
                isActive = false;
            }
        }

        public bool CheckIntersection(Game3DObject game3DObject)
        {
            if (game3DObject.Collider is null)
            {
                return false;
            }

            if (Collider.IntersectWith(game3DObject) && creator.GetType() != game3DObject.GetType())
            {
                isActive = false;
                return true;
            }

            return false;
        }
    }
}
