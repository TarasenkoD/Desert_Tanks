using SharpDX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PGZ_Desert_Battle
{
    abstract class UIElement
    {
        protected DirectX2DGraphics directX2DGraphics;

        protected Vector2 relativeTranslate;
        public Vector2 RelativeTranslate { get => relativeTranslate; }

        protected Vector2 translate;
        protected Vector2 pivot;
        protected Vector2 scale;
        protected float rotation;
        protected bool isDraw;
        public bool IsDraw { get => isDraw; set => isDraw = value; }

        public UIElement(DirectX2DGraphics directX2DGraphics, Vector2 relativeTranslate, Vector2 pivot, Vector2 scale, float rotation = 0f, bool isDraw = true)
        {
            this.relativeTranslate = relativeTranslate;
            this.pivot = pivot;
            this.scale = scale;
            this.rotation = rotation;
            this.directX2DGraphics = directX2DGraphics;
            this.isDraw = isDraw;
        }

        public virtual void MoveBy(Vector2 delta)
        {
            relativeTranslate += delta;
        }

        public virtual void MoveBy(float deltaX, float deltaY)
        {
            relativeTranslate.X += deltaX;
            relativeTranslate.Y += deltaY;
        }

        public virtual void MoveTo(Vector2 position)
        {
            relativeTranslate = position;
        }

        public void SetScreenSize(Vector2 size)
        {
            translate = size + relativeTranslate;
        }

        public Matrix3x2 GetTransform()
        {
            return Matrix.Transformation2D(pivot, 0f, scale, pivot, rotation, translate);
        }

        public abstract void Draw();
    }
}
