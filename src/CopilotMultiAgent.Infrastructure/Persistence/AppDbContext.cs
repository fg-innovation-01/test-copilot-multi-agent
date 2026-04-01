using CopilotMultiAgent.Domain.Entities;
using CopilotMultiAgent.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace CopilotMultiAgent.Infrastructure.Persistence;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<TodoItem> TodoItems => Set<TodoItem>();
    public DbSet<Schedule> Schedules => Set<Schedule>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<TodoItem>(entity =>
        {
            entity.HasKey(t => t.Id);

            entity.Property(t => t.Title)
                .IsRequired()
                .HasMaxLength(200);

            entity.Property(t => t.IsCompleted)
                .IsRequired();

            entity.Property(t => t.CreatedAt)
                .IsRequired();

            entity.Property(t => t.IsDeleted).IsRequired().HasDefaultValue(false);

            entity.Property(t => t.Tags)
                .HasConversion(
                    v => JsonSerializer.Serialize(v, (JsonSerializerOptions?)null),
                    v => JsonSerializer.Deserialize<List<string>>(v, (JsonSerializerOptions?)null) ?? new List<string>());

            entity.HasQueryFilter(t => !t.IsDeleted);
        });

        modelBuilder.Entity<Schedule>(entity =>
        {
            entity.HasKey(s => s.Id);

            entity.Property(s => s.Title)
                .IsRequired()
                .HasMaxLength(200);

            entity.Property(s => s.ScheduledAt)
                .IsRequired();

            entity.Property(s => s.RecurrenceType)
                .IsRequired()
                .HasConversion<int>();

            entity.Property(s => s.IsCompleted)
                .IsRequired();

            entity.Property(s => s.CreatedAt)
                .IsRequired();

            entity.Property(s => s.IsDeleted)
                .IsRequired()
                .HasDefaultValue(false);

            entity.HasQueryFilter(s => !s.IsDeleted);
        });
    }
}
