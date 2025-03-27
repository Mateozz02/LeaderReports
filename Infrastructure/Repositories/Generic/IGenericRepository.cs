namespace LeaderReport.Infrastructure.Repositories.Generic
{
    public interface IGenericRepository<T> where T : class
    {
        Task<IEnumerable<T>> GetAll();
        Task<T> GetById(int id);
        Task Add(T entidad);
        Task Update(T entidad);
        Task Delete(int id);
    }

}
