using ClassLibrary2.FIXF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary2
{
    public class Class1
    {
        public static string BuildStringFIX1(string message, Type enumType)
        {
            List<int> fixFields = f.GetValues(enumType).Cast<int>().ToList();

            var elements = message.Split('\u0001');
            var updatedElements = new List<string>();

            foreach (var element in elements)
            {
                var tagValue = element.Split('=');
                if (tagValue.Length == 2 && int.TryParse(tagValue[0], out int tag))
                {
                    if (fixFields.Contains(tag))
                    {
                        updatedElements.Add("|" + element);
                    }
                    else
                    {
                        updatedElements.Add(element);
                    }
                }
            }
            return string.Join("\u0001", updatedElements);
        }
    }
}
