using AutoFixture;
using Moq;

namespace MyBeerCellar.API.UnitTests
{
    /// <summary>
    /// Base class for unit tests, provides a MockRepository and a Fixture for use in
    /// unit tests
    /// </summary>
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