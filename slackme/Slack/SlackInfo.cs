using System;
using System.Linq;
using NiceIO;
using slackme.Utils;

namespace slackme.Slack
{
	public class SlackInfo
	{
        public readonly string HookUrl;
        public readonly string Channel;
        public readonly string Username;

        public SlackInfo()
        {
            HookUrl = ConfigFile.HookUrl;
            Channel = ConfigFile.Channel;
            Username = ConfigFile.Username;
        }
	}
}