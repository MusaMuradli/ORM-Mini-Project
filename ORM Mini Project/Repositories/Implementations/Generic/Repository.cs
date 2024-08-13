using ORM_Mini_Project.Models;
using ORM_Mini_Project.Repositories.Interfaces.GenericRepository;

namespace ORM_Mini_Project.Repositories.Implementations.Generic
{
    public class Repository<T> : IRepository<T> where T : BaseEntity, new()
    {
        public Task CreateAsync(T entity)
        {
            throw new NotImplementedException();
        }
        public Task DeleteAsync(T entity)
        {
            throw new NotImplementedException();
        }
    }
}
