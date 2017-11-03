using System;
using System.Collections.Concurrent;
using System.Text.RegularExpressions;

namespace DynamicExpresso.Template
{
	public class TemplateEngine<T>
	{
		private readonly ConcurrentDictionary<string, Func<T, object>> 
			_expressionCatalog = new ConcurrentDictionary<string, Func<T, object>>();

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
			var expression = NormalizeExpression(match);

			var expDelegate = GetOrCreateDelegate(expression);

			return expDelegate(model)?.ToString();
		}

		private Func<T, object> GetOrCreateDelegate(string expression)
		{
			return _expressionCatalog.GetOrAdd(expression, CreateDelegate);
		}

		private static string NormalizeExpression(Match match)
		{
			var expression = match.Value.Replace("@Model.", "Model.");
			return expression;
		}

		private Func<T, object> CreateDelegate(string expression)
		{
			var expressionDelegate = _interpreter.ParseAsDelegate<Func<T, object>>(
				expression,
				"Model");
			return expressionDelegate;
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
