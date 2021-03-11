using System;
using System.Buffers;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace Kadmium_Osc.Arguments
{
	public class OscTimeTag : OscArgument
	{
		public OscTimeTag(DateTime value)
		{
			Value = value;
		}

		public DateTime Value { get; set; }

		public override int Length
		{
			get
			{
				return 8;
			}
		}

		public override char TypeTag { get; } = 't';

		public static implicit operator DateTime(OscTimeTag t) => t.Value;
		public static implicit operator OscTimeTag(DateTime t) => new OscTimeTag(t);
	}
}
