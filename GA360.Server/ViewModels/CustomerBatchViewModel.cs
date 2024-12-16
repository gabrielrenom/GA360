using GA360.Domain.Core.Models;

namespace GA360.Server.ViewModels;

public class CustomerBatchViewModel
{
    public List<CustomerViewModel> Customers { get; set; }
}

public class CustomerViewModel
{
    public int Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Name { get; set; }
    public string Gender { get; set; }
    public int Age { get; set; }
    public string Contact { get; set; }
    public string Email { get; set; }
    public string Country { get; set; }
    public string Location { get; set; }
    public string FatherName { get; set; }
    public string Role { get; set; }
    public string About { get; set; }
    public int Status { get; set; }
    public string Time { get; set; }
    public string Date { get; set; }
    public string CountryName { get; set; }
    public string Portfolio { get; set; }
    public string DOB { get; set; }
    public string Street { get; set; }
    public string City { get; set; }
    public int Number { get; set; }
    public string Postcode { get; set; }
    public string DateOfBirth { get; set; }
    public string Ethnicity { get; set; }
    public string Disability { get; set; }
    public string EmployeeStatus { get; set; }
    public string Employer { get; set; }
    public string TrainingCentre { get; set; }
    public string NationalInsurance { get; set; }
}

public static class CustomerMappingExtensions
{
    public static CustomerBatchModel ToModel(this CustomerBatchViewModel viewModel)
    {
        return new CustomerBatchModel
        {
            Customers = viewModel.Customers.Select(c => c.ToModel()).ToList()
        };
    }

    public static CustomerBatchItemModel ToModel(this CustomerViewModel viewModel)
    {
        return new CustomerBatchItemModel
        {
            Id = viewModel.Id,
            FirstName = viewModel.FirstName,
            LastName = viewModel.LastName,
            Name = viewModel.Name,
            Gender = viewModel.Gender,
            Age = viewModel.Age,
            Contact = viewModel.Contact,
            Email = viewModel.Email,
            Country = viewModel.Country,
            Location = viewModel.Location,
            FatherName = viewModel.FatherName,
            Role = viewModel.Role,
            About = viewModel.About,
            Status = viewModel.Status,
            Time = viewModel.Time,
            Date = viewModel.Date,
            CountryName = viewModel.CountryName,
            Portfolio = viewModel.Portfolio,
            DOB = viewModel.DOB,
            Street = viewModel.Street,
            City = viewModel.City,
            Number = viewModel.Number,
            Postcode = viewModel.Postcode,
            DateOfBirth = viewModel.DateOfBirth,
            Ethnicity = viewModel.Ethnicity,
            Disability = viewModel.Disability,
            EmployeeStatus = viewModel.EmployeeStatus,
            Employer = viewModel.Employer,
            TrainingCentre = viewModel.TrainingCentre,
            NationalInsurance = viewModel.NationalInsurance
        };
    }

    public static CustomerBatchViewModel ToViewModel(this CustomerBatchModel model)
    {
        return new CustomerBatchViewModel
        {
            Customers = model.Customers.Select(c => c.ToViewModel()).ToList()
        };
    }

    public static CustomerViewModel ToViewModel(this CustomerBatchItemModel model)
    {
        return new CustomerViewModel
        {
            Id = model.Id,
            FirstName = model.FirstName,
            LastName = model.LastName,
            Name = model.Name,
            Gender = model.Gender,
            Age = model.Age,
            Contact = model.Contact,
            Email = model.Email,
            Country = model.Country,
            Location = model.Location,
            FatherName = model.FatherName,
            Role = model.Role,
            About = model.About,
            Status = model.Status,
            Time = model.Time,
            Date = model.Date,
            CountryName = model.CountryName,
            Portfolio = model.Portfolio,
            DOB = model.DOB,
            Street = model.Street,
            City = model.City,
            Number = model.Number,
            Postcode = model.Postcode,
            DateOfBirth = model.DateOfBirth,
            Ethnicity = model.Ethnicity,
            Disability = model.Disability,
            EmployeeStatus = model.EmployeeStatus,
            Employer = model.Employer,
            TrainingCentre = model.TrainingCentre,
            NationalInsurance = model.NationalInsurance
        };
    }
}
