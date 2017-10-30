using AccountManager.Providers.Utilities;

namespace AccountManager.Providers.Models
{
    public class Category : IReadable
    {
        public readonly string Id;
        public readonly string Name;

        public Category(string name)
        {
            Name = name;
            Id = $"{Name}".GenerateMd5();

        }

        public override string ToString()
        {
            return $"{Name}";
        }
    }
}
