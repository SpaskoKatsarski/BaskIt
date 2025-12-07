using BaskIt.Domain.Common;
using BaskIt.Shared.DTOs.Page;

namespace BaskIt.Data.Common.Repository;

public interface IRepository
{
    /// <summary>
    /// Retrieves all records of type T from the database.
    /// </summary>
    /// <returns>An IQueryable representing the queryable expression tree.</returns>
    IQueryable<T> All<T>()
        where T : class;

    /// <summary>
    /// Retrieves all records of type T from the database without tracking changes.
    /// </summary>
    /// <returns>An IQueryable representing the queryable expression tree without change tracking.</returns>
    IQueryable<T> AllReadOnly<T>()
        where T : class;

    /// <summary>
    /// Retrieves paginated records of type T.
    /// </summary>
    /// <param name="pageParams">Pagination parameters.</param>
    /// <param name="selector">Projection selector.</param>
    /// <param name="filterBy">Optional filter.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns></returns>
    Task<PageResult<TDto>> GetPaginatedAsync<TEntity, TDto>(PaginationParams pageParams, Func<IQueryable<TEntity>, IQueryable<TDto>> selector, Func<IQueryable<TEntity>, IQueryable<TEntity>>? filterBy = null, CancellationToken cancellationToken = default)
    where TEntity : class;

    /// <summary>
    /// Asynchronously adds the specified entity to the database.
    /// </summary>
    /// <param name="entity">The entity to add to the database.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task AddAsync<T>(T entity, CancellationToken cancellationToken = default)
        where T : class;

    /// <summary>
    /// Asynchronously retrieves an entity of type T from the database based on the specified id.
    /// </summary>
    /// <param name="id">The id of the entity to retrieve.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Returns the retrieved entity or null if not founded.</returns>
    Task<T?> GetByIdAsync<T>(object id, CancellationToken cancellationToken = default)
        where T : class;

    /// <summary>
    /// Deletes the specified entity from the database.
    /// </summary>
    /// <param name="entity">The entity to delete from the database.</param>
    void Delete<T>(T entity)
        where T : class;

    /// <summary>
    /// Deletes the entity with the specified id from the database.
    /// </summary>
    /// <param name="id">The id of the entity to delete from the database.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    Task DeleteById<T>(object id, CancellationToken cancellationToken = default)
        where T : class;

    /// <summary>
    /// Soft deletes the specified entity by setting IsDeleted to true and DeletedAt to current time.
    /// </summary>
    /// <param name="entity">The entity to soft delete.</param>
    void SoftDelete<T>(T entity)
        where T : BaseEntity;

    /// <summary>
    /// Soft deletes the entity with the specified id by setting IsDeleted to true and DeletedAt to current time.
    /// </summary>
    /// <param name="id">The id of the entity to soft delete.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    Task SoftDeleteById<T>(object id, CancellationToken cancellationToken = default)
        where T : BaseEntity;

    /// <summary>
    /// Asynchronously saves all changes made in the database context.
    /// </summary>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Returns the number of affected rows.</returns>
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Deletes multiple entities from the database.
    /// </summary>
    /// <typeparam name="T">The type of entities to delete.</typeparam>
    /// <param name="entities">The list of entities to delete.</param>
    void DeleteRange<T>(IEnumerable<T> entities)
        where T : class;

    /// <summary>
    /// Retrieves all records of type T, including soft-deleted entities, from the database without tracking changes.
    /// </summary>
    /// <returns>An IQueryable representing the queryable expression tree without change tracking.</returns>
    IQueryable<T> AllWithDeletedReadOnly<T>()
        where T : class;

    /// <summary>
    /// Retrieves all records of type T, including soft-deleted entities, from the database.
    /// </summary>
    /// <returns>An IQueryable representing the queryable expression tree.</returns>
    IQueryable<T> AllWithDeleted<T>()
        where T : class;
}
