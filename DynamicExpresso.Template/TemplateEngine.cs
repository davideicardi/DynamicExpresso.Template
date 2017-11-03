using System;
using System.Text.RegularExpressions;

namespace DynamicExpresso.Template
{
	public class TemplateEngine<T>
	{
		private readonly Interpreter _interpreter = new Interpreter();

		public string Render(string template, T model)
		{
			if (template == null)
				return null;

			var result = ParserConst.ExpressionRegex.Replace(template, match => Replacer(match, model));

			return result;
		}

		private string Replacer(Match match, T model)
		{
			var expression = match.Value.Replace("@Model.", "Model.");

			var expressionDelegate = _interpreter.ParseAsDelegate<Func<T, string>>(
				expression,
				"Model");

			return expressionDelegate(model);
		}
	}

	internal static class ParserConst
	{
		public static readonly Regex ExpressionRegex =
		 new Regex(
			@"@Model\.\w+(\((?:[^()]|(?<OPEN>\()|(?<-OPEN>\)))*(?(OPEN)(?!))\))?"
			, RegexOptions.Compiled
			);
	}

}
