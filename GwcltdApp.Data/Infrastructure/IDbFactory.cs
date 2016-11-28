using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GwcltdApp.Data.Infrastructure
{
    public interface IDbFactory : IDisposable
    {
        GwcltdAppContext Init();
    }
}
