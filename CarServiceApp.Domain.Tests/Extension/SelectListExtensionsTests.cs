using Microsoft.AspNetCore.Mvc.Rendering;
using CarServiceApp.Domain.Extension;
using System.Linq;
using Xunit;
using System.Collections.Generic;

namespace CarServiceApp.Domain.Tests.Extension
{
    public class SelectListExtensionsTests
    {
        [Fact]
        public void ToSelectList_IntDictionary_ReturnsCorrectSelectList()
        {
            // Arrange
            var dictionary = new Dictionary<int, int>
            {
                { 1, 100 },
                { 2, 200 },
                { 3, 300 }
            };

            // Act
            var selectList = dictionary.ToSelectList("Value", "Text") as SelectList;
            var items = selectList.Select(item => item.Value);

            // Assert
            Assert.NotNull(selectList);
            Assert.NotNull(items);
            Assert.Equal(new[] { "1", "2", "3" }, items);
        }

        [Fact]
        public void ToSelectList_StringDictionary_ReturnsCorrectSelectList()
        {
            // Arrange
            var dictionary = new Dictionary<int, string>
            {
                { 1, "Value1" },
                { 2, "Value2" },
                { 3, "Value3" }
            };

            // Act
            var selectList = dictionary.ToSelectList("Value", "Text") as SelectList;
            var items = selectList.Select(item => item.Text);

            // Assert
            Assert.NotNull(selectList);
            Assert.NotNull(items);
            Assert.Equal(new[] { "Value1", "Value2", "Value3" }, items);
        }

        [Fact]
        public void ToSelectList_ObjectDictionary_ReturnsCorrectSelectList()
        {
            // Arrange
            var dictionary = new Dictionary<object, string>
            {
                { "A", "Value1" },
                { 2, "Value2" },
                { 3.5, "Value3" }
            };

            // Act
            var selectList = dictionary.ToSelectList("Value", "Text") as SelectList;
            var items = selectList.Select(item => item.Text);

            // Assert
            Assert.NotNull(selectList);
            Assert.NotNull(items);
            Assert.Equal(new[] { "Value1", "Value2", "Value3" }, items);
        }
    }
}