using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Chef_Helper_Web.Pages
{
    public class warehousepage : PageModel
    {
        private readonly ILogger<warehousepage> _logger;

        public warehousepage(ILogger<warehousepage> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {
        }
    }
}