using System.Collections.Generic;

namespace AccountManager.Providers.Models
{
    public class AccountOperation : IReadable
    {
        public readonly Account Account;
        public readonly IEnumerable<Operation> Operations;

        public AccountOperation(Account account, IEnumerable<Operation> operations)
        {
            Account = account;
            Operations = operations;
        }
    }
}
