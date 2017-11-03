using NUnit.Framework;

namespace DynamicExpresso.Template.Test
{
	[TestFixture]
	public class TemplateEngineTest
	{
		[TestCase(null, null)]
		[TestCase("", "")]
		[TestCase("<p>Ciao</p>", "<p>Ciao</p>")]
		[TestCase("<p>@Model.Method1()</p>", "<p>output 1</p>")]
		[TestCase("<p>@Model.Method1() (@Model.Method1())</p>", "<p>output 1 (output 1)</p>")]
		[TestCase("<p class=\"@Model.Method1()\">bla bla</p>", "<p class=\"output 1\">bla bla</p>")]
		[TestCase("<p class=\"@Model.Hello(@Model.Method1())\">bla bla</p>", "<p class=\"Hello output 1!\">bla bla</p>")]
		[TestCase("<p>@Model.Method1() (@Model.Method1())</p>", "<p>output 1 (output 1)</p>")]
		[TestCase("<p>@Model.Property1</p>", "<p>my property 1</p>")]
		[TestCase("<p>@Model.Hello(\"world\")</p>", "<p>Hello world!</p>")]
		[TestCase("<p>@Model.Hello(@Model.Method1())</p>", "<p>Hello output 1!</p>")]
		[TestCase("<p>@Model.Concat(\"Hello\", \" world!\")</p>", "<p>Hello world!</p>")]
		public void Render(string template, string expected)
		{
			var engine = new TemplateEngine<MyModel>();

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
