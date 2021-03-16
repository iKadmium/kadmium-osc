using System;
using System.Buffers.Binary;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kadmium_Osc.Arguments
{
	public class OscBlob : OscArgument, IEquatable<OscBlob>
	{
		public byte[] Value { get; }
		
		public override UInt32 Length
		{
			get
			{
				return OscArgument.Pad((UInt32)Value.Length + 4, 4);
			}
		}

		public override char TypeTag { get; } = 'b';

		public OscBlob(ReadOnlySpan<byte> value)
		{
			Value = new byte[value.Length];
			value.CopyTo(Value);
		}

		public override void Write(Span<byte> bytes)
		{
			BinaryPrimitives.WriteUInt32BigEndian(bytes, (UInt32)Value.Length);
			Value.CopyTo(bytes.Slice(4));
		}

		public static implicit operator ReadOnlyMemory<byte>(OscBlob b) => b.Value;
		public static implicit operator OscBlob(ReadOnlyMemory<byte> b) => new OscBlob(b.Span);
		public static implicit operator OscBlob(ReadOnlySpan<byte> b) => new OscBlob(b);
		public static implicit operator Memory<byte>(OscBlob b) => b.Value;
		public static implicit operator byte[](OscBlob b) => b.Value;
		public static implicit operator OscBlob(byte[] b) => new OscBlob(b);

		public static OscBlob Parse(ReadOnlySpan<byte> bytes)
		{
			var length = BinaryPrimitives.ReadUInt32BigEndian(bytes);
			return new OscBlob(bytes.Slice(4, (int)length));
		}

		public bool Equals(OscBlob other)
		{
			return other != null &&
				Value.SequenceEqual(other.Value);
		}
	}
}
