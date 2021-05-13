using SharpDX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PGZ_Desert_Battle
{
    class MenuSceneBuilder : ISceneBuilder
    {
        private Scene scene = new Scene();

        public MenuSceneBuilder()
        {
            this.Reset();
        }

        public void Reset()
        {
            this.scene = new Scene();
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
            MenuController menuController = new MenuController(inputController, scene, game);
            scene.Controllers.Add(menuController);
        }

        public void BuildPlayer(Loader loader)
        {
            Player player = new Player(Vector3.Zero, Vector3.Zero);

            List<MeshObject> meshObjects = loader.LoadFbx("assets\\models\\tank.fbx", Vector3.Zero, scene.Materials[SceneData.DefaultMaterialName], scene.Textures[SceneData.DefaultTextureName], scene, true);
            scene.Meshes.Add(meshObjects[0]);
            scene.Meshes.Add(meshObjects[1]);
            player.AddMeshObjects(meshObjects.ToArray());
            scene.Player = player;
        }

        public void BuildUserInterface(DirectX2DGraphics directX2DGraphics)
        {
            scene.UserInterface = new MenuUI(directX2DGraphics);
        }

        public Scene GetResult()
        {
            Scene result = this.scene;
            this.Reset();

            return result;
        }
    }
}
