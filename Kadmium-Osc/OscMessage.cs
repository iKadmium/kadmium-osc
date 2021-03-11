using Kadmium_Osc.Arguments;
using Kadmium_Osc.ByteConversion;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Kadmium_Osc
{
	public class OscMessage : OscPacket
	{
		public OscString TypeTag 
		{ 
			get
			{
				return new OscString("," + string.Join("", Arguments.Select(x => x.TypeTag)));
			}
		}
		public OscString Address { get; set; }
		public List<OscArgument> Arguments { get; }
		
		public override OscInt Length
		{
			get
			{
				return Address.Length + TypeTag.Length + Arguments.Sum(x => x.Length);
			}
		}

		public OscMessage()
		{
			Arguments = new List<OscArgument>();
		}

		public OscMessage(string address, params OscArgument[] arguments) : this()
		{
			Address = address;
			Arguments.AddRange(arguments);
		}

		public T GetArgument<T>(int index)
			where T : OscArgument
		{
			return Arguments[index] as T;
		}
	}
}
