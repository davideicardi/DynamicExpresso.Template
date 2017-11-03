# DynamicExpresso.Template

**ALPHA VERSION!!!**

Simple Razor template engine for dummies.

Template for now supports only expressions like `@Model.Xyz`. The main advantage over standard Razor is that doesn't generate an assembly 
but only `Expression` tree using `DynamicExpresso` library.

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
