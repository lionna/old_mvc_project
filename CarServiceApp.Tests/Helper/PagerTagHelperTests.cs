using CarServiceApp.Domain.Common;
using CarServiceApp.Helper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Moq;

namespace CarServiceApp.Tests.Helper
{
    public class PagerTagHelperTests
    {
        [Fact]
        public void Process_PagedResultWithMultiplePages_GeneratesCorrectHtml()
        {
            // Arrange
            var pagedResult = new GridItem { CurrentPage = 1, TotalItems = 5, ItemsPerPage = 1, Data = new List<string> { "111", "222", "333", "444", "555" } };

            // Mock IHttpContextAccessor
            var httpContextAccessorMock = new Mock<IHttpContextAccessor>();
            var httpRequestMock = new Mock<HttpRequest>();
            httpRequestMock.Setup(req => req.Scheme).Returns("http");
            httpRequestMock.Setup(req => req.Host).Returns(new HostString("localhost"));
            httpRequestMock.Setup(req => req.Path).Returns(new PathString("/"));
            httpRequestMock.Setup(req => req.QueryString).Returns(QueryString.Empty);
            httpContextAccessorMock.Setup(x => x.HttpContext.Request).Returns(httpRequestMock.Object);

            var tagHelper = new PagerTagHelper(httpContextAccessorMock.Object);
            tagHelper.PagedResult = pagedResult;

            var tagHelperContext = new TagHelperContext(
                new TagHelperAttributeList(),
                new Dictionary<object, object>(),
                Guid.NewGuid().ToString("N"));
            var output = new TagHelperOutput(
                "pager",
                new TagHelperAttributeList(),
                (useCachedResult, encoder) => Task.FromResult<TagHelperContent>(new DefaultTagHelperContent()));

            // Act
            tagHelper.Process(tagHelperContext, output);

            // Log actual output
            var outputContent = output.Content.GetContent();
            Console.WriteLine("Actual Output:");
            Console.WriteLine(outputContent);

            // Assert
            Assert.Contains("<li class=\"page-item active\"><span style=\"background-color:#000000;border-color:#000000;\" class=\"page-link\">1</span></li>", outputContent);
            Assert.Contains("<li class=\"page-item\"><a href=\"http://localhost/?page=2\"  class=\"page-link\">2</a></li>", outputContent);
            Assert.Contains("<li class=\"page-item\"><a href=\"http://localhost/?page=3\"  class=\"page-link\">3</a></li>", outputContent);
            Assert.Contains("<li class=\"page-item\"><a href=\"http://localhost/?page=4\"  class=\"page-link\">4</a></li>", outputContent);
            Assert.Contains("<li class=\"page-item\"><a href=\"http://localhost/?page=5\"  class=\"page-link\">5</a></li>", outputContent);
        }


        [Fact]
        public void Process_PagedResultWithSinglePage_GeneratesEmptyHtml()
        {
            // Arrange
            var pagedResult = new GridItem { CurrentPage = 1, TotalItems = 1 };
            var httpContextAccessor = GetHttpContextAccessorMock();
            var tagHelper = new PagerTagHelper(httpContextAccessor.Object);
            tagHelper.PagedResult = pagedResult;

            var tagHelperContext = new TagHelperContext(
                new TagHelperAttributeList(),
                new Dictionary<object, object>(),
                Guid.NewGuid().ToString("N"));
            var output = new TagHelperOutput(
                "pager",
                new TagHelperAttributeList(),
                (useCachedResult, encoder) => Task.FromResult<TagHelperContent>(new DefaultTagHelperContent()));

            // Act
            tagHelper.Process(tagHelperContext, output);

            // Assert
            var outputContent = output.Content.GetContent();
            Assert.Equal(string.Empty, outputContent);
        }

        private Mock<IHttpContextAccessor> GetHttpContextAccessorMock()
        {
            var httpContextAccessor = new Mock<IHttpContextAccessor>();
            var httpContext = new DefaultHttpContext();
            httpContext.Request.QueryString = new QueryString("?page=1");
            httpContextAccessor.Setup(x => x.HttpContext).Returns(httpContext);
            return httpContextAccessor;
        }
    }
}