using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Ticket_project.TagHelpers
{
    [HtmlTargetElement("submit-button")]
    public class SubmitButtonTagHelper : TagHelper
    {
        public string Text { get; set; } = "Submit";
        public string Variant { get; set; } = "primary";  
        public bool Small { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "button";
            output.TagMode = TagMode.StartTagAndEndTag;
            output.Attributes.SetAttribute("type", "submit");
            output.Attributes.SetAttribute("class",
                $"btn btn-{Variant}" + (Small ? " btn-sm" : ""));
            output.Content.SetContent(Text);
        }
    }
}
