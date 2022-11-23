using FormatterConsole.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace FormatterConsole
{
    public class StringFormatter : IStringFormatter
    {
        public string Format(string template, params object[] parameters)
        {
            StringBuilder resultSB = new StringBuilder(template);
            List<string> variableNames = new List<string>();
            List<int> bracketIndexes = new List<int>();

            // Получаем индексы элементов в строке, по которым расположены квадратные скобки
            for (int i = 0; i < resultSB.Length; i++)
            {
                if (resultSB[i] is '{' || resultSB[i] is '}')
                    bracketIndexes.Add(i);
            }

            if (bracketIndexes.Count % 2 != 0)
                throw new Exception("Wrong brackets number");

            // Ниже мы получаем список только из тех скобок, которые не были экранированы той же самой скобкой
            bool hasRemovedSymbolBrackets = false;
            while (!hasRemovedSymbolBrackets)
            {
                for (int i = 0; i < bracketIndexes.Count - 1; i++)
                {
                    if (bracketIndexes[i] == bracketIndexes[i + 1] - 1) // Если собки стоят друг за другом - их не включаем
                    {
                        bracketIndexes.RemoveRange(i, 2);
                        break;
                    }
                    if (i == bracketIndexes.Count - 2)
                        hasRemovedSymbolBrackets = true;
                }
            }

            List<int> bracketIndexesTempList = new List<int>(bracketIndexes);
            // Из оставшихся скобок извлекаем наименования параметров, для которых необходимо осуществить подстановку
            while (bracketIndexesTempList.Count != 0)
            {
                for (int i = 0; i < bracketIndexesTempList.Count - 1; i++)
                {
                    variableNames.Add(template.Substring(bracketIndexesTempList[i] + 1, bracketIndexesTempList[i + 1] - bracketIndexesTempList[i] - 1));
                    bracketIndexesTempList.RemoveRange(i, 2);
                    break;
                }
            }

            // Получаем все поля переданного объекта через ссылку this на этот объект

            var bindingFlags = BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public;
            List<FieldInfo> objectsFields = new List<FieldInfo>();

            foreach (var parameter in parameters)
            {
                var fieldValues = parameter.GetType().GetFields(bindingFlags).Select(field => field.GetValue(parameter)).ToList();
                var fieldNames = parameter.GetType().GetFields(bindingFlags).Select(field => field.Name).ToList();

                for (int i = 0; i < fieldValues.Count; i++)
                {
                    objectsFields.Add(new FieldInfo()
                    {
                        FieldName = fieldNames[i].Substring(fieldNames[i].IndexOf('<') + 1, fieldNames[i].IndexOf('>') - fieldNames[i].IndexOf('<') - 1),
                        FieldValue = fieldValues[i].ToString()
                    });
                }
            }

            StringBuilder res = new StringBuilder(string.Empty);
            int firstBracketIndex = bracketIndexes[0];

            for (int i = 0; i < template.Length;)
            {
                if (i != firstBracketIndex)
                {
                    res.Append(template[i]);
                    i++;
                    continue;
                }

                string variableInBrackets = template.Substring(firstBracketIndex + 1, bracketIndexes[1] - firstBracketIndex - 1);

                try
                {
                    res.Append(objectsFields.First(fieldInfo => fieldInfo.FieldName == variableInBrackets).FieldValue);
                }
                catch (InvalidOperationException)
                {
                    throw new Exception("Field in the string does not exist in the passed object");
                }

                i = bracketIndexes[1] + 1;

                bracketIndexes.RemoveRange(0, 2);

                if (bracketIndexes.Count != 0)
                    firstBracketIndex = bracketIndexes[0];

            }

            return res.ToString();
        }
        private class FieldInfo
        {
            public string FieldName { get; set; }
            public string FieldValue { get; set; }
        }
    }
}
