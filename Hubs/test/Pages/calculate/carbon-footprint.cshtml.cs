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
    [BindProperty]
    public required string LightsStandby {get;set;}

    // Transport Section
    [BindProperty]
    public required string OwnsCar { get; set; }
    [BindProperty]
    public string? VehicleType { get; set; } // Nullable since it depends on OwnsCar
    [BindProperty]
    public int? CommuteHoursPerWeek { get; set; } // Nullable since it depends on OwnsCar
    [BindProperty]
    public int TrainHoursPerWeek { get; set; } 
    [BindProperty]
    public int BusHoursPerWeek { get; set; }
    
    // Lifestyle Section
    [BindProperty]
    public required string ClothingPurchaseFrequency {get;set;}
    [BindProperty]
    public required string RecyclesRegularly {get;set;}
    [BindProperty]
    public required string DietType { get; set; }

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

        // Lights on standby
        if (LightsStandby == "Yes")
            footprint -= 50;
        else if (LightsStandby == "No")
            footprint += 100;

        return footprint;
    }

    private double GetTransportFootprint()
    {
        double footprint = 0;

        if (OwnsCar == "Yes" && VehicleType != null && CommuteHoursPerWeek.HasValue)
        {
            // Carbon footprint estimates per hour of commuting (in kg CO2)
            double emissionRate = VehicleType switch
            {
                "Electric" => 0.05, // Electric: 0.05 kg CO2/hour
                "PlugInHybrid" => 0.1, // Plug-in Hybrid: 0.1 kg CO2/hour
                "Hybrid" => 0.15, // Hybrid: 0.15 kg CO2/hour
                "SmallPetrolDiesel" => 0.2, // Small Petrol/Diesel: 0.2 kg CO2/hour
                "MediumPetrolDiesel" => 0.3, // Medium Petrol/Diesel: 0.3 kg CO2/hour
                "LargePetrolDiesel" => 0.4, // Large Petrol/Diesel: 0.4 kg CO2/hour
                _ => 0
            };

            // Calculate footprint based on commute hours
            footprint += emissionRate * CommuteHoursPerWeek.Value * 52; // 52 weeks in a year
        }

        // Train travel
        footprint += TrainHoursPerWeek * 0.04 * 52; // Train: 0.04 kg CO2/hour

        // Bus travel
        footprint += BusHoursPerWeek * 0.06 * 52; // Bus: 0.06 kg CO2/hour

        return footprint;
    }

    private double GetLifestyleFootprint()
    {
        double footprint = 0;

        // Clothing purchase frequency
        if (ClothingPurchaseFrequency == "Frequently")
            footprint += 200;
        else if (ClothingPurchaseFrequency == "Occasionally")
            footprint += 100;
        else
            footprint += 50;

        // Diet type
        if (DietType == "MeatEveryMeal")
            footprint += 1500;
        else if (DietType == "MeatSomeMeals")
            footprint += 1000;
        else if (DietType == "MeatRarely")
            footprint += 500;
        else if (DietType == "NoMeat")
            footprint += 300;
        else if (DietType == "Vegetarian")
            footprint += 200;
        else if (DietType == "Vegan")
            footprint += 100;
        
        // Recycling 
        if (RecyclesRegularly == "Yes")
            footprint -= 100;
        else
            footprint += 200;
        
        return footprint;
    }

    public void OnGet()
    {
    }

    public IActionResult OnPost()
    {
        // Calculate the carbon footprint
        double household = GetHouseholdFootprint();
        double transport = GetTransportFootprint();
        double lifestyle = GetLifestyleFootprint();

        double totalFootprint = household + transport + lifestyle;

        // Round the result to 2 decimal places
        totalFootprint = Math.Round(totalFootprint, 2);

        return RedirectToPage("/calculate/carbon-results", new 
        { 
            carbonFootprint = totalFootprint, 
            household = household, 
            transport = transport, 
            lifestyle = lifestyle 
        });
    }
}