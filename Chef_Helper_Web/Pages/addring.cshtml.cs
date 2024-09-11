using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Chef_Helper_Web.Pages
{
    public class addingModel : PageModel
    {
        private readonly ILogger<addingModel> _logger;

        public addingModel(ILogger<addingModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {
        }
    }
}