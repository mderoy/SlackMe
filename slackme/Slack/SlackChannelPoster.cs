using System;
using System.Collections.Generic;
using Slack.Webhooks;

namespace slackme.Slack
{
	public class SlackChannelPoster
	{
		private readonly ISlackClientWrapper _client;
		private readonly SlackInfo _info;

		public SlackChannelPoster(SlackInfo info)
			: this(info, new DefaultSlackClientWrapper(info.HookUrl))
		{
		}

		public SlackChannelPoster(SlackInfo info, ISlackClientWrapper client)
		{
			_info = info;
			_client = client;
		}

		public bool MissingSlackSettings
		{
			get { return _info == null || string.IsNullOrEmpty(_info.HookUrl) || string.IsNullOrEmpty(_info.Channel); }
		}

		public void Post(string message, params object[] args)
		{
			Post(string.Format(message, args));
		}

		public void Post(string message, List<SlackAttachment> attachment)
		{
			// Just ignore the post if slack settings were not provided
			if (MissingSlackSettings)
			{
				Console.WriteLine("Slack settings missing.  Skipping post");
				return;
			}

			var slackMessage = new SlackMessage
			{
				Channel = _info.Channel,
				Text = message,
				Username = _info.Username,
				Attachments = attachment
			};

				_client.Post(slackMessage);
		}
	}
}
