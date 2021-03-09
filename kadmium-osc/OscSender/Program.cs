using Kadmium_Osc;
using PowerArgs;
using System;
using System.Net;
using System.Threading.Tasks;

namespace OscSender
{
	class Program
	{
		async static Task Main(string[] args)
		{
			try
			{
				var settings = Args.Parse<OscSenderArgs>(args);
				using (OscClient client = new OscClient())
				{
					OscMessage message = new OscMessage()
					{
						Address = settings.Address
					};
					message.Arguments.Add(settings.StringValue);

					await client.Send(settings.Hostname, settings.Port, message);
				}

			}
			catch(ArgException ex)
			{
				Console.WriteLine(ex.Message);
				Console.WriteLine(ArgUsage.GenerateUsageFromTemplate<OscSenderArgs>());
			}

		}
	}
}
