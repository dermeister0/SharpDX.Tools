using ICSharpCode.SharpZipLib.Zip;
using SharpDX.Toolkit.Content;
using System;

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

            return zipFile.GetInputStream(entry);
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
