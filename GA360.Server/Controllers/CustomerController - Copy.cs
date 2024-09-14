using GA360.DAL.Entities.Entities;
using GA360.Domain.Core.Interfaces;
using GA360.Server.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GA360.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EthnicityController : ControllerBase
    {
        private readonly ILogger<EthnicityController> _logger;
        private readonly ICustomerService _customerService;
        public EthnicityController(ILogger<EthnicityController> logger, ICustomerService customerService)
        {
            _logger = logger;
            _customerService = customerService;
        }

        [AllowAnonymous]
        [HttpGet("list")]
        public async Task<IActionResult> GetAllContacts()
        {
            var result = await _customerService.GetAll();
            return Ok(result == null ? new List<UserViewModel>() : result.Select(x => FromCustomerToUserViewModel(x)).ToList());
            var users = new List<User>
            {
                new User
                {
                    Id = 1,
                    FirstName = "Phoebe",
                    LastName = "Venturi",
                    Name = "Phoebe Venturi",
                    Gender = "Female",
                    Age = 52,
                    Contact = "(887) 744-6950",
                    Email = "ke@gmail.com",
                    Country = "Thailand",
                    Location = "1804 Ahedi Trail, Owottug, Bolivia - 47403",
                    FatherName = "Helen Stewart",
                    Role = "Logistics Manager",
                    About = "Udukaape gozune jig fu foslinan tadka kumu no guw upe or cifdasbej di ige.",
                    OrderStatus = "Delivered",
                    Orders = 7174,
                    Progress = 84,
                    Status = 1,
                    Skills = new List<string> { "React", "Mobile App", "Prototyping", "UX", "Figma" },
                    Time = new List<string> { "1 day ago" },
                    Date = "14.07.2023",
                    Avatar = 8
                },
                new User
                {
                    Id = 2,
                    FirstName = "John",
                    LastName = "Doe",
                    Name = "John Doe",
                    Gender = "Male",
                    Age = 34,
                    Contact = "(123) 456-7890",
                    Email = "john.doe@example.com",
                    Country = "USA",
                    Location = "123 Main St, Springfield, USA - 12345",
                    FatherName = "Robert Doe",
                    Role = "Software Engineer",
                    About = "Passionate about coding and technology.",
                    OrderStatus = "Pending",
                    Orders = 1234,
                    Progress = 75,
                    Status = 1,
                    Skills = new List<string> { "C#", "ASP.NET", "SQL", "Azure", "JavaScript" },
                    Time = new List<string> { "2 days ago" },
                    Date = "01.08.2023",
                    Avatar = 5
                },
                new User
                {
                    Id = 3,
                    FirstName = "Jane",
                    LastName = "Smith",
                    Name = "Jane Smith",
                    Gender = "Female",
                    Age = 28,
                    Contact = "(987) 654-3210",
                    Email = "jane.smith@example.com",
                    Country = "Canada",
                    Location = "456 Elm St, Toronto, Canada - 67890",
                    FatherName = "Michael Smith",
                    Role = "Product Manager",
                    About = "Experienced in managing product lifecycles.",
                    OrderStatus = "Shipped",
                    Orders = 5678,
                    Progress = 90,
                    Status = 1,
                    Skills = new List<string> { "Product Management", "Agile", "Scrum", "JIRA", "Confluence" },
                    Time = new List<string> { "3 days ago" },
                    Date = "15.07.2023",
                    Avatar = 3
                }
            };

            return Ok(users);
        }

        [AllowAnonymous]
        [HttpPost("create")]
        public async Task<IActionResult> AddContact([FromBody] UserViewModel contact)
        {
            var result = await _customerService.AddCustomer(FromUserViewModelToCustomer(contact));
            return Ok(result);
        }

        [AllowAnonymous]
        [HttpPut("update/{id}")]
        public async Task<IActionResult> UpdateCustomer([FromBody] UserViewModel contact, int id)
        {
            var result = await _customerService.UpdateCustomer(id, FromUserViewModelToCustomer(contact));
            return Ok(result);
        }

        [AllowAnonymous]
        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteContact(int id)
        {
            await _customerService.DeleteCustomer(id);
            return Ok();
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

    public class User
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
        public string OrderStatus { get; set; }
        public int Orders { get; set; }
        public int Progress { get; set; }
        public int Status { get; set; }
        public List<string> Skills { get; set; }
        public List<string> Time { get; set; }
        public string Date { get; set; }
        public int Avatar { get; set; }
    }
}
