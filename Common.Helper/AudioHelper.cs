using System.Threading.Tasks;
using Common.Ioc;

namespace Common.Helper
{
    public class AudioHelper
    {
        private static readonly string Ffmpegpath = string.Format(@"{0}addon\ffmpeg.exe", AssemblyHelper.GetAssembleFilePath());


        private static void Cmd(string c)
        {
            var process = new System.Diagnostics.Process
            {
                StartInfo =
                {
                    FileName = Ffmpegpath,
                    Arguments = c,
                    RedirectStandardOutput = false,
                    UseShellExecute = false,
                    CreateNoWindow = true,
                }
            };

            process.Start();
            process.WaitForExit();
        }

        private static Task CmdAsync(string c)
        {
            /*  process.StartInfo.FileName = "cmd.exe";
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.CreateNoWindow = true;
                process.StartInfo.RedirectStandardOutput = true;
                process.StartInfo.RedirectStandardInput = true;
                process.Start();
                process.StandardInput.WriteLine(c);
                process.StandardInput.AutoFlush = true;
                process.StandardInput.WriteLine("exit");
                process.WaitForExit(waitingTime);*/

            // there is no non-generic TaskCompletionSource
            var tcs = new TaskCompletionSource<bool>();
            var process = new System.Diagnostics.Process
            {
                StartInfo =
                {
                    FileName = Ffmpegpath,
                    Arguments = c,
                    UseShellExecute = false,
                    CreateNoWindow = false,
                    RedirectStandardOutput = true
                }
            };

            process.Exited += (sender, args) =>
            {
                tcs.SetResult(true);
                process.Dispose();
            };
            process.Start();
            process.WaitForExit();


            return tcs.Task;
        }

        private static async Task ConvertToMp3Async(string inputFilePath, string outputFilePath)
        {
            var c = @"-y -i " + inputFilePath + " -q:a 9 " + outputFilePath;
            await CmdAsync(c);
        }

        private static void ConvertToMp3(string inputFilePath, string outputFilePath)
        {
            IoHelper.CreateDirIfNotExists(outputFilePath);
            var c = @" -y -i " + inputFilePath + " -q:a 9 " + outputFilePath;
            Cmd(c);
        }

        public static async Task<byte[]> ConvertToMp3Async(string inputFilePath, string outputFilePath, byte[] bytes)
        {

            await IoHelper.WriteFileAsync(inputFilePath, bytes);
            await ConvertToMp3Async(inputFilePath, outputFilePath);
            return await IoHelper.ReadFileAsync(outputFilePath);
        }

        public static byte[] ConvertToMp3(string inputFilePath, string outputFilePath, byte[] bytes)
        {

            IoHelper.WriteFile(inputFilePath, bytes);
            ConvertToMp3(inputFilePath, outputFilePath);
            return IoHelper.ReadFile(outputFilePath);
        }
    }
}
