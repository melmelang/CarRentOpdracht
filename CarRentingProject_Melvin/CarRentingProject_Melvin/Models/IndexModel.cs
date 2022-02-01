using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CarRentingProject_Melvin.Models
{
    public class IndexModel : PageModel
    {
        public IActionResult OnGetPartial()
        {
            return new PartialViewResult
            {
                ViewName = "_HelloWorldPartial",
                ViewData = this.ViewData
            };
        }
    }
}
