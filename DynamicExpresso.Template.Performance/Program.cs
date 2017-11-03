using System;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace DynamicExpresso.Template.Performance
{
	public class Program
	{
		public static void Main()
		{
			Console.WriteLine("DynamicExpresso.Template performance tests:");
			Console.WriteLine("A - run automatic tests");
			Console.WriteLine("F - render file");

			switch (Console.ReadLine())
			{
				case "A":
					AutoPerformanceTests();
					break;
				case "F":
					RenderFile();
					break;
			}

			Console.WriteLine("Enter to exit");
			Console.ReadLine();
		}

		private static void RenderFile()
		{
			var engine = new TemplateEngine<MyModel>();

			Console.WriteLine("Enter file name:");

			var file = Console.ReadLine();
			var template = File.ReadAllText(file);

			var stopWatch = new Stopwatch();
			stopWatch.Start();

			var result = engine.Render(template, new MyModel());

			stopWatch.Stop();

			File.WriteAllText(file + ".out", result);

			Console.WriteLine("Elapsed ms: " + stopWatch.Elapsed.TotalMilliseconds);
		}

		private static void AutoPerformanceTests()
		{
			MultipleTemplates(100000);

			BigTemplate(100000);

			MultipleTemplatesSameExpressions(100000);
		}

		private static void BigTemplate(int count)
		{
			Console.WriteLine("Big template " + count);

			var engine = new TemplateEngine<MyModel>();
			var model = new MyModel();

			var builder = new StringBuilder();
			for (var i = 0; i < count; i++)
			{
				builder.Append($"<div>@Model.Hello(\"{i}\")</div>");
			}

			var template = builder.ToString();

			var stopWatch = new Stopwatch();
			stopWatch.Start();
			engine.Render(template, model);
			stopWatch.Stop();

			Console.WriteLine("Elapsed ms: " + stopWatch.Elapsed.TotalMilliseconds);
		}

		private static void MultipleTemplates(int count)
		{
			Console.WriteLine("Multiple templates " + count);

			var engine = new TemplateEngine<MyModel>();
			var model = new MyModel();

			var stopWatch = new Stopwatch();
			stopWatch.Start();

			for (var i = 0; i < count; i++)
			{
				var template = $"<div>@Model.Hello(\"{i}\")</div>";

				engine.Render(template, model);
			}

			stopWatch.Stop();

			Console.WriteLine("Elapsed ms: " + stopWatch.Elapsed.TotalMilliseconds);
		}

		private static void MultipleTemplatesSameExpressions(int count)
		{
			Console.WriteLine("Multiple templates same expressions " + count);

			var engine = new TemplateEngine<MyModel>();
			var model = new MyModel();

			var rnd = new Random();

			var stopWatch = new Stopwatch();
			stopWatch.Start();

			for (var i = 0; i < count; i++)
			{
				var template = $"<div>@Model.Hello(\"{rnd.Next(0, 1000)}\")</div>";

				engine.Render(template, model);
			}

			stopWatch.Stop();

			Console.WriteLine("Elapsed ms: " + stopWatch.Elapsed.TotalMilliseconds);
		}
	}

	public class MyModel
	{
		public string Hello(string target)
		{
			return $"Hello {target}!";
		}

		public string Translate(string value)
		{
			return "Translate:" + value;
		}

		public string GetAthletePopoutUrl(string value)
		{
			return "GetAthletePopoutUrl:" + value;
		}

		public string GetAthleteShortName(string value)
		{
			return "GetAthleteShortName:" + value;
		}

		public string GetAthleteFullName(string value)
		{
			return "GetAthleteFullName:" + value;
		}

		public string GetAthletePageUrl(string value)
		{
			return "GetAthletePageUrl:" + value;
		}

		public string GetAthleteSurname(string value)
		{
			return "GetAthleteSurname:" + value;
		}

		public string GetAthleteCountryName(string value)
		{
			return "GetAthleteCountryName:" + value;
		}

		public string GetTransparentFlag(string value)
		{
			return "GetTransparentFlag:" + value;
		}

		public string GetAthleteNoc(string value)
		{
			return "GetAthleteNoc:" + value;
		}

		public string ParamTranslate(string value, string param)
		{
			return "ParamTranslate:" + value + param;
		}

		public string GetFlag(string value, string param)
		{
			return "GetFlag:" + value + param;
		}
	}
}
