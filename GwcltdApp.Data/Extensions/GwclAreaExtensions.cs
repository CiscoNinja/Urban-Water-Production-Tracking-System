using GwcltdApp.Data.Repositories;
using GwcltdApp.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GwcltdApp.Data.Extensions
{
    public static class GwclAreaExtensions
    {
        public static bool AreaExists(this IEntityBaseRepository<GwclArea> gwclareasRepository, string name, string code)
        {
            bool _areaExists = false;

            _areaExists = gwclareasRepository.GetAll()
                .Any(c => c.Name.ToLower() == name ||
                c.Code.ToLower() == code);

            return _areaExists;
        }
    }
}
