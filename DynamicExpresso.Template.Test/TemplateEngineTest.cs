using System;
using NUnit.Framework;

namespace DynamicExpresso.Template.Test
{
	[TestFixture]
	public class TemplateEngineTest
	{
		[Test]
		public void Demo()
		{
			var engine = new TemplateEngine<DateTime>();

			var model = DateTime.Now;
			const string template = "<div>@Model.Millisecond</div>";

			var result = engine.Render(template, model);

			var expected = $"<div>{model.Millisecond}</div>";
			Assert.AreEqual(expected, result);
		}

		[Test]
		public void Same_expression_with_different_model()
		{
			var engine = new TemplateEngine<DateTime>();

			const string template = "<div>@Model.ToOADate()</div>";

			var model = DateTime.Now;
			var result = engine.Render(template, model);
			var expected = $"<div>{model.ToOADate()}</div>";
			Assert.AreEqual(expected, result);

			var model2 = DateTime.Now;
			var result2 = engine.Render(template, model2);
			var expected2 = $"<div>{model2.ToOADate()}</div>";
			Assert.AreEqual(expected2, result2);
		}

		[TestCase(
			null,
			null)]
		[TestCase(
			"",
			"")]
		[TestCase(
			"<p>Ciao</p>",
			"<p>Ciao</p>")]
		[TestCase(
			"<p>@Model.Method1()</p>",
			"<p>output 1</p>")]
		[TestCase(
			"<p>@Model.Method1() (@Model.Method1())</p>",
			"<p>output 1 (output 1)</p>")]
		[TestCase(
			"<p class=\"@Model.Method1()\">bla bla</p>",
			"<p class=\"output 1\">bla bla</p>")]
		[TestCase(
			"<p class=\"@Model.Hello(@Model.Method1())\">bla bla</p>",
			"<p class=\"Hello output 1!\">bla bla</p>")]
		[TestCase(
			"<p>@Model.Method1() (@Model.Method1())</p>",
			"<p>output 1 (output 1)</p>")]
		[TestCase(
			"<p>@Model.Property1</p>",
			"<p>my property 1</p>")]
		[TestCase(
			"<p>@Model.Hello(\"world\")</p>",
			"<p>Hello world!</p>")]
		[TestCase(
			"<p>@Model.Hello(@Model.Method1())</p>",
			"<p>Hello output 1!</p>")]
		[TestCase(
			"<p>@Model.Concat(\"Hello\", \" world!\")</p>",
			"<p>Hello world!</p>")]
		public void Render(string template, string expected)
		{
			var engine = new TemplateEngine<MyModel>();

			Assert.AreEqual(expected, engine.Render(template, new MyModel()));
		}

		[TestCase(
	"<p>@Model.MethodInt()</p>",
	"<p>55</p>")]
		public void Render_with_conversion(string template, string expected)
		{
			var engine = new TemplateEngine<MyModel>();

			Assert.AreEqual(expected, engine.Render(template, new MyModel()));
		}

		[Test]
		public void Render_multi_line()
		{
			var engine = new TemplateEngine<MyModel>();

			const string template = @"<div>
<span class=""@Model.Concat(""Hello"", "" world!"")"">
	@Model.Hello(@Model.Method1())
</span>
</div>";

			const string expected = @"<div>
<span class=""Hello world!"">
	Hello output 1!
</span>
</div>";

			Assert.AreEqual(expected, engine.Render(template, new MyModel()));
		}
	}

	public class MyModel
	{
		public string Property1 { get; } = "my property 1";

		public string Method1()
		{
			return "output 1";
		}

		public int MethodInt()
		{
			return 55;
		}

		public string Hello(string target)
		{
			return $"Hello {target}!";
		}

		public string Concat(string p1, string p2)
		{
			return string.Concat(p1, p2);
		}
	}
}
