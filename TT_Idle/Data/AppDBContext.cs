using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using TT_Idle.Models;
using TT_Idle.Models;

namespace TT_Idle.Data;

public class AppDbContext : DbContext
{
	public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

	public DbSet<Heartbeat> Heartbeats { get; set; }
}
