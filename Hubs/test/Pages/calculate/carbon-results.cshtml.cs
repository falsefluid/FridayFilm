using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace RolsaTechnologies.Pages;

public class CarbonResultsModel : PageModel
{
    [BindProperty]
    public double CarbonFootprint { get; set; } 
    
    public void OnGet(double carbonFootprint)
    {
        CarbonFootprint = carbonFootprint; 
    }
}

