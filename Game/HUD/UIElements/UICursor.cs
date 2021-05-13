using SharpDX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PGZ_Desert_Battle
{
    class UICursor : UIImage
    {
        float w, h;
        public UICursor(string name, string fileName, DirectX2DGraphics directX2DGraphics, Vector2 translate, Vector2 pivot, Vector2 scale, float rotation = 0f, bool isDraw = true)
            : base(name, fileName, directX2DGraphics, translate, pivot, scale, rotation, isDraw)
        {

        }

        public void SetScreen(float w, float h)
        {
            this.w = w;
            this.h = h;
        }

        public override void MoveBy(Vector2 delta)
        {
            base.MoveBy(delta);

            if (relativeTranslate.X < 0 || relativeTranslate.Y < 0 || relativeTranslate.X > w || relativeTranslate.Y > h)
            {
                base.MoveBy(-delta);
            }
        }
    }
}
