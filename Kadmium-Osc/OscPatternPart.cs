using Kadmium_Osc.Arguments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Kadmium_Osc
{
	public class OscPatternPart : OscString
	{
		public OscPatternPart(string value) : base(value)
		{
		}

		public bool IsMatch(string other)
		{
			var regexBuilder = new StringBuilder();
			for (int i = 0; i < Value.Length; i++)
			{
				switch (Value[i])
				{
					case '?':
						regexBuilder.Append(".");
						break;
					case '*':
						regexBuilder.Append(".*");
						break;
					case '[':
						regexBuilder.Append("[");
						i++;
						if (Value[i] == '!')
						{
							regexBuilder.Append("^");
							i++;
						}
						while (Value[i] != ']')
						{
							regexBuilder.Append(Value[i]);
							i++;
						}
						regexBuilder.Append(']');
						break;
					case '{':
						regexBuilder.Append("(");
						i++;
						while (Value[i] != '}')
						{
							if (Value[i] == ',')
							{
								regexBuilder.Append("|");
							}
							else
							{
								regexBuilder.Append(Value[i]);
							}
							i++;
						}
						regexBuilder.Append(')');
						break;
					default:
						regexBuilder.Append(Value[i]);
						break;
				}
			}
			var regexPattern = $"^{regexBuilder}$";
			var regex = new Regex(regexPattern);
			return regex.IsMatch(other);
		}

		public static implicit operator string(OscPatternPart s) => s.Value;
		public static implicit operator OscPatternPart(string s) => new OscPatternPart(s);
	}
}
