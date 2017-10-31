namespace AccountManager.Providers.Models
{
    public class Account : IReadable
    {
        public readonly string Title;
        public readonly string Firstname;
        public readonly string Lastname;
        public readonly decimal Sold;
        public readonly bool IsCredit;

        public Account(string title, string firstname, string lastname, decimal sold, bool isCredit)
        {
            Title = title;
            Lastname = lastname;
            Firstname = firstname;
            Sold = sold;
            IsCredit = isCredit;
        }
    }
}
