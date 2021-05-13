using SharpDX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PGZ_Desert_Battle
{
    class UIText : UIElement
    {
        private string text;
        private string brushName;
        private string textFormatName;

        public UIText(string text, string brushName, string textFormatName, DirectX2DGraphics directX2DGraphics, Vector2 translate, Vector2 pivot, Vector2 scale, float rotation = 0f, bool isDraw = true)
           : base(directX2DGraphics, translate, pivot, scale, rotation, isDraw)
        {
            this.text = text;
            this.brushName = brushName;
            this.textFormatName = textFormatName;
        }

        public void UpdateText(string newText)
        {
            text = newText;
        }

        public override void Draw()
        {
            directX2DGraphics.DrawText(text, textFormatName, GetTransform(), directX2DGraphics.RenderTargetClientRectangle, brushName);
        }
    }
}
