using Microsoft.AspNetCore.Razor.TagHelpers;

namespace FirstWebApp.Customizations.TagHelpers
{
    public class RatingTagHelper : TagHelper
    {
        public double Value { get; set; }
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            //double value = (double) context.AllAttributes["value"].Value; //si può semplificare nel modo che segue;

            for (int i = 1; i <= 5; i++)
            { 
                if(Value >= i )
                {
                    output.Content.AppendHtml("<i class=\"bi bi-star-fill text-warning\"></i>");
                }
                else
                {
                    output.Content.AppendHtml("<i class=\"bi bi-star-fill\"></i>");
                }
            }
           
        }
    }
}
