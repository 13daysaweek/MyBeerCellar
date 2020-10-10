using AutoFixture;
using Moq;

namespace MyBeerCellar.API.UnitTests
{
    public abstract class BaseUnitTest
    {
        protected BaseUnitTest()
        {
            TestMockRepository = new MockRepository(MockBehavior.Strict);
            TestFixture = new Fixture();
        }

        protected MockRepository TestMockRepository { get; }

        protected Fixture TestFixture { get; }
    }
}