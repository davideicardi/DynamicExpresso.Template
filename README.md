# DynamicExpresso.Template

Simple Razor template engine for dummies

## Usage

	public class MyModel
	{
		public string Method1()
		{
			return "output 1";
		}
	}

	...

	var engine = new TemplateEngine<DateTime>();

	var model = new MyModel();
	const string template = "<div>@Model.Method1()</div>";
	var result = engine.Render(template, model);
