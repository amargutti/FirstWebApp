using FirstWebApp.Models.ValueTypes;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace FirstWebApp.Customizations.TagHelpers
{
    public class PriceTagHelper : TagHelper
    {
        public Money CurrentPrice { get; set; }
        public Money FullPrice { get; set; }

        public override void Process (TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "div";
            output.Content.AppendHtml($"{CurrentPrice} <br>");

            if(!CurrentPrice.Equals(FullPrice))
            {
                output.Content.AppendHtml($"<s class=\"text-secondary\">{FullPrice}</s>");
            }
        }
    }
}
