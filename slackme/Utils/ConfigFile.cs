using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NiceIO;

namespace slackme.Utils
{
    internal static class ConfigFile
    {
        private const char ConfigurationDelimiter = '=';
        private const string ConfigurationFileName = "slackme.cfg";

        public static string HookUrl;
        public static string Channel;
        public static string Username;
        public static string WindowsOutputDirectory;
        public static string NonWindowsOutputDirectory;

        internal static void Load()
        {
            if (!TryParseConfig())
            {
                Console.WriteLine("Unable to Parse slackme.cfg");
                Environment.Exit(1);
            }
            Username = "Slack Me!";
        }

        static bool TryParseConfig()
        {
            string username = "";
            string hookurl = "";

            NPath configurationFilePath = LocateConfigurationFilePath();
            //Console.WriteLine(configurationFilePath);
            if (configurationFilePath.Exists())
            {
                foreach (string line in configurationFilePath.ReadAllLines())
                {
                    if (line.Contains("slackurl"))
                    {
                        hookurl = line.Split(ConfigurationDelimiter).Last();
                    }
                    else if (line.Contains("username"))
                    {
                        username = line.Split(ConfigurationDelimiter).Last();
                    }
                    else if (line.Contains("windowsoutput"))
                    {
                        WindowsOutputDirectory = line.Split(ConfigurationDelimiter).Last();
                    }
                    else if (line.Contains("nonwindowssoutput"))
                    {
                        NonWindowsOutputDirectory = line.Split(ConfigurationDelimiter).Last();
                    }
                }
                if (hookurl != "" && username != "")
                {
                    Channel = "@" + username;
                    HookUrl = hookurl;
                    return true;
                }
                else
                {
                    return false;
                }

            }
            else
            {
                return false;
            }

            return true;
        }

        private static NPath LocateConfigurationFilePath()
        {
            var assemblyDirectory = typeof(SlackMe).Assembly.Location.ToNPath().Parent;
            var possibleConfigPath = assemblyDirectory.Combine(ConfigurationFileName);
            if (possibleConfigPath.Exists())
                return possibleConfigPath;

            return Environment.GetFolderPath(Environment.SpecialFolder.UserProfile).ToNPath().Combine(ConfigurationFileName);
        }
    }
}
