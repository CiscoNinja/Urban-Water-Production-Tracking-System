using GwcltdApp.Data.Repositories;
using GwcltdApp.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GwcltdApp.Data.Extensions
{
    public static class GwclOptionExtensions
    {
        public static bool OptionExists(this IEntityBaseRepository<Option> gwcloptionsRepository, string name)
        {
            bool _optionExists = false;

            _optionExists = gwcloptionsRepository.GetAll()
                .Any(c => c.Name.ToLower() == name);

            return _optionExists;
        }
    }
}
