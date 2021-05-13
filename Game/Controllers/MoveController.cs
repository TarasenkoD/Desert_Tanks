using SharpDX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PGZ_Desert_Battle
{
    class MoveController : AController
    {
        const float SPEED = 5f;

        InputController inputController;
        Player player;
        Camera camera;
        Loader loader;

        public MoveController(InputController inputController, Loader loader, Scene scene, Game game)
            : base(scene, game)
        {
            this.loader = loader;
            this.inputController = inputController;
            player = scene.Player;
            camera = scene.Camera;
        }

        public override void Update(float delta)
        {
            inputController.UpdateKeyboardState();
            inputController.UpdateMouseState();

            if (scene.Player.Health <= 0)
            {
                scene.Audio["lose"].Play();
                Thread.Sleep(2000);
                game.ChangeScene();
            }

            if (inputController.KeyboardUpdated)
            {
                if (inputController.KeyEsc) game.ChangeScene();

                Vector4 moveDirection = Vector4.Zero;
                if (inputController.KeyW)
                {
                    moveDirection += Vector4.UnitZ;
                    scene.Audio["engine"].Play();
                }

                if (inputController.KeyS)
                {
                    moveDirection -= Vector4.UnitZ;
                    scene.Audio["engine"].Play();
                }

                if (inputController.KeyD) player.RotateZ(1);
                if (inputController.KeyA) player.RotateZ(-1);

                moveDirection.Normalize();
                Matrix rotation = Matrix.RotationYawPitchRoll((float)((player.Rotation.Z * Math.PI) / 180), 0, 0);
                Vector4.Transform(ref moveDirection, ref rotation, out moveDirection);
                moveDirection *= delta * SPEED;

                player.MoveBy(new Vector3(moveDirection.X, moveDirection.Y, moveDirection.Z));
                camera.MoveBy(new Vector3(moveDirection.X, moveDirection.Y, moveDirection.Z));

                foreach(var collider in ((GameScene)scene).Colliders)
                {
                    if (player.Collider.IntersectWith(collider))
                    {
                        player.RollBack();
                    }
                }
            }

            if (inputController.MouseUpdated)
            {
                float deltaAngle = camera.FOVY / inputController.RenderForm.ClientSize.Height;
                camera.RotateX(deltaAngle * inputController.MouseRelativePositionX * 40);
                camera.RotateY(deltaAngle * inputController.MouseRelativePositionY * 40);
                player.RotateTower(deltaAngle * inputController.MouseRelativePositionX * 40);

                if (inputController.MouseLeft)
                {
                    Vector4 vector = new Vector4(0, 0, 1, 0);
                    Matrix rotation = Matrix.RotationYawPitchRoll((float)((camera.Rotation.X * Math.PI) / 180), 0, 0);
                    Vector4.Transform(ref vector, ref rotation, out vector);

                    player.Shoot((GameScene)scene, loader.LoadFbx("assets\\models\\projectile.fbx", Vector3.Zero, scene.Materials[SceneData.DefaultMaterialName], scene.Textures[SceneData.DefaultTextureName], scene, true)[0], (Vector3)vector);
                }
            }
        }
    }
}
