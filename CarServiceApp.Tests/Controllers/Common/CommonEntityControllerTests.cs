using CarServiceApp.Controllers.Common;
using CarServiceApp.Domain.Service.Abstract;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace CarServiceApp.Tests.Controllers.Common
{
    public class CommonEntityControllerTests
    {
        [Fact]
        public async Task Create_Get_ReturnsViewResult()
        {
            // Arrange
            var mockService = new Mock<IGenericServiceAsync<SomeTestEntity, int>>();
            var controller = new CommonEntityController<SomeTestEntity, int>(mockService.Object);

            // Act
            var result = await controller.Create() as ViewResult;

            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public async Task Create_Post_WithValidModel_ReturnsRedirectToActionResult()
        {
            // Arrange
            var mockService = new Mock<IGenericServiceAsync<SomeTestEntity, int>>();
            var controller = new CommonEntityController<SomeTestEntity, int>(mockService.Object);
            var entity = new SomeTestEntity();

            // Act
            var result = await controller.Create(entity) as RedirectToActionResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Index", result.ActionName);
        }

        [Fact]
        public async Task Edit_Get_WithValidId_ReturnsViewResult_WithEntity()
        {
            // Arrange
            var entityId = 1;
            var entity = new SomeTestEntity { Id = entityId };
            var mockService = new Mock<IGenericServiceAsync<SomeTestEntity, int>>();
            mockService.Setup(s => s.GetByIdAsync(entityId)).ReturnsAsync(entity);
            var controller = new CommonEntityController<SomeTestEntity, int>(mockService.Object);

            // Act
            var result = await controller.Edit(entityId) as ViewResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(entity, result.Model);
        }

        [Fact]
        public async Task Edit_Post_WithValidModel_ReturnsRedirectToActionResult()
        {
            // Arrange
            var entityId = 1;
            var entity = new SomeTestEntity { Id = entityId };
            var mockService = new Mock<IGenericServiceAsync<SomeTestEntity, int>>();
            var controller = new CommonEntityController<SomeTestEntity, int>(mockService.Object);

            // Act
            var result = await controller.Edit(entityId, entity) as RedirectToActionResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Index", result.ActionName);
        }

        [Fact]
        public async Task Detail_Get_WithValidId_ReturnsViewResult_WithEntity()
        {
            // Arrange
            var entityId = 1;
            var entity = new SomeTestEntity { Id = entityId };
            var mockService = new Mock<IGenericServiceAsync<SomeTestEntity, int>>();
            mockService.Setup(s => s.GetByIdAsync(entityId)).ReturnsAsync(entity);
            var controller = new CommonEntityController<SomeTestEntity, int>(mockService.Object);

            // Act
            var result = await controller.Detail(entityId) as ViewResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(entity, result.Model);
        }

        [Fact]
        public async Task Delete_WithValidId_ReturnsRedirectToActionResult()
        {
            // Arrange
            var entityId = 1;
            var mockService = new Mock<IGenericServiceAsync<SomeTestEntity, int>>();
            var controller = new CommonEntityController<SomeTestEntity, int>(mockService.Object);

            // Act
            var result = await controller.Delete(entityId) as RedirectToActionResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Index", result.ActionName);
        }
    }
}