using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

namespace Kadmium_Osc.Arguments
{
	public class OscFloat : OscArgument
	{
		public OscFloat(float value)
		{
			Value = value;
		}

		public float Value { get; set; }

		public override int Length
		{
			get
			{
				return 4;
			}
		}

		public override char TypeTag { get; } = 'f';

		public static implicit operator float(OscFloat f) => f.Value;
		public static implicit operator OscFloat(float f) => new OscFloat(f);
	}
}
