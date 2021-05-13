using System;
using System.Collections.Generic;
using System.Linq;
using SharpDX;

namespace PGZ_Desert_Battle
{
    class Scene : IDisposable
    {
        public Player Player;
        public Camera Camera;
        public IUserInterface UserInterface;

        private Dictionary<string, Texture> _textures;
        public Dictionary<string, Texture> Textures { get => _textures; }

        private Dictionary<string, SharpAudioVoice> _audio;
        public Dictionary<string, SharpAudioVoice> Audio { get => _audio; }

        private Dictionary<string, Material> _materials;
        public Dictionary<string, Material> Materials { get => _materials; }

        private List<MeshObject> _meshes;
        public List<MeshObject> Meshes { get => _meshes; }

        private List<AController> _controllers;
        public List<AController> Controllers { get => _controllers; }

        public Scene()
        {
            _textures = new Dictionary<string, Texture>();
            _materials = new Dictionary<string, Material>();
            _meshes = new List<MeshObject>();
            _controllers = new List<AController>();
            _audio = new Dictionary<string, SharpAudioVoice>();
        }

        public void UpdateControllers(float delta)
        {
            foreach(var controller in _controllers)
            {
                controller.Update(delta);
            }
        }

        public void Dispose()
        {
            Utils.DisposeListElements(_meshes);

            foreach (KeyValuePair<string, Material> keyValuePair in _materials)
                keyValuePair.Value.Texture = null;
            Utils.DisposeDictionaryElements(_textures);
        }
    }
}
