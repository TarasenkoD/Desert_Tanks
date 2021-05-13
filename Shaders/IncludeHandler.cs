using System;
using System.IO;
using System.Windows.Forms;
using SharpDX.D3DCompiler;

namespace PGZ_Desert_Battle
{
    class IncludeHandler : Include
    {
        private IDisposable _shadow = null;
        public IDisposable Shadow { get => _shadow; set => _shadow = value; }

        private Stream _stream;

        public void Close(Stream stream)
        {
            _stream.Dispose();
            _stream = null;
        }

        public void Dispose()
        {
            _stream?.Dispose();
        }

        public Stream Open(IncludeType type, string fileName, Stream parentStream)
        {
            string path = Application.StartupPath;
            FileInfo[] files = new DirectoryInfo(path).GetFiles(fileName, SearchOption.AllDirectories);
            FileStream fileStream = new FileStream(files[0].FullName, FileMode.Open);
            return fileStream;
        }
    }
}
