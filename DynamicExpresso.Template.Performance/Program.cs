using System;
using System.Diagnostics;
using System.Text;

namespace DynamicExpresso.Template.Performance
{
	public class Program
	{
		public static void Main()
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
	}
}
