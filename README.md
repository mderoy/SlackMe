# SlackMe
Why keep looking at that terminal window? Run your build, tests, or whatever through SlackMe and get notified when its done!

Simply create a slackme.cfg file in you home directory (on windows C:\Users\<Username>\slackme.cfg)

slackurl=SlackHookURL

username=SlackUsername

Optionally, you can add these two fields to have output files copied to a directory

windowsoutput=%HOMEPATH%

nonwindowssoutput=$HOME

If you're a part of Unity, our webhook URL can be obtained by logging into our slack and looking at https://unity.slack.com/services/B3DGVAUCE

If you're not a part of unity, you can generate a webhook url following instructions on the slack website here https://api.slack.com/incoming-webhooks

Currently Windows only, but should be compatible with Mac & Linux soon after a few minor changes! Stay Tuned!

# Usage
slackme.exe command
