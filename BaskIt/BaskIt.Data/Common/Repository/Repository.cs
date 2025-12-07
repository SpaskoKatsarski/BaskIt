using BaskIt.Data;
using BaskIt.Data.Common.Repository;
using BaskIt.Domain.Common;
using BaskIt.Shared.DTOs.Page;
using Microsoft.EntityFrameworkCore;

namespace BaskIt.Data.Common.Repository;

public class Repository : IRepository
{
    private readonly BaskItDbContext context;

    public Repository(BaskItDbContext context)
    {
        this.context = context;
    }

    /// <summary>
    /// Asynchronously adds the specified entity to the database.
    /// </summary>
    /// <param name="entity">The entity to add to the database.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async Task AddAsync<T>(T entity, CancellationToken cancellationToken = default)
        where T : class
    {
        await DbSet<T>().AddAsync(entity, cancellationToken);
    }

    /// <summary>
    /// Retrieves all records of type T from the database.
    /// </summary>
    /// <returns>An IQueryable representing the queryable expression tree.</returns>
    public IQueryable<T> All<T>()
        where T : class
    {
        return DbSet<T>();
    }

    /// <summary>
    /// Retrieves all records of type T from the database without tracking changes.
    /// </summary>
    /// <returns>An IQueryable representing the queryable expression tree without change tracking.</returns>
    public IQueryable<T> AllReadOnly<T>()
        where T : class
    {
        return DbSet<T>().AsNoTracking();
    }

    /// <summary>
    /// Retrieves paginated records of type T.
    /// </summary>
    /// <param name="pageParams">Pagination parameters.</param>
    /// <param name="selector">Projection selector.</param>
    /// <param name="filterBy">Optional filter.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns></returns>
    public async Task<PageResult<TDto>> GetPaginatedAsync<TEntity, TDto>(
        PaginationParams pageParams,
        Func<IQueryable<TEntity>, IQueryable<TDto>> selector,
        Func<IQueryable<TEntity>, IQueryable<TEntity>>? filterBy = null,
        CancellationToken cancellationToken = default
    )
        where TEntity : class
    {
        IQueryable<TEntity> query = DbSet<TEntity>();

        if (filterBy != null)
        {
            query = filterBy(query);
        }

        var totalCount = await query.CountAsync(cancellationToken);

        var projectedQuery = selector(query);

        var result = await projectedQuery
            .Skip((pageParams.PageNumber - 1) * pageParams.PageSize)
            .Take(pageParams.PageSize)
            .ToListAsync(cancellationToken);

        return new PageResult<TDto>()
        {
            TotalCount = totalCount,
            CurrentPage = pageParams.PageNumber,
            PageSize = pageParams.PageSize,
            Items = result,
        };
    }

    /// <summary>
    /// Retrieves all records of type T, including soft-deleted entities, from the database.
    /// </summary>
    /// <returns>An IQueryable representing the queryable expression tree.</returns>
    public IQueryable<T> AllWithDeleted<T>()
        where T : class
    {
        return DbSet<T>().IgnoreQueryFilters();
    }

    /// <summary>
    /// Retrieves all records of type T, including soft-deleted entities, from the database without tracking changes.
    /// </summary>
    /// <returns>An IQueryable representing the queryable expression tree without change tracking.</returns>
    public IQueryable<T> AllWithDeletedReadOnly<T>()
        where T : class
    {
        return DbSet<T>().IgnoreQueryFilters().AsNoTracking();
    }

    /// <summary>
    /// Asynchronously retrieves an entity of type T from the database based on the specified id.
    /// </summary>
    /// <param name="id">The id of the entity to retrieve.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Returns the retrieved entity or null if not found.</returns>
    public async Task<T?> GetByIdAsync<T>(object id, CancellationToken cancellationToken = default)
        where T : class
    {
        return await DbSet<T>().FindAsync(new[] { id }, cancellationToken);
    }

    /// <summary>
    /// Deletes the specified entity from the database.
    /// </summary>
    /// <param name="entity">The entity to delete from the database.</param>
    public void Delete<T>(T entity)
        where T : class
    {
        DbSet<T>().Remove(entity);
    }

    /// <summary>
    /// Retrieves a DbSet for the specified entity type from the database context.
    /// </summary>
    /// <returns>The DbSet instance for the specified entity type.</returns>
    private DbSet<T> DbSet<T>()
        where T : class
    {
        return context.Set<T>();
    }

    /// <summary>
    /// Asynchronously saves all changes made in the database context.
    /// </summary>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Returns the number of affected rows.</returns>
    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var entries = context.ChangeTracker.Entries<BaseEntity>();

        foreach (var entry in entries)
        {
            if (entry.State == EntityState.Added)
            {
                entry.Entity.CreatedAt = DateTime.UtcNow;
            }
        }

        return await context.SaveChangesAsync(cancellationToken);
    }

    /// <summary>
    /// Deletes the entity with the specified id from the database.
    /// </summary>
    /// <param name="id">The id of the entity to delete from the database.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    public async Task DeleteById<T>(object id, CancellationToken cancellationToken = default)
        where T : class
    {
        var entity = await GetByIdAsync<T>(id, cancellationToken);
        if (entity != null)
        {
            DbSet<T>().Remove(entity);
        }
    }

    /// <summary>
    /// Deletes multiple entities from the database.
    /// </summary>
    /// <typeparam name="T">The type of entities to delete.</typeparam>
    /// <param name="entities">The list of entities to delete.</param>
    public void DeleteRange<T>(IEnumerable<T> entities)
        where T : class
    {
        DbSet<T>().RemoveRange(entities);
    }

    /// <summary>
    /// Soft deletes the specified entity by setting IsDeleted to true and DeletedAt to current time.
    /// </summary>
    /// <param name="entity">The entity to soft delete.</param>
    public void SoftDelete<T>(T entity)
        where T : BaseEntity
    {
        entity.IsDeleted = true;
        entity.DeletedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Soft deletes the entity with the specified id by setting IsDeleted to true and DeletedAt to current time.
    /// </summary>
    /// <param name="id">The id of the entity to soft delete.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    public async Task SoftDeleteById<T>(object id, CancellationToken cancellationToken = default)
        where T : BaseEntity
    {
        var entity = await GetByIdAsync<T>(id, cancellationToken);
        if (entity != null)
        {
            SoftDelete(entity);
        }
    }
}