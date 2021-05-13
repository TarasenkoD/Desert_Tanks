using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PGZ_Desert_Battle
{
    class ProjectileController : AController
    {
        GameScene gameScene;
        public ProjectileController(Scene scene, Game game)
          : base(scene, game)
        {
            gameScene = (GameScene)scene;
        }

        public override void Update(float delta)
        {
            for (int i = 0; i < gameScene.Projectiles.Count; i++)
            {
                if (gameScene.Projectiles[i].IsActive)
                {
                    gameScene.Projectiles[i].Update(delta);
                    CheckPlayerCollision(gameScene.Projectiles[i]);
                    CheckEnemyCollision(gameScene.Projectiles[i]);
                }
                else
                {
                    gameScene.Meshes.Remove(gameScene.Projectiles[i].GetMesh());
                    gameScene.Projectiles.Remove(gameScene.Projectiles[i]);
                }

            }

            for (int i = 0; i < gameScene.Projectiles.Count; i++)
            {
                foreach (var collider in ((GameScene)scene).Colliders)
                {
                    gameScene.Projectiles[i].CheckIntersection(collider);
                }
            }
        }

        private void CheckPlayerCollision(Projectile projectile)
        {
            if (projectile.CheckIntersection(gameScene.Player))
            {
                gameScene.Player.Health -= projectile.Damage;
            }
        }

        private void CheckEnemyCollision(Projectile projectile)
        {
            foreach(var enemy in gameScene.Enemies)
            {
                if (projectile.CheckIntersection(enemy))
                {
                    enemy.Health -= projectile.Damage;
                }
            }
        }
    }
}
