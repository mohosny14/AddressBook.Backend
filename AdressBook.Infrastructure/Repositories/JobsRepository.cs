using AdressBook.Domain.IRepositories;
using AdressBook.Domain.Models;
using AdressBook.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdressBook.Infrastructure.Repositories
{
    public class JobsRepository : BaseRepository<Jobs>, IJobsRepository
    {
        public JobsRepository(AppDbContext context) : base(context)
        {
        }
    }
}