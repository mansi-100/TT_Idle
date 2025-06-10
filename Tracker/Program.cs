using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;

class Program
{
	// Import user32.dll for mouse event (Windows only)
	[DllImport("user32.dll")]
	private static extern void mouse_event(int dwFlags, int dx, int dy, int dwData, int dwExtraInfo);

	private const int MOUSEEVENTF_MOVE = 0x0001;

	static async Task Main(string[] args)
	{
		string apiUrl = "https://localhost:7156/api/heartbeat"; // Use HTTPS & your port
		string machineName = Environment.MachineName;

		var handler = new HttpClientHandler();
		handler.ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator;

		using HttpClient client = new(handler);

		Console.WriteLine("Agent started. Press Ctrl+C to stop.");

		int counter = 0;

		while (true)
		{
			try
			{
				var heartbeat = new
				{
					machineName = machineName
				};

				var response = await client.PostAsJsonAsync(apiUrl, heartbeat);
				if (response.IsSuccessStatusCode)
				{
					Console.WriteLine($"{DateTime.Now}: API Before Call sent.");
				}
				else
				{
					Console.WriteLine($"{DateTime.Now}: Failed to send heartbeat. Status: {response.StatusCode}");
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Error sending heartbeat: {ex.Message}");
			}

			if (counter % 2 == 0)//every 60 sec call
			{
				PreventIdle();
				Console.WriteLine($"{DateTime.Now}: API Console Success.");
			}

			counter++;

			await Task.Delay(TimeSpan.FromSeconds(30));
		}
	}

	static void PreventIdle()
	{
		// Move mouse 1 pixel right and back left quickly
		mouse_event(MOUSEEVENTF_MOVE, 1, 0, 0, 0);
		mouse_event(MOUSEEVENTF_MOVE, -1, 0, 0, 0);
	}
}
