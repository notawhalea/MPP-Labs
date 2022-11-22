using MainApp.TestClasses;
using MainApp.TestStructures;

namespace Faker.Tests
{
    public class Tests
    {
        private readonly FakerGenerator _systemUnderTest;

        public Tests()
        {
            _systemUnderTest = new FakerGenerator();
        }


        [Fact]
        public void GenerateRandomBoolean_UsingTheFakerGenerator_ReturnNewObjectWithRandomFields()
        {
            var randomBoolVariable = _systemUnderTest.Create<bool>();

            Assert.IsType<bool>(randomBoolVariable);
        }

        [Fact]
        public void GenerateRandomByte_UsingTheFakerGenerator_ReturnNewObjectWithRandomFields()
        {
            var randomByteVariable = _systemUnderTest.Create<byte>();

            Assert.IsType<byte>(randomByteVariable);
        }

        [Fact]
        public void GenerateRandomChar_UsingTheFakerGenerator_ReturnNewObjectWithRandomFields()
        {
            var randomCharVariable = _systemUnderTest.Create<char>();

            Assert.IsType<char>(randomCharVariable);
        }

        [Fact]
        public void GenerateRandomShort_UsingTheFakerGenerator_ReturnNewObjectWithRandomFields()
        {
            var randomShortVariable = _systemUnderTest.Create<short>();

            Assert.IsType<short>(randomShortVariable);
        }

        [Fact]
        public void GenerateRandomInteger_UsingTheFakerGenerator_ReturnNewObjectWithRandomFields()
        {
            var randomIntegerVariable = _systemUnderTest.Create<int>();

            Assert.IsType<int>(randomIntegerVariable);
        }

        [Fact]
        public void GenerateRandomLong_UsingTheFakerGenerator_ReturnNewObjectWithRandomFields()
        {
            var randomLongVariable = _systemUnderTest.Create<long>();

            Assert.IsType<long>(randomLongVariable);
        }

        [Fact]
        public void GenerateRandomFloat_UsingTheFakerGenerator_ReturnNewObjectWithRandomFields()
        {
            var randomFloatVariable = _systemUnderTest.Create<float>();

            Assert.IsType<float>(randomFloatVariable);
        }

        [Fact]
        public void GenerateRandomDecimal_UsingTheFakerGenerator_ReturnNewObjectWithRandomFields()
        {
            var randomDecimalVariable = _systemUnderTest.Create<decimal>();

            Assert.IsType<decimal>(randomDecimalVariable);
        }

        [Fact]
        public void GenerateRandomDouble_UsingTheFakerGenerator_ReturnNewObjectWithRandomFields()
        {
            var randomDoubleVariable = _systemUnderTest.Create<double>();

            Assert.IsType<double>(randomDoubleVariable);
        }

        [Fact]
        public void GenerateRandomStructure_UsingTheFakerGenerator_ReturnNewStructureWithRandomFields()
        {
            var randomStructureVariable = _systemUnderTest.Create<BrokenHeartClub>();

            Assert.IsType<BrokenHeartClub>(randomStructureVariable);
        }

        [Fact]
        public void CheckForTheCyclicDependency_UsingTheFakerGenerator_ReturnException()
        {
            Assert.Throws<Exception>(() => _systemUnderTest.Create<A>());

        }
    }
}