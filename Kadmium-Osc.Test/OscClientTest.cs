using Kadmium_Udp;
using Moq;
using System;
using System.Buffers;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Kadmium_Osc.Test
{
	public class OscClientTest
	{
		[Fact]
		public async Task When_SendMessageIsCalled_Then_TheMessageIsPassedToTheUdpWrapper()
		{
			OscMessage message = new OscMessage("/test", "Hello World!");
			using var memoryOwner = MemoryPool<byte>.Shared.Rent((int)message.Length);
			var expectedPayload = memoryOwner.Memory.Slice(0, (int)message.Length);
			message.Write(expectedPayload.Span);

			string expectedHostname = "www.example.com";
			int expectedPort = 1234;

			string actualHostname = null;
			int actualPort = 0;
			ReadOnlyMemory<byte> actualPayload = null;

			var udpClient = Mock.Of<IUdpWrapper>();
			Mock.Get(udpClient)
				.Setup(x => x.Send(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<ReadOnlyMemory<byte>>()))
				.Returns(Task.CompletedTask)
				.Callback<string, int, ReadOnlyMemory<byte>>((hostname, port, payload) =>
				{
					actualHostname = hostname;
					actualPort = port;
					actualPayload = payload;
				});

			using OscClient client = new OscClient(udpClient);
			await client.Send(expectedHostname, expectedPort, message);

			Assert.Equal(expectedHostname, actualHostname);
			Assert.Equal(expectedPort, actualPort);
			Assert.Equal(expectedPayload.ToArray(), actualPayload.ToArray());
		}
	}
}
