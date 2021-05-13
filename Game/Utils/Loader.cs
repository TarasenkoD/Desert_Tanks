using System;
using SharpDX;
using SharpDX.DXGI;
using SharpDX.WIC;
using SharpDX.Direct3D;
using SharpDX.Direct3D11;
using System.Collections.Generic;

namespace PGZ_Desert_Battle
{
    class Loader : IDisposable
    {
        private DirectX3DGraphics _directX3DGraphics;
        private ImagingFactory _imagingFactory;

        private SampleDescription _sampleDescription = new SampleDescription(1, 0);

        public Loader(DirectX3DGraphics directX3DGraphics)
        {
            _directX3DGraphics = directX3DGraphics;
            _imagingFactory = new ImagingFactory();
        }

        public BitmapFrameDecode BitmapFrameDecode(string fileName, ImagingFactory imagingFactory)
        {
            BitmapDecoder decoder = new BitmapDecoder(imagingFactory, fileName, DecodeOptions.CacheOnDemand);
            BitmapFrameDecode bitmapFirstFrame = decoder.GetFrame(0);

            Utilities.Dispose(ref decoder);

            return bitmapFirstFrame;
        }

        public Texture LoadTextureFromFile(string fileName, SamplerState samplerState, bool generateMips, int mipLevels = -1)
        {
            BitmapFrameDecode bitmapFirstFrame = BitmapFrameDecode(fileName, _imagingFactory);

            FormatConverter formatConverter = new FormatConverter(_imagingFactory);
            formatConverter.Initialize(bitmapFirstFrame, PixelFormat.Format32bppRGBA,
                BitmapDitherType.None, null, 0.0f, BitmapPaletteType.Custom);

            int stride = formatConverter.Size.Width * 4;
            DataStream buffer = new DataStream(
                formatConverter.Size.Height * stride, true, true);
            formatConverter.CopyPixels(stride, buffer);

            int width = formatConverter.Size.Width;
            int height = formatConverter.Size.Height;

            Texture2DDescription texture2DDescription = new Texture2DDescription()
            {
                Width = width,
                Height = height,
                MipLevels = (generateMips ? 0 : 1),
                ArraySize = 1,
                Format = Format.R8G8B8A8_UNorm,
                SampleDescription = _sampleDescription,
                Usage = ResourceUsage.Default,
                BindFlags = (
                    generateMips ?
                    BindFlags.ShaderResource | BindFlags.RenderTarget :
                    BindFlags.ShaderResource
                    ),
                CpuAccessFlags = CpuAccessFlags.None,
                OptionFlags = (
                   generateMips ?
                   ResourceOptionFlags.GenerateMipMaps:
                   ResourceOptionFlags.None
                   )
            };

            Texture2D textureObject;
                
            if(generateMips)
                textureObject = new Texture2D(_directX3DGraphics.Device, texture2DDescription);
            else
            {
                DataRectangle dataRectangle = new DataRectangle(buffer.DataPointer, stride);
                textureObject = new Texture2D(_directX3DGraphics.Device, texture2DDescription, dataRectangle);
            }

            ShaderResourceViewDescription shaderResourceViewDescription =
                new ShaderResourceViewDescription()
                {
                    Dimension = ShaderResourceViewDimension.Texture2D,
                    Format = Format.R8G8B8A8_UNorm,
                    Texture2D = new ShaderResourceViewDescription.Texture2DResource
                    {
                        MostDetailedMip = 0,
                        MipLevels = (generateMips ? mipLevels : 1)
                    }
                };
            ShaderResourceView shaderResourceView =
                new ShaderResourceView(_directX3DGraphics.Device, textureObject, shaderResourceViewDescription);

            if(generateMips)
            {
                DataBox dataBox = new DataBox(buffer.DataPointer, stride, 1);
                _directX3DGraphics.DeviceContext.UpdateSubresource(dataBox, textureObject, 0);
                _directX3DGraphics.DeviceContext.GenerateMips(shaderResourceView);
            }

            Utilities.Dispose(ref formatConverter);

            return new Texture(textureObject, shaderResourceView, width, height, samplerState);
        }

        public MeshObject MakeCollider(Vector3 position, Vector3 rotation, Vector3[] verticies, Material material, bool visible)
        {
            Renderer.VertexDataStruct[] vertices =
                new Renderer.VertexDataStruct[24]
                {
                    new Renderer.VertexDataStruct // front 0
                    {
                        position = new Vector4(verticies[1].X, verticies[1].Y, verticies[1].Z, 1.0f),
                        normal = new Vector4(0.0f, 0.0f, -1.0f, 1.0f),
                        color = new Vector4(1.0f, 1.0f, 1.0f, 1.0f),
                        texCoord = new Vector2(0.25f, 0.25f)
                    },
                    new Renderer.VertexDataStruct // front 1
                    {
                        position = new Vector4(verticies[3].X, verticies[3].Y, verticies[3].Z, 1.0f),
                        normal = new Vector4(0.0f, 0.0f, -1.0f, 1.0f),
                        color = new Vector4(1.0f, 1.0f, 1.0f, 1.0f),
                        texCoord = new Vector2(0.25f, 0.5f)
                    },
                    new Renderer.VertexDataStruct // front 2
                    {
                        position = new Vector4(verticies[5].X, verticies[5].Y, verticies[5].Z, 1.0f),
                        normal = new Vector4(0.0f, 0.0f, -1.0f, 1.0f),
                        color = new Vector4(1.0f, 1.0f, 1.0f, 1.0f),
                        texCoord = new Vector2(0.5f, 0.5f)
                    },
                    new Renderer.VertexDataStruct // front 3
                    {
                        position = new Vector4(verticies[7].X, verticies[7].Y, verticies[7].Z, 1.0f),
                        normal = new Vector4(0.0f, 0.0f, -1.0f, 1.0f),
                        color = new Vector4(1.0f, 1.0f, 1.0f, 1.0f),
                        texCoord = new Vector2(0.5f, 0.25f)
                    },
                    new Renderer.VertexDataStruct // right 4
                    {
                        position = new Vector4(verticies[0].X, verticies[0].Y, verticies[0].Z, 1.0f),
                        normal = new Vector4(1.0f, 0.0f, 0.0f, 1.0f),
                        color = new Vector4(1.0f, 1.0f, 1.0f, 1.0f),
                        texCoord = new Vector2(0.5f, 0.25f)
                    },
                    new Renderer.VertexDataStruct // right 5
                    {
                        position = new Vector4(verticies[1].X, verticies[1].Y, verticies[1].Z, 1.0f),
                        normal = new Vector4(1.0f, 0.0f, 0.0f, 1.0f),
                        color = new Vector4(1.0f, 0.0f, 1.0f, 1.0f),
                        texCoord = new Vector2(0.5f, 0.5f)
                    },
                    new Renderer.VertexDataStruct // right 6
                    {
                        position = new Vector4(verticies[2].X, verticies[2].Y, verticies[2].Z, 1.0f),
                        normal = new Vector4(1.0f, 0.0f, 0.0f, 1.0f),
                        color = new Vector4(1.0f, 1.0f, 1.0f, 1.0f),
                        texCoord = new Vector2(0.75f, 0.5f)
                    },
                    new Renderer.VertexDataStruct // right 7
                    {
                        position = new Vector4(verticies[3].X, verticies[3].Y, verticies[3].Z, 1.0f),
                        normal = new Vector4(1.0f, 0.0f, 0.0f, 1.0f),
                        color = new Vector4(1.0f, 1.0f, 1.0f, 1.0f),
                        texCoord = new Vector2(0.75f, 0.25f)
                    },
                    new Renderer.VertexDataStruct // back 8
                    {
                        position = new Vector4(verticies[0].X, verticies[0].Y, verticies[0].Z, 1.0f),
                        normal = new Vector4(0.0f, 0.0f, 1.0f, 1.0f),
                        color = new Vector4(1.0f, 1.0f, 1.0f, 1.0f),
                        texCoord = new Vector2(0.75f, 0.25f)
                    },
                    new Renderer.VertexDataStruct // back 9
                    {
                        position = new Vector4(verticies[2].X, verticies[2].Y, verticies[2].Z, 1.0f),
                        normal = new Vector4(0.0f, 0.0f, 1.0f, 1.0f),
                        color = new Vector4(1.0f, 1.0f, 1.0f, 1.0f),
                        texCoord = new Vector2(0.75f, 0.5f)
                    },
                    new Renderer.VertexDataStruct // back 10
                    {
                        position = new Vector4(verticies[4].X, verticies[4].Y, verticies[4].Z, 1.0f),
                        normal = new Vector4(0.0f, 0.0f, 1.0f, 1.0f),
                        color = new Vector4(1.0f, 1.0f, 1.0f, 1.0f),
                        texCoord = new Vector2(1.0f, 0.5f)
                    },
                    new Renderer.VertexDataStruct // back 11
                    {
                        position = new Vector4(verticies[6].X, verticies[6].Y, verticies[6].Z, 1.0f),
                        normal = new Vector4(0.0f, 0.0f, 1.0f, 1.0f),
                        color = new Vector4(1.0f, 1.0f, 1.0f, 1.0f),
                        texCoord = new Vector2(1.0f, 0.25f)
                    },
                    new Renderer.VertexDataStruct // left 12
                    {
                        position = new Vector4(verticies[4].X, verticies[4].Y, verticies[4].Z, 1.0f),
                        normal = new Vector4(-1.0f, 0.0f, 0.0f, 1.0f),
                        color = new Vector4(1.0f, 1.0f, 1.0f, 1.0f),
                        texCoord = new Vector2(0.0f, 0.25f)
                    },
                    new Renderer.VertexDataStruct // left 13
                    {
                        position = new Vector4(verticies[5].X, verticies[5].Y, verticies[5].Z, 1.0f),
                        normal = new Vector4(-1.0f, 0.0f, 0.0f, 1.0f),
                        color = new Vector4(1.0f, 1.0f, 1.0f, 1.0f),
                        texCoord = new Vector2(0.0f, 0.5f)
                    },
                    new Renderer.VertexDataStruct // left 14
                    {
                        position = new Vector4(verticies[6].X, verticies[6].Y, verticies[6].Z, 1.0f),
                        normal = new Vector4(-1.0f, 0.0f, 0.0f, 1.0f),
                        color = new Vector4(1.0f, 1.0f, 1.0f, 1.0f),
                        texCoord = new Vector2(0.25f, 0.5f)
                    },
                    new Renderer.VertexDataStruct // left 15
                    {
                        position = new Vector4(verticies[7].X, verticies[7].Y, verticies[7].Z, 1.0f),
                        normal = new Vector4(-1.0f, 0.0f, 0.0f, 1.0f),
                        color = new Vector4(1.0f, 1.0f, 1.0f, 1.0f),
                        texCoord = new Vector2(0.25f, 0.25f)
                    },
                    new Renderer.VertexDataStruct // top 16
                    {
                        position = new Vector4(verticies[2].X, verticies[2].Y, verticies[2].Z, 1.0f),
                        normal = new Vector4(0.0f, 1.0f, 0.0f, 1.0f),
                        color = new Vector4(1.0f, 1.0f, 1.0f, 1.0f),
                        texCoord = new Vector2(0.25f, 0.0f)
                    },
                    new Renderer.VertexDataStruct // top 17
                    {
                        position = new Vector4(verticies[3].X, verticies[3].Y, verticies[3].Z, 1.0f),
                        normal = new Vector4(0.0f, 1.0f, 0.0f, 1.0f),
                        color = new Vector4(1.0f, 1.0f, 1.0f, 1.0f),
                        texCoord = new Vector2(0.25f, 0.25f)
                    },
                    new Renderer.VertexDataStruct // top 18
                    {
                        position = new Vector4(verticies[6].X, verticies[6].Y, verticies[6].Z, 1.0f),
                        normal = new Vector4(0.0f, 1.0f, 0.0f, 1.0f),
                        color = new Vector4(1.0f, 1.0f, 1.0f, 1.0f),
                        texCoord = new Vector2(0.5f, 0.25f)
                    },
                    new Renderer.VertexDataStruct // top 19
                    {
                        position = new Vector4(verticies[7].X, verticies[7].Y, verticies[7].Z, 1.0f),
                        normal = new Vector4(0.0f, 1.0f, 0.0f, 1.0f),
                        color = new Vector4(1.0f, 1.0f, 1.0f, 1.0f),
                        texCoord = new Vector2(0.5f, 0.0f)
                    },
                    new Renderer.VertexDataStruct // bottom 20
                    {
                        position = new Vector4(verticies[0].X, verticies[0].Y, verticies[0].Z, 1.0f),
                        normal = new Vector4(0.0f, -1.0f, 0.0f, 1.0f),
                        color = new Vector4(1.0f, 1.0f, 1.0f, 1.0f),
                        texCoord = new Vector2(0.25f, 0.5f)
                    },
                    new Renderer.VertexDataStruct // bottom 21
                    {
                        position = new Vector4(verticies[1].X, verticies[1].Y, verticies[1].Z, 1.0f),
                        normal = new Vector4(0.0f, -1.0f, 0.0f, 1.0f),
                        color = new Vector4(1.0f, 1.0f, 1.0f, 1.0f),
                        texCoord = new Vector2(0.25f, 0.75f)
                    },
                    new Renderer.VertexDataStruct // bottom 22
                    {
                        position = new Vector4(verticies[4].X, verticies[4].Y, verticies[4].Z, 1.0f),
                        normal = new Vector4(0.0f, -1.0f, 0.0f, 1.0f),
                        color = new Vector4(1.0f, 1.0f, 1.0f, 1.0f),
                        texCoord = new Vector2(0.5f, 0.75f)
                    },
                    new Renderer.VertexDataStruct // bottom 23
                    {
                        position = new Vector4(verticies[5].X, verticies[5].Y, verticies[5].Z, 1.0f),
                        normal = new Vector4(0.0f, -1.0f, 0.0f, 1.0f),
                        color = new Vector4(1.0f, 1.0f, 1.0f, 1.0f),
                        texCoord = new Vector2(0.5f, 0.5f)
                    }
                };
            uint[] indices = new uint[36]
            {
                8, 9, 10,       10, 11, 8,
                12, 13, 14,     14, 15, 12,
                20, 21, 22,     22, 23, 20,
                0, 1, 2,        2, 3, 0,
                4, 5, 6,        6, 7, 4,
                16, 17, 18,     18, 19, 16
            };

            return new MeshObject(_directX3DGraphics, position,
                rotation, vertices, indices, PrimitiveTopology.TriangleList,
                material, visible);
        }

        public MeshObject MakeMesh(Vector3 position, Material material)
        {
            Renderer.VertexDataStruct[] vertices = new Renderer.VertexDataStruct[441];

            int index = 0;

            for (float i = -10; i < 11; i++)
            {
                for (float j = -10; j < 11; j++)
                {
                    vertices[index++] = new Renderer.VertexDataStruct
                    {
                        position = new Vector4(i, 0.0f, j, 1.0f),
                        normal = new Vector4(0.0f, 1.0f, 0.0f, 1.0f),
                        color = new Vector4(0.8f, 0.8f, 0.8f, 1.0f)
                    };
                }
            }

            uint[] indices = new uint[400 * 6];

            uint pos = 0;

            index = 0;

            for (uint i = 0; i < 20; i++)
            {
                for (uint j = 0; j < 20; j++)
                {
                    pos = j + i * 21;
                    indices[index++] = pos;
                    indices[index++] = pos + 21;
                    indices[index++] = pos + 1;
                    indices[index++] = pos + 1;
                    indices[index++] = pos + 21;
                    indices[index++] = pos + 22;
                }
            }

            return new MeshObject(_directX3DGraphics, position,
                Vector3.Zero, vertices, indices, PrimitiveTopology.TriangleList, material, true);
        }

        public List<MeshObject> LoadFbx(string filename, Vector3 rotation, Material material, Texture textur, Scene scene, bool visible)
        {
            Assimp.Scene model;
            Assimp.AssimpContext importer = new Assimp.AssimpContext();
            importer.SetConfig(new Assimp.Configs.NormalSmoothingAngleConfig(66.0f));
            model = importer.ImportFile(filename, Assimp.PostProcessPreset.TargetRealTimeMaximumQuality);

            List<MeshObject> meshes = new List<MeshObject>();

            for (int mc = 0; mc < model.Meshes.Count; mc++)
            {
                Assimp.Mesh mesh = model.Meshes[mc];
                Assimp.Material mat = model.Materials[mesh.MaterialIndex];

                Material fbxMaterial = material;
                List<Renderer.VertexDataStruct> vertices = new List<Renderer.VertexDataStruct>();
                if (mat.GetMaterialTexture(Assimp.TextureType.Diffuse, 0, out Assimp.TextureSlot slot))
                {
                    var ambient = mat.ColorAmbient;
                    var diffuse = mat.ColorDiffuse;
                    var emissive = mat.ColorEmissive;
                    var secular = mat.ColorSpecular;
                    var texturePath = slot.FilePath;

                    while (texturePath[0] == '.')
                    {
                        texturePath = texturePath.Substring(3, texturePath.Length - 3);
                    }

                    texturePath = string.Concat("assets\\", texturePath);

                    Texture texture;
                    if (!scene.Textures.ContainsKey(texturePath))
                    {
                        texture = LoadTextureFromFile(texturePath, new Renderer(_directX3DGraphics).PointSampler, false);
                        scene.Textures.Add(texturePath, texture);
                    }
                    else
                    {
                        texture = scene.Textures[texturePath];
                    }

                    fbxMaterial = new Material(texture,
                        new Vector3(emissive.R, emissive.G, emissive.B),
                        new Vector3(ambient.R, ambient.G, ambient.B),
                        new Vector3(diffuse.R, diffuse.G, diffuse.B),
                        new Vector3(secular.R, secular.G, secular.B),
                        8.0f);
                    try
                    {
                        for (int i = 0; i < mesh.VertexCount; i++)
                        {
                            vertices.Add(
                                 new Renderer.VertexDataStruct
                                 {
                                     position = new Vector4(mesh.Vertices[i].X, mesh.Vertices[i].Y, mesh.Vertices[i].Z, 1.0f),
                                     normal = new Vector4(mesh.Normals[i].X, mesh.Normals[i].Y, mesh.Normals[i].Z, 1.0f),
                                     color = new Vector4(1.0f, 1.0f, 1.0f, 1.0f),
                                     texCoord = new Vector2(mesh.TextureCoordinateChannels[0][i].X, 1f - mesh.TextureCoordinateChannels[0][i].Y)
                                 });
                        }
                    }
                    catch
                    {
                        continue;
                    }
                }
                else
                {
                    var ambient = mat.ColorAmbient;
                    var diffuse = mat.ColorDiffuse;
                    var emissive = mat.ColorEmissive;
                    var secular = mat.ColorSpecular;

                    fbxMaterial = new Material(textur,
                        new Vector3(emissive.R, emissive.G, emissive.B),
                        new Vector3(ambient.R, ambient.G, ambient.B),
                        new Vector3(diffuse.R, diffuse.G, diffuse.B),
                        new Vector3(secular.R, secular.G, secular.B),
                        8.0f);

                    for (int i = 0; i < mesh.VertexCount; i++)
                    {
                        vertices.Add(
                             new Renderer.VertexDataStruct
                             {
                                 position = new Vector4(mesh.Vertices[i].X, mesh.Vertices[i].Y, mesh.Vertices[i].Z, 1.0f),
                                 normal = new Vector4(mesh.Normals[i].X, mesh.Normals[i].Y, mesh.Normals[i].Z, 1.0f),
                                 color = new Vector4(1.0f, 1.0f, 1.0f, 1.0f),
                             });
                    }
                }
                List<uint> indices = new List<uint>();
                try
                {
                    for (int i = 0; i < mesh.FaceCount; i++)
                    {
                        indices.Add((uint)mesh.Faces[i].Indices[2]);
                        indices.Add((uint)mesh.Faces[i].Indices[1]);
                        indices.Add((uint)mesh.Faces[i].Indices[0]);
                    }
                }
                catch
                {
                    continue;
                }

                meshes.Add(new MeshObject(_directX3DGraphics, Vector3.Zero,
                new Vector3(0, -90, 0) + rotation, vertices.ToArray(), indices.ToArray(), PrimitiveTopology.TriangleList,
                fbxMaterial, visible));
            }

            return meshes;
        }

        public MeshObject MakeCube(float side, Vector3 position, Vector3 rotation, Material material, bool visible)
        {
            float a = side;
            Renderer.VertexDataStruct[] vertices =
                new Renderer.VertexDataStruct[24]
                {
                    new Renderer.VertexDataStruct // front 0
                    {
                        position = new Vector4(-a, a, -a, 1.0f),
                        normal = new Vector4(0.0f, 0.0f, -1.0f, 1.0f),
                        color = new Vector4(1.0f, 1.0f, 1.0f, 1.0f),
                        texCoord = new Vector2(0.25f, 0.25f)
                    },
                    new Renderer.VertexDataStruct // front 1
                    {
                        position = new Vector4(-a, -a, -a, 1.0f),
                        normal = new Vector4(0.0f, 0.0f, -1.0f, 1.0f),
                        color = new Vector4(1.0f, 1.0f, 1.0f, 1.0f),
                        texCoord = new Vector2(0.25f, 0.5f)
                    },
                    new Renderer.VertexDataStruct // front 2
                    {
                        position = new Vector4(a, -a, -a, 1.0f),
                        normal = new Vector4(0.0f, 0.0f, -1.0f, 1.0f),
                        color = new Vector4(1.0f, 1.0f, 1.0f, 1.0f),
                        texCoord = new Vector2(0.5f, 0.5f)
                    },
                    new Renderer.VertexDataStruct // front 3
                    {
                        position = new Vector4(a, a, -a, 1.0f),
                        normal = new Vector4(0.0f, 0.0f, -1.0f, 1.0f),
                        color = new Vector4(1.0f, 1.0f, 1.0f, 1.0f),
                        texCoord = new Vector2(0.5f, 0.25f)
                    },
                    new Renderer.VertexDataStruct // right 4
                    {
                        position = new Vector4(a, a, -a, 1.0f),
                        normal = new Vector4(1.0f, 0.0f, 0.0f, 1.0f),
                        color = new Vector4(1.0f, 1.0f, 1.0f, 1.0f),
                        texCoord = new Vector2(0.5f, 0.25f)
                    },
                    new Renderer.VertexDataStruct // right 5
                    {
                        position = new Vector4(a, -a, -a, 1.0f),
                        normal = new Vector4(1.0f, 0.0f, 0.0f, 1.0f),
                        color = new Vector4(1.0f, 0.0f, 1.0f, 1.0f),
                        texCoord = new Vector2(0.5f, 0.5f)
                    },
                    new Renderer.VertexDataStruct // right 6
                    {
                        position = new Vector4(a, -a, a, 1.0f),
                        normal = new Vector4(1.0f, 0.0f, 0.0f, 1.0f),
                        color = new Vector4(1.0f, 1.0f, 1.0f, 1.0f),
                        texCoord = new Vector2(0.75f, 0.5f)
                    },
                    new Renderer.VertexDataStruct // right 7
                    {
                        position = new Vector4(a, a, a, 1.0f),
                        normal = new Vector4(1.0f, 0.0f, 0.0f, 1.0f),
                        color = new Vector4(1.0f, 1.0f, 1.0f, 1.0f),
                        texCoord = new Vector2(0.75f, 0.25f)
                    },
                    new Renderer.VertexDataStruct // back 8
                    {
                        position = new Vector4(a, a, a, 1.0f),
                        normal = new Vector4(0.0f, 0.0f, 1.0f, 1.0f),
                        color = new Vector4(1.0f, 1.0f, 1.0f, 1.0f),
                        texCoord = new Vector2(0.75f, 0.25f)
                    },
                    new Renderer.VertexDataStruct // back 9
                    {
                        position = new Vector4(a, -a, a, 1.0f),
                        normal = new Vector4(0.0f, 0.0f, 1.0f, 1.0f),
                        color = new Vector4(1.0f, 1.0f, 1.0f, 1.0f),
                        texCoord = new Vector2(0.75f, 0.5f)
                    },
                    new Renderer.VertexDataStruct // back 10
                    {
                        position = new Vector4(-a, -a, a, 1.0f),
                        normal = new Vector4(0.0f, 0.0f, 1.0f, 1.0f),
                        color = new Vector4(1.0f, 1.0f, 1.0f, 1.0f),
                        texCoord = new Vector2(1.0f, 0.5f)
                    },
                    new Renderer.VertexDataStruct // back 11
                    {
                        position = new Vector4(-a, a, a, 1.0f),
                        normal = new Vector4(0.0f, 0.0f, 1.0f, 1.0f),
                        color = new Vector4(1.0f, 1.0f, 1.0f, 1.0f),
                        texCoord = new Vector2(1.0f, 0.25f)
                    },
                    new Renderer.VertexDataStruct // left 12
                    {
                        position = new Vector4(-a, a, a, 1.0f),
                        normal = new Vector4(-1.0f, 0.0f, 0.0f, 1.0f),
                        color = new Vector4(1.0f, 1.0f, 1.0f, 1.0f),
                        texCoord = new Vector2(0.0f, 0.25f)
                    },
                    new Renderer.VertexDataStruct // left 13
                    {
                        position = new Vector4(-a, -a, a, 1.0f),
                        normal = new Vector4(-1.0f, 0.0f, 0.0f, 1.0f),
                        color = new Vector4(1.0f, 1.0f, 1.0f, 1.0f),
                        texCoord = new Vector2(0.0f, 0.5f)
                    },
                    new Renderer.VertexDataStruct // left 14
                    {
                        position = new Vector4(-a, -a, -a, 1.0f),
                        normal = new Vector4(-1.0f, 0.0f, 0.0f, 1.0f),
                        color = new Vector4(1.0f, 1.0f, 1.0f, 1.0f),
                        texCoord = new Vector2(0.25f, 0.5f)
                    },
                    new Renderer.VertexDataStruct // left 15
                    {
                        position = new Vector4(-a, a, -a, 1.0f),
                        normal = new Vector4(-1.0f, 0.0f, 0.0f, 1.0f),
                        color = new Vector4(1.0f, 1.0f, 1.0f, 1.0f),
                        texCoord = new Vector2(0.25f, 0.25f)
                    },
                    new Renderer.VertexDataStruct // top 16
                    {
                        position = new Vector4(-a, a, a, 1.0f),
                        normal = new Vector4(0.0f, 1.0f, 0.0f, 1.0f),
                        color = new Vector4(1.0f, 1.0f, 1.0f, 1.0f),
                        texCoord = new Vector2(0.25f, 0.0f)
                    },
                    new Renderer.VertexDataStruct // top 17
                    {
                        position = new Vector4(-a, a, -a, 1.0f),
                        normal = new Vector4(0.0f, 1.0f, 0.0f, 1.0f),
                        color = new Vector4(1.0f, 1.0f, 1.0f, 1.0f),
                        texCoord = new Vector2(0.25f, 0.25f)
                    },
                    new Renderer.VertexDataStruct // top 18
                    {
                        position = new Vector4(a, a, -a, 1.0f),
                        normal = new Vector4(0.0f, 1.0f, 0.0f, 1.0f),
                        color = new Vector4(1.0f, 1.0f, 1.0f, 1.0f),
                        texCoord = new Vector2(0.5f, 0.25f)
                    },
                    new Renderer.VertexDataStruct // top 19
                    {
                        position = new Vector4(a, a, a, 1.0f),
                        normal = new Vector4(0.0f, 1.0f, 0.0f, 1.0f),
                        color = new Vector4(1.0f, 1.0f, 1.0f, 1.0f),
                        texCoord = new Vector2(0.5f, 0.0f)
                    },
                    new Renderer.VertexDataStruct // bottom 20
                    {
                        position = new Vector4(-a, -a, -a, 1.0f),
                        normal = new Vector4(0.0f, -1.0f, 0.0f, 1.0f),
                        color = new Vector4(1.0f, 1.0f, 1.0f, 1.0f),
                        texCoord = new Vector2(0.25f, 0.5f)
                    },
                    new Renderer.VertexDataStruct // bottom 21
                    {
                        position = new Vector4(-a, -a, a, 1.0f),
                        normal = new Vector4(0.0f, -1.0f, 0.0f, 1.0f),
                        color = new Vector4(1.0f, 1.0f, 1.0f, 1.0f),
                        texCoord = new Vector2(0.25f, 0.75f)
                    },
                    new Renderer.VertexDataStruct // bottom 22
                    {
                        position = new Vector4(a, -a, a, 1.0f),
                        normal = new Vector4(0.0f, -1.0f, 0.0f, 1.0f),
                        color = new Vector4(1.0f, 1.0f, 1.0f, 1.0f),
                        texCoord = new Vector2(0.5f, 0.75f)
                    },
                    new Renderer.VertexDataStruct // bottom 23
                    {
                        position = new Vector4(a, -a, -a, 1.0f),
                        normal = new Vector4(0.0f, -1.0f, 0.0f, 1.0f),
                        color = new Vector4(1.0f, 1.0f, 1.0f, 1.0f),
                        texCoord = new Vector2(0.5f, 0.5f)
                    }
                };
            uint[] indices = new uint[36]
            {
                8, 9, 10,       10, 11, 8,
                12, 13, 14,     14, 15, 12,
                20, 21, 22,     22, 23, 20,
                0, 1, 2,        2, 3, 0,
                4, 5, 6,        6, 7, 4,
                16, 17, 18,     18, 19, 16
            };

            return new MeshObject(_directX3DGraphics, position,
                rotation, vertices, indices, PrimitiveTopology.TriangleList,
                material, visible);
        }

        public MeshObject MakeAxes(Material material, bool visible)
        {
            Renderer.VertexDataStruct[] vertices =
                new Renderer.VertexDataStruct[6]
                {
                    new Renderer.VertexDataStruct // x
                    {
                        position = new Vector4(0.0f, 0.0f, 0.0f, 1.0f),
                        color = new Vector4(1.0f, 0.0f, 0.0f, 1.0f),
                        texCoord = new Vector2(0.0f, 0.0f)
                    },
                    new Renderer.VertexDataStruct
                    {
                        position = new Vector4(1.0f, 0.0f, 0.0f, 1.0f),
                        color = new Vector4(1.0f, 0.0f, 0.0f, 1.0f),
                        texCoord = new Vector2(0.0f, 0.0f)
                    },
                    new Renderer.VertexDataStruct // y
                    {
                        position = new Vector4(0.0f, 0.0f, 0.0f, 1.0f),
                        color = new Vector4(0.0f, 1.0f, 0.0f, 1.0f),
                        texCoord = new Vector2(0.0f, 0.0f)
                    },
                    new Renderer.VertexDataStruct
                    {
                        position = new Vector4(0.0f, 1.0f, 0.0f, 1.0f),
                        color = new Vector4(0.0f, 1.0f, 0.0f, 1.0f),
                        texCoord = new Vector2(0.0f, 0.0f)
                    },
                    new Renderer.VertexDataStruct // z
                    {
                        position = new Vector4(0.0f, 0.0f, 0.0f, 1.0f),
                        color = new Vector4(0.0f, 0.0f, 1.0f, 1.0f),
                        texCoord = new Vector2(0.0f, 0.0f)
                    },
                    new Renderer.VertexDataStruct
                    {
                        position = new Vector4(0.0f, 0.0f, 1.0f, 1.0f),
                        color = new Vector4(0.0f, 0.0f, 1.0f, 1.0f),
                        texCoord = new Vector2(0.0f, 0.0f)
                    }
                };

            uint[] indices = new uint[6];
            for (uint i = 0; i <= 6 - 1; ++i) indices[i] = i;

            return new MeshObject(_directX3DGraphics, Vector3.Zero,
                Vector3.Zero, vertices, indices, PrimitiveTopology.LineList,
                material, visible);
        }

        public void Dispose()
        {
            Utilities.Dispose(ref _imagingFactory);
        }
    }
}
