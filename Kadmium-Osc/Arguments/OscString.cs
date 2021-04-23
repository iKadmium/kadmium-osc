using System;
using System.Buffers;
using System.Collections.Generic;
using System.Text;

namespace Kadmium_Osc.Arguments
{
	public class OscString : OscArgument, IEquatable<OscString>
	{
		public string Value { get; }
		public override UInt32 Length
		{
			get
			{
				return OscArgument.Pad((UInt32)Value.Length + 1, 4);
			}
		}

		public override char TypeTag { get; } = 's';

		public OscString(string value)
		{
			Value = value;
		}

		public static implicit operator string(OscString s) => s.Value;
		public static implicit operator OscString(string s) => new OscString(s);

		public override void Write(Span<byte> bytes)
		{
			var stringBytes = Encoding.ASCII.GetBytes(Value);
			stringBytes.CopyTo(bytes);
			for (int i = stringBytes.Length; i < Length; i++)
			{
				bytes[i] = 0x00;
			}
		}

		public static OscString Parse(ReadOnlySpan<byte> bytes)
		{
			var length = bytes.IndexOf((byte)0);
			var result = Encoding.ASCII.GetString(bytes.Slice(0, length).ToArray());
			return result;
		}

		public bool Equals(OscString other)
		{
			return other is OscString str &&
				   Value.Equals(str.Value);
		}

		public override int GetHashCode()
		{
			return Value.GetHashCode();
		}
	}
}
