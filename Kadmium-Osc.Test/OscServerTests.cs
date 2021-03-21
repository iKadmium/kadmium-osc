using Kadmium_Osc.Arguments;
using Kadmium_Udp;
using Moq;
using System;
using System.Buffers;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Kadmium_Osc.Test
{
	public class OscServerTests
	{
		[Fact]
		public void When_MessageAreReceived_Then_TheyAreParsedAndTheResultTriggersTheEvent()
		{
			var udpServer = Mock.Of<IUdpWrapper>();
			var timeProvider = Mock.Of<ITimeProvider>(x => x.Now == OscTimeTag.MinValue);
			
			using var server = new OscServer(udpServer, timeProvider);
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
			var timeProvider = Mock.Of<ITimeProvider>(x => x.Now == OscTimeTag.MinValue);

			using var server = new OscServer(udpServer, timeProvider);
			var message = new OscMessage("/test", 1f);
			var sent = new OscBundle(OscTimeTag.MinValue, message);
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
			var timeProvider = Mock.Of<ITimeProvider>(x => x.Now == OscTimeTag.MinValue);

			using var server = new OscServer(udpServer, timeProvider);
			var sentMessages = new OscMessage[] {
				new OscMessage("/test1", 25),
				new OscMessage("/test2", 1f)
			};
			var sent = new OscBundle(OscTimeTag.MinValue, sentMessages);
			var receivedCount = 0;

			server.OnMessageReceived += (object sender, OscMessage message) => receivedCount++;
			server.Listen(-1);

			using var owner = MemoryPool<byte>.Shared.Rent((int)sent.Length);
			var bytes = owner.Memory.Slice(0, (int)sent.Length);
			sent.Write(bytes.Span);

			Mock.Get(udpServer).Raise(x => x.OnPacketReceived += null, null, new UdpReceiveResult(bytes.ToArray(), new IPEndPoint(0, 0)));

			Assert.Equal(2, receivedCount);
		}

		[Fact]
		public async Task Given_TheTimeTagIsInTheFuture_When_BundlesAreReceived_Then_TheyAreProcessedWhenTheTimeTagSays()
		{
			var timeInTheFuture = OscTimeTag.MinValue.AddDays(1);
			var waitTaskCompleted = false;

			var udpServer = Mock.Of<IUdpWrapper>();
			var timeProvider = Mock.Of<ITimeProvider>(x => x.Now == OscTimeTag.MinValue);
			Mock.Get(timeProvider)
				.Setup(x => x.WaitUntil(It.IsAny<DateTime>()))
				.Returns(() => Task.Run(async () =>
				{
					while (!waitTaskCompleted)
					{
						await Task.Delay(10);
					}
				}));

			using var server = new OscServer(udpServer, timeProvider);
			var message = new OscMessage("/test", 1f);
			var sent = new OscBundle(timeInTheFuture, message);
			var receivedCount = 0;

			server.OnMessageReceived += (object sender, OscMessage message) => receivedCount++;
			server.Listen(-1);

			using var owner = MemoryPool<byte>.Shared.Rent((int)sent.Length);
			var bytes = owner.Memory.Slice(0, (int)sent.Length);
			sent.Write(bytes.Span);

			Mock.Get(udpServer).Raise(x => x.OnPacketReceived += null, null, new UdpReceiveResult(bytes.ToArray(), new IPEndPoint(0, 0)));

			Assert.Equal(0, receivedCount);
			waitTaskCompleted = true;
			await Task.Delay(100);
			Assert.Equal(1, receivedCount);
		}
	}
}
