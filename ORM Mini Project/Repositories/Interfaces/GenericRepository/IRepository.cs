using ORM_Mini_Project.Models;

namespace ORM_Mini_Project.Repositories.Interfaces.GenericRepository
{
    public interface IRepository<T> where T : BaseEntity, new()
    {
        Task CreateAsync(T entity);


    }
}
