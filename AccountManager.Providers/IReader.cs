using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccountManager.Providers
{
    public interface IReader<Tin> where Tin : IReadable
    {
        IEnumerable<Tin> Read();
    }
}
