using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace RolsaTechnologies.Pages;

public class CalculateCarbonModel : PageModel {
    // Household Section
    [BindProperty]
    public int NumberOfPeople {get;set;}
    [BindProperty]
    public required string TypeOfHousing {get;set;}
    [BindProperty]
    public required string HouseSize {get;set;}
    [BindProperty]
    public required string HeatingMethod {get;set;}

    // Transport Section
    [BindProperty]
    public required string UsesPublicTransport {get;set;}
    [BindProperty]
    public required string OwnsCar {get;set;}
    [BindProperty]
    public required string DrivingFrequency {get;set;}
    
    // Lifestyle Section
    [BindProperty]
    public required string ClothingPurchaseFrequency {get;set;}
    [BindProperty]
    public required string RecyclesRegularly {get;set;}
    [BindProperty]
    public required string PurchasesSustainableProducts{get;set;}

    public double CarbonFootprint {get;set;}

    private double GetHouseholdFootprint()
    {
        // All calculation will be placeholders - Change if you need
        double footprint = 0;
        
        // Number of people in the household
        footprint += NumberOfPeople * 100;

        // House type
        if (TypeOfHousing == "Apartment")
            footprint += 500;
        else if (TypeOfHousing == "Detached House")
            footprint += 1000;
        else if (TypeOfHousing == "Semi-Detached")
            footprint += 800;
        else if (TypeOfHousing == "Terrace")
            footprint += 600;

        // House size
        if (HouseSize == "Small")
            footprint += 300;
        else if (HouseSize == "Medium")
            footprint += 600;
        else if (HouseSize == "Large")
            footprint += 1000;

        // Heating method
        if (HeatingMethod == "Gas")
            footprint += 500;
        else if (HeatingMethod == "Oil")
            footprint += 800;
        else if (HeatingMethod == "Electricity")
            footprint += 200;
        else if (HeatingMethod == "Wood")
            footprint += 400;
        else if (HeatingMethod == "Heat Pump")
            footprint += 300;

        return footprint;
    }

    private double GetTransportFootprint()
    {
        double footprint = 0;

        // Public Transport
        if (UsesPublicTransport == "Yes")
            footprint += 100;
        
        // Car Ownership
        if (OwnsCar == "Yes")
        {
            if (DrivingFrequency == "Daily")
                footprint += 1500;
            else if (DrivingFrequency == "Weekly")
                footprint += 300;
            else if (DrivingFrequency == "Monthly")
                footprint += 300;
            else if (DrivingFrequency == "Rarely")
                footprint += 100;
        }

        return footprint;
    }

    private double GetLifestyleFootprint()
    {
        double footprint = 0;

        if (ClothingPurchaseFrequency == "Frequently")
            footprint += 200;
        else if (ClothingPurchaseFrequency == "Occasionally")
            footprint += 100;
        else
            footprint += 50;
        
        if (RecyclesRegularly == "Yes")
            footprint -= 100;
        else
            footprint += 200;
        
        if (PurchasesSustainableProducts == "Yes")
            footprint -= 50;
        else
            footprint += 100;
        
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

        return RedirectToPage("/calculate/carbon-results", new { carbonFootprint = footprint });
    }
}