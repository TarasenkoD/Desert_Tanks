using SharpDX;
using SharpDX.Direct3D11;
using System.Runtime.InteropServices;


namespace PGZ_Desert_Battle
{
    [StructLayout(LayoutKind.Sequential)]
    public struct PerObjectData
    {
        public Matrix worldViewProjectionMatrix;
        public Matrix worldMatrix;
        public Matrix inverseTransposeWorldMatrix;
        public int timeScaling;
        public Vector3 _padding;
    }

    class PerObjectConstantBuffer : ConstantBuffer<PerObjectData>
    {
        public PerObjectConstantBuffer(Device device, DeviceContext deviceContext, CommonShaderStage commonShaderStage, int subresource, int slot) : 
            base(device, deviceContext, commonShaderStage, subresource, slot)
        {
        }

        public void Update(Matrix world, Matrix view, Matrix projection, int timeScaling)
        {

            _data.worldViewProjectionMatrix = Matrix.Multiply(Matrix.Multiply(world, view), projection);
            _data.worldViewProjectionMatrix.Transpose();

            _data.timeScaling = timeScaling;

            _data.worldMatrix = world;
            _data.worldMatrix.Transpose();

            _data.inverseTransposeWorldMatrix = Matrix.Invert(world);

            Update();
        }
    }
}
