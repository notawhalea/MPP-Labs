namespace FormatterConsole.Tests
{
    public class CoreTests
    {
        private static readonly StringFormatter _systemUnderTest;

        static CoreTests()
        {
            _systemUnderTest = new StringFormatter();
        }

        public class User
        {
            public string FirstName { get; }
            public string LastName { get; }
            public int Mark { get; set; }

            public User(string firstName, string lastName)
            {
                FirstName = firstName;
                LastName = lastName;
            }

            public string GetGreeting()
            {
                return _systemUnderTest.Format("Привет! Меня зовут {FirstName}", this);
            }

            public string GetTranslator()
            {
                return _systemUnderTest.Format("{{FirstName}} транслируется в {FirstName}", this);
            }

            public void GetGreetingsWithWrongField()
            {
                _systemUnderTest.Format("Меня зовут {LastName} {FirstName} {FatherName}", this);
            }

            public void GetGreetingsWithWrongNumberOfBrackets()
            {
                _systemUnderTest.Format("Меня зовут {LastName} {FirstName}}", this);
            }


        }

        [Fact]
        public void AskForGreetings_WithValidData_ReturnRightString()
        {
            var user = new User("Любитель", "СПП");

            Assert.Matches("Привет! Меня зовут Любитель", user.GetGreeting());
        }

        [Fact]
        public void AskForTranslator_WithValidData_ReturnRightString()
        {
            var user = new User("Любитель", "СПП");

            Assert.Matches("{{FirstName}} транслируется в Любитель", user.GetTranslator());
        }

        [Fact]
        public void AskForGreetingsWithWrongField_WithValidData_ReturnException()
        {
            var user = new User("Любитель", "СПП");

            Action action = user.GetGreetingsWithWrongField;

            Exception exception = Assert.Throws<Exception>(action);

            Assert.Matches("Field in the string does not exist in the passed object", exception.Message);
        }

        [Fact]
        public void AskForGreetingsWithWrongNumberOfBrackets_WithValidData_ReturnException()
        {
            var user = new User("Любитель", "СПП");

            Action action = user.GetGreetingsWithWrongNumberOfBrackets;

            Exception exception = Assert.Throws<Exception>(action);

            Assert.Matches("Wrong brackets number", exception.Message);
        }


    }
}