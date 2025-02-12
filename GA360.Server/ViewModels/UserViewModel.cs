using GA360.DAL.Entities.Entities;
using GA360.Domain.Core.Models;

namespace GA360.Server.ViewModels
{
    public class UserViewModel
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
        public int Orders { get; set; }
        public int Progress { get; set; }
        public int Status { get; set; }
        public List<string> Skills { get; set; }
        public string Time { get; set; }
        public string Date { get; set; }
        public int Avatar { get; set; }
        public int CountryId { get; set; }
        public string Portfolio {  get; set; }
        public string DOB { get; set; }
        public string Street { get; set; }
        public string City { get; set; }
        public string Number { get; set; }
        public string Postcode { get; set; }
        public string? AvatarImage { get; set; }
        public string DateOfBirth { get; set; }
        public string Ethnicity { get; set; }
        public string Disability { get; set; }
        public string EmployeeStatus { get; set; }
        public string Employer { get; set; }
        public int TrainingCentre { get; set; }
        public string? AppointmentDate { get; set; }
        public string? AppointmentTime { get; set; }
        public string NationalInsurance { get; set; }
        public List<FileModel>? FileDocuments { get; internal set; }
    }

    public class BasicUserViewModel
    {
        public int Id { get; set; }
        public string Email { get; set; }
    }

    public static class UserMapper
    {
        public static BasicUserViewModel ToBasicViewModel(this Customer customer)
        {
            return new()
            {
                Id = customer.Id,
                Email = customer.Email
            };
        }
    }

    public static class CustomerExtensions
    {
        public static UserViewModel ToUserViewModel(this Customer customer)
        {
            return new UserViewModel
            {
                Id = customer.Id,
                FirstName = customer.FirstName,
                LastName = customer.LastName,
                Name = $"{customer.FirstName} {customer.LastName}",
                Age = 11,
                Contact = customer.Contact,
                Email = customer.Email,
                Country = customer.Country.Name,
                Location = customer.Location,
                Status = (int)customer.Status,
                Time = DateTime.Now.ToString("HH:mm:ss"), // Assuming current time
                Date = DateTime.Now.ToString("yyyy-MM-dd"), // Assuming current date
                Avatar = 0, // Assuming Avatar is not mapped from Customer
                CountryId = customer.CountryId,
                DOB = customer.DOB,
                Street = customer.Address?.Street,
                City = customer.Address?.City,
                Number = customer.Address?.Number,
                Postcode = customer.Address?.Postcode,
                //AvatarImage = customer.AvatarImage,
                DateOfBirth = customer.DOB,
                TrainingCentre = customer.TrainingCentreId ?? 0,               
            };
        }

        private static int CalculateAge(string dob)
        {
            if (DateTime.TryParse(dob, out DateTime birthDate))
            {
                var today = DateTime.Today;
                var age = today.Year - birthDate.Year;
                if (birthDate.Date > today.AddYears(-age)) age--;
                return age;
            }
            return 0;
        }
    }

}
