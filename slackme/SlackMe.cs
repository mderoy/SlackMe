using System;
using System.Collections.Generic;
using slackme.Slack;
using slackme.Utils;
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
		const int NumLinesToSave = 50;
		static void Main(string[] args)
		{
			using (var runnerResult = new SlackMeRunner().Run(args))
				PostToSlack(args.AggregateWithSpace(), runnerResult);
		}

		private static void PostToSlack(string command, SlackMeRunner.SlackMeRunResult result)
		{
			SlackInfo info = new SlackInfo();
			SlackChannelPoster poster = new SlackChannelPoster(info);
			var successOrFail = result.ExecuteResult.ExitCode == 0 ? "SUCCESS" : "FAIL";
			String output = $"[{successOrFail}] Command: \"{command}\" Has Finished\n";

			SlackAttachment attachment = new SlackAttachment();
			attachment.Color = "#FF9C27";
			attachment.Title = "Command Output (Truncated)";
			attachment.Text = "";
			var allLines = result.AllOutputFilePath.ReadAllLines();
			var startIndex = allLines.Length <= NumLinesToSave ? 0 : allLines.Length - NumLinesToSave;
			for (int i = startIndex; i < allLines.Length; i++)
			{
			    attachment.Text += allLines[i];
			}

			List<SlackAttachment> attachments = new List<SlackAttachment>();
			attachments.Add(attachment);

			poster.Post(output, attachments);
		}
	}
}
