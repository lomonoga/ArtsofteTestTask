using Data.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Data;

public sealed class ArtsofteDbContext : DbContext
{
    public ArtsofteDbContext(DbContextOptions<ArtsofteDbContext> options) : base(options)
    {
        AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
    }

    public DbSet<User> Users { get; set; } = default!;
    
    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.Entity<User>().HasAlternateKey(x => x.Email);
        builder.Entity<User>().HasAlternateKey(x => x.Phone);
        
        base.OnModelCreating(builder);
    }
}