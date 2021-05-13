using SharpDX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PGZ_Desert_Battle
{
    class EnemyController : AController
    {
        private Loader loader;
        GameScene gameScene;
        public EnemyController(Loader loader, Scene scene, Game game)
            : base(scene, game)
        {
            this.loader = loader;
            gameScene = (GameScene)scene;
        }

        public override void Update(float delta)
        {
            if (gameScene.Enemies.Count == 0)
            {
                scene.Audio["win"].Play();
                Thread.Sleep(2000);
                game.ChangeScene();
            }

            foreach (var enemy in gameScene.Enemies)
            {
                enemy.Update(delta);
                enemy.UpdatePlayerDetector(scene.Player);

                if (enemy.IsPlayerDetected)
                {
                    enemy.Shoot(gameScene, loader.LoadFbx("assets\\models\\projectile.fbx", Vector3.Zero, scene.Materials[SceneData.DefaultMaterialName], scene.Textures[SceneData.DefaultTextureName], scene, true)[0]);
                    enemy.RotateTower(scene.Player.Position);
                }

                if (enemy is TankEnemy)
                {
                    foreach (var collider in ((GameScene)scene).Colliders)
                    {
                        if (enemy.Collider.IntersectWith(collider))
                        {
                            ((TankEnemy)enemy).SetRotation(180);
                        }
                    }
                }
            }

            for(int i = 0; i < gameScene.Enemies.Count; i++)
            {
                if (gameScene.Enemies[i].IsDead)
                {
                    gameScene.RemoveEnemyWithMesh(gameScene.Enemies[i]);
                    gameScene.Player.Health += 20f;
                }
            }
        }
    }
}
