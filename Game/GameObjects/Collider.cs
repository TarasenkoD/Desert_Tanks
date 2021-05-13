using SharpDX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PGZ_Desert_Battle
{
    class Collider : Game3DObject
    {
        public Vector3[] Coordinates;

        public float Top { get => top; }
        private float top;
        public float Bottom { get => bottom; }
        private float bottom;
        public float Front { get => front; }
        private float front;
        public float Back { get => back; }
        private float back;
        public float Left { get => left; }
        private float left;
        public float Right { get => right; }
        private float right;

        public Collider(Vector3 position, Renderer.VertexDataStruct[] vertices)
            : base(position, Vector3.Zero)
        {
            UpdateColliderData(vertices);
        }

        private void UpdateColliderData(Renderer.VertexDataStruct[] vertices)
        {
            for (int i = 0; i < vertices.Length; i++)
            {
                vertices[i].position.X += Position.X;
                vertices[i].position.Y += Position.Z;
                vertices[i].position.Z += Position.Y;
            }

            top = vertices[0].position.Y;
            bottom = vertices[0].position.Y;
            left = vertices[0].position.X;
            right = vertices[0].position.X;
            front = vertices[0].position.Z;
            back = vertices[0].position.Z;

            for (int i = 0; i < vertices.Length; i++)
            {
                if (vertices[i].position.Y > top) top = vertices[i].position.Y;
                if (vertices[i].position.Y < bottom) bottom = vertices[i].position.Y;
                if (vertices[i].position.X > left) left = vertices[i].position.X;
                if (vertices[i].position.X < right) right = vertices[i].position.X;
                if (vertices[i].position.Z > front) front = vertices[i].position.Z;
                if (vertices[i].position.Z < back) back = vertices[i].position.Z;
            }

            Coordinates = new Vector3[8];
            Coordinates[0] = new Vector3(left, back, bottom);
            Coordinates[1] = new Vector3(left, back, top);
            Coordinates[2] = new Vector3(left, front, bottom);
            Coordinates[3] = new Vector3(left, front, top);
            Coordinates[4] = new Vector3(right, back, bottom);
            Coordinates[5] = new Vector3(right, back, top);
            Coordinates[6] = new Vector3(right, front, bottom);
            Coordinates[7] = new Vector3(right, front, top);
        }

        public bool IntersectWith(Game3DObject gameObject)
        {
            int count = 0;
            Vector2[][] projections = CreateProjections(Coordinates);
            Vector2[][] objectProjections = CreateProjections(gameObject.Collider.Coordinates);

            for (int i = 0; i < 3; i++)
            {
                if (!(projections[i][2].X > objectProjections[i][1].X || projections[i][1].X < objectProjections[i][2].X
                    || projections[i][2].Y > objectProjections[i][1].Y || projections[i][1].Y < objectProjections[i][2].Y))
                {
                    count++;
                }
            }

            return count > 1;
        }

        public Vector2[][] CreateProjections(Vector3[] colliderCoordinates)
        {
            int num;
            Vector2[][] projections = new Vector2[3][];

            num = 0;
            projections[0] = new Vector2[4];
            for (int i = 2; i < 6; i++)
            {
                projections[0][num] = new Vector2(colliderCoordinates[i].Y, colliderCoordinates[i].Z);
                num++;
            }
            num = 0;
            projections[1] = new Vector2[4];
            for (int i = 0; i < 6; i += ((i + 1) % 2 == 0 && i != 0) ? 3 : 1)
            {
                projections[1][num] = new Vector2(colliderCoordinates[i].X, colliderCoordinates[i].Z);
                num++;
            }
            num = 0;
            projections[2] = new Vector2[4];
            for (int i = 0; i < 8; i += 2)
            {
                projections[2][num] = new Vector2(colliderCoordinates[i].X, colliderCoordinates[i].Y);
                num++;
            }

            return projections;
        }

        public override void MoveBy(float deltaX, float deltaY, float deltaZ)
        {
            base.MoveBy(deltaX, deltaY, deltaZ);
            UpdateCoordinates(new Vector3(deltaX, deltaY, deltaZ));
        }

        public override void MoveBy(Vector3 delta)
        {
            base.MoveBy(delta);
            UpdateCoordinates(delta);
        }

        public void UpdateCoordinates(Vector3 delta)
        {
            for (int i = 0; i < Coordinates.Length; i++)
            {
                Coordinates[i].X += delta.X;
                Coordinates[i].Y += delta.Y;
                Coordinates[i].Z += delta.Z;
            }
        }

        public void RotateCoordinates(float deltaZ, Vector3 center)
        {
            for (int i = 0; i < Coordinates.Length; i++)
            {
                var newX = (float)Math.Cos(deltaZ) * (Coordinates[i].X - center.X) - (float)Math.Sin(deltaZ) * (Coordinates[i].Z - center.Z) + center.X;
                var newZ = (float)Math.Sin(deltaZ) * (Coordinates[i].X - center.X) + (float)Math.Cos(deltaZ) * (Coordinates[i].Z - center.Z) + center.Z;
                Coordinates[i].X = newX;
                Coordinates[i].Z = newZ;
            }
        }
    }
}
