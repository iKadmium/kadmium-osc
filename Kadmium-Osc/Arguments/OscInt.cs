using System;
using System.Buffers.Binary;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace Kadmium_Osc.Arguments
{
	public class OscInt : OscArgument, IEquatable<OscInt>
	{
		public override UInt32 Length
		{
			get
			{
				return 4;
			}
		}

		public Int32 Value { get; }

		public override char TypeTag { get; } = 'i';

		public OscInt(int value)
		{
			Value = value;
		}

		public static implicit operator Int32(OscInt i) => i.Value;
		public static implicit operator OscInt(Int32 i) => new OscInt(i);

		public override void Write(Span<byte> bytes)
		{
			BinaryPrimitives.WriteInt32BigEndian(bytes, Value);
		}

		public static OscInt Parse(ReadOnlySpan<byte> bytes)
		{
			return BinaryPrimitives.ReadInt32BigEndian(bytes);
		}

		public override bool Equals(object obj)
		{
			return Equals(obj as OscInt);
		}

		public bool Equals(OscInt other)
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
