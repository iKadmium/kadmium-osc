using Kadmium_Osc.Arguments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kadmium_Osc
{
	public class OscPattern : OscString
	{
		public OscPatternPart[] Parts { get; }

		public OscPattern(string value) : base(value)
		{
			Parts = Value.Split("/").Select(x => new OscPatternPart(x)).ToArray();
		}

		public bool Matches(OscString other)
		{
			var otherParts = other.Value.Split("/").ToArray();
			if (Parts.Length != otherParts.Length)
			{
				return false;
			}

			for (int i = 0; i < Parts.Length; i++)
			{
				if (!Parts[i].IsMatch(otherParts[i]))
				{
					return false;
				}
			}

			return true;
		}

		public static implicit operator string(OscPattern s) => s.Value;
		public static implicit operator OscPattern(string s) => new OscPattern(s);

		public override int GetHashCode()
		{
			return Value.GetHashCode();
		}
	}
}
