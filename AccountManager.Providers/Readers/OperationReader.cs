using AccountManager.Providers.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace AccountManager.Providers.Readers
{
    public class OperationReader : IReader<Operation>
    {
        private readonly Dictionary<string, Label> _labels = LabelHelper.ToTextDictionary();
        private readonly Stream _stream;

        public OperationReader(string filename)
        {
            _stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(filename);
        }

        public IEnumerable<Operation> Read()
        {
            using (var streamReader = new StreamReader(_stream))
            {
                var currentLine = streamReader.ReadLine();
                var operationRow = new Dictionary<Label, string>();

                while(currentLine != null)
                {
                    var (key, value) = GetOperationRow(currentLine);

                    if (key != Label.NotFound)
                    {
                        if(operationRow.TryGetValue(key, out var v))
                        {
                            yield return CreateOperation(operationRow);
                            operationRow.Clear();
                        }

                        operationRow.Add(key, value);
                    }
                    currentLine = streamReader.ReadLine();
                }
            }
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
            var amount = float.Parse(isCredit ? credit : values[Label.Debit], System.Globalization.NumberStyles.Any);

            return new Operation(date, libelle, amount, isCredit);
        }
    }
}
