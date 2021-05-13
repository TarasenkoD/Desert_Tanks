using SharpDX.Windows;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PGZ_Desert_Battle
{
    interface ISceneBuilder
    {
        void BuildBase(Renderer renderer, Loader loader);
        void BuildPlayer(Loader loader);
        void BuildCamera();
        void BuildControllers(Renderer renderer, Loader loader, InputController inputController, Game game);
        void BuildUserInterface(DirectX2DGraphics directX2DGraphics);
        Scene GetResult();
    }
}
