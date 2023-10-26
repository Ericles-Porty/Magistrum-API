namespace Services.IServices;

// M = Model
public interface IBaseService<M> where M : class
{
    public Task<M> CreateAsync(M model);
    public Task<M> UpdateAsync(int id, M model);
    public Task DeleteAsync(int id);
    public Task<IEnumerable<M>> GetAllAsync();
    public Task<M> GetByIdAsync(int id);
}