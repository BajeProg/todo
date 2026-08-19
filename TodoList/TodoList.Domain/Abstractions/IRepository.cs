namespace TodoList.Domain.Abstractions
{
    public interface IRepository<T>
    {
        public Task<T?> Get(Guid id, CancellationToken cancellationToken);
        public Task<IEnumerable<T>> GetAll(CancellationToken cancellationToken);
        public Task<T> Add(T entity, CancellationToken cancellationToken);
        public Task<T> Update(T entity, CancellationToken cancellationToken);
        public Task Delete(Guid id, CancellationToken cancellationToken);
    }
}
