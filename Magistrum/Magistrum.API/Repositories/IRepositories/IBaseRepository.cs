namespace DMata.Repositories.IRepositories;

// E = Entity
public interface IBaseRepository<E> where E : class
{
    public Task<E> CreateAsync(E model);
    public Task<E> UpdateAsync(int id, E model);
    public Task DeleteAsync(int id);
    public Task<IEnumerable<E>> GetAllAsync();
    public Task<E> GetByIdAsync(int id);
}