using System;
using System.Buffers;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace Kadmium_Osc.Arguments
{
	public class OscTimeTag : OscArgument
	{
		public static DateTime MaxValue { get; } = new DateTime(2036, 2, 7, 6, 28, 16).AddTicks(-1);
		public static DateTime MinValue { get; } = new DateTime(1900, 1, 1);

		public OscTimeTag(DateTime value)
		{
			if (value > MaxValue)
			{
				throw new ArgumentException("Value was greater than the max value");
			}
			if (value < MinValue)
			{
				throw new ArgumentException("Value was less than the min value");
			}

			Value = value;
		}

		public DateTime Value { get; }

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
