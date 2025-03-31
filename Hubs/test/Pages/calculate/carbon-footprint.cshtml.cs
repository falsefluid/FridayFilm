using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace RolsaTechnologies.Pages;

public class CalculateCarbonModel : PageModel
{
    [BindProperty]
    public int NumberOfPeople { get; set; }

    [BindProperty]
    public required string TypeOfHousing { get; set; }

    [BindProperty]
    public required string HouseSize { get; set; }

    [BindProperty]
    public required string HeatingMethod { get; set; }

    // Transport Section
    [BindProperty]
    public required string UsesPublicTransport { get; set; }

    [BindProperty]
    public required string OwnsCar { get; set; }

    [BindProperty]
    public required string DrivingFrequency { get; set; }

    // Lifestyle Section
    [BindProperty]
    public required string ClothingPurchaseFrequency { get; set; }

    [BindProperty]
    public required string RecyclesRegularly { get; set; }

    [BindProperty]
    public required string PurchasesSustainableProducts { get; set; }
    public double CarbonFootprint { get; set; }

    private double GetHouseholdFootprint()
    {
        double footprint = 0;

        // Number of people in the household 
        footprint += NumberOfPeople * 100;

        // Housing type multiplier
        if (TypeOfHousing == "Apartment")
            footprint += 500; // Apartment carbon footprint multiplier
        else if (TypeOfHousing == "Detached House")
            footprint += 1000; // Detached house multiplier
        else if (TypeOfHousing == "Townhouse")
            footprint += 700; // Townhouse multiplier
        else if (TypeOfHousing == "Mobile Home")
            footprint += 300; // Mobile home multiplier

        // House size multiplier (assuming average size-based increase)
        if (HouseSize == "Small (Under 1000 sq ft)")
            footprint += 300;
        else if (HouseSize == "Medium (1000 - 2500 sq ft)")
            footprint += 600;
        else if (HouseSize == "Large (2500+ sq ft)")
            footprint += 1000;

        // Heating method multiplier
        if (HeatingMethod == "Natural Gas")
            footprint += 500; // Average CO2 for natural gas heating
        else if (HeatingMethod == "Electricity")
            footprint += 300; // Average CO2 for electric heating
        else if (HeatingMethod == "Oil")
            footprint += 800; // Oil heating multiplier
        else if (HeatingMethod == "Wood")
            footprint += 400; // Wood heating multiplier
        else if (HeatingMethod == "Solar")
            footprint += 50; // Solar heating multiplier (lower impact)

        return footprint;
    }

    private double GetTransportFootprint()
    {
        double footprint = 0;

        // Public transport usage multiplier
        if (UsesPublicTransport == "Yes")
            footprint += 100; // Lower footprint for public transport users
        else
            footprint += 500; // Higher footprint for non-users (owning cars)

        // Car ownership multiplier
        if (OwnsCar == "Yes")
        {
            if (DrivingFrequency == "Daily")
                footprint += 1500; // High carbon footprint for daily drivers
            else if (DrivingFrequency == "Weekly")
                footprint += 700; // Moderate footprint for weekly drivers
            else if (DrivingFrequency == "Monthly")
                footprint += 300; // Low footprint for monthly drivers
            else if (DrivingFrequency == "Rarely")
                footprint += 100; // Very low footprint for rarely driving cars
        }

        return footprint;
    }

    private double GetLifestyleFootprint()
    {
        double footprint = 0;

        // Clothing purchase multiplier
        if (ClothingPurchaseFrequency == "Frequently")
            footprint += 200; // High impact for frequent clothing purchases
        else if (ClothingPurchaseFrequency == "Occasionally")
            footprint += 100; // Medium impact
        else
            footprint += 50; // Low impact for rare purchases

        // Recycling behavior multiplier
        if (RecyclesRegularly == "Yes")
            footprint -= 100; // Positive impact for regular recycling
        else
            footprint += 200; // Negative impact for no recycling

        // Sustainable product purchase multiplier
        if (PurchasesSustainableProducts == "Yes")
            footprint -= 50; // Positive impact for sustainable product purchases
        else
            footprint += 100; // Negative impact for not purchasing sustainable products

        return footprint;
    }

    public void OnGet()
    {
    }

    public IActionResult OnPost()
    {
        // Calculate the carbon footprint
        double footprint = 0;

        // Household Calculations
        footprint += GetHouseholdFootprint();

        // Transport Calculations
        footprint += GetTransportFootprint();

        // Lifestyle Calculations
        footprint += GetLifestyleFootprint();

        // Round the result to 2 decimal places
        footprint = Math.Round(footprint, 2);
        Console.WriteLine($"Calculated Carbon Footprint: {footprint}");

        return RedirectToPage("/calculate/carbon-results", new { carbonFootprint = footprint });
    }

}
