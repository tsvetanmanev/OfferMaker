namespace OfferMaker.Tests.Web.Controllers
{
    using FluentAssertions;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.ViewFeatures;
    using Moq;
    using OfferMaker.Data.Models;
    using OfferMaker.Services;
    using OfferMaker.Tests.Mocks;
    using OfferMaker.Web;
    using OfferMaker.Web.Controllers;
    using OfferMaker.Web.Models.Account;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Security.Claims;
    using System.Threading.Tasks;
    using Xunit;

    public class AccountsControllerTest
    {
        private const string FirstUserId = "1";
        private const string FirstUserUsername = "First";
        private const string SecondUserId = "2";
        private const string SecondUserUsername = "Second";

        [Fact]
        public void CreateShouldBeOnlyForUsersInAccountManagerRole()
        {
            // Arrange
            var controller = typeof(AccountsController);
            var method = controller.GetMethod(nameof(AccountsController.Create), new Type[] { });

            // Act
            var methodAttribute = method
                .GetCustomAttributes(false)
                .FirstOrDefault(a => a.GetType() == typeof(AuthorizeAttribute))
                as AuthorizeAttribute;

            // Assert
            methodAttribute.Should().NotBeNull();
            methodAttribute.Roles.Should().Be(WebConstants.AccountManagerRole);
        }

        [Fact]
        public void GetCreateShouldReturnViewWithValidModel()
        {
            // Arrange
            var controller = new AccountsController(null, null);

            // Act
            var result = controller.Create();

            // Assert
            result.Should().BeOfType<ViewResult>();
            var model = result.As<ViewResult>().Model;
            model.Should().BeOfType<AccountFormModel>();
        }

        [Fact]
        public async Task PostCreateShouldReturnViewWithCorrectModelWhenModelStateIsInvalid()
        {
            // Arrange
            var controller = new AccountsController(null, null);
            controller.ModelState.AddModelError(string.Empty, "Error");

            // Act
            var result = await controller.Create(new AccountFormModel());

            // Assert
            result.Should().BeOfType<ViewResult>();
            var model = result.As<ViewResult>().Model;
            model.Should().BeOfType<AccountFormModel>();
        }

        [Fact]
        public async Task PostCreateShouldReturnRedirectWithValidModel()
        {
            // Arrange
            const string nameValue = "Name";
            const string addressValue = "Address";
            const string descriptionValue = "Description";

            string modelName = null;
            string modelAddress = null;
            string modelDescription = null;
            string modelManagerId = null;
            string successMessage = null;

            var userManager = GetUserManagerMock();

            var accountService = new Mock<IAccountService>();
            accountService
                .Setup(a => a.CreateAsync(
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>()))
                .Callback((string name, string address, string description, string managerId) =>
                {
                    modelName = name;
                    modelAddress = address;
                    modelDescription = description;
                    modelManagerId = managerId;
                })
                .Returns(Task.FromResult(true));

            var tempData = new Mock<ITempDataDictionary>();
            tempData
                .SetupSet(t => t[WebConstants.TempDataSuccessMessageKey] = It.IsAny<string>())
                .Callback((string key, object message) =>
                {
                    successMessage = message as string;
                });

            var controller = new AccountsController(accountService.Object, userManager.Object);
            controller.TempData = tempData.Object;

            // Act
            var result = await controller.Create(new AccountFormModel
            {
                Name = nameValue,
                Address = addressValue,
                Description = descriptionValue
            });

            // Assert
            modelName.Should().Be(nameValue);
            modelAddress.Should().Be(addressValue);
            modelDescription.Should().Be(descriptionValue);
            modelManagerId.Should().Be(FirstUserId);

            successMessage.Should().Be($"Account {nameValue} created successfully!");

            result.Should().BeOfType<RedirectToActionResult>();
            result.As<RedirectToActionResult>().ActionName.Should().Be(nameof(AccountsController.Index));
        }

        private Mock<UserManager<User>> GetUserManagerMock()
        {
            var userManager = UserManagerMock.New;
            userManager
                .Setup(u => u.GetUsersInRoleAsync(It.IsAny<string>()))
                .ReturnsAsync(new List<User>
                {
                    new User { Id = FirstUserId, UserName = FirstUserUsername },
                    new User { Id = SecondUserId, UserName = SecondUserUsername }
                });

            userManager
                .Setup(u => u.GetUserId(It.IsAny<ClaimsPrincipal>()))
                .Returns(FirstUserId);

            return userManager;
        }
    }
}
