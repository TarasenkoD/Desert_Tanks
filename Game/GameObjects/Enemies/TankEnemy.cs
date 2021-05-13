using SharpDX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PGZ_Desert_Battle
{
    class TankEnemy : AEnemy
    {
        public delegate void ActiveState(float delta);
        public ActiveState State;
        public bool IsStuck;

        private Random random;
        private float speed;
        private float rotation;

        private float rotationTimer;
        private float rotationInterval;
        public TankEnemy(Vector3 position, Vector3 rotation, float detectionRadius)
            : base(position, rotation, detectionRadius)
        {
            speed = 2.5f;
            IsStuck = false;
            rotationTimer = 0;
            rotationInterval = 10f;

            random = new Random();
            State = MoveForward;
        }

        public override void Update(float delta)
        {
            base.Update(delta);
            State?.Invoke(delta);
        }

        public void MoveForward(float delta)
        {
            if (rotationTimer < rotationInterval)
            {
                rotationTimer += delta;

                Vector4 vector = new Vector4(0, 0, 1, 0);
                Matrix rotation = Matrix.RotationYawPitchRoll((float)((Rotation.Z * Math.PI) / 180), 0, 0);
                Vector4.Transform(ref vector, ref rotation, out vector);

                MoveBy((Vector3)vector * delta * speed);
                if (IsStuck && rotationTimer > 1f)
                {
                    IsStuck = false;
                }
            }
            else
            {
                rotationTimer = 0;

                State = SetRandomRotation;
            }
        }

        public void SetRandomRotation(float delta)
        {
            rotation = random.Next(30, 270);

            State = RotateByRotation;
        }

        public void SetRotation(float delta)
        {
            if (!IsStuck)
            {
                rotation = delta;
                rotationTimer = 0;

                State = RotateByRotation;
                IsStuck = true;
            }
        }

        public void RotateByRotation(float delta)
        {
            if (rotation > 0)
            {
                RotateZ(delta * 25f);
                rotation -= delta * 25f;
            }
            else
            {
                State = MoveForward;
            }
        }
    }
}
