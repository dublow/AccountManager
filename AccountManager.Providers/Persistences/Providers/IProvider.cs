using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccountManager.Providers.Persistences.Providers
{
    public interface IProvider
    {
        IDbConnection Create();
    }
}
