using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Delegates.Reports
{
	public class HtmlReportMaker
	{
		public string Caption { get; }

		private static string BeginList => "<ul>";
		private static string EndList => "</ul>";

		private readonly Func<IEnumerable<double>, object> _statistics;

		public HtmlReportMaker(string caption, Func<IEnumerable<double>, object> statistics)
		{
			Caption = caption;
			_statistics = statistics;
		}

		public string MakeReport(IEnumerable<Measurement> data)
		{
			var list = data.ToList();
			var result = new StringBuilder();
			result.Append(MakeCaption(Caption));
			result.Append(BeginList);
			result.Append(MakeItem("Temperature", _statistics(list.Select(z => z.Temperature)).ToString()));
			result.Append(MakeItem("Humidity", _statistics(list.Select(z => z.Humidity)).ToString()));
			result.Append(EndList);
			return result.ToString();
		}

		private static string MakeCaption(string caption) => $"<h1>{caption}</h1>";
		
		private static string MakeItem(string valueType, string entry) => $"<li><b>{valueType}</b>: {entry}";
	}

	public class MarkdownReportMaker
	{
		public string Caption { get; }

		private static string BeginList => "";
		private static string EndList => "";

		private readonly Func<IEnumerable<double>, object> _statistics;

		public MarkdownReportMaker(string caption, Func<IEnumerable<double>, object> statistics)
		{
			Caption = caption;
			_statistics = statistics;
		}

		public string MakeReport(IEnumerable<Measurement> data)
		{
			var list = data.ToList();
			var result = new StringBuilder();
			result.Append(MakeCaption(Caption));
			result.Append(BeginList);
			result.Append(MakeItem("Temperature", _statistics(list.Select(z => z.Temperature)).ToString()));
			result.Append(MakeItem("Humidity", _statistics(list.Select(z => z.Humidity)).ToString()));
			result.Append(EndList);
			return result.ToString();
		}

		private static string MakeCaption(string caption) => $"## {caption}\n\n";
		
		private static string MakeItem(string valueType, string entry) => $" * **{valueType}**: {entry}\n\n";
	}

	public static class ReportMakerHelper
	{
		public static string MeanAndStdHtmlReport(IEnumerable<Measurement> data)
		{
			return new HtmlReportMaker("Mean and Std", (stats) =>
			{
				var list = stats.ToList();
				var mean = list.Average();
				var std = Math.Sqrt(list.Select(z => Math.Pow(z - mean, 2)).Sum() / (list.Count - 1));

				return new MeanAndStd
				{
					Mean = mean,
					Std = std
				};
			}).MakeReport(data);
		}

		public static string MedianMarkdownReport(IEnumerable<Measurement> data)
		{
			return new MarkdownReportMaker("Median", (stats) =>
			{
				var list = stats.OrderBy(z => z).ToList();
				if (list.Count % 2 == 0)
					return (list[list.Count / 2] + list[list.Count / 2 - 1]) / 2;

				return list[list.Count / 2];
			}).MakeReport(data);
		}

		public static string MeanAndStdMarkdownReport(IEnumerable<Measurement> data)
		{
			return new MarkdownReportMaker("Mean and Std", (stats) =>
			{
				var list = stats.ToList();
				var mean = list.Average();
				var std = Math.Sqrt(list.Select(z => Math.Pow(z - mean, 2)).Sum() / (list.Count - 1));

				return new MeanAndStd
				{
					Mean = mean,
					Std = std
				};
			}).MakeReport(data);
		}

		public static string MedianHtmlReport(IEnumerable<Measurement> data)
		{
			return new HtmlReportMaker("Median", (stats) =>
			{
				var list = stats.OrderBy(z => z).ToList();
				if (list.Count % 2 == 0)
					return (list[list.Count / 2] + list[list.Count / 2 - 1]) / 2;

				return list[list.Count / 2];
			}).MakeReport(data);
		}
	}
}
