using System;
using System.Buffers.Binary;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

namespace Kadmium_Osc.Arguments
{
	public class OscFloat : OscArgument, IEquatable<OscFloat>
	{
		public OscFloat(float value)
		{
			Value = value;
		}

		public float Value { get; set; }

		public override UInt32 Length
		{
			get
			{
				return 4;
			}
		}

		public override char TypeTag { get; } = 'f';

		public override void Write(Span<byte> bytes)
		{
			BinaryPrimitives.WriteSingleBigEndian(bytes, Value);
		}

		public static implicit operator float(OscFloat f) => f.Value;
		public static implicit operator OscFloat(float f) => new OscFloat(f);

		public static OscFloat Parse(ReadOnlySpan<byte> bytes)
		{
			return BinaryPrimitives.ReadSingleBigEndian(bytes);
		}

		public override bool Equals(object obj)
		{
			return Equals(obj as OscFloat);
		}

		public bool Equals(OscFloat other)
		{
			return other != null &&
				   Value == other.Value;
		}

		public override int GetHashCode()
		{
			return Value.GetHashCode();
		}
	}
}
