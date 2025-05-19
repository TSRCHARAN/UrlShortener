using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UrlShortener.Domain.Repositories
{
    public interface IGenericRepository<TEntity>
    {
        Task<TEntity> GetByIdAsync(int id);

        TEntity GetById(int id);

        Task<IEnumerable<TEntity>> GetAllAsync();

        Task<bool> AddAsync(TEntity model);

        bool Add(TEntity model);

        void Update(TEntity model);

        void Reload(TEntity model);

        void Remove(TEntity model);
    }
}
