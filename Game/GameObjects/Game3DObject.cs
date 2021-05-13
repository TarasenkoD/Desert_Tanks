using SharpDX;
using System;
using System.Collections.Generic;

namespace PGZ_Desert_Battle
{
    class Game3DObject
    {
        protected List<MeshObject> meshes;
        public List<MeshObject> Meshes { get => meshes; }

        protected const float pi = (float)Math.PI;
        private Vector3 position;
        private Vector3 rotation;
        private Collider collider;

        public Collider Collider { get => collider; }
        public Vector3 Position { get => position; }
        public Vector3 Rotation { get => rotation; }
        public bool IsHidden { get; set; } = false;

        public Game3DObject(Vector3 position, Vector3 rotation)
        {
            meshes = new List<MeshObject>();

            this.position = position;
            this.rotation = rotation;
        }

        public virtual void AddMeshObjects(params MeshObject[] meshes)
        {
            this.meshes.AddRange(meshes);
            foreach(var mesh in this.meshes)
            {
                mesh.position = Position;
            }
        }

        public void CreateCollider(MeshObject meshObject)
        {
            collider = new Collider(Position, meshObject.Vertices);
        }

        public virtual void Update(float delta)
        {
            
        }

        private void ClampAngle(ref float angle)
        {
            if (angle > 180) angle -= 360;
            else if (angle < -180) angle += 360;
        }

        public virtual void SetRotationX(float x)
        {
            rotation.X = x;
            ClampAngle(ref rotation.X);
        }

        public virtual void SetRotationY(float y)
        {
            rotation.Y = y;
            ClampAngle(ref rotation.Y);
        }

        public virtual void SetRotationZ(float z)
        {
            rotation.Z = z;
            ClampAngle(ref rotation.Z);
        }
        public virtual void RotateX(float deltaX)
        {
            rotation.X += deltaX;
            ClampAngle(ref rotation.X);
        }

        public float DistanceTo(Vector3 position)
        {
            return Vector3.Distance(Position, position);
        }

        public virtual void RotateY(float deltaY)
        {
            rotation.Y += deltaY;
            ClampAngle(ref rotation.Y);
        }

        public virtual void RotateZ(float deltaZ)
        {
            rotation.Z += deltaZ;
            ClampAngle(ref rotation.Z);
        }

        public virtual void MoveBy(float deltaX, float deltaY, float deltaZ)
        {
            position.X += deltaX;
            position.Y += deltaY;
            position.Z += deltaZ;
            collider?.UpdateCoordinates(new Vector3(deltaX, deltaY, deltaZ));

            foreach (var mesh in this.meshes)
            {
                mesh.MoveBy(deltaX, deltaY, deltaZ);
            }
        }

        public virtual void MoveBy(Vector3 delta)
        {
            position += delta;
            collider?.UpdateCoordinates(delta);

            foreach (var mesh in this.meshes)
            {
                mesh.MoveBy(delta);
            }
        }

        public virtual void MoveTo(Vector3 position)
        {
            this.position = position;

            foreach (var mesh in this.meshes)
            {
                mesh.MoveTo(position);
            }
        }

        public Matrix GetWorldMatrix()
        {
            return Matrix.Multiply(Matrix.RotationYawPitchRoll((Rotation.Z * pi) / 180, (Rotation.Y * pi) / 180, (Rotation.X * pi) / 180), Matrix.Translation(Position));
        }

        public void Dispose()
        {
            foreach (var mesh in this.meshes)
            {
                mesh.Dispose();
            }
        }
    }
}
