using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccountManager.Providers
{
    public interface IReader<out Tin> where Tin : IReadable
    {
        Tin Read();
    }
}
