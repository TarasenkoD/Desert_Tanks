﻿using System;
using SharpDX;
using SharpDX.Direct3D;
using SharpDX.Direct3D11;
using Buffer11 = SharpDX.Direct3D11.Buffer;

namespace PGZ_Desert_Battle
{
    class MeshObject : Game3DObject, IDisposable
    {
        private DirectX3DGraphics _directX3DGraphics;

        private int _verticesCount;

        private Renderer.VertexDataStruct[] _vertices;
        public Renderer.VertexDataStruct[] Vertices { get => _vertices; }

        private Buffer11 _vertexBufferObject;
        private VertexBufferBinding _vertexBufferBinding;
        public VertexBufferBinding VertexBufferBinding { get => _vertexBufferBinding; }

        private int _indicesCount;
        public int IndicesCount { get => _indicesCount; }
        private uint[] _indices;
        private Buffer11 _indicesBufferObject;
        public Buffer11 IndicesBufferObject { get => _indicesBufferObject; }

        private PrimitiveTopology _primitiveTopology;
        public PrimitiveTopology PrimitiveTopology { get => _primitiveTopology; }

        private Material _material;
        public Material Material { get => _material; }

        public Material MaterialNormal { get; set; }
        public bool Visible { get; set; }

        private VertexBufferBinding _vertexBufferNormalBinding;
        public VertexBufferBinding VertexBufferNormalBinding { get => _vertexBufferNormalBinding; }

        private Buffer11 _indicesBufferNormalObject;
        public Buffer11 IndicesBufferNormalObject { get => _indicesBufferNormalObject; }

        private Renderer.VertexDataStruct[] copyVertices;
        private uint[] copyIndices;

        public MeshObject(DirectX3DGraphics directX3DGraphics, 
            Vector3 position, Vector3 rotation,
            Renderer.VertexDataStruct[] vertices, uint[] indices,
            PrimitiveTopology primitiveTopology, Material material, 
            bool visible)
            : base(position, rotation)
        {
            _directX3DGraphics = directX3DGraphics;
            _vertices = vertices;
            _verticesCount = _vertices.Length;
            _indices = indices;
            _indicesCount = _indices.Length;
            _primitiveTopology = primitiveTopology;
            _material = material;
            Visible = visible;

            copyIndices = indices;
            copyVertices = vertices;

            _vertexBufferObject = Buffer11.Create(
                _directX3DGraphics.Device,
                BindFlags.VertexBuffer,
                _vertices,
                Utilities.SizeOf<Renderer.VertexDataStruct>() * _verticesCount);
            _vertexBufferBinding = new VertexBufferBinding(
                _vertexBufferObject,
                Utilities.SizeOf<Renderer.VertexDataStruct>(),
                0);
            _indicesBufferObject = Buffer11.Create(
                _directX3DGraphics.Device,
                BindFlags.IndexBuffer,
                _indices,
                Utilities.SizeOf<uint>() * _indicesCount);
        }

        public MeshObject Copy()
        {
            return new MeshObject(_directX3DGraphics, Vector3.Zero, Vector3.Zero, copyVertices, copyIndices, PrimitiveTopology, Material, Visible);
        }

        public void Dispose()
        {
            Utilities.Dispose(ref _indicesBufferObject);
            Utilities.Dispose(ref _vertexBufferObject);
            Utilities.Dispose(ref _indicesBufferNormalObject);
        }
    }
}
