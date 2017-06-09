using System;
using System.Collections.Generic;
using System.Linq;

namespace slackme.Utils
{
	internal static class Helpers
	{
		internal static string AggregateWithSpace(this IEnumerable<string> elements)
		{
			return elements.AggregateWith(" ");
		}

		internal static string AggregateWith(this IEnumerable<string> elements, string separator)
		{
			if (elements.Any())
				return elements.Aggregate((buff, s) => buff + separator + s);

			return string.Empty;
		}
	}
}
