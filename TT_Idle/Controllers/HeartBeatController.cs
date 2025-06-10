using Microsoft.AspNetCore.Mvc;
using TT_Idle.Data;
using TT_Idle.Models;
using TT_Idle.Data;
using TT_Idle.Models;

namespace TimeTrackerBackend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class HeartbeatController : ControllerBase
{
	private readonly AppDbContext _context;

	public HeartbeatController(AppDbContext context)
	{
		_context = context;
	}

	[HttpPost]
	public async Task<IActionResult> PostHeartbeat([FromBody] Heartbeat hb)
	{
		hb.Timestamp = DateTime.UtcNow;
		_context.Heartbeats.Add(hb);
		await _context.SaveChangesAsync();
		return Ok(new { status = "received" });
	}

	[HttpGet("logs")]
	public IActionResult GetLogs()
	{
		var logs = _context.Heartbeats
			.OrderByDescending(h => h.Timestamp)
			.Take(100)
			.ToList();

		return Ok(logs);
	}
}
