using SharpDX;
using System;

namespace PGZ_Desert_Battle
{
    class Camera : Game3DObject
    {
        private Game3DObject objectOfObservation;
        private Vector3 relativePosition;

        private float _fovY; 
        public float FOVY { get => _fovY; set => _fovY = value; }

        private float _aspect; 
        public float Aspect { get => _aspect; set => _aspect = value; }

        private float _radius;
        private float _height;

        public Camera(float radius, float height, Game3DObject objectOfObservation, float fovY = MathUtil.PiOverFour,
            float aspect = 1.0f) : base(Vector3.Zero, Vector3.Zero)
        {
            _fovY = fovY;
            _aspect = aspect;
            _radius = radius;
            _height = height;

            this.objectOfObservation = objectOfObservation;
            relativePosition = new Vector3(0, height, -radius);
            MoveTo(objectOfObservation.Position + relativePosition);
        }

        public override void RotateX(float deltaX)
        {
            float x = (float)(_radius * Math.Sin(-(Rotation.X * pi) / 180));
            float y = (float)(_radius * Math.Cos(-(Rotation.X * pi) / 180));

            relativePosition = new Vector3(x, _height, -y);
            base.RotateX(deltaX);
        }

        public override void RotateY(float deltaY)
        {
            if (Rotation.Y + deltaY < 60 && Rotation.Y + deltaY > -90)
            {
                base.RotateY(deltaY);
            }
        }

        public override void Update(float delta)
        {
            base.Update(delta);
            MoveTo(objectOfObservation.Position + relativePosition);
        }

        public Matrix GetPojectionMatrix()
        {
            return Matrix.PerspectiveFovLH(_fovY, _aspect, 0.1f, 2000.0f);
        }

        public Matrix GetViewMatrix()
        {
            Matrix rotation = Matrix.RotationYawPitchRoll((float)((Rotation.X * Math.PI) / 180), (float)((Rotation.Y * Math.PI) / 180), (float)((Rotation.Z * Math.PI) / 180));
            Vector3 viewTo = (Vector3)Vector4.Transform(Vector4.UnitZ, rotation);
            Vector3 viewUp = (Vector3)Vector4.Transform(Vector4.UnitY, rotation);
            return Matrix.LookAtLH(Position, Position + viewTo, viewUp);
        }
    }

}
