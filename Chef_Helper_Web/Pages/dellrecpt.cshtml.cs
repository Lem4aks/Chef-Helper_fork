using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Chef_Helper_Web.Pages
{
    public class dellrecptModel : PageModel
    {
        private readonly ILogger<dellrecptModel> _logger;

        public dellrecptModel(ILogger<dellrecptModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {
        }
    }
}