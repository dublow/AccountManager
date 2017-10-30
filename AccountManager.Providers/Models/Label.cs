using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccountManager.Providers.Models
{
    [AttributeUsage(AttributeTargets.Field)]
    public class LabelTextAttribute : Attribute
    {
        public readonly string Text;

        public LabelTextAttribute(string text)
        {
            Text = text;
        }
    }
    public enum Label
    {
        [LabelText("NotFound")]
        NotFound = -1,
        [LabelText("Date")]
        Date = 0,
        [LabelText("Libellé")]
        Libelle = 1,
        [LabelText("Débit")]
        Debit = 2,
        [LabelText("Crédit")]
        Credit = 3
    }

    public static class LabelHelper
    {
        public static string ToText(this Label label)
        {
            var memberInfo = typeof(Label).GetMember(label.ToString())
                                              .FirstOrDefault();

            if (memberInfo != null)
            {
                var attribute = (LabelTextAttribute)
                             memberInfo.GetCustomAttributes(typeof(LabelTextAttribute), false)
                                       .FirstOrDefault();
                return attribute.Text;
            }
            
            throw new InvalidOperationException("Text not found");
        }

        public static Dictionary<string, Label> ToTextDictionary()
        {
            return (from label in Enum.GetValues(typeof(Label)).OfType<Label>()
                    select new { key = label.ToText(), value = label })
                    .ToDictionary(key => key.key, value => value.value);
        }
    }
}
