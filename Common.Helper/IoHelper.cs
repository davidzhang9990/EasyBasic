using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Helper
{
    public class IoHelper
    {
        public static void WriteFile(string filePath, byte[] buffer)
        {
            CreateDirIfNotExists(filePath);
            File.WriteAllBytes(filePath, buffer);
        }

        public static byte[] ReadFile(string filePath)
        {

            return File.ReadAllBytes(filePath);
        }

        public static async Task WriteFileAsync(string filePath, byte[] buffer)
        {
            CreateDirIfNotExists(filePath);
            using (var writer = File.OpenWrite(filePath))
            {
                await writer.WriteAsync(buffer, 0, buffer.Length);
            }
        }

        public static async Task<byte[]> ReadFileAsync(string filePath)
        {

            var buffer = new byte[0x1000];
            using (var reader = File.OpenRead(filePath))
            {
                await reader.ReadAsync(buffer, 0, buffer.Length);
                return buffer;
            }
        }

        public static void CreateDirIfNotExists(string filePath)
        {
            var filePathInfo = new FileInfo(filePath);
            if (filePathInfo.Directory != null) filePathInfo.Directory.Create();
        }

        public static void DeleteFile(string inputFilePath)
        {
            File.Delete(inputFilePath);
        }
    }
}
