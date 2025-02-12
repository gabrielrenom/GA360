using GA360.DAL.Entities.BaseEntities;
using GA360.DAL.Entities.Entities;

namespace GA360.Server.ViewModels;

public class TrainingCentreViewModel
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int AddressId { get; set; }
    public string Street { get; set; }
    public string Number { get; set; }
    public string Postcode { get; set; }
    public string City { get; set; }
    public string? Logo { get; set; }
}

public static class TrainingMapper
{
    public static TrainingCentreViewModel ToViewModel(this TrainingCentre trainingCentre)
    {
        return new TrainingCentreViewModel
        {
            Id = trainingCentre.Id,
            Name = trainingCentre.Name,
            AddressId = trainingCentre.AddressId,
            Street = trainingCentre.Address.Street,
            Number = trainingCentre.Address.Number,
            Postcode = trainingCentre.Address.Postcode,
            City = trainingCentre.Address.City,
            Logo = trainingCentre.Logo,
        };
    }

    public static TrainingCentre ToEntity(this TrainingCentreViewModel trainingViewModel)
    {
        return new TrainingCentre
        {
            Id = trainingViewModel.Id,
            Name = trainingViewModel.Name,
            Logo = trainingViewModel.Logo,
            AddressId = trainingViewModel.AddressId,
            Address = new Address
            {
                Street = trainingViewModel.Street,
                Number = trainingViewModel.Number,
                Postcode = trainingViewModel.Postcode,
                City = trainingViewModel.City
            },
            Customers = new List<Customer>() // Initialize with an empty list or map accordingly
        };
    }
}

