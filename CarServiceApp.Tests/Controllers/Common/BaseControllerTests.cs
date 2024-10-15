using CarServiceApp.Controllers.Common;
using CarServiceApp.Domain.Common;
using CarServiceApp.Domain.Entity.Abstract;
using CarServiceApp.Domain.Service.Abstract;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Linq.Expressions;

namespace CarServiceApp.Tests.Controllers.Common
{
    public class BaseControllerTests
    {
        [Fact]
        public async Task Index_ReturnsViewResult_WithListOfEntities()
        {
            // Arrange
            var mockService = new Mock<IGenericServiceAsync<SomeTestEntity, int>>();
            var controller = new Mock<BaseController<SomeTestEntity, int>>(mockService.Object) { CallBase = true };
            var entities = Enumerable.Range(1, 5).Select(i => new SomeTestEntity()).ToList();
            mockService.Setup(s => s.GetAllAsync(
                It.IsAny<int>(),
                It.IsAny<int>(),
                It.IsAny<Expression<Func<SomeTestEntity, bool>>>(),
                It.IsAny<Func<IQueryable<SomeTestEntity>, IOrderedQueryable<SomeTestEntity>>>(),
                It.IsAny<Expression<Func<SomeTestEntity, object>>[]>()
            )).ReturnsAsync(new GridItem { Data = entities }); // Adjust the return type as per your implementation

            var searchString = "";

            // Act
            var result = await controller.Object.Index(1, 5, "", searchString) as ViewResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(entities, (result.Model as GridItem)?.Data);
        }


        [Fact]
        public async Task Create_WithValidModel_ReturnsRedirectToActionResult()
        {
            // Arrange
            var mockService = new Mock<IGenericServiceAsync<SomeTestEntity, int>>();
            var controller = new Mock<BaseController<SomeTestEntity, int>>(mockService.Object) { CallBase = true };
            var entity = new SomeTestEntity();

            // Act
            var result = await controller.Object.Create(entity) as RedirectToActionResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Index", result.ActionName);
        }

        [Fact]
        public async Task Edit_WithValidId_ReturnsViewResult_WithEntity()
        {
            // Arrange
            var mockService = new Mock<IGenericServiceAsync<SomeTestEntity, int>>();
            var controller = new Mock<BaseController<SomeTestEntity, int>>(mockService.Object) { CallBase = true };
            var entityId = 1;
            var entity = new SomeTestEntity { Id = entityId };
            mockService.Setup(s => s.GetByIdAsync(entityId)).ReturnsAsync(entity);

            // Act
            var result = await controller.Object.Edit(entityId) as ViewResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(entity, result.Model);
        }

        [Fact]
        public async Task Delete_WithValidId_ReturnsRedirectToActionResult()
        {
            // Arrange
            var mockService = new Mock<IGenericServiceAsync<SomeTestEntity, int>>();
            var controller = new Mock<BaseController<SomeTestEntity, int>>(mockService.Object) { CallBase = true };
            var entityId = 1;

            // Act
            var result = await controller.Object.Delete(entityId) as RedirectToActionResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Index", result.ActionName);
        }
    }   
}
