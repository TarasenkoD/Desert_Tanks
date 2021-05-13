using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using SharpDX;
using SharpDX.Direct3D11;

namespace PGZ_Desert_Battle
{
    [StructLayout(LayoutKind.Sequential)]
    public struct PerFrameData
    {
        public float time;
        public Vector3 _padding;
    }

    class PerFrameConstantBuffer : ConstantBuffer<PerFrameData>
    {
        public PerFrameConstantBuffer(Device device, DeviceContext deviceContext, CommonShaderStage commonShaderStage, int subresource, int slot) 
            : base(device, deviceContext, commonShaderStage, subresource, slot)
        {
        }

        public void Update(float time)
        {
            _data.time = time;
            Update();
        }
    }
}
