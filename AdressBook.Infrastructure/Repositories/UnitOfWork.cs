using AdressBook.Domain.IRepositories;
using AdressBook.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdressBook.Infrastructure.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private bool disposed = false;
        private readonly AppDbContext _dbContext;
        public IEmployeeRepository EmployeeRepository { get; }

        public UnitOfWork(AppDbContext dbContext,
                          IEmployeeRepository employeeRepository)
        {
            _dbContext = dbContext;
            EmployeeRepository = employeeRepository;
        }
        public int Complete()
        {
            return _dbContext.SaveChanges();
        }

        public async Task<int> CompleteAsync()
        {
            return await _dbContext.SaveChangesAsync();
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    _dbContext.Dispose();
                }
            }
            this.disposed = true;
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}