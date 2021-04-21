using System;
using System.IO;

namespace JToolbox.Core.Utilities
{
    public static class FileSystem
    {
        private static Random random = new Random();

        public static void CreateRandomFile(string filePath, long size)
        {
            var data = new byte[size];
            random.NextBytes(data);
            File.WriteAllBytes(filePath, data);
        }

        public static void CreateEmptyFile(string filePath, long size)
        {
            using (var fs = new FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.None))
            {
                fs.SetLength(size);
            }
        }
    }
}