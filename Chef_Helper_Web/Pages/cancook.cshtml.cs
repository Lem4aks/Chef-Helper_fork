using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Chef_Helper_Web.Pages
{
    public class AvailableDishesModel : PageModel
    {
        private readonly ILogger<AvailableDishesModel> _logger;

        public AvailableDishesModel(ILogger<AvailableDishesModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {
        }
    }
}