using SharpDX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PGZ_Desert_Battle
{
    class MenuUI : IUserInterface
    {
        public static string HudTextFormatName = "hudTextFormat";
        public static string WhiteBrushName = "whiteBrush";

        private DirectX2DGraphics _directX2DGraphics;

        private float width;
        private float height;

        public List<UIElement> menuElements = new List<UIElement>();

        private UIImage background;
        private UICursor cursor;
        private UIButton play;
        private UIButton exit;
        public UIButton PlayButton { get => play; }
        public UIButton ExitButton { get => exit; }
        public UICursor Cursor { get => cursor; }

        public MenuUI(DirectX2DGraphics directX2DGraphics)
        {
            _directX2DGraphics = directX2DGraphics;

            cursor = new UICursor("cursor_ui", @"assets\textures\ui\cursor_ui.png", _directX2DGraphics, new Vector2(150, 350), new Vector2(0), new Vector2(0.1f));
            background = new UIImage("background_ui", @"assets\textures\ui\background_ui.jpg", _directX2DGraphics, new Vector2(0), new Vector2(0), new Vector2(1f));
            play = new UIButton("play_ui", @"assets\textures\ui\play_ui.png", _directX2DGraphics, Vector2.Zero, Vector2.Zero, new Vector2(1f));
            exit = new UIButton("exit_ui", @"assets\textures\ui\quit_ui.png", _directX2DGraphics, Vector2.Zero, Vector2.Zero, new Vector2(1f));

            menuElements.Add(background);
            menuElements.Add(play);
            menuElements.Add(exit);
            menuElements.Add(cursor);
        }

        public void Render()
        {
            width = _directX2DGraphics.RenderTargetClientRectangle.Right;
            height = _directX2DGraphics.RenderTargetClientRectangle.Bottom;

            background.ChangeScale(width, height);
            cursor.SetScreen(width, height);
            play.MoveTo(new Vector2(width / 2 - 160, height / 2));
            exit.MoveTo(new Vector2(width / 2 - 160, height / 2 + 130));

            for (int i = 1; i < menuElements.Count; i++)
            {
                menuElements[i].SetScreenSize(new Vector2(0, 0));
            }

            _directX2DGraphics.BeginDraw();

            foreach (var element in menuElements)
            {
                if (element.IsDraw)
                {
                    element.Draw();
                }
            }

            _directX2DGraphics.EndDraw();
        }
    }
}
