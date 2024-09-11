using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Chef_Helper_Web.Pages
{
    public class dellingModel : PageModel
    {
        private readonly ILogger<dellingModel> _logger;

        public dellingModel(ILogger<dellingModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {
        }
    }
}