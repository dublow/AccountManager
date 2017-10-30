namespace AccountManager.Providers.Models
{
    public class Account
    {
        public readonly string Name;
        public readonly float Sold;
        public readonly bool IsCredit;

        public Account(string name, float sold, bool isCredit)
        {
            Name = name;
            Sold = sold;
            this.IsCredit = isCredit;
        }
    }
}
