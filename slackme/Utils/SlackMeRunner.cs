using System;
using System.IO;
using System.Linq;
using NiceIO;
using Unity.IL2CPP;

namespace slackme.Utils
{
	internal class SlackMeRunner : Shell.IExecuteController
	{
		private NPath allOutputFilePath;
		private StreamWriter allOutputStream;

		internal SlackMeRunResult Run(string[] args)
		{
			allOutputFilePath = NPath.SystemTemp.Combine($"slackme-{Path.GetRandomFileName()}.log");
			if (allOutputFilePath.Exists())
				allOutputFilePath.Delete();

			allOutputStream = new StreamWriter(allOutputFilePath.ToString());
			try
			{
				using (var disablePopups = new ChangeErrorMode(ErrorModes.FailCriticalErrors | ErrorModes.NoGpFaultErrorBox))
				{
					var commandExe = args[0];
					var commandArgs = args.Skip(1).AggregateWithSpace();

					var executeArgs = new Shell.ExecuteArgs
					{
						Executable = commandExe,
						Arguments = commandArgs
					};

					var executeResult = Shell.Execute(executeArgs, this);

					return new SlackMeRunResult
					{
						AllOutputFilePath = allOutputFilePath,
						ExecuteResult = executeResult,
                        ExecuteArgs = executeArgs
					};
				}
			}
			finally
			{
				allOutputStream.Dispose();
			}
		}

		public void OnStdoutReceived(string data)
		{
			Console.Out.WriteLine(data);
			allOutputStream.WriteLine(data);
		}

		public void OnStderrReceived(string data)
		{
			Console.Error.WriteLine(data);
			allOutputStream.WriteLine(data);
		}

		public void AboutToCleanup(string tempOutputFile, string tempErrorFile)
		{
		}

		public class SlackMeRunResult : IDisposable
		{
			public NPath AllOutputFilePath;
			public Shell.ExecuteResult ExecuteResult;
		    public Shell.ExecuteArgs ExecuteArgs;

			public void Dispose()
			{
				if (AllOutputFilePath.Exists())
					AllOutputFilePath.Delete();
			}
		}
	}
}
