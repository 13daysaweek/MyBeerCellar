using System;
using Microsoft.EntityFrameworkCore;
using MyBeerCellar.API.Data;

namespace MyBeerCellar.API.UnitTests
{
    public abstract class BaseContextUnitTest : BaseUnitTest, IDisposable
    {
        protected BaseContextUnitTest()
        {
            // Hopefully using new guid in the db name will eliminate the inconsistent test failures :\
            var builder = new DbContextOptionsBuilder<MyBeerCellarContext>()
                .UseInMemoryDatabase($"MyBeerCellar{Guid.NewGuid()}");

            Context = new MyBeerCellarContext(builder.Options);
        }

        protected MyBeerCellarContext Context { get; }

        public void Dispose()
        {
            Context?.Dispose();
        }
    }
}