using Kadmium_Osc.ByteConversion;
using System;
using System.Buffers;
using System.Collections.Generic;
using System.Text;

namespace Kadmium_Osc.Arguments
{
	public class OscString : OscArgument
	{
		public string Value { get; }
		public override int Length
		{
			get
			{
				return ByteConverter.Pad(Value.Length + 1, 4);
			}
		}

		public override char TypeTag { get; } = 's';

		public OscString(string value)
		{
			Value = value;
		}

		public static implicit operator string(OscString s) => s.Value;
		public static implicit operator OscString(string s) => new OscString(s);
	}
}
