# kadmium-osc
An OSC Library for .Net

## Usage

### Receiving OSC events

```
int port = 10000;
using(OscServer server = new OscServer())
{
	// Listen for any message at all
	server.OnMessageReceived += (object sender, OscMessage message) => 
	{
		Console.WriteLine(message.Address);
		foreach (var argument in message.Arguments)
		{
			if (argument is OscString str)
			{
				Console.WriteLine(str);
			}
			else if (argument is OscFloat flt)
			{
				// process it somehow...
			}
		}
	};
	server.Listen(IPAddress.Loopback.ToString(), port);
}
```

## Sending OSC events

```
int port = 10000;
string address = "/test";
string payload = "Hello world!";
string hostname = "example.com";

using (OscClient client = new OscClient())
{
	OscMessage message = new OscMessage(address, payload);
	await client.Send(hostname, port, message);
}
```