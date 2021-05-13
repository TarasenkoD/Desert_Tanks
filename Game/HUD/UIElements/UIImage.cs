using SharpDX;
using SharpDX.WIC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PGZ_Desert_Battle
{
    class UIImage : UIElement
    {
        private string name;
        public string Name { get => name; }

        public UIImage(string name, string fileName, DirectX2DGraphics directX2DGraphics, Vector2 translate, Vector2 pivot, Vector2 scale, float rotation = 0f, bool isDraw = true)
            : base(directX2DGraphics, translate, pivot, scale, rotation, isDraw)
        {
            this.name = name;

            directX2DGraphics.NewBitmap(name, BitmapFrameDecode(fileName));
        }

        public void ChangeScale(float width, float height)
        {
            scale.X = width / 1920;
            scale.Y = height / 1080;
        }

        private BitmapFrameDecode BitmapFrameDecode(string fileName)
        {
            ImagingFactory imagingFactory = new ImagingFactory();
            BitmapDecoder decoder = new BitmapDecoder(imagingFactory, fileName, DecodeOptions.CacheOnDemand);
            BitmapFrameDecode bitmapFirstFrame = decoder.GetFrame(0);

            Utilities.Dispose(ref decoder);
            Utilities.Dispose(ref imagingFactory);

            return bitmapFirstFrame;
        }

        public override void Draw()
        {
            directX2DGraphics.DrawBitmap(name, GetTransform(), 1, SharpDX.Direct2D1.BitmapInterpolationMode.Linear);
        }
    }
}
