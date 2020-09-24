using System;
using Microsoft.EntityFrameworkCore;
using MyBeerCellar.API.Data;

namespace MyBeerCellar.API.UnitTests
{
    public abstract class BaseContextUnitTest : BaseUnitTest, IDisposable
    {
        protected BaseContextUnitTest()
        {
            var builder = new DbContextOptionsBuilder<MyBeerCellarContext>()
                .UseInMemoryDatabase("MyBeerCellar");

            Context = new MyBeerCellarContext(builder.Options);
        }

        protected MyBeerCellarContext Context { get; }

        public void Dispose()
        {
            Context?.Dispose();
        }
    }
}