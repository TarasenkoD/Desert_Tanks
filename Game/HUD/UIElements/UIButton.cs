using SharpDX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PGZ_Desert_Battle
{
    class UIButton : UIImage
    {
        public UIButton(string name, string fileName, DirectX2DGraphics directX2DGraphics, Vector2 translate, Vector2 pivot, Vector2 scale, float rotation = 0f, bool isDraw = true) 
            : base(name, fileName, directX2DGraphics, translate, pivot, scale, rotation, isDraw)
        {

        }

        public bool IsIntersect(UICursor cursor)
        {
            if (cursor.RelativeTranslate.X > RelativeTranslate.X && cursor.RelativeTranslate.X < RelativeTranslate.X + 300
                && cursor.RelativeTranslate.Y > RelativeTranslate.Y && cursor.RelativeTranslate.Y < RelativeTranslate.Y + 78)
            {
                return true;
            }

            return false;
        }
    }
}
