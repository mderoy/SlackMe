using System;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using NiceIO;

namespace slackme.Slack
{
	public class SlackInfo
	{
		private const char ConfigurationDelimiter = '=';
		private const string ConfigurationFileName = "slackme.cfg";

		public string HookUrl;
		public string Channel;
		public string Username;

		bool TryParseConfig()
		{
			string username = "";
			string hookurl = "";
			
			NPath configurationFilePath = LocateConfigurationFilePath();
			if (configurationFilePath.Exists())
			{
				foreach (string line in configurationFilePath.ReadAllLines())
				{
					if (line.Contains("slackurl"))
					{
						hookurl = line.Split(ConfigurationDelimiter).Last();
					}
					if (line.Contains("username"))
					{
						username = line.Split(ConfigurationDelimiter).Last();
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

		public SlackInfo()
		{
			if (!TryParseConfig())
			{
				Console.WriteLine("Unable to Parse slackme.cfg");
				Environment.Exit(1);
			}
			Username = "Slack Me!";
		}

		private NPath LocateConfigurationFilePath()
		{
			var assemblyDirectory = typeof(SlackMe).Assembly.Location.ToNPath().Parent;
			var possibleConfigPath = assemblyDirectory.Combine(ConfigurationFileName);
			if (possibleConfigPath.Exists())
				return possibleConfigPath;

			return Environment.GetFolderPath(Environment.SpecialFolder.UserProfile).ToNPath().Combine(ConfigurationFileName);
		}
	}
}