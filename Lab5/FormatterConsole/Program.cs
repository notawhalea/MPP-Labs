using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static FormatterConsole.Program;

namespace FormatterConsole
{
    internal class Program
    {
        public static readonly StringFormatter Shared = new StringFormatter();

        static void Main(string[] args)
        {
            var user = new User("Спп", "Любитель");
            var fullName = user.GetGreeting();
        }

        public class User
        {
            public string FirstName { get; }
            public string LastName { get; }

            public User(string firstName, string lastName)
            {
                FirstName = firstName;
                LastName = lastName;
            }

            public string GetGreeting()
            {
                return Shared.Format("{{FirstName}} транслируется в {FirstName}", this);
            }
        }
    }
}
