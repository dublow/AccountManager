using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccountManager.Providers.Persistences.Models
{
    public class CategoryDb : IDatabasable
    {
        public readonly string Id;
        public readonly string Name;

        public CategoryDb(string id, string name)
        {
            Id = id;
            Name = name;
        }
    }
}
