using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Delegates.Reports
{
	public abstract class ReportMaker
	{
		protected abstract string MakeCaption(string caption);
		protected abstract string BeginList();
		protected abstract string MakeItem(string valueType, string entry);
		protected abstract string EndList();
		protected abstract object MakeStatistics(IEnumerable<double> data);
		protected abstract string Caption { get; }
		public string MakeReport(IEnumerable<Measurement> measurements)
		{
			var data = measurements.ToList();
			var result = new StringBuilder();
			result.Append(MakeCaption(Caption));
			result.Append(BeginList());
			result.Append(MakeItem("Temperature", MakeStatistics(data.Select(z => z.Temperature)).ToString()));
			result.Append(MakeItem("Humidity", MakeStatistics(data.Select(z => z.Humidity)).ToString()));
			result.Append(EndList());
			return result.ToString();
		}
	}

	public class MeanAndStdHtmlReportMaker : ReportMaker
	{
		protected override string Caption => "Mean and Std";

		protected override string MakeCaption(string caption) => $"<h1>{caption}</h1>";

		protected override string BeginList() => "<ul>";

		protected override string EndList() => "</ul>";

		protected override string MakeItem(string valueType, string entry) => $"<li><b>{valueType}</b>: {entry}";

		protected override object MakeStatistics(IEnumerable<double> data)
		{
			var list = data.ToList();
			var mean = list.Average();
			var std = Math.Sqrt(list.Select(z => Math.Pow(z - mean, 2)).Sum() / (list.Count - 1));

			return new MeanAndStd
			{
				Mean = mean,
				Std = std
			};
		}
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
			return new MeanAndStdHtmlReportMaker().MakeReport(data);
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

		public static string MedianHtmlReport(IEnumerable<Measurement> measurements)
		{
			throw new NotImplementedException();
		}
	}
}
