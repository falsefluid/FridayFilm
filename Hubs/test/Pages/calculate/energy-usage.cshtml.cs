using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace RolsaTechnologies.Pages;

public class EnergyUsageModel : PageModel
{
    private readonly ILogger<EnergyUsageModel> _logger;

    public EnergyUsageModel(ILogger<EnergyUsageModel> logger)
    {
        _logger = logger;
    }

    public void OnGet()
    {
    }
}

