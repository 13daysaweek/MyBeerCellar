using System;
using FluentAssertions;
using Xunit;

namespace MyBeerCellar.API.UnitTests
{
    public class Class1
    {
        [Fact]
        public void Test_True()
        {
            true.Should()
                .BeTrue();
        }
    }
}
