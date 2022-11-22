using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using MainApp.TestClasses;
using MainApp.TestStructures;
using Faker;

namespace ConsoleApplication
{
    public class Test
    {

    }

    public class Program
    {
        static void Main(string[] args)
        {
            FakerGenerator faker = new Faker.FakerGenerator();

            var boolVar = faker.Create<bool>();
            var bool2Var = faker.Create<bool>();
            var byteVar = faker.Create<byte>();
            var byte2Var = faker.Create<byte>();
            var byte3Var = faker.Create<byte>();
            var charVar = faker.Create<char>();
            var shortVar = faker.Create<short>();
            var intVar = faker.Create<int>();
            var longVar = faker.Create<long>();
            var floatVar = faker.Create<float>();
            var decimalVar = faker.Create<decimal>();
            var doubleVar = faker.Create<double>();

            Console.WriteLine("Value Types:\n");

            Console.WriteLine($"{nameof(boolVar)} = {boolVar}");
            Console.WriteLine($"{nameof(bool2Var)} = {bool2Var}");
            Console.WriteLine($"{nameof(byteVar)} = {byteVar}");
            Console.WriteLine($"{nameof(byte2Var)} = {byte2Var}");
            Console.WriteLine($"{nameof(byte3Var)} = {byte3Var}");
            Console.WriteLine($"{nameof(charVar)} = {charVar}");
            Console.WriteLine($"{nameof(shortVar)} = {shortVar}");
            Console.WriteLine($"{nameof(intVar)} = {intVar}");
            Console.WriteLine($"{nameof(longVar)} = {longVar}");
            Console.WriteLine($"{nameof(floatVar)} = {floatVar}");
            Console.WriteLine($"{nameof(decimalVar)} = {decimalVar}");
            Console.WriteLine($"{nameof(doubleVar)} = {doubleVar}");

            Console.WriteLine("----------------------------------------------");
            Console.WriteLine("\nReference type object (Single Person):\n");

            var person = faker.Create<Person>();
            if (person != null)
            {
                Console.WriteLine($"Name: {person.Name}");
                Console.WriteLine($"Surname: {person.Surname}");
                Console.WriteLine($"Age: {person.Age}");
                Console.WriteLine($"Have a broken heart?: {person.IsHeartBroken}");
                Console.WriteLine("----------------------------------------------");
            }

            Console.WriteLine("\nList of ints:\n");

            List<int> ints = faker.Create<List<int>>();

            foreach (var intVal in ints)
            {
                Console.WriteLine($"{nameof(intVal)}_{ints.IndexOf(intVal)} = {intVal}");
            }

            Console.WriteLine("----------------------------------------------");
            Console.WriteLine("\nList of Lists of ints:\n");

            List<List<int>> ints2 = faker.Create<List<List<int>>>();

            foreach (var intList in ints2)
            {
                Console.WriteLine($"List_{ints2.IndexOf(intList)}");
                foreach (var intVal in intList)
                {
                    Console.WriteLine($"{nameof(intVal)}_{intList.IndexOf(intVal)} = {intVal}");
                }
                Console.WriteLine();
            }

            Console.WriteLine("----------------------------------------------");
            Console.WriteLine("\nList of Persons:\n");

            List<Person> persons = faker.Create<List<Person>>();

            foreach (var personItem in persons)
            {
                if (personItem != null)
                {
                    Console.WriteLine($"Person_{persons.IndexOf(personItem)}");
                    Console.WriteLine($"Name: {personItem.Name}");
                    Console.WriteLine($"Surname: {personItem.Surname}");
                    Console.WriteLine($"Age: {personItem.Age}");
                    Console.WriteLine($"Have a broken heart?: {personItem.IsHeartBroken}\n");
                }
            }

            Console.WriteLine("----------------------------------------------");
            Console.WriteLine("Structure of CLUB:\n");
            Console.WriteLine("ЭТО КЛУБ РАЗБИТЫХ СЕРДЕЦ");

            var brokenHeartClub = faker.Create<BrokenHeartClub>();
            Console.WriteLine($"NumberOfMembers: {brokenHeartClub.NumberOfMembers}");
            Console.WriteLine($"NumberOfHearts: {brokenHeartClub.NumberOfHearts}");

            Console.WriteLine("----------------------------------------------");
            Console.WriteLine("\nCYCLIC DEPENDENCY CHECKING-_-:\n");

            try
            {
                var a = faker.Create<A>();
            }
            catch (Exception)
            {
                Console.WriteLine("ERROR, WE CATCH CYCLIC(Суслик...Что он тут делает?) DEPENDENCY!");
            }

            Console.ReadLine();
        }
    }
}
