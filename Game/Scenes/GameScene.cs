using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PGZ_Desert_Battle
{
    class GameScene : Scene
    {
        private List<Projectile> projectiles;
        public List<Projectile> Projectiles { get => projectiles; }

        private List<AEnemy> enemies;
        public List<AEnemy> Enemies { get => enemies; }

        private List<Game3DObject> colliders;
        public List<Game3DObject> Colliders { get => colliders; }

        public GameScene()
        {
            enemies = new List<AEnemy>();
            colliders = new List<Game3DObject>();
            projectiles = new List<Projectile>();
        }
        
        public void RemoveEnemyWithMesh(AEnemy enemy)
        {
            foreach(var mesh in enemy.Meshes)
            {
                Meshes.Remove(mesh);
            }

            Enemies.Remove(enemy);
        }
    }
}
