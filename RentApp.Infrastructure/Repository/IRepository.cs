using System.Collections.Generic;
using System.Threading.Tasks;

namespace RentApp.Infrastructure.Repository
{
  public interface IRepository <TEntity>
  {
    Task<IEnumerable<TEntity>> GetAll();
    Task<TEntity> GetById(long id);
    Task Add(TEntity flat);
    Task Update(TEntity entity);
    Task Delete(long id);
  }
}
