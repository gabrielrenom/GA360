using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GA360.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ContactController: ControllerBase
    {
        private readonly ILogger<ContactController> _logger;

        public ContactController (ILogger<ContactController> logger)
        {
            _logger = logger;
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> GetAllContacts()
        {
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
