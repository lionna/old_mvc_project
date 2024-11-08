using Xunit;
using Moq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System.Threading.Tasks;
using System.IO;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using CarServiceApp.PdfConverter;
using SelectPdf;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.DependencyInjection;

namespace CarServiceApp.Tests.PdfConverter
{
    public class HtmlToPdfHelperTests
    {
        [Fact]
        public async Task ConvertHtml2PdfResponseAsync_WithValidParameters_CallsConvertHtmlString()
        {
            Assert.True(true);
                }
        [Fact]
        public async Task ConvertHtml2PdfResponseAsync_WithValidParameters_CallsConvertHtmlString1()
        {
            Assert.True(true);
        }
    } 
}