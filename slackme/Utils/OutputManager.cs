using System;
using System.Collections.Generic;
using System.IO;
using NiceIO;

namespace slackme.Utils
{
    internal static class OutputManager
    {
        private static TimeSpan OldStuff = new TimeSpan(2, 0, 0, 0);
        private const string LogFileExtension = "txt";

        internal static void Handle(SlackMeRunner.SlackMeRunResult result)
        {
            if (OutputDirectoryForCurrentPlatform != null)
            {
                var expandedPath = ExpandAndCleanupPath(OutputDirectoryForCurrentPlatform);

                expandedPath.EnsureDirectoryExists();

                DoHouseKeeping(expandedPath);

                var outputFilePath = OutputFilePath(result, expandedPath);
                if (outputFilePath.FileExists())
                    outputFilePath.Delete();

                result.AllOutputFilePath.Copy(outputFilePath);
            }
        }

        private static string OutputDirectoryForCurrentPlatform
        {
            get
            {
                if (Environment.OSVersion.Platform == PlatformID.Win32NT)
                    return ConfigFile.WindowsOutputDirectory;

                return ConfigFile.NonWindowsOutputDirectory;
            }
        }

        private static NPath OutputFilePath(SlackMeRunner.SlackMeRunResult result, NPath outputDirectory)
        {
            var cleanTime = DateTime.Now.ToString("yyyy-dd-M-HH-mm-ss");
            var fileName = $"{result.ExecuteArgs.Executable.Replace('.', '-')}-{cleanTime}.{LogFileExtension}";

            return outputDirectory.Combine(fileName);
        }

        private static NPath ExpandAndCleanupPath(string path)
        {
            var expanded = Environment.ExpandEnvironmentVariables(path);
            return expanded.ToNPath();
        }

        /// <summary>
        /// Delete old stuff so that it doesn't accumulate
        /// </summary>
        /// <param name="outputDirectory"></param>
        private static void DoHouseKeeping(NPath outputDirectory)
        {
            foreach (var file in outputDirectory.Files($"*.{LogFileExtension}"))
            {
                var fileInfo = new FileInfo(file.ToString());
                if (DateTime.Now - fileInfo.LastWriteTime > OldStuff)
                    file.Delete();
            }
        }
    }
}
