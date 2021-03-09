using Kadmium_Osc;
using Kadmium_Osc.Arguments;
using System;
using System.Net;
using System.Text;

namespace OscLogger
{
	class Program
	{
		private static string Indent(string str, int indent)
		{
			StringBuilder output = new StringBuilder();
			for (int i = 0; i < indent; i++)
			{
				output.Append("\t");
			}
			output.Append(str);
			return output.ToString();
		}

		private static void WriteMessage(OscMessage message, int indent)
		{
			Console.WriteLine(Indent("Message", indent));
			indent++;
			Console.WriteLine(Indent(message.Address, indent));
			Console.WriteLine(Indent(message.TypeTag, indent));
			foreach (var argument in message.Arguments)
			{
				if (argument is OscString str)
				{
					Console.WriteLine(Indent(str, indent));
				}
				else if (argument is OscFloat flt)
				{
					Console.WriteLine(Indent(flt.Value.ToString(), indent));
				}

			}
		}

		static void Main(string[] args)
		{
			IPEndPoint endPoint = new IPEndPoint(IPAddress.Loopback, 10000);
			OscServer server = new OscServer();

			server.OnMessageReceived += (object sender, OscMessage message) => WriteMessage(message, 0);
			server.Listen(endPoint);
			Console.ReadLine();
		}
	}
}
