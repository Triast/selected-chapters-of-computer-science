using Authentication.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Authentication.Helpers.Tag
{
    public class PaginationTagHelper : TagHelper
    {
        private IUrlHelperFactory urlHelperFactory;

        public PaginationTagHelper(IUrlHelperFactory helperFactory)
        {
            urlHelperFactory = helperFactory;
        }

        [ViewContext]
        [HtmlAttributeNotBound]
        public ViewContext ViewContext { get; set; }
        public PageViewModel PageModel { get; set; }
        public string PageController { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            IUrlHelper urlHelper = urlHelperFactory.GetUrlHelper(ViewContext);
            output.TagName = "div";
            
            string prevTag = "";
            if (PageModel.HasPreviousPage)
            {
                prevTag = $"<a href=\"{PageController}?page={(PageModel.PageNumber - 1).ToString()}\"" +
                    "class=\"btn btn-default\"><i class=\"glyphicon glyphicon-chevron-left\"></i>Назад</a>";
            }

            string nextTag = "";
            if (PageModel.HasNextPage)
            {
                nextTag = $"<a href=\"{PageController}?page={(PageModel.PageNumber + 1).ToString()}\"" +
                    "class=\"btn btn-default\"><i class=\"glyphicon glyphicon-chevron-right\"></i>Вперёд</a>";
            }

            output.Content.SetHtmlContent(prevTag + nextTag);
        }
    }
}
