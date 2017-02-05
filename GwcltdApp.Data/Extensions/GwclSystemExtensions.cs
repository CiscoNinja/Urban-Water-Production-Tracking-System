using GwcltdApp.Data.Repositories;
using GwcltdApp.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GwcltdApp.Data.Extensions
{
    public static class GwclSystemExtensions
    {
        public static bool SystemExists(this IEntityBaseRepository<WSystem> gwclsystemsRepository, string name, string code)
        {
            bool _systemExists = false;

            _systemExists = gwclsystemsRepository.GetAll()
                .Any(c => c.Name.ToLower() == name ||
                c.Code.ToLower() == code);

            return _systemExists;
        }
    }
}
