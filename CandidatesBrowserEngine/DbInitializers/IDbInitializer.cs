using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CandidatesBrowserEngine.DbInitializers
{
    public interface IDbInitializer
    {
        void Initialize(string adminL, string adminP);
    }
}
