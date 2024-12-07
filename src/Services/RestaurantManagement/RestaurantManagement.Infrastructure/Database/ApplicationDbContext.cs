using MediatR;
using Microsoft.EntityFrameworkCore;
using RestaurantManagement.Domain.Contracts;
using RestaurantManagement.Domain.Exceptions;
using RestaurantManagement.Domain.Models.FoodAggregate;
using RestaurantManagement.Domain.Models.FoodTypeAggregate;
using RestaurantManagement.Domain.Models.MenuAggregate;
using RestaurantManagement.Domain.Models.MenuCategoryAggregate;
using RestaurantManagement.Domain.Models.RestaurantAggregate;

namespace RestaurantManagement.Infrastructure.Database;

public class ApplicationDbContext(DbContextOptions options, IMediator _mediator) : DbContext(options), IUnitOfWork
{
    public DbSet<Restaurant> Restaurants { get; set; }
    public DbSet<RestaurantOwner> RestaurantOwners { get; set; }
    public DbSet<Menu> Menus { get; set; }
    public DbSet<MenuItem> MenuItems { get; set; }
    public DbSet<MenuCategory> MenuCategories { get; set; }
    public DbSet<Food> Foods { get; set; }
    public DbSet<FoodType> FoodTypes { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
        modelBuilder.ApplyDefaultConfigurations();
    }

    public async Task CommitAsync(CancellationToken cancellationToken = default)
    {
        ChangeTracker.DetectChanges();
        ChangeTracker.AutoDetectChangesEnabled = false;

        UpdateTimeStamps();
        await DispatchDomainEventsAsync();

        ChangeTracker.AutoDetectChangesEnabled = true;

        try
        {
            await SaveChangesAsync(cancellationToken);
        }
        catch (DbUpdateConcurrencyException)
        {
            ChangeTracker.Clear();
            throw new UpdateConcurrencyException();
        }
    }

    private void UpdateTimeStamps()
    {
        var now = DateTime.UtcNow;
        foreach (var entity in ChangeTracker.Entries<Entity>())
        {
            if (entity.State == EntityState.Added)
                entity.Entity.CreatedAt = now;

            if (entity.State == EntityState.Modified)
                entity.Entity.UpdatedAt = now;
        }
    }

    private async Task DispatchDomainEventsAsync()
    {
        var aggregateRoots = ChangeTracker
            .Entries<AggregateRoot>()
            .Where(x => x.Entity.DomainEvents.Count != 0)
            .ToList();

        var domainEvents = aggregateRoots
            .SelectMany(x => x.Entity.DomainEvents)
            .ToList();

        aggregateRoots.ForEach(e => e.Entity.ClearDomainEvents());

        foreach (var domainEvent in domainEvents)
            await _mediator.Publish(domainEvent);
    }

    public async Task MigrateAsync(CancellationToken cancellationToken)
    {
        if ((await Database.GetPendingMigrationsAsync(cancellationToken)).Any())
            await Database.MigrateAsync(cancellationToken);
    }
}
