using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Authentication.Helpers.Html
{
    public static class ListHelper
    {
        public static HtmlString CreateList(this IHtmlHelper html, string ulClass, string[] items)
        {
            string result = $"<ul class=\"{ulClass}\">";

            foreach (string item in items)
            {
                result += $"<li>{item}</li>";
            }

            result += "</ul>";

            return new HtmlString(result);
        }
    }
}
