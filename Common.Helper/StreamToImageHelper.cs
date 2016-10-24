using System;
using System.IO;

namespace Common.Helper
{
    public class StreamToImageHelper
    {
        private const string AnswerDirectory = @"Images\Answer\";
        private const string AnswerWebUri = @"Images/Answer/";

        private const string StudentDirectory = @"Images\Student\";
        public static string StudentWebUri = @"Images/Student/";

        public static byte[] ImageToByteArray(string fileName)
        {
            if (File.Exists(fileName))
            {
                using (var fs = new FileStream(fileName, FileMode.Open, FileAccess.Read))
                {
                    var content = new byte[fs.Length];
                    fs.Read(content, 0, (int)fs.Length);
                    return content;
                }
            }
            return null;
        }

        public static string ByteArrayToImage(byte[] byteArray, string directoryPath, string fileName, string urlPrefix)
        {
            if (byteArray == null || byteArray.Length < 10)
                return string.Empty;

            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }
            if (!File.Exists(Path.Combine(directoryPath, fileName)))
            {
                WriteImage(byteArray, Path.Combine(directoryPath, fileName));
            }
            return new Uri(new Uri(urlPrefix), fileName).ToString();
        }

        private static void WriteImage(byte[] byteArray, string fileName)
        {
            using (var fileStream = File.Create(fileName, (int)byteArray.Length))
            {
                fileStream.Write(byteArray, 0, byteArray.Length);
            }
        }

        public static string WriteAnswer(byte[] byteArray, string fileName, string hostUrl)
        {
            var directory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, AnswerDirectory);
            return ByteArrayToImage(byteArray, directory, fileName, new Uri(new Uri(hostUrl), AnswerWebUri).ToString());
        }

        public static string WriteStudentIcon(byte[] byteArray, string fileName, string hostUrl)
        {
            var directory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, StudentDirectory);
            return ByteArrayToImage(byteArray, directory, fileName, new Uri(new Uri(hostUrl), StudentWebUri).ToString());
        }
    }
}
