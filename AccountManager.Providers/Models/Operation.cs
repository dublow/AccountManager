using AccountManager.Providers.Utilities;
using System;

namespace AccountManager.Providers.Models
{
    public class Operation : IReadable
    {
        public readonly string Id;
        public readonly DateTime Date;
        public readonly string Libelle;
        public readonly float Amount;
        public readonly bool IsCredit;

        public Operation(DateTime date, string libelle, float amount, bool isCredit)
        {
            Date = date;
            Libelle = libelle;
            Amount = amount;
            IsCredit = isCredit;
            Id = $"{Date}{Libelle}{Amount}{IsCredit}".GenerateMd5();

        }

        public override string ToString()
        {
            var op = IsCredit ? "+" : "-";
            return $"{op} {Date}> {Amount}€ - {Libelle}";
        }
    }
}
