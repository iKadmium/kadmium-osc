using Kadmium_Osc.Arguments;
using Kadmium_Udp;
using Moq;
using System;
using System.Buffers;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using Xunit;

namespace Kadmium_Osc.Test
{
	public class OscServerTests
	{
		[Fact]
		public void When_MessageAreReceived_Then_TheyAreParsedAndTheResultTriggersTheEvent()
		{
			var udpServer = Mock.Of<IUdpWrapper>();
			
			using var server = new OscServer(udpServer);
			var sent = new OscMessage("/test", 1f);
			OscMessage received = null;

			server.OnMessageReceived += (object sender, OscMessage message) => received = message;
			server.Listen(null, -1);

			using var owner = MemoryPool<byte>.Shared.Rent((int)sent.Length);
			var bytes = owner.Memory.Slice(0, (int)sent.Length);
			sent.Write(bytes.Span);

			Mock.Get(udpServer).Raise(x => x.OnPacketReceived += null, null, new UdpReceiveResult(bytes.ToArray(), new IPEndPoint(0, 0)));
			
			Assert.NotNull(received);
		}

		[Fact]
		public void When_BundlesAreReceived_Then_TheyAreParsedAndTheResultTriggersTheEvent()
		{
			var udpServer = Mock.Of<IUdpWrapper>();

			using var server = new OscServer(udpServer);
			var message = new OscMessage("/test", 1f);
			var sent = new OscBundle();
			sent.TimeTag = new OscTimeTag(OscTimeTag.MinValue);
			sent.Contents.Add(message);
			OscMessage received = null;

			server.OnMessageReceived += (object sender, OscMessage message) => received = message;
			server.Listen(null, -1);

			using var owner = MemoryPool<byte>.Shared.Rent((int)sent.Length);
			var bytes = owner.Memory.Slice(0, (int)sent.Length);
			sent.Write(bytes.Span);

			Mock.Get(udpServer).Raise(x => x.OnPacketReceived += null, null, new UdpReceiveResult(bytes.ToArray(), new IPEndPoint(0, 0)));

			Assert.NotNull(received);
		}

		[Fact]
		public void When_BundlesAreReceived_Then_AllSubsequentMessagesTriggerEvents()
		{
			var udpServer = Mock.Of<IUdpWrapper>();

			using var server = new OscServer(udpServer);
			var message = new OscMessage("/test", 1f);
			var sent = new OscBundle();
			sent.TimeTag = new OscTimeTag(OscTimeTag.MinValue);
			sent.Contents.Add(message);
			OscMessage received = null;

			server.OnMessageReceived += (object sender, OscMessage message) => received = message;
			server.Listen(null, -1);

			using var owner = MemoryPool<byte>.Shared.Rent((int)sent.Length);
			var bytes = owner.Memory.Slice(0, (int)sent.Length);
			sent.Write(bytes.Span);

			Mock.Get(udpServer).Raise(x => x.OnPacketReceived += null, null, new UdpReceiveResult(bytes.ToArray(), new IPEndPoint(0, 0)));

			Assert.NotNull(received);
		}
	}
}
