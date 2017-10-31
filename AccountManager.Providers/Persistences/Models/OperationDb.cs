using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccountManager.Providers.Persistences.Models
{
    public class OperationDb : IDatabasable
    {
        public readonly string Id;
        public readonly DateTime Date;
        public readonly string Libelle;
        public readonly decimal Amount;
        public readonly bool IsCredit;

        public OperationDb(string id, DateTime date, string libelle, decimal amount, bool isCredit)
        {
            Id = id;
            Date = date;
            Libelle = libelle;
            Amount = amount;
            IsCredit = isCredit;
        }
    }
}
