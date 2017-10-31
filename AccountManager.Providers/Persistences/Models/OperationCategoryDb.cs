using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccountManager.Providers.Persistences.Models
{
    public class OperationCategoryDb : IDatabasable
    {
        public readonly string OperationId;
        public readonly DateTime Date;
        public readonly string Libelle;
        public readonly decimal Amount;
        public readonly bool IsCredit;
        public readonly string CategoryId;
        public readonly string Name;

        public OperationCategoryDb(string operationId, DateTime date, string libelle, decimal amount, bool isCredit, string categoryId, string name)
        {
            OperationId = operationId;
            Date = date;
            Libelle = libelle;
            Amount = amount;
            IsCredit = isCredit;
            CategoryId = categoryId;
            Name = name;
        }
    }
}
