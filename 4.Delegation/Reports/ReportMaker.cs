using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Delegates.Reports
{
	public abstract class ReportMaker
	{
		protected abstract string Caption { get; }
		protected abstract string BeginList { get; }
		protected abstract string EndList { get; }
		protected abstract Func<IEnumerable<double>, object> StatisticsFunc { get; }
		protected abstract string MakeCaption(string caption);
		protected abstract string MakeItem(string valueType, string entry);

		public string MakeReport(IEnumerable<Measurement> data)
		{
			var list = data.ToList();
			var result = new StringBuilder();
			result.Append(MakeCaption(Caption));
			result.Append(BeginList);
			result.Append(MakeItem("Temperature", StatisticsFunc(list.Select(z => z.Temperature)).ToString()));
			result.Append(MakeItem("Humidity", StatisticsFunc(list.Select(z => z.Humidity)).ToString()));
			result.Append(EndList);
			return result.ToString();
		}
	}
	
	public class HtmlReportMaker : ReportMaker
	{
		protected override string Caption { get; }
		protected override Func<IEnumerable<double>, object> StatisticsFunc { get; }

		protected override string BeginList => "<ul>";
		protected override string EndList => "</ul>";

		public HtmlReportMaker(string caption, Func<IEnumerable<double>, object> statisticsFunc)
		{
			Caption = caption;
			StatisticsFunc = statisticsFunc;
		}

		protected override string MakeCaption(string caption) => $"<h1>{caption}</h1>";

		protected override string MakeItem(string valueType, string entry) => $"<li><b>{valueType}</b>: {entry}";
	}

	public class MarkdownReportMaker : ReportMaker
	{
		protected override string Caption { get; }
		protected override Func<IEnumerable<double>, object> StatisticsFunc { get; }

		protected override string BeginList => "";
		protected override string EndList => "";

		public MarkdownReportMaker(string caption, Func<IEnumerable<double>, object> statisticsFunc)
		{
			Caption = caption;
			StatisticsFunc = statisticsFunc;
		}

		protected override string MakeCaption(string caption) => $"## {caption}\n\n";

		protected override string MakeItem(string valueType, string entry) => $" * **{valueType}**: {entry}\n\n";
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
