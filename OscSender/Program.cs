using Kadmium_Osc;
using PowerArgs;
using System;
using System.Linq;
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
				using OscClient client = new OscClient();
				OscMessage message = new OscMessage(settings.Address, settings.StringValue);
				var addresses = await Dns.GetHostAddressesAsync(settings.Hostname);
				await client.Send(new IPEndPoint(addresses.First(), settings.Port), message);

			}
			catch (ArgException ex)
			{
				Console.WriteLine(ex.Message);
				Console.WriteLine(ArgUsage.GenerateUsageFromTemplate<OscSenderArgs>());
			}

		}
	}
}
