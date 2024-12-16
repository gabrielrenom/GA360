namespace GA360.Domain.Core.Models;

public class CustomerBatchModel
{
    public List<CustomerBatchItemModel> Customers { get; set; }
}

public class CustomerBatchItemModel
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
