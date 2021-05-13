using SharpDX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PGZ_Desert_Battle
{
    class GameSceneBuilder : ISceneBuilder
    {
        private GameScene scene = new GameScene();

        public GameSceneBuilder()
        {
            this.Reset();
        }

        public void Reset()
        {
            this.scene = new GameScene();
        }

        public void BuildBase(Renderer renderer, Loader loader)
        {
            Texture texture;
            Material material;

            texture = loader.LoadTextureFromFile("assets\\textures\\white.png", renderer.PointSampler, false);
            scene.Textures.Add(SceneData.DefaultTextureName, texture);
            material = new Material(texture, new Vector3(1.0f), new Vector3(0.0f), new Vector3(0.0f), new Vector3(0.0f), 1.0f);
            scene.Materials.Add(SceneData.EmmisiveMaterialName, material);
            material = new Material(texture, new Vector3(0.0f), new Vector3(1.0f), new Vector3(1.0f), new Vector3(1.0f), 1.0f);
            scene.Materials.Add(SceneData.DefaultMaterialName, material);
        }

        public void BuildCamera()
        {
            if (scene.Player is null)
            {
                throw new NullReferenceException("Player must be defined");
            }

            Camera camera = new Camera(5, 5, scene.Player);
            scene.Camera = camera;
        }

        public void BuildControllers(Renderer renderer, Loader loader, InputController inputController, Game game)
        {
            MoveController moveController = new MoveController(inputController, loader, scene, game);
            scene.Controllers.Add(moveController);

            LightController lightController = new LightController(renderer, scene, game);
            scene.Controllers.Add(lightController);

            EnemyController enemyController = new EnemyController(loader, scene, game);
            scene.Controllers.Add(enemyController);

            ProjectileController projectileController = new ProjectileController(scene, game);
            scene.Controllers.Add(projectileController);
        }

        public void BuildModels(Loader loader)
        {
            MeshObject sky = loader.LoadFbx("assets\\models\\sky.fbx", Vector3.Zero, scene.Materials[SceneData.DefaultMaterialName], scene.Textures[SceneData.DefaultTextureName], scene, true)[0];
            scene.Meshes.Add(sky);

            var desert = loader.LoadFbx("assets\\models\\desert.fbx", Vector3.Zero, scene.Materials[SceneData.DefaultMaterialName], scene.Textures[SceneData.DefaultTextureName], scene, true);

            foreach (var mesh in desert)
            {
                scene.Meshes.Add(mesh);
            }
        }

        public void BuildColliders(Loader loader)
        {
            var colliders = loader.LoadFbx("assets\\models\\colliders.fbx", Vector3.Zero, scene.Materials[SceneData.DefaultMaterialName], scene.Textures[SceneData.DefaultTextureName], scene, true);

            foreach (var mesh in colliders)
            {
                mesh.CreateCollider(mesh);
                scene.Colliders.Add(mesh);
                //scene.Meshes.Add(loader.MakeCollider(Vector3.Zero, Vector3.Zero, mesh.Collider.Coordinates, scene.Materials[SceneData.DefaultMaterialName], true));
            }
        }

        public void BuildEnemies(Loader loader)
        {
            TankEnemy tankEnemy = new TankEnemy(new Vector3(70, 0, 0), Vector3.Zero, 30f);
            List<MeshObject> meshObjects = loader.LoadFbx("assets\\models\\enemies\\tank.fbx", new Vector3(0, 0, 180), scene.Materials[SceneData.DefaultMaterialName], scene.Textures[SceneData.DefaultTextureName], scene, true);
            scene.Meshes.AddRange(meshObjects);
            tankEnemy.AddMeshObjects(meshObjects.ToArray());
            scene.Enemies.Add(tankEnemy);

            tankEnemy = new TankEnemy(new Vector3(-40, 0, -60), Vector3.Zero, 30f);
            meshObjects = loader.LoadFbx("assets\\models\\enemies\\tank.fbx", new Vector3(0, 0, 180), scene.Materials[SceneData.DefaultMaterialName], scene.Textures[SceneData.DefaultTextureName], scene, true);
            scene.Meshes.AddRange(meshObjects);
            tankEnemy.AddMeshObjects(meshObjects.ToArray());
            scene.Enemies.Add(tankEnemy);

            tankEnemy = new TankEnemy(new Vector3(85, 0, -90), Vector3.Zero, 30f);
            meshObjects = loader.LoadFbx("assets\\models\\enemies\\tank.fbx", new Vector3(0, 0, 180), scene.Materials[SceneData.DefaultMaterialName], scene.Textures[SceneData.DefaultTextureName], scene, true);
            scene.Meshes.AddRange(meshObjects);
            tankEnemy.AddMeshObjects(meshObjects.ToArray());
            scene.Enemies.Add(tankEnemy);

            tankEnemy = new TankEnemy(new Vector3(12, 0, 90), Vector3.Zero, 30f);
            meshObjects = loader.LoadFbx("assets\\models\\enemies\\tank.fbx", new Vector3(0, 0, 180), scene.Materials[SceneData.DefaultMaterialName], scene.Textures[SceneData.DefaultTextureName], scene, true);
            scene.Meshes.AddRange(meshObjects);
            tankEnemy.AddMeshObjects(meshObjects.ToArray());
            scene.Enemies.Add(tankEnemy);

            tankEnemy = new TankEnemy(new Vector3(-80, 0, 95), Vector3.Zero, 30f);
            meshObjects = loader.LoadFbx("assets\\models\\enemies\\tank.fbx", new Vector3(0, 0, 180), scene.Materials[SceneData.DefaultMaterialName], scene.Textures[SceneData.DefaultTextureName], scene, true);
            scene.Meshes.AddRange(meshObjects);
            tankEnemy.AddMeshObjects(meshObjects.ToArray());
            scene.Enemies.Add(tankEnemy);

            TurretEnemy turretEnemy = new TurretEnemy(new Vector3(50, 0, 0), Vector3.Zero, 30f);
            meshObjects = loader.LoadFbx("assets\\models\\enemies\\turret.fbx", Vector3.Zero, scene.Materials[SceneData.DefaultMaterialName], scene.Textures[SceneData.DefaultTextureName], scene, true);
            scene.Meshes.AddRange(meshObjects);
            turretEnemy.AddMeshObjects(meshObjects.ToArray());
            scene.Enemies.Add(turretEnemy);

            turretEnemy = new TurretEnemy(new Vector3(20, 0, -115), Vector3.Zero, 30f);
            meshObjects = loader.LoadFbx("assets\\models\\enemies\\turret.fbx", Vector3.Zero, scene.Materials[SceneData.DefaultMaterialName], scene.Textures[SceneData.DefaultTextureName], scene, true);
            scene.Meshes.AddRange(meshObjects);
            turretEnemy.AddMeshObjects(meshObjects.ToArray());
            scene.Enemies.Add(turretEnemy);

            turretEnemy = new TurretEnemy(new Vector3(95, 0, -55), Vector3.Zero, 30f);
            meshObjects = loader.LoadFbx("assets\\models\\enemies\\turret.fbx", Vector3.Zero, scene.Materials[SceneData.DefaultMaterialName], scene.Textures[SceneData.DefaultTextureName], scene, true);
            scene.Meshes.AddRange(meshObjects);
            turretEnemy.AddMeshObjects(meshObjects.ToArray());
            scene.Enemies.Add(turretEnemy);

            turretEnemy = new TurretEnemy(new Vector3(-50, 0, 35), Vector3.Zero, 30f);
            meshObjects = loader.LoadFbx("assets\\models\\enemies\\turret.fbx", Vector3.Zero, scene.Materials[SceneData.DefaultMaterialName], scene.Textures[SceneData.DefaultTextureName], scene, true);
            scene.Meshes.AddRange(meshObjects);
            turretEnemy.AddMeshObjects(meshObjects.ToArray());
            scene.Enemies.Add(turretEnemy);

            turretEnemy = new TurretEnemy(new Vector3(30, 0, 115), Vector3.Zero, 30f);
            meshObjects = loader.LoadFbx("assets\\models\\enemies\\turret.fbx", Vector3.Zero, scene.Materials[SceneData.DefaultMaterialName], scene.Textures[SceneData.DefaultTextureName], scene, true);
            scene.Meshes.AddRange(meshObjects);
            turretEnemy.AddMeshObjects(meshObjects.ToArray());
            scene.Enemies.Add(turretEnemy);
        }

        public void BuildSounds(SharpAudioDevice audioDevice)
        {
            scene.Audio.Add("lose", new SharpAudioVoice(audioDevice, @"assets\sounds\lose.wav"));
            scene.Audio.Add("shoot", new SharpAudioVoice(audioDevice, @"assets\sounds\shoot.wav"));
            scene.Audio.Add("win", new SharpAudioVoice(audioDevice, @"assets\sounds\win.wav"));
            scene.Audio.Add("engine", new SharpAudioVoice(audioDevice, @"assets\sounds\engine.wav"));
        }

        public void BuildPlayer(Loader loader)
        {
            Player player = new Player(Vector3.Zero, Vector3.Zero);

            List<MeshObject> meshObjects = loader.LoadFbx("assets\\models\\player.fbx", new Vector3(0, 0, 180), scene.Materials[SceneData.DefaultMaterialName], scene.Textures[SceneData.DefaultTextureName], scene, true);
            scene.Meshes.Add(meshObjects[0]);
            scene.Meshes.Add(meshObjects[1]);
            player.AddMeshObjects(meshObjects.ToArray());
            scene.Player = player;
        }

        public void BuildUserInterface(DirectX2DGraphics directX2DGraphics)
        {
            if (scene.Player is null)
            {
                throw new NullReferenceException("Player must be defined");
            }

            scene.UserInterface = new GameUI(directX2DGraphics, scene);
        }

        public Scene GetResult()
        {
            GameScene result = this.scene;
            this.Reset();

            return result;
        }
    }
}
