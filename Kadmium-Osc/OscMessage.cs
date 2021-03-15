using Kadmium_Osc.Arguments;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Kadmium_Osc
{
	public class OscMessage : OscPacket, IEquatable<OscMessage>
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
		
		public override UInt32 Length
		{
			get
			{
				return (UInt32)(Address.Length + TypeTag.Length + Arguments.Sum(x => x.Length));
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

		public static new OscMessage Parse(ReadOnlySpan<byte> bytes)
		{
			OscMessage message = new OscMessage
			{
				Address = OscString.Parse(bytes)
			};
			bytes = bytes.Slice((int)message.Address.Length);
			var typeTag = new OscString(OscString.Parse(bytes));
			bytes = bytes.Slice((int)typeTag.Length);

			foreach (char typeChar in typeTag.Value)
			{
				OscArgument argument = OscArgument.Parse(bytes, typeChar);
				if (argument != null)
				{
					message.Arguments.Add(argument);
					bytes = bytes.Slice((int)argument.Length);
				}
			}

			return message;
		}

		public override void Write(Span<byte> bytes)
		{
			Address.Write(bytes);
			bytes = bytes.Slice((int)Address.Length);

			TypeTag.Write(bytes);
			bytes = bytes.Slice((int)TypeTag.Length);

			foreach (var argument in Arguments)
			{
				if (argument.Length > 0)
				{
					argument.Write(bytes);
					bytes = bytes.Slice((int)argument.Length);
				}
			}
		}

		public override bool Equals(object obj)
		{
			return Equals(obj as OscMessage);
		}

		public bool Equals(OscMessage other)
		{
			return other != null &&
				   Length == other.Length &&
				   TypeTag.Equals(other.TypeTag) &&
				   Address.Equals(other.Address) &&
				   Arguments.SequenceEqual(other.Arguments) &&
				   Length == other.Length;
		}

		public override int GetHashCode()
		{
			return HashCode.Combine(Length, TypeTag, Address, Arguments, Length);
		}
	}
}
