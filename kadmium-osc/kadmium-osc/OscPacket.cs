using Kadmium_Osc.Arguments;
using Kadmium_Osc.ByteConversion;
using System;
using System.Collections.Generic;
using System.Text;

namespace Kadmium_Osc
{
	public abstract class OscPacket
	{
		public abstract OscInt Length { get; }
	}
}
