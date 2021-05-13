using SharpDX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PGZ_Desert_Battle
{
    static class SceneData
    {
        public static string DefaultTextureName { get => defaultTextureName; }
        private static string defaultTextureName = "defaultTexture";
        public static string EmmisiveMaterialName { get => emmisiveMaterialName; }
        private static string emmisiveMaterialName = "emmisiveMaterial";
        public static string DefaultMaterialName { get => defaultMaterialName; }
        private static string defaultMaterialName = "defaultMaterial";

        public static void InitScene(this Scene scene, Renderer renderer, Loader loader)
        {
            Texture texture;
            Material material;

            texture = loader.LoadTextureFromFile("assets\\white.png", renderer.PointSampler, false);
            scene.Textures.Add(defaultTextureName, texture);
            material = new Material(texture, new Vector3(1.0f), new Vector3(0.0f), new Vector3(0.0f), new Vector3(0.0f), 1.0f);
            scene.Materials.Add(emmisiveMaterialName, material);
            material = new Material(texture, new Vector3(0.0f), new Vector3(1.0f), new Vector3(1.0f), new Vector3(1.0f), 1.0f);
            scene.Materials.Add(defaultMaterialName, material);
        }
    }
}
