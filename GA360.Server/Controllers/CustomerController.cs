using GA360.DAL.Entities.Entities;
using GA360.Domain.Core.Interfaces;
using GA360.Server.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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
            var result = await _customerService.GetAll();
            return Ok(result == null ? new List<UserViewModel>() : result.Select(x => FromCustomerToUserViewModel(x)).ToList());
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
}
