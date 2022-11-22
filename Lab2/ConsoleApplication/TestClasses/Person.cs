using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainApp.TestClasses
{
    public class Person
    {
        public Person(string name, string surname, int age, bool isHeartBroken)
        {
            Name = name;
            Surname = surname;
            Age = age;
            IsHeartBroken = isHeartBroken;
        }

        public Person(string name,string surname)
        {
            Surname = surname;
            Name = name;
        }

        private Person(string surname)
        {
            Surname = surname;
        }

        public string Name { get; set; }
        public string Surname { get; set; }
        public int Age { get; set; }
        public bool IsHeartBroken { get; set; }

    }
}

