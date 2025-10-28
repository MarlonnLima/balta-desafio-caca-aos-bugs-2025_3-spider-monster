using System;
using BugStore.Api.Data;
using Microsoft.EntityFrameworkCore;

namespace BugStore.Test
{
    public static class TestingDb
    {
        public static AppDbContext NewInMemory()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .EnableSensitiveDataLogging()
                .Options;
            return new AppDbContext(options);
        }
    }
}
