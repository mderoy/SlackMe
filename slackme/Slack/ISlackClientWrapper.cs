using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Slack.Webhooks;

namespace slackme.Slack
{
	public interface ISlackClientWrapper
	{
		void Post(SlackMessage message);
	}
}
