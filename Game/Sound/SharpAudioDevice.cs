using SharpDX;
using SharpDX.Multimedia;
using SharpDX.Windows;
using SharpDX.XAudio2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PGZ_Desert_Battle
{
    public class SharpAudioDevice : IDisposable
    {
        XAudio2 _device;
        MasteringVoice _master;

        /// <summary>
        /// Audio Device
        /// </summary>
        public XAudio2 Device
        {
            get { return _device; }
        }

        /// <summary>
        /// Master voice
        /// </summary>
        public MasteringVoice Master
        {
            get { return _master; }
            set { _master = value; }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public SharpAudioDevice()
        {
            _device = new XAudio2();
            _master = new MasteringVoice(_device);
        }

        /// <summary>
        /// Release resource
        /// </summary>
        public void Dispose()
        {
            _master.Dispose();
            _device.Dispose();
        }
    }
}
