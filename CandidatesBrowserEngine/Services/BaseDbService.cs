using CandidatesBrowserEngine.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CandidatesBrowserEngine.Services
{
    public abstract class BaseDbService
    {
        protected readonly ApplicationDbContext _dbContext;

        public BaseDbService(ApplicationDbContext dbContext){
            _dbContext = dbContext;
        }
    }
}
