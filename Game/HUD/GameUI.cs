using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using PGZ_Desert_Battle.Properties;
using SharpDX;
using SharpDX.DirectWrite;
using SharpDX.Mathematics.Interop;

namespace PGZ_Desert_Battle
{
    class GameUI : IUserInterface
    {
        public static string HudTextFormatName = "hudTextFormat";
        public static string WhiteBrushName = "whiteBrush";

        private DirectX2DGraphics directX2DGraphics;
        private Scene scene;

        private float width;
        private float height;

        public List<UIElement> uiElements = new List<UIElement>();

        public GameUI(DirectX2DGraphics directX2DGraphics, Scene scene)
        {
            this.directX2DGraphics = directX2DGraphics;
            this.scene = scene;

            uiElements.Add(new UIImage("armor_ui", @"assets\textures\ui\armor_ui.png", this.directX2DGraphics, new Vector2(-110, 0), new Vector2(0), new Vector2(0.1f)));
            uiElements.Add(new UIText("x 0", WhiteBrushName, HudTextFormatName, this.directX2DGraphics, new Vector2(-50, 0), new Vector2(0), new Vector2(1f)));
            uiElements.Add(new UIText("final text", WhiteBrushName, HudTextFormatName, this.directX2DGraphics, Vector2.Zero, new Vector2(0), new Vector2(2f), isDraw:false));
        }

        public void Render()
        {
            width = directX2DGraphics.RenderTargetClientRectangle.Right;
            height = directX2DGraphics.RenderTargetClientRectangle.Bottom;

            foreach (var element in uiElements)
            {
                element.SetScreenSize(new Vector2(width, 0));
            }

            ((UIText)uiElements[1]).UpdateText($"x {scene.Player.Health}");

            if (((GameScene)scene).Enemies.Count == 0)
            {
                ((UIText)uiElements[2]).UpdateText("You win!");
                ((UIText)uiElements[2]).IsDraw = true;
                ((UIText)uiElements[2]).MoveTo(new Vector2(width / 2f - 60, height / 2f - 50));
                ((UIText)uiElements[2]).SetScreenSize(new Vector2(0, 0));
            }

            if (scene.Player.Health <= 0)
            {
                ((UIText)uiElements[2]).UpdateText("You lose!");
                ((UIText)uiElements[2]).IsDraw = true;
                ((UIText)uiElements[2]).MoveTo(new Vector2(width / 2f - 60, height / 2f - 50));
                ((UIText)uiElements[2]).SetScreenSize(new Vector2(0, 0));
            }

            directX2DGraphics.BeginDraw();

            foreach (var element in uiElements)
            {
                if (element.IsDraw)
                {
                    element.Draw();
                }
            }

            directX2DGraphics.EndDraw();
        }
    }
}
