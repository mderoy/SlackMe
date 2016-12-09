using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.Remoting.Channels;
using System.Text;
using System.Threading.Tasks;
using slackme.Slack;
using Slack.Webhooks;

/*
 * Place slackme.cfg in your C:\Users\<user folder>\
 * 
 * slackme.cfg Example
 * slackurl=<slack hook URL>"
 * username=<slack username>
 * 
 * Unity employees can get our slack hook URL from here https://unity.slack.com/services/B3DGVAUCE
 */

namespace slackme
{
	class SlackMe
	{
		const int NumLinesToSave = 20;
		static void Main(string[] args)
		{
			string commandPassTru = "";
			foreach (string str in args)
				commandPassTru += str + " ";

			ProcessStartInfo startInfo = new ProcessStartInfo("cmd", "/c " + commandPassTru)
			{
				//Fixme Mac, Linux support
				FileName = "powershell.exe",
				WindowStyle = ProcessWindowStyle.Hidden,
				UseShellExecute = false,
				RedirectStandardOutput = true,
				CreateNoWindow = false,
				WorkingDirectory = Environment.CurrentDirectory
			};

			SlackInfo info = new SlackInfo();
			SlackChannelPoster poster = new SlackChannelPoster(info);
			SlackMessage msg = new SlackMessage();
			Process process = Process.Start(startInfo);
			List<string> lastLines = new List<string>();
			process.OutputDataReceived += (sender, e) =>
			{
				lastLines.Add(e.Data);
				if (lastLines.Count > NumLinesToSave)
				{
					lastLines.RemoveAt(0);
				}
				Console.WriteLine(e.Data);
			};
			process.BeginOutputReadLine();
			process.WaitForExit();

			String output = "Command: \"" + commandPassTru + "\" Has Finished\n";

			SlackAttachment attachment = new SlackAttachment();
			attachment.Color = "#FF9C27";
			attachment.Title = "Command Output (Truncated)";
			attachment.Text = "";
			foreach (string line in lastLines)
			{
				attachment.Text += line;
			}

			List<SlackAttachment> attachments = new List<SlackAttachment>();
			attachments.Add(attachment);

			poster.Post(output, attachments);
		}
	}
}
