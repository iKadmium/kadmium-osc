using Kadmium_Udp;
using Moq;
using System;
using System.Buffers;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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
			var message = new OscMessage("/test", "Hello World!");
			using var memoryOwner = MemoryPool<byte>.Shared.Rent((int)message.Length);
			var expectedPayload = memoryOwner.Memory.Slice(0, (int)message.Length);
			message.Write(expectedPayload.Span);

			var expectedEndpoint = new IPEndPoint(IPAddress.Loopback, 1234);

			IPEndPoint actualEndpoint = null;
			ReadOnlyMemory<byte> actualPayload = null;

			var udpClient = Mock.Of<IUdpWrapper>();
			Mock.Get(udpClient)
				.Setup(x => x.Send(It.IsAny<IPEndPoint>(), It.IsAny<ReadOnlyMemory<byte>>()))
				.Returns(Task.CompletedTask)
				.Callback<IPEndPoint, ReadOnlyMemory<byte>>((endpoint, payload) =>
				{
					actualEndpoint = endpoint;
					actualPayload = payload;
				});

			using OscClient client = new OscClient(udpClient);
			await client.Send(expectedEndpoint, message);

			Assert.Equal(expectedEndpoint, actualEndpoint);
			Assert.Equal(expectedPayload.ToArray(), actualPayload.ToArray());
		}
	}
}
