using ICSharpCode.SharpZipLib.Zip;
using SharpDX.Toolkit.Content;
using System;
using System.IO;

namespace SharpDX.Extended
{
    public class ZipContentResolver : IContentResolver, IDisposable
    {
        ZipFile zipFile;

        bool isDisposed;

        public ZipContentResolver(ZipFile zipFile)
        {
            this.zipFile = zipFile;
        }

        public bool Exists(string assetName)
        {
            return zipFile.GetEntry(ConvertPath(assetName)) != null;
        }

        public System.IO.Stream Resolve(string assetName)
        {
            var entry = zipFile.GetEntry(ConvertPath(assetName));
            if (entry == null)
                throw new AssetNotFoundException(assetName);

            var ms = new MemoryStream();
            zipFile.GetInputStream(entry).CopyTo(ms);
            ms.Seek(0, SeekOrigin.Begin);

            return ms;
        }

        string ConvertPath(string path)
        {
            if (string.IsNullOrEmpty(path))
                return path;

            return path.Replace('\\', '/');
        }
    
        public void Dispose()
        {
 	        Dispose(true);
        }

        protected virtual void Dispose(bool isDisposing)
        {
            if (!isDisposing && !isDisposed)
            {
                if (zipFile != null)
                    zipFile.Close();
                zipFile = null;
                isDisposed = true;
            }
        }
    }
}
