using BaskIt.Data;
using BaskIt.Data.Common.Repository;
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
    /// <returns>A task representing the asynchronous operation.</returns>
    public async Task AddAsync<T>(T entity)
        where T : class
    {
        await DbSet<T>().AddAsync(entity);
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
    /// <returns></returns>
    public async Task<PageResult<TDto>> GetPaginatedAsync<TEntity, TDto>(
        PaginationParams pageParams,
        Func<IQueryable<TEntity>, IQueryable<TDto>> selector,
        Func<IQueryable<TEntity>, IQueryable<TEntity>>? filterBy = null
    )
        where TEntity : class
    {
        IQueryable<TEntity> query = DbSet<TEntity>();

        if (filterBy != null)
        {
            query = filterBy(query);
        }

        var totalCount = await query.CountAsync();

        var projectedQuery = selector(query);

        var result = await projectedQuery
            .Skip((pageParams.PageNumber - 1) * pageParams.PageSize)
            .Take(pageParams.PageSize)
            .ToListAsync();

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
    /// <returns>Returns the retrieved entity or null if not found.</returns>
    public async Task<T?> GetByIdAsync<T>(object id)
        where T : class
    {
        return await DbSet<T>().FindAsync(id);
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
    /// <returns>Returns the number of affected rows.</returns>
    public async Task<int> SaveChangesAsync()
    {
        return await context.SaveChangesAsync();
    }

    /// <summary>
    /// Deletes the entity with the specified id from the database.
    /// </summary>
    /// <param name="id">The id of the entity to delete from the database.</param>
    public async Task DeleteById<T>(object id)
        where T : class
    {
        var entity = await GetByIdAsync<T>(id);
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
}