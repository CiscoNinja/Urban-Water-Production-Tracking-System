using GwcltdApp.Data.Repositories;
using GwcltdApp.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GwcltdApp.Data.Extensions
{
    public static class GwclTypeExtensions
    {
        public static bool TypeExists(this IEntityBaseRepository<OptionType> gwclotypesRepository, string name)
        {
            bool _typeExists = false;

            _typeExists = gwclotypesRepository.GetAll()
                .Any(c => c.Name.ToLower() == name );

            return _typeExists;
        }
    }
}
