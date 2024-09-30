using Microsoft.EntityFrameworkCore;
using RTM.Core;

namespace RTM.UI.Data;

public class TaskContext : DbContext
{
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite("Data Source=recurring-tasks-manager.db");
    }

    public DbSet<Task> Tasks => Set<Task>();
}