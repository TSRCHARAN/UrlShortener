using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UrlShortener.Domain.Models;
using UrlShortener.Domain.Repositories;
using UrlShortener.Persistance.Repositories;

namespace UrlShortener.Persistence.Repositories
{
    public class UnitOfWork:IUnitOfWork
    {
        private readonly UrlDbContext _urlDbContext;
        private readonly ILogger _logger;
        private IShortUrlRepository _shortUrlRepository;

        public UnitOfWork(UrlDbContext urlDbContext, ILogger Logger)
        {
            _urlDbContext = urlDbContext;
            _logger = Logger;
        }

        public IShortUrlRepository ShortUrlRepository
        {
            get { return _shortUrlRepository = _shortUrlRepository ?? new ShortUrlRepository(_urlDbContext, _logger); }
        }
        public async Task<int> SaveAsync()
        {
            return await _urlDbContext.SaveChangesAsync();
        }

        public void DisableDetectChanges()
        {
            _urlDbContext.ChangeTracker.AutoDetectChangesEnabled = false;
            return;
        }

        public void EnableDetectChanges()
        {
            _urlDbContext.ChangeTracker.AutoDetectChangesEnabled = true;
            return;
        }
        public int Save()
        {
            return _urlDbContext.SaveChanges();
        }
    }
}
