using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace RolsaTechnologies.Pages;

public class ReduceFootprintModel : PageModel
{
    private readonly ILogger<ReduceFootprintModel> _logger;

    public ReduceFootprintModel(ILogger<ReduceFootprintModel> logger)
    {
        _logger = logger;
    }

    public void OnGet()
    {
    }
}

