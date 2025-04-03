using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace RolsaTechnologies.Pages;

public class CarbonResultsModel : PageModel
{
    [BindProperty]
    public double CarbonFootprint { get; set; }

    [BindProperty]
    public double HouseholdPercentage { get; set; }

    [BindProperty]
    public double TransportPercentage { get; set; }

    [BindProperty]
    public double LifestylePercentage { get; set; }

    public void OnGet(double carbonFootprint, double household, double transport, double lifestyle)
    {
        CarbonFootprint = carbonFootprint;

        // Calculate percentages
        HouseholdPercentage = Math.Round((household / carbonFootprint) * 100, 2);
        TransportPercentage = Math.Round((transport / carbonFootprint) * 100, 2);
        LifestylePercentage = Math.Round((lifestyle / carbonFootprint) * 100, 2);
    }
}