using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace Kadmium_Osc.Arguments
{
	public class OscInt : OscArgument
	{
		public override int Length
		{
			get
			{
				return 4;
			}
		}

		public int Value { get; }

		public override char TypeTag { get; } = 'i';

		public OscInt(int value)
		{
			Value = value;
		}

		public static implicit operator int(OscInt i) => i.Value;
		public static implicit operator OscInt(int i) => new OscInt(i);
	}
}
