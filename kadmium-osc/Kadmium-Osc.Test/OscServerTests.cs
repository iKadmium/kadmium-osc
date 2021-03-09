using Kadmium_Osc.ByteConversion;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Kadmium_Osc.Test
{
	public class OscServerTests
	{
		[Fact]
		public void When_MessageAreReceived_Then_TheyAreParsedAndTheResultTriggersTheEvent()
		{
			var udpServer = Mock.Of<IUdpServer>();
			var byteConverter = Mock.Of<IByteConverter>(x =>
				x.GetPacket(It.IsAny<ReadOnlyMemory<byte>>()) == new OscMessage { Address = "" }
			);

			OscServer server = new OscServer(udpServer, byteConverter);
			OscMessage received = null;

			server.OnMessageReceived += (object sender, OscMessage message) => received = message;
			server.Listen(null, -1);

			Mock.Get(udpServer).Raise(x => x.OnPacketReceived += null, null, null);
			
			Assert.NotNull(received);
		}
	}
}
