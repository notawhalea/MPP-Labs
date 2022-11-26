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

        public T Create<T>()
        {
            Thread.Sleep(1);
            return (T)Create(typeof(T));
        }

        //Type t - тип переменных,объектов
        private object Create(Type t)
        {
            if (!HasNotCyclicDependency(t))
                throw new Exception("ERROR! Cyclic Dependency was found.");

            // проверка ссылка это или нет
            if (t.IsValueType)
            {
                try 
                {
                    if (!t.IsSecurityCritical && t.IsSecurityTransparent && t.IsSerializable)
                        return GenerateInstanceWithValueTypeVariable(t);
                    else
                        return GenerateInstanceWithAStructureVariable(t);
                }
                catch (Exception)
                {
                    return GenerateInstanceWithValueTypeVariable(t);
                }
            }
            else // ссылочный тип
            {
                // строка
                if (t.IsSerializable && t.IsSecurityTransparent && t.IsSealed && !t.IsSecurityCritical)
                    return GenerateInstanceWithAStringValue(t,10);

                // list of lists
                if (t.IsGenericType)
                    return GenerateInstanceWithAGenericTypeVariable(t);

                return GenerateInstanceWithAClassTypeVariable(t);
            }
        }


        private object CreateInstance(Type t)
        {
            //Console.WriteLine(t);
            //устанавливает дефолт значения для примитивных типов и String
            if (t.IsValueType)
            {
                // Для типов-значений вызов конструктора по умолчанию даст default(T).
                //Activator Содержит методы, позволяющие локально или удаленно создавать типы объектов или получать ссылки на существующие удаленные объекты.
                //Console.WriteLine(Activator.CreateInstance(t));
                return Activator.CreateInstance(t);
            }
            else
                // Для ссылочных типов значение по умолчанию всегда null.
                return null;
        }

        //примитивные типы
        private object GenerateInstanceWithValueTypeVariable(Type t)
        {
            Thread.Sleep(1);
            //var instance = CreateInstance(t);

            var valueTypeGeneratorMethods = GetAllValueTypesGeneratorMethodsName();

           
            foreach (var item in valueTypeGeneratorMethods)
            {
                //NonPublic извлекает не публичные методы
                //Instance получает только методы экземпляра
                //Здесь просиходит вызов методов
                /*  GenerateRandomDoubleNumber
                    GenerateRandomDecimalNumber
                    GenerateRandomFloatNumber
                    GenerateRandomLongNumber
                    GenerateRandomIntegerNumber
                    GenerateRandomShortNumber
                    GenerateRandomCharValue
                    GenerateRandomByteNumber
                    GenerateRandomBoolValue*/
                var temp = GetType().GetMethod(item, BindingFlags.NonPublic | BindingFlags.Instance).Invoke(this, new object[] { });

                //проверка типа с типом метода
                if (t != temp.GetType())
                    continue;       //если не совпадает
                else
                {
                    var instance = temp;
                    return instance;
                }

            }
            throw new Exception("Error. Value type was not created.");
        }


        private object GenerateInstanceWithAStringValue(Type t, byte strLength)
        {
            Thread.Sleep(1);
            var instance = CreateInstance(t);

            try
            {
                instance = GenerateRandomString(strLength);
                return instance;
            }
            catch (Exception)
            {
                throw new Exception("Error. String type was not created.");
            }
            
        }
        private string GenerateRandomString(int length)
        {
            Random random = new Random();
            //StringBuilder Предоставляет изменяемую строку символов
            //так как при добавлении новых подстрок String постоянно выделяет нужное количество памяти
            //то приходится заново перекопировать,что неэффективно . StringBuilder выделяет память с небольшим запасом,что делает его более гибким
            var sb = new StringBuilder(string.Empty);

            for (int i = 0; i < length; i++)
            {
                sb.Append(Convert.ToChar(random.Next(97, 123)));
            }

            return sb.ToString();
        }


        private object GenerateInstanceWithAGenericTypeVariable(Type t)
        {
            Thread.Sleep(1);
            //Activator Содержит методы, позволяющие локально или удаленно создавать типы объектов или получать ссылки на существующие удаленные объекты.
            //Метод CreateInstance создает экземпляр типа, определенного в сборке, путем вызова конструктора, который лучше всего соответствует указанным аргументам.
            var instance = (IList)Activator.CreateInstance(t);

            //получение массива аргументов универсального типа
            var genericTypeInsideVariable = t.GenericTypeArguments.FirstOrDefault();

            instance = GenerateRandomList(genericTypeInsideVariable, instance, 5);

            return instance;
        }

        private ConstructorInfo ReceiveNewConstructor(Type t,ConstructorInfo prevConstructor)
        {

            var listOfConstructors = t.GetConstructors();
            int index = 0;

            //если означает,что все конструкторы закончились (все они отработали с ошибкой)
            if (prevConstructor.GetParameters().Length == listOfConstructors[0].GetParameters().Length)
                return null;

            var max = listOfConstructors[0];
            foreach(var constructor in listOfConstructors)
            {
                if (constructor.GetParameters().Length > max.GetParameters().Length && constructor.GetParameters().Length < prevConstructor.GetParameters().Length)
                {
                    index = constructor.GetParameters().Length;
                }
            }

            ConstructorInfo newConstructor = listOfConstructors.Where(c => c.GetParameters().Length == index).First();
            return newConstructor;
        }

        private object GenerateInstanceWithAClassTypeVariable(Type t)
        {
            var maxParamConstructor = FindAConstructorWithMaxParametersNumber(t);
            var parameters = GenerateParamsForAClassTypeVariables(t);

            //вызов конструктора с макс параметррами - СОЗДАНИЕ ОБЪЕКТА
            bool flag = false;
            bool cycle = true;

            Object randomlyGeneratedObject = 0;
            while (cycle)
            {
                try
                {
                    if (flag == false)
                    {
                        randomlyGeneratedObject = maxParamConstructor.Invoke(parameters);
                        cycle = false;
                    }                                
                    else
                    {
                        maxParamConstructor = ReceiveNewConstructor(t, maxParamConstructor);

                        //ни один объект не был создан из всех предоставляемых конструкторов
                        if (maxParamConstructor == null)
                            return null;

                        parameters = GenerateParamsForAClassTypeVariables(t);
                        randomlyGeneratedObject = maxParamConstructor.Invoke(parameters);
                        cycle = false;
                    }
                }
                catch
                {
                    flag = true;                  
                }
            }

            //property - получаем список всех свойств (например для класса Person это Name,Surname,Age,IsHasDog
            var propertyInfos = randomlyGeneratedObject.GetType().GetProperties().Where(p => !p.SetMethod.IsPrivate).ToList();
            //публичные поля
            var publicFields = randomlyGeneratedObject.GetType().GetFields().Where(p => !p.IsPrivate).ToList();

            //если в конструкторе что-то не заполнено то автоматически заполнить
            for (int i = 0; i < propertyInfos.Count; i++)
            {
                Type generatedObjectType = randomlyGeneratedObject.GetType();
                PropertyInfo objectPropertyTakenByName = generatedObjectType.GetProperty(propertyInfos[i].Name);

                //проперти уже содержит тип
                var objectPropertyType = objectPropertyTakenByName.PropertyType;
                var generatedProperty = this.Create(objectPropertyType);

                objectPropertyTakenByName.SetValue(randomlyGeneratedObject,
                generatedProperty, null);
            }

            for (int i = 0; i < publicFields.Count; i++)
            {
                var generatedObjectType = randomlyGeneratedObject.GetType();
                var objectFieldTakenByName = generatedObjectType.GetField(publicFields[i].Name);

                var objectFieldType = objectFieldTakenByName.GetValue(randomlyGeneratedObject).GetType();
                var generatedProperty = this.Create(objectFieldType);

                objectFieldTakenByName.SetValue(randomlyGeneratedObject,
                generatedProperty);
            }

            // рандомно созданный объект
            return randomlyGeneratedObject;
        }
        private object[] GenerateParamsForAClassTypeVariables(Type t)
        {
            ConstructorInfo maxParamConstructor = FindAConstructorWithMaxParametersNumber(t);

            // получение информации - получаем типы параметров в конструкторе
            ParameterInfo[] parametersInfo = maxParamConstructor.GetParameters();

            // генерация массива который будет иметь рандомные параметры чтобы создать новый объект типа t
            object[] parameters = new object[parametersInfo.Length];

            // заполнение массива - присваиваем параметрам конструктора рандомные значения
            for (int i = 0; i < parameters.Length; i++)
            {
                parameters[i] = this.Create(parametersInfo[i].ParameterType);
            }

            return parameters;
        }

        //поиск конструктора с максимальным числом параметров
        private ConstructorInfo FindAConstructorWithMaxParametersNumber(Type t)
        {
            // получить все конструкторы класса
            var constructorInfoObjects = t.GetConstructors();

            // поиск конструктора с максимальным числом параметров
            int ctorParametersLength = 0;
            foreach (var constructor in constructorInfoObjects)
            {
                if (constructor.GetParameters().Length > ctorParametersLength)
                {
                    ctorParametersLength = constructor.GetParameters().Length;
                }
            }

            // получение конструктора с максимальным числом параметров
            var maxParamConstructor = constructorInfoObjects.Where(c => c.GetParameters().Length == ctorParametersLength).First();
            
            return maxParamConstructor;
        }

        private object GenerateInstanceWithAStructureVariable(Type t)
        {
            // тоже самое как и у класса
            return GenerateInstanceWithAClassTypeVariable(t);
        }
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
            int intBool = random.Next(0,2);

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
