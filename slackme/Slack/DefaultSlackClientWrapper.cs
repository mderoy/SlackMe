using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Slack.Webhooks;

namespace slackme.Slack
{
	public class DefaultSlackClientWrapper : ISlackClientWrapper
	{
		private readonly SlackClient _client;

		public DefaultSlackClientWrapper(string webhookUrl)
		{
			_client = new SlackClient(webhookUrl);
		}

		public void Post(SlackMessage message)
		{
			_client.Post(message);
		}

		public static ISlackClientWrapper Create(string webhookUrl)
		{
			return new DefaultSlackClientWrapper(webhookUrl);
		}
	}
}
