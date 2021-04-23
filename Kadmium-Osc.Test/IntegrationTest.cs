using Kadmium_Osc.Arguments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Kadmium_Osc.Test
{
	public class IntegrationTest
	{
		[Fact]
		public async Task Given_TheServerAndClientAreRunning_When_TheClientSendsAMessage_Then_TheServerReceivesIt()
		{
			using var client = new OscClient();
			using var server = new OscServer();

			var expectedMessage = new OscMessage("/test", "Hello world!");

			bool messageReceived = false;

			server.OnMessageReceived += (sender, message) =>
			{
				Assert.Equal(message, expectedMessage);
				messageReceived = true;
			};

			IPEndPoint endPoint = new IPEndPoint(IPAddress.Loopback, 41234);
			server.Listen(endPoint);
			await client.Send(endPoint, expectedMessage);
			await Task.Delay(50);

			Assert.True(messageReceived);
		}
	}
}
