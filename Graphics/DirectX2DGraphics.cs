using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpDX;
using SharpDX.Direct2D1;
using SharpDX.DirectWrite;
using SharpDX.WIC;
using SharpDX.DXGI;
using SharpDX.Mathematics;
using SharpDX.Mathematics.Interop;
using System.Diagnostics;
using Direct2DFactory = SharpDX.Direct2D1.Factory;
using WriteFactory = SharpDX.DirectWrite.Factory;
using Direct2DBitmap = SharpDX.Direct2D1.Bitmap;
using Direct2DPixelFormat = SharpDX.Direct2D1.PixelFormat;
using Direct2DAlphaMode = SharpDX.Direct2D1.AlphaMode;
using Direct2DTextAntialiasMode = SharpDX.Direct2D1.TextAntialiasMode;
using WicPixelFormat = SharpDX.WIC.PixelFormat;
using Direct2DBitmapInterpolationMode = SharpDX.Direct2D1.BitmapInterpolationMode;

namespace PGZ_Desert_Battle
{
    class DirectX2DGraphics : IDisposable                                   // __
    {
        private DirectX3DGraphics _directX3DGraphics;

        private Direct2DFactory _factory;
        private WriteFactory _writeFactory;
        private ImagingFactory _imagingFactory;

        private RenderTargetProperties _renderTargetProperties;
        private RenderTarget _renderTarget;
        private RawRectangleF _renderTargetClientRectangle;
        public RawRectangleF RenderTargetClientRectangle { get => _renderTargetClientRectangle; }

        private Dictionary<string, TextFormat> _textFormats;
        //public Dictionary<string, TextFormat> TextFormats { get => _textFormats; }
        private Dictionary<string, RawColor4> _solidColorBrushColors;
        private Dictionary<string, SolidColorBrush> _solidColorBrushes;
        //public Dictionary<string, SolidColorBrush> SolidColorBrushes { get => _solidColorBrushes; }

        private Dictionary<string, BitmapFrameDecode> _decodedBitmapFirstFrames;
        private Dictionary<string, Direct2DBitmap> _bitmaps;
        //public Dictionary<string, Direct2DBitmap> Bitmaps { get => _bitmaps; }

        public DirectX2DGraphics(Game game, DirectX3DGraphics directX3DGraphics)
        {
            game.SwapChainResizing += Game_SwapChainResizing;
            game.SwapChainResized += Game_SwapChainResized;

            _directX3DGraphics = directX3DGraphics;

            _factory = new Direct2DFactory();
            _writeFactory = new WriteFactory();
            _imagingFactory = new ImagingFactory();

            _renderTargetProperties.DpiX = 0;
            _renderTargetProperties.DpiY = 0;
            _renderTargetProperties.MinLevel = FeatureLevel.Level_10;
            _renderTargetProperties.PixelFormat = new Direct2DPixelFormat(
                Format.Unknown,                                           // SharpDX.DXGI.Format.R8G8B8A8_UNorm
                Direct2DAlphaMode.Premultiplied);                         // ????  Straight not supported
            _renderTargetProperties.Type = RenderTargetType.Hardware;     // Default
            _renderTargetProperties.Usage = RenderTargetUsage.None;

            _textFormats = new Dictionary<string, TextFormat>();
            _solidColorBrushColors = new Dictionary<string, RawColor4>();
            _solidColorBrushes = new Dictionary<string, SolidColorBrush>();

            _decodedBitmapFirstFrames = new Dictionary<string, BitmapFrameDecode>();
            _bitmaps = new Dictionary<string, Direct2DBitmap>();


            NewTextFormat("hudTextFormat", "Console", FontWeight.Normal, FontStyle.Normal, FontStretch.Normal, 20.0f, TextAlignment.Leading, ParagraphAlignment.Near);
            NewSolidBrush("whiteBrush", new RawColor4(1.0f, 1.0f, 1.0f, 1.0f));
        }

        public void NewTextFormat(string formatName, string fontFamilyName, FontWeight fontWeight,
            FontStyle fontStyle, FontStretch fontStretch, float fontSize,
            TextAlignment textAlignment, ParagraphAlignment paragraphAlignment)
        {
            TextFormat textFormat = new TextFormat(_writeFactory, fontFamilyName, fontWeight,
                fontStyle, fontStretch, fontSize);
            textFormat.TextAlignment = textAlignment;
            textFormat.ParagraphAlignment = paragraphAlignment;
            _textFormats.Add(formatName, textFormat);
        }

        public void NewSolidBrush(string brushName, RawColor4 color)
        {
            _solidColorBrushColors.Add(brushName, color);
        }

        public void NewBitmap(string bitmapName, BitmapFrameDecode bitmapFrame)
        {
            if (!_decodedBitmapFirstFrames.Keys.Contains(bitmapName))
            {
                _decodedBitmapFirstFrames.Add(bitmapName, bitmapFrame);
            }
        }

        private void Game_SwapChainResizing(object sender, EventArgs e)
        {
            Utils.DisposeDictionaryElements<Direct2DBitmap>(_bitmaps);
            Utils.DisposeDictionaryElements<SolidColorBrush>(_solidColorBrushes);
            Utilities.Dispose(ref _renderTarget);
        }

        private void CreateAndAddSolidBrush(string brushName)
        {
            SolidColorBrush brush = new SolidColorBrush(_renderTarget, _solidColorBrushColors[brushName]);
            _solidColorBrushes.Add(brushName, brush);
        }

        private void CreateAndAddBitmap(string bitmapName)
        {
            BitmapFrameDecode decodedBitmapFirstFrame = _decodedBitmapFirstFrames[bitmapName];
            FormatConverter imageFormatConverter = new FormatConverter(_imagingFactory);
            imageFormatConverter.Initialize(
                decodedBitmapFirstFrame,
                WicPixelFormat.Format32bppPRGBA, // PRGBA = RGB premultiplied to alpha channel!!!!! YoPRST!
                BitmapDitherType.Ordered4x4, null, 0.0, BitmapPaletteType.Custom);
            Direct2DBitmap bitmap = Direct2DBitmap.FromWicBitmap(_renderTarget, imageFormatConverter);

            _bitmaps.Add(bitmapName, bitmap);

            Utilities.Dispose(ref imageFormatConverter);
        }

        private void Game_SwapChainResized(object sender, EventArgs e)
        {
            Surface surface = _directX3DGraphics.BackBuffer.QueryInterface<Surface>();
            _renderTarget = new RenderTarget(_factory, surface, _renderTargetProperties);
            Utilities.Dispose(ref surface);

            _renderTarget.AntialiasMode = AntialiasMode.PerPrimitive;
            _renderTarget.TextAntialiasMode = Direct2DTextAntialiasMode.Cleartype;
            _renderTargetClientRectangle.Left = 0;
            _renderTargetClientRectangle.Top = 0;
            _renderTargetClientRectangle.Right = _renderTarget.Size.Width;
            _renderTargetClientRectangle.Bottom = _renderTarget.Size.Height;

            string[] brushNames = _solidColorBrushColors.Keys.ToArray();
            for (int i = 0; i <= brushNames.Count() - 1; ++i)
                CreateAndAddSolidBrush(brushNames[i]);

            string[] bitmapNames = _decodedBitmapFirstFrames.Keys.ToArray();
            for (int i = 0; i <= bitmapNames.Count() - 1; ++i)
                CreateAndAddBitmap(bitmapNames[i]);
        }

        public void BeginDraw()
        {
            _renderTarget.BeginDraw();
        }

        public void DrawText(string text, string formatName, Matrix3x2 transformMatrix, RawRectangleF layoutRectangle, string brushName)
        {
            _renderTarget.Transform = transformMatrix;
            _renderTarget.DrawText(text, _textFormats[formatName], layoutRectangle, _solidColorBrushes[brushName]);
        }

        public void DrawBitmap(string bitmapName, Matrix3x2 transformMatrix, float opacity,
            Direct2DBitmapInterpolationMode interpolationMode)
        {
            _renderTarget.Transform = transformMatrix;
            _renderTarget.DrawBitmap(_bitmaps[bitmapName], opacity, interpolationMode);
        }

        public void EndDraw()
        {
            _renderTarget.EndDraw();
        }

        public void Dispose()
        {
            Utils.DisposeDictionaryElements<Direct2DBitmap>(_bitmaps);
            Utils.DisposeDictionaryElements<BitmapFrameDecode>(_decodedBitmapFirstFrames);
            Utils.DisposeDictionaryElements<SolidColorBrush>(_solidColorBrushes);
            Utils.DisposeDictionaryElements<TextFormat>(_textFormats);
            Utilities.Dispose(ref _renderTarget);
            Utilities.Dispose(ref _imagingFactory);
            Utilities.Dispose(ref _writeFactory);
            Utilities.Dispose(ref _factory);
        }
    }
}
