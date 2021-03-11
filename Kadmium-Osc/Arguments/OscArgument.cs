using System;
using System.Collections.Generic;
using System.Text;

namespace Kadmium_Osc.Arguments
{
	public abstract class OscArgument
	{
		public abstract int Length { get; }
		public abstract char TypeTag { get; }

		public static implicit operator OscArgument(string s) => new OscString(s);
		public static implicit operator OscArgument(int i) => new OscInt(i);
		public static implicit operator OscArgument(float f) => new OscFloat(f);
		public static implicit operator OscArgument(ReadOnlyMemory<byte> b) => new OscBlob(b);
		public static implicit operator OscArgument(byte[] b) => new OscBlob(b);
	}
}
