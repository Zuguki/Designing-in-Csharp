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

	public class MedianMarkdownReportMaker : ReportMaker
	{
		protected override string Caption => "Median";

		protected override string BeginList() => "";

		protected override string EndList() => "";

		protected override string MakeCaption(string caption) => $"## {caption}\n\n";

		protected override string MakeItem(string valueType, string entry) => $" * **{valueType}**: {entry}\n\n";

		protected override object MakeStatistics(IEnumerable<double> data)
		{
			var list = data.OrderBy(z => z).ToList();
			if (list.Count % 2 == 0)
				return (list[list.Count / 2] + list[list.Count / 2 - 1]) / 2;
			
			return list[list.Count / 2];
		}
	}

	public static class ReportMakerHelper
	{
		public static string MeanAndStdHtmlReport(IEnumerable<Measurement> data)
		{
			return new MeanAndStdHtmlReportMaker().MakeReport(data);
		}

		public static string MedianMarkdownReport(IEnumerable<Measurement> data)
		{
			return new MedianMarkdownReportMaker().MakeReport(data);
		}

		public static string MeanAndStdMarkdownReport(IEnumerable<Measurement> measurements)
		{
			throw new NotImplementedException();
		}

		public static string MedianHtmlReport(IEnumerable<Measurement> measurements)
		{
			throw new NotImplementedException();
		}
	}
}
