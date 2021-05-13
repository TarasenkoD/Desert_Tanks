using SharpDX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PGZ_Desert_Battle
{
    class MenuController : AController
    {
        private InputController inputController;
        private MenuUI menu;
        public MenuController(InputController inputController, Scene scene, Game game)
            : base(scene, game)
        {
            menu = (MenuUI)scene.UserInterface;
            this.inputController = inputController;
        }

        public override void Update(float delta)
        {
            inputController.UpdateMouseState();

            if (inputController.MouseUpdated)
            {
                menu.Cursor.MoveBy(new Vector2(inputController.MouseRelativePositionX, inputController.MouseRelativePositionY));
            }

            if (inputController.MouseLeft)
            {
                if (menu.PlayButton.IsIntersect(menu.Cursor))
                {
                    game.ChangeScene();
                }

                if (menu.ExitButton.IsIntersect(menu.Cursor))
                {
                    inputController.RenderForm.Close();
                }
            }
        }
    }
}
