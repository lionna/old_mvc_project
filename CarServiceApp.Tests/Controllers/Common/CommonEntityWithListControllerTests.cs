using CarServiceApp.Controllers.Common;
using CarServiceApp.Domain.Service.Abstract;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace CarServiceApp.Tests.Controllers.Common
{
    public class CommonEntityWithListControllerTests
    {
        [Fact]
        public async Task Create_Get_ReturnsViewResult_WithEntityAndDropdownList()
        {
            // Arrange
            var mockService = new Mock<IGenericServiceAsync<SomeEntityWithDictionary, int>>();
            var mockDictionaryService = new Mock<ISomeDictionaryService>();
            var controller = new TestCommonEntityWithListController(mockService.Object);

            // Setup mock dictionary service
            var expectedDictionary = new Dictionary<object, string> { { 1, "Option 1" }, { 2, "Option 2" } };
            mockDictionaryService.Setup(s => s.GetDictionaryAsync()).ReturnsAsync(expectedDictionary);

            // Act
            var result = await controller.Create() as ViewResult;

            // Assert
            Assert.NotNull(result);
            var model = result.Model as SomeEntityWithDictionary;
            Assert.NotNull(model);
            Assert.NotNull(model.DropdownList);
            Assert.Equal(expectedDictionary, model.DropdownList);
        }

        [Fact]
        public async Task Edit_Get_WithValidId_ReturnsViewResult_WithEntityAndDropdownList()
        {
            // Arrange
            var entityId = 1;
            var entity = new SomeEntityWithDictionary { Id = entityId };
            var mockService = new Mock<IGenericServiceAsync<SomeEntityWithDictionary, int>>();
            mockService.Setup(s => s.GetByIdAsync(entityId)).ReturnsAsync(entity);
            var mockDictionaryService = new Mock<ISomeDictionaryService>();
            var controller = new TestCommonEntityWithListController(mockService.Object);

            // Setup mock dictionary service
            var dictionary = new Dictionary<object, string> { { 1, "Option 1" }, { 2, "Option 2" } };
            mockDictionaryService.Setup(s => s.GetDictionaryAsync()).ReturnsAsync(dictionary);

            // Act
            var result = await controller.Edit(entityId) as ViewResult;

            // Assert
            Assert.NotNull(result);
            var model = result.Model as SomeEntityWithDictionary;
            Assert.NotNull(model);
            Assert.NotNull(model.DropdownList);
            Assert.Equal(dictionary, model.DropdownList);
        }


        [Fact]
        public async Task Create_Post_WithValidModel_RedirectsToIndex()
        {
            // Arrange
            var mockService = new Mock<IGenericServiceAsync<SomeEntityWithDictionary, int>>();
            var mockDictionaryService = new Mock<ISomeDictionaryService>();
            var controller = new TestCommonEntityWithListController(mockService.Object);
            var entity = new SomeEntityWithDictionary();

            // Act
            var result = await controller.Create(entity) as RedirectToActionResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Index", result.ActionName);
        }

        [Fact]
        public async Task Edit_Post_WithValidModel_RedirectsToIndex()
        {
            // Arrange
            var entityId = 1;
            var entity = new SomeEntityWithDictionary { Id = entityId };
            var mockService = new Mock<IGenericServiceAsync<SomeEntityWithDictionary, int>>();
            var mockDictionaryService = new Mock<ISomeDictionaryService>();
            var controller = new TestCommonEntityWithListController(mockService.Object);

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
            var entity = new SomeEntityWithDictionary { Id = entityId };
            var mockService = new Mock<IGenericServiceAsync<SomeEntityWithDictionary, int>>();
            mockService.Setup(s => s.GetByIdAsync(entityId)).ReturnsAsync(entity);
            var mockDictionaryService = new Mock<ISomeDictionaryService>();
            var controller = new TestCommonEntityWithListController(mockService.Object);

            // Act
            var result = await controller.Detail(entityId) as ViewResult;

            // Assert
            Assert.NotNull(result);
            var model = result.Model as SomeEntityWithDictionary;
            Assert.NotNull(model);
            Assert.Equal(entity, model);
        }

        [Fact]
        public async Task Delete_WithValidId_RedirectsToIndex()
        {
            // Arrange
            var entityId = 1;
            var mockService = new Mock<IGenericServiceAsync<SomeEntityWithDictionary, int>>();
            var mockDictionaryService = new Mock<ISomeDictionaryService>();
            var controller = new TestCommonEntityWithListController(mockService.Object);

            // Act
            var result = await controller.Delete(entityId) as RedirectToActionResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Index", result.ActionName);
        }
    }

    public class TestCommonEntityWithListController : CommonEntityWithListController<SomeEntityWithDictionary, int>
    {
        public TestCommonEntityWithListController(IGenericServiceAsync<SomeEntityWithDictionary, int> service)
            : base(service)
        {
        }

        protected override Task<Dictionary<object, string>> GetDictionaryAsync()
        {
            return Task.FromResult(new Dictionary<object, string> { { 1, "Option 1" }, { 2, "Option 2" } });
        }
    }
}
