using System;
using System.Buffers;
using System.Buffers.Binary;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace Kadmium_Osc.Arguments
{
	public class OscTimeTag : OscArgument, IEquatable<OscTimeTag>
	{
		private static long TicksPerSecond = 10_000_000;
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

		public override UInt32 Length
		{
			get
			{
				return 8;
			}
		}

		public override char TypeTag { get; } = 't';

		public static implicit operator DateTime(OscTimeTag t) => t.Value;
		public static implicit operator OscTimeTag(DateTime t) => new OscTimeTag(t);

		public override void Write(Span<byte> bytes)
		{
			var totalSeconds = (Value - OscTimeTag.MinValue).TotalSeconds;
			var wholeSeconds = (UInt32)Math.Floor(totalSeconds);
			var fractionSeconds = (totalSeconds - wholeSeconds);
			var fraction = (UInt32)(fractionSeconds * UInt32.MaxValue);

			BinaryPrimitives.WriteUInt32BigEndian(bytes, wholeSeconds);
			BinaryPrimitives.WriteUInt32BigEndian(bytes[4..], fraction);
		}

		public static OscTimeTag Parse(ReadOnlySpan<byte> bytes)
		{
			var seconds = BinaryPrimitives.ReadUInt32BigEndian(bytes[..]);
			var fraction = BinaryPrimitives.ReadUInt32BigEndian(bytes[4..]);
			long fractionTicks = (long)(((double)fraction / ((double)UInt32.MaxValue + 1)) * TicksPerSecond);

			var date = OscTimeTag.MinValue.AddSeconds(seconds).AddTicks(fractionTicks);

			return date;
		}

		public bool Equals(OscTimeTag other)
		{
			return other != null &&
				   Value == other.Value;
		}
	}
}
