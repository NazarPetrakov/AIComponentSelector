using ComponentSelector.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace ComponentSelector.Infrastructure.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
    public DbSet<Component> Components { get; set;}
}
