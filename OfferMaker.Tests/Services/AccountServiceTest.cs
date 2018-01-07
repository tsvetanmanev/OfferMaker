namespace OfferMaker.Tests.Services
{
    using OfferMaker.Data;
    using OfferMaker.Data.Models;
    using OfferMaker.Services.Implementations;
    using Xunit;
    using FluentAssertions;
    using Microsoft.EntityFrameworkCore;
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    public class AccountServiceTest
    {
        public AccountServiceTest()
        {
            Tests.Initialize();
        }

        [Fact]
        public async Task GetAllAsyncShouldReturnCorrectResult()
        {
            // Arrange
            var db = this.GetDatabase();

            var firstAccount = new Account {Id = 1, Name = "First"};
            var secondAccount = new Account { Id = 2, Name = "Second" };
            var thirdAccount = new Account { Id = 3, Name = "Third" };

            db.AddRange(firstAccount, secondAccount, thirdAccount);

            await db.SaveChangesAsync();

            var accountService = new AccountService(db);

            // Act
            var result = await accountService.GetAllAsync();

            // Assert
            result
                .Should()
                .HaveCount(3);
        }

        [Fact]
        public async Task CreateAsyncShouldSaveCorrectData()
        {
            // Arrange
            var db = this.GetDatabase();

            const string managerId = "TestManagerId";
            const string accountName = "First Account";

            var manager = new User
            {
                Id = managerId
            };

            db.Add(manager);
            await db.SaveChangesAsync();

            var accountService = new AccountService(db);

            // Act
            await accountService.CreateAsync(
                    accountName,
                    "Vitosha street 21, Sofia",
                    "Description first account",
                    managerId);

            var savedAccount = db.Find<Account>(1);

            // Assert
            savedAccount
                .Should()
                .NotBeNull();

            savedAccount.ManagerId
                .Should()
                .Be(managerId);

            savedAccount.Name
                .Should()
                .Be(accountName);
        }


        private OfferMakerDbContext GetDatabase()
        {
            var dbOptions = new DbContextOptionsBuilder<OfferMakerDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            return new OfferMakerDbContext(dbOptions);
        }

    }
}
