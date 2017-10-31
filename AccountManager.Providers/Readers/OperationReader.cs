using AccountManager.Providers.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace AccountManager.Providers.Readers
{
    public class OperationReader : IReader<AccountOperation>
    {
        private readonly Dictionary<string, Label> _labels = LabelHelper.ToTextDictionary();

        private (string title, string firstname, string lastname) _accountName =
            (string.Empty, string.Empty, string.Empty);

        private bool HasAccountName
        {
            get
            {
                (string title, string firstname, string lastname) = _accountName;
                return !string.IsNullOrEmpty(title) &&
                       !string.IsNullOrEmpty(firstname) &&
                       !string.IsNullOrEmpty(lastname);
            }
        }

        private (decimal?, bool) _amount;

        private bool HasAmount
        {
            get
            {
                (decimal? sold, bool isCredit) = _amount;
                return sold.HasValue;
            }
        }

        private readonly Stream _stream;

        public OperationReader(string filename)
        {
            _stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(filename);
        }

        public AccountOperation Read()
        {
            using (var streamReader = new StreamReader(_stream))
            {
                var currentLine = streamReader.ReadLine();

                var operationRow = new Dictionary<Label, string>();

                var operations = new List<Operation>();
                while(currentLine != null)
                {
                    if(!HasAccountName)
                        _accountName = GetAccountName(currentLine);

                    if (!HasAmount)
                        _amount = GetAmount(currentLine);

                    var (key, value) = GetOperationRow(currentLine);

                    if (key != Label.NotFound)
                    {
                        if(operationRow.TryGetValue(key, out var v))
                        {
                            operations.Add(CreateOperation(operationRow));
                            operationRow.Clear();
                        }

                        operationRow.Add(key, value);
                    }
                    currentLine = streamReader.ReadLine();

                }

                (string title, string firstname, string lastname) = _accountName;
                (decimal? sold, bool isCredit) = _amount;
                var account = new Account(title, firstname, lastname, sold ?? 0, isCredit);
                return new AccountOperation(account, operations);
            }
        }

        private (string title, string firstname, string lastname) GetAccountName(string line)
        {
            if (line.Contains("DELFOUR NICOLAS"))
            {
                var acountName = line.Split(' ').Where(x => x != string.Empty).ToArray();

                if (acountName.Length == 3)
                {
                    return (acountName[0], acountName[2], acountName[1]);
                }
            }

            return (string.Empty, string.Empty, string.Empty);
        }

        private (decimal?sold, bool isCredit) GetAmount(string line)
        {
            if (line.Contains("Solde au "))
            {
                var amount = line.Split(' ').Where(x => x != string.Empty).ToArray();

                if (amount.Length == 7)
                {
                    var isCredit = amount[3][0] == '+';
                    var sold = decimal.Parse((amount[3][1] + amount[4]));
                    return (sold, isCredit);
                }
                if (amount.Length == 6)
                {
                    var isCredit = amount[3][0] == '+';
                    var sold = decimal.Parse(amount[3].Substring(1));
                    return (sold, isCredit);
                }
            }

            return (null, false);
        }

        private (Label label, string value) GetOperationRow(string line)
        {
            var split = line.Split(':');

            if (split.Length == 2) {
                var cleanLabel = split[0].Split(' ');
                var label = cleanLabel[0];

                if(_labels.TryGetValue(label, out var value))
                {
                    return (value, split[1].Trim());
                }
            }

            return (Label.NotFound,string.Empty);
        }

        private Operation CreateOperation(Dictionary<Label, string> values)
        {
            var isCredit = values.TryGetValue(Label.Credit, out var credit);
            var date = DateTime.Parse($"{values[Label.Date]}/2017");
            var libelle = values[Label.Libelle].Replace("'", " ");
            var amount = decimal.Parse(isCredit ? credit : values[Label.Debit], System.Globalization.NumberStyles.Any);

            return new Operation(date, libelle, amount, isCredit);
        }
    }
}
