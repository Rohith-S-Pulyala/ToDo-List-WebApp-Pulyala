using Microsoft.AspNetCore.Razor.TagHelpers;

namespace ToDo_List_WebApp_Pulyala.TagHelpers
{
    // This targets <input> elements that have the 'ticket-style' attribute
    [HtmlTargetElement("input", Attributes = "ticket-style")]
    public class SubmitButtonTagHelper : TagHelper
    {
        // maps to the value of ticket-style="..."
        public string TicketStyle { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            // Always ensure it's a submit button
            output.Attributes.SetAttribute("type", "submit");

            if (TicketStyle == "Backlog") {
                output.Attributes.SetAttribute("value", "Add to Backlog");
                // Adds specific styling for the button.
                output.Attributes.SetAttribute("class", "btn btn-success shadow-sm fw-bold");
            }
            else if (TicketStyle == "Standard") {
                output.Attributes.SetAttribute("value", "Submit Ticket");
                // Adds specific styling for the button.
                output.Attributes.SetAttribute("class", "btn btn-primary");
            }
        }
    }
}
