using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;

using System.Threading;
using System.Threading.Tasks;



namespace Faker
{
    public class FakerGenerator
    {
        ////////////////////////////////////////////  ПРИМИТИВНЫЕ ТИПЫ    ////////////////////////////////////////////////////////////////////////////////

        private int GenerateRandomIntegerNumber()
        {
            Random random = new Random();
            return random.Next(int.MinValue, int.MaxValue);
        }
        private double GenerateRandomDoubleNumber()
        {
            Random random = new Random();
            return random.NextDouble() + random.Next(0, int.MaxValue);
        }
        private bool GenerateRandomBoolValue()
        {
            Random random = new Random();
            int intBool = random.Next(0, 2);

            if (intBool == 1)
                return true;
            else
                return false;
        }
        private long GenerateRandomLongNumber()
        {
            Random random = new Random();
            byte[] bytes = new byte[8];
            random.NextBytes(bytes);
            return BitConverter.ToInt64(bytes, 0);
        }
        private float GenerateRandomFloatNumber()
        {
            Random random = new Random();

            var array = new byte[4];
            random.NextBytes(array);

            return BitConverter.ToSingle(array, 0);
        }
        private byte GenerateRandomByteNumber()
        {
            Random random = new Random();
            return (byte)random.Next(0, 256);
        }
        private char GenerateRandomCharValue()
        {
            Random random = new Random();
            char result = Convert.ToChar(random.Next(97, 123));

            bool append = GenerateRandomBoolValue();

            if (append)
                return Convert.ToChar(result.ToString().ToUpper());
            else
                return result;
        }
        private short GenerateRandomShortNumber()
        {
            Random random = new Random();
            return (short)random.Next(short.MinValue, short.MaxValue);
        }
        private decimal GenerateRandomDecimalNumber()
        {
            Random random = new Random();
            return (decimal)GenerateRandomDoubleNumber();
        }



        private IList GenerateRandomList(Type t, IList emptyList, int countOfTheVariablesInside)
        {

            for (int i = 0; i < countOfTheVariablesInside; i++)
            {
                // Поиск конструктора с максимальным числом параметров
                object variable;

                List<ConstructorInfo> sortedCtors = t.GetConstructors().ToList();

                //x И y -два разных констуркторы которые сравниваются
                sortedCtors.Sort((x, y) => x.GetParameters().Length.CompareTo(y.GetParameters().Length));

                var ctor = sortedCtors.FirstOrDefault();

                // если нету конструктора с параметрами -> создать дефолтный экземпляр
                if (ctor == null || ctor.GetParameters().Length == 0)
                {
                    variable = Activator.CreateInstance(t);
                }
                else // иначе создать экземпляр с параметрами
                {
                    variable = Activator.CreateInstance(t, GenerateParamsForAClassTypeVariables(t));
                }

                // конвертирование тип объекта в нужный тип
                var createdItem = Convert.ChangeType(this.Create(t), t);

                emptyList.Add(createdItem);
            }
            return emptyList;
        }

        // список методов для генерации рандомных значений для примитивов
        private List<string> GetAllValueTypesGeneratorMethodsName()
        {
            List<string> result = new List<string>();

            result.Add(nameof(GenerateRandomDoubleNumber));
            result.Add(nameof(GenerateRandomDecimalNumber));
            result.Add(nameof(GenerateRandomFloatNumber));
            result.Add(nameof(GenerateRandomLongNumber));
            result.Add(nameof(GenerateRandomIntegerNumber));
            result.Add(nameof(GenerateRandomShortNumber));
            result.Add(nameof(GenerateRandomCharValue));
            result.Add(nameof(GenerateRandomByteNumber));
            result.Add(nameof(GenerateRandomBoolValue));

            return result;
        }

        // поиск циклической зависимости
        private bool HasNotCyclicDependency(Type t)
        {
            // список свойств 
            var objectProperties = t.GetProperties().ToList();

            foreach (var property in objectProperties)
            {
                //рекурсивный вызов для каждого свойства
                // t - тип объекта который мы проверяем на зависимости
                // и второй - тип данных отобранного поля
                if (WrongInsideDependency(t, property.PropertyType))
                    return false;
            }
            return true;
        }

        // рекурсивный поиск зависимостей
        private bool WrongInsideDependency(Type baseType, Type currentType)
        {
            // текущий тип свойства
            var currObjectProperties = currentType.GetProperties().ToList();

            //здесь происходит отбор всех полей у нового типа,после чего полученные типы сравниваются
            //с типом главного бъекта. И если типы совпали -> нашли зависимость
            foreach (var property in currObjectProperties)
            {
                // циклическая зависимость
                if (property.PropertyType == baseType)
                    return true;
                else //если нет -> Другой рекурсивный поиск
                    WrongInsideDependency(baseType, property.PropertyType);
            }
            return false;
        }
    }
}
