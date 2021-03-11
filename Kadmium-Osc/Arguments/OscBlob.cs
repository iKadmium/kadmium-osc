using System;
using System.Collections.Generic;
using System.Text;

namespace Kadmium_Osc.Arguments
{
	public class OscBlob : OscArgument
	{
		public byte[] Value { get; }
		
		public override int Length
		{
			get
			{
				var nulls = 4 - ((Value.Length) % 4);
				var length = Value.Length + nulls;
				return length;
			}
		}

		public override char TypeTag { get; } = 'b';

		public OscBlob(ReadOnlyMemory<byte> value)
		{
			Value = new byte[value.Length];
			value.CopyTo(Value);
		}

		public static implicit operator ReadOnlyMemory<byte>(OscBlob b) => b.Value;
		public static implicit operator OscBlob(ReadOnlyMemory<byte> b) => new OscBlob(b);
		public static implicit operator Memory<byte>(OscBlob b) => b.Value;
		public static implicit operator byte[](OscBlob b) => b.Value;
		public static implicit operator OscBlob(byte[] b) => new OscBlob(b);
	}
}
