using CarServiceApp.Domain.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.AspNetCore.WebUtilities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Web;

namespace CarServiceApp.Helper
{
    [HtmlTargetElement("pager")]
    public class PagerTagHelper : TagHelper
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public PagerTagHelper(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        [HtmlAttributeName("paged-result")]
        public GridItem PagedResult { get; set; } = null!;

        [HtmlAttributeName("page-handler")]
        public string PageHandler { get; set; } = "Index";

        [HtmlAttributeName("page-top-values")]
        public int[] TopValues { get; set; } = new int[] { 20, 50, 100, 200 };

        [HtmlAttributeName("css-class")]
        public string CssClass { get; set; } = "pagination";

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            var listBuilder = new StringBuilder();

            int totalPages = PagedResult.TotalPages;

            var pagingIndexes = GetPagingIndexes(
                PagedResult.CurrentPage - 1,
                totalPages);

            if (totalPages > 1)
            {
                listBuilder.AppendLine("<form class=\"form-inline\" method=\"get\">");
                listBuilder.AppendLine($"<div class=\"input-group mr-sm-2\">");
                listBuilder.AppendLine($"<ul class=\"{CssClass}\">");

                for (int i = 0; i < pagingIndexes.Length; i++)
                {
                    if (i > 0 && pagingIndexes[i - 1] != pagingIndexes[i] - 1)
                    {
                        listBuilder.AppendLine($"<li class=\"page-item disabled\"><a href=\"#\" class=\"page-link\">&hellip;</a></li>");
                    }

                    if (PagedResult.CurrentPage == pagingIndexes[i] + 1)
                    {
                        listBuilder.AppendLine($"<li class=\"page-item active\"><span style=\"background-color:#000000;border-color:#000000;\" class=\"page-link\">{pagingIndexes[i] + 1}</span></li>");
                    }
                    else
                    {
                        var queryParams = new Dictionary<string, string>();
                        queryParams["page"] = (pagingIndexes[i] + 1).ToString();

                        // Получаем параметры из текущего URL
                        var uri = new Uri(_httpContextAccessor.HttpContext.Request.GetDisplayUrl());
                        var query = QueryHelpers.ParseQuery(uri.Query);
                        foreach (var queryParam in query)
                        {
                            if (!string.Equals(queryParam.Key, "page", StringComparison.OrdinalIgnoreCase))
                            {
                                queryParams[queryParam.Key] = queryParam.Value;
                            }
                        }

                        // Формируем новый URL с обновленными параметрами
                        var newUrl = QueryHelpers.AddQueryString(uri.GetLeftPart(UriPartial.Path), queryParams);

                        listBuilder.AppendLine($"<li class=\"page-item\"><a href=\"{newUrl}\"  class=\"page-link\">{pagingIndexes[i] + 1}</a></li>");
                    }
                }

                listBuilder.AppendLine("</ul>");
                listBuilder.AppendLine("</div>");

                if (PagedResult.ItemsPerPage >= 20)
                {
                    // Добавляем скрытые поля для остальных параметров запроса
                    var query = _httpContextAccessor.HttpContext.Request.Query;
                    foreach (var item in query)
                    {
                        if (!string.Equals(item.Key, "page", StringComparison.OrdinalIgnoreCase) &&
                            !string.Equals(item.Key, "pageSize", StringComparison.OrdinalIgnoreCase))
                        {
                            foreach (var value in item.Value)
                            {
                                listBuilder.AppendFormat("<input type=\"hidden\" name=\"{0}\" value=\"{1}\" />", item.Key, value);
                            }
                        }
                    }

                    listBuilder.AppendLine("<div class=\"input-group\">");
                    listBuilder.AppendLine("<div class=\"input-group-prepend\">");
                    listBuilder.AppendLine("<div class=\"input-group-text\"><i class=\"fa fa-book text-primary\"></i></div>");
                    listBuilder.AppendLine("</div>");

                    listBuilder.AppendLine("<select class=\"form-control\" name=\"pageSize\" onchange=\"this.form.submit()\">");

                    foreach (var value in TopValues)
                    {
                        listBuilder.AppendFormat("<option value=\"{0}\"{1}>{0}</option>", value, PagedResult.ItemsPerPage == value ? " selected" : string.Empty);
                    }

                    listBuilder.AppendLine("</select>");
                    listBuilder.AppendLine("</div>");
                }

                listBuilder.AppendLine("</form>");
            }

            output.TagName = "div";
            output.Content.SetHtmlContent(listBuilder.ToString());

            output.PreElement.SetHtmlContent("<nav class=\"d-print-none\">");
            output.PostElement.SetHtmlContent("</nav>");
        }

        private static int[] GetPagingIndexes(int currentIndex, int totalPages)
        {
            var result = new HashSet<int>();

            for (int i = 0; i < 2; i++)
            {
                if (i <= totalPages)
                {
                    result.Add(i);
                }
            }

            int current = currentIndex - 2;

            while (current <= currentIndex + 2)
            {
                if (current > 0 && current < totalPages)
                {
                    result.Add(current);
                }

                current++;
            }

            for (int i = totalPages - 2; i < totalPages; i++)
            {
                result.Add(i);
            }

            return result.ToArray();
        }
    }
}
