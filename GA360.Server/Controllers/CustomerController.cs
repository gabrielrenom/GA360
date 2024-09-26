﻿using GA360.DAL.Entities.Entities;
using GA360.Domain.Core.Interfaces;
using GA360.Domain.Core.Models;
using GA360.Server.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace GA360.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CustomerController : ControllerBase
    {
        private readonly ILogger<CustomerController> _logger;
        private readonly ICustomerService _customerService;
        public CustomerController(ILogger<CustomerController> logger, ICustomerService customerService)
        {
            _logger = logger;
            _customerService = customerService;
        }

        [AllowAnonymous]
        [HttpGet("list")]
        public async Task<IActionResult> GetAllContacts()
        {
            var result = await _customerService.GetAllCustomersWithEntities(null, null, c => c.Email, true);

            return Ok(result == null ? new List<UserViewModel>() : result.Select(x => FromUserModelToViewModel(x)).ToList());
        }

        [AllowAnonymous]
        [HttpPost("create")]
        public async Task<IActionResult> AddContact([FromBody] UserViewModel contact)
        {
            var result = await _customerService.AddCustomer(FromUserViewModelToCustomerModel(contact));
            return Ok(result);
        }

        //[AllowAnonymous]
        //[HttpPut("update/{id}")]
        //public async Task<IActionResult> UpdateCustomer([FromBody] UserViewModel contact, int id)
        //{
        //    var result = await _customerService.UpdateCustomer(id, FromUserViewModelToCustomerModel(contact));
        //    return Ok(result);
        //}
        
        [AllowAnonymous]
        [HttpPut("updatewithdocuments/{id}")]
        public async Task<IActionResult> UpdateCustomerWithDocuments([FromForm] CustomerUploadViewModel contact, int id)
        {
            // Process the JSON payload
            var customer = JsonSerializer.Deserialize<UserViewModel>(contact.Customer);

            // Process the uploaded files
            foreach (var file in contact.Files)
            {
                if (file.Length > 0)
                {
                    var filePath = Path.Combine("uploads", file.FileName);
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                    }
                }
            }
            return Ok();
            //var result = await _customerService.UpdateCustomer(id, FromUserViewModelToCustomerModel(contact));
            //return Ok(result);
        }

        [AllowAnonymous]
        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteContact(int id)
        {
            await _customerService.DeleteCustomer(id);
            return Ok();
        }

        private UserViewModel FromUserModelToViewModel(CustomerModel source)
        {
            var destination = new UserViewModel();

            destination.Id = source.Id;
            destination.FirstName = source.FirstName;
            destination.LastName = source.LastName;
            destination.Name = $"{source.FirstName} {source.LastName}";
            destination.Gender = source.Gender.Replace("Gender.", string.Empty); ;
            destination.Contact = source.Contact;
            destination.Email = source.Email;
            destination.Country = source.Country;
            destination.Location = source.Location;
            destination.FatherName = source.FatherName;
            destination.Role = source.Role;
            destination.About = source.About;
            destination.Status = source.Status;
            destination.CountryId = source.CountryId;
            destination.Portfolio = source.Portfolio;
            destination.DOB = source.DOB;
            destination.Street = source.Street;
            destination.City = source.City;
            destination.Number = source.Number;
            destination.Postcode = source.Postcode;
            destination.AvatarImage = source.AvatarImage;
            destination.DateOfBirth = source.DOB;
            destination.Ethnicity = source.EthnicOrigin;
            destination.Disability = source.Disability;
            destination.EmployeeStatus = source.EmploymentStatus;
            destination.Employer = source.Employer;
            destination.TrainingCentre = source.TrainingCentre ?? 0;
            destination.NationalInsurance = source.NationalInsurance;
            destination.Skills = source.Skills?.ToList();

            // Additional properties that need to be calculated or set
            destination.Age = CalculateAge(source.DOB); // Assuming you have a method to calculate age
            destination.Orders = 0; // Set default or calculate based on your logic
            destination.Progress = 0; // Set default or calculate based on your logic
            destination.Status = source.Status; // Set default or calculate based on your logic
            destination.Time = DateTime.Now.ToString("HH:mm"); // Set current time
            destination.Date = DateTime.Now.ToString("yyyy-MM-dd"); // Set current date
            destination.Avatar = 0; // Set default or calculate based on your logic

            return destination;
        }

        private static int CalculateAge(string dob)
        {
            if (DateTime.TryParse(dob, out DateTime dateOfBirth))
            {
                int age = DateTime.Now.Year - dateOfBirth.Year;
                if (DateTime.Now.DayOfYear < dateOfBirth.DayOfYear)
                    age--;
                return age;
            }
            return 0;
        }

        private CustomerModel FromUserViewModelToCustomerModel(UserViewModel userViewModel)
        {
            return new CustomerModel
            {
                About = userViewModel.About,
                Contact = userViewModel.Contact,
                Email = userViewModel.Email,
                LastName = userViewModel.LastName,
                FirstName = userViewModel.FirstName,
                Location = userViewModel.Location,
                Role = userViewModel.Role,
                Description = userViewModel.About,
                FatherName = userViewModel.FatherName,
                Gender = userViewModel.Gender.Replace("Gender.", string.Empty),
                CountryId = userViewModel.CountryId,
                Country = userViewModel.Country,
                AvatarImage = userViewModel.AvatarImage,
                City = userViewModel.City,
                DateOfBirth = userViewModel.DateOfBirth,
                DOB = userViewModel.DateOfBirth,
                Disability = userViewModel.Disability,
                Employer = userViewModel.Employer,
                EmployeeStatus = userViewModel.EmployeeStatus,
                EmploymentStatus = userViewModel.EmployeeStatus,
                ePortfolio = userViewModel.Portfolio,
                Number = userViewModel.Number,
                Portfolio = userViewModel.Portfolio,
                NationalInsurance = userViewModel.NationalInsurance,
                NI = userViewModel.NationalInsurance,
                Status = userViewModel.Status,
                Skills = userViewModel.Skills.ToArray(),
                Postcode = userViewModel.Postcode,
                Street = userViewModel.Street,
                TrainingCentre = userViewModel.TrainingCentre,
                Ethnicity = userViewModel.Ethnicity
            };
        }

        private Customer FromUserViewModelToCustomer(UserViewModel userViewModel)
        {
            return new Customer
            {
                About = userViewModel.About,
                Contact = userViewModel.Contact,
                Email = userViewModel.Email,
                LastName = userViewModel.LastName,
                FirstName = userViewModel.FirstName,
                Location = userViewModel.Location,
                Role = userViewModel.Role,
                Description = userViewModel.About,
                FatherName = userViewModel.FatherName,
                Gender = userViewModel.Gender,
                CountryId = userViewModel.CountryId,
                Country = new Country
                {
                    Name = userViewModel.Country
                }
            };
        }

        private UserViewModel FromCustomerToUserViewModel(Customer customer)
        {
            return new UserViewModel
            {
                Id = customer.Id,
                About = customer.About,
                Contact = customer.Contact,
                Email = customer.Email,
                LastName = customer.LastName,
                FirstName = customer.FirstName,
                Location = customer.Location,
                Role = customer.Role,
                FatherName = customer.FatherName,
                Gender = customer.Gender,
                CountryId = customer.CountryId,
                Skills = customer.CustomerSkills.Select(x => x.Skill.Name).ToList()
            };
        }
    }
}
