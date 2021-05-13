using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PGZ_Desert_Battle
{
    class SceneDirector
    {
        private ISceneBuilder builder;
        public ISceneBuilder Builder { set => builder = value; }

        public void BuildScene(Game game, Renderer renderer, Loader loader, SharpAudioDevice audioDevice, InputController inputController, DirectX2DGraphics directX2DGraphics)
        {
            if (builder is GameSceneBuilder)
            {
                BuildGameScene(game, renderer, loader, audioDevice, inputController, directX2DGraphics);
            }
            else if (builder is MenuSceneBuilder)
            {
                BuildMenuScene(game, renderer, loader, inputController, directX2DGraphics);
            }
        }

        private void BuildGameScene(Game game, Renderer renderer, Loader loader, SharpAudioDevice audioDevice, InputController inputController, DirectX2DGraphics directX2DGraphics)
        {
            GameSceneBuilder builder = (GameSceneBuilder)this.builder;
            builder.BuildBase(renderer, loader);
            builder.BuildPlayer(loader);
            builder.BuildEnemies(loader);
            builder.BuildCamera();
            builder.BuildModels(loader);
            builder.BuildColliders(loader);
            builder.BuildSounds(audioDevice);
            builder.BuildUserInterface(directX2DGraphics);
            builder.BuildControllers(renderer, loader, inputController, game);
        }

        private void BuildMenuScene(Game game, Renderer renderer, Loader loader, InputController inputController, DirectX2DGraphics directX2DGraphics)
        {
            MenuSceneBuilder builder = (MenuSceneBuilder)this.builder;
            builder.BuildBase(renderer, loader);
            builder.BuildPlayer(loader);
            builder.BuildCamera();
            builder.BuildUserInterface(directX2DGraphics);
            builder.BuildControllers(renderer, loader, inputController, game);
        }
    }
}
