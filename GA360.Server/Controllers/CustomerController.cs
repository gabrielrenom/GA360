using GA360.DAL.Entities.Entities;
using GA360.Domain.Core.Interfaces;
using GA360.Domain.Core.Models;
using GA360.Domain.Core.Services;
using GA360.Server.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using System.Linq.Expressions;
using System.Text.Json;
using System.Text.Json.Serialization;
using static GA360.Commons.Helpers.JsonHelper;
using Microsoft.Extensions.Caching.Memory;

namespace GA360.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CustomerController : ControllerBase
    {
        private readonly ILogger<CustomerController> _logger;
        private readonly ICustomerService _customerService;
        private readonly IConfiguration _configuration;
        private readonly IMemoryCache _memoryCache;

        public CustomerController(ILogger<CustomerController> logger, ICustomerService customerService, IConfiguration configuration, IMemoryCache memoryCache)
        {
            _logger = logger;
            _customerService = customerService;
            _configuration = configuration;
            _memoryCache = memoryCache;
        }

        [AllowAnonymous]
        [HttpGet("list")]
        public async Task<IActionResult> GetAllContacts()
        {

            var result = await _customerService.GetAllCustomersWithEntities(1, 10, c => c.Email, true);

            return Ok(result == null ? new List<UserViewModel>() : result.Select(x => FromUserModelToViewModel(x)).ToList());
        }

        [AllowAnonymous]
        [HttpGet("list/basic")]
        public async Task<IActionResult> GetAllBasicContacts()
        {
            var cacheKey = "GetAllBasicContacts";
            if (!_memoryCache.TryGetValue(cacheKey, out List<BasicUserViewModel> cachedResult))
            {
                var result = await _customerService.GetAll();

                var basicContacts = result == null ? new List<BasicUserViewModel>() : result.Select(x => x.ToBasicViewModel()).Distinct().ToList();

                var cacheEntryOptions = new MemoryCacheEntryOptions()
                    .SetSlidingExpiration(TimeSpan.FromMinutes(30)) // Adjust cache duration as needed
                    .SetAbsoluteExpiration(TimeSpan.FromHours(1));

                _memoryCache.Set(cacheKey, basicContacts, cacheEntryOptions);

                cachedResult = basicContacts;
            }

            return Ok(cachedResult);
        }

        [AllowAnonymous]
        [HttpGet("customerwithcoursequalificationrecords")]
        public async Task<IActionResult> GetCustomerWithCourseQualificationRecords(
            int? pageNumber, int? pageSize, string orderBy = "Email", bool ascending = true)
        {
            var emailClaim = User?.Claims?.FirstOrDefault(x => x.Type == "email")?.Value;

            // Convert the string `orderBy` to a lambda expression
            var parameter = Expression.Parameter(typeof(Customer), "x");
            var property = Expression.Property(parameter, orderBy);
            var lambda = Expression.Lambda<Func<Customer, object>>(Expression.Convert(property, typeof(object)), parameter);

            var result = await _customerService.GetAllCustomerWithCourseQualificationRecords(emailClaim, pageNumber, pageSize, lambda, ascending);
            
            return Ok(result!=null?
                result?.SelectMany(c => c.ToCustomersWithCourseQualificationRecordsViewModel()).ToList():
                new List<CustomersWithCourseQualificationRecordsViewModel>());
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("customerswithcoursequalificationrecords")]
        public async Task<IActionResult> GetAllCustomersWithCourseQualificationRecords(
            int? pageNumber, int? pageSize, string orderBy = "Email", bool ascending = true)
        {
            var cacheKey = $"GetAllCustomersWithCourseQualificationRecords";
            if (!_memoryCache.TryGetValue(cacheKey, out List<CustomersWithCourseQualificationRecordsViewModel> cachedResult))
            {
                // Convert the string `orderBy` to a lambda expression
                var parameter = Expression.Parameter(typeof(Customer), "x");
                var property = Expression.Property(parameter, orderBy);
                var lambda = Expression.Lambda<Func<Customer, object>>(Expression.Convert(property, typeof(object)), parameter);

                var customers = await _customerService.GetAllCustomersWithCourseQualificationRecords(
                    pageNumber, pageSize, lambda, ascending);

                // Convert each customer to its view model and flatten the results
                cachedResult = customers?.SelectMany(c => c.ToCustomersWithCourseQualificationRecordsViewModel()).ToList()
                                         ?? new List<CustomersWithCourseQualificationRecordsViewModel>();

                var cacheEntryOptions = new MemoryCacheEntryOptions()
                   .SetSlidingExpiration(TimeSpan.FromMinutes(30)) // Adjust cache duration as needed
                   .SetAbsoluteExpiration(TimeSpan.FromHours(1));

                _memoryCache.Set(cacheKey, cachedResult, cacheEntryOptions);
            }

            return Ok(cachedResult);
        }


        [AllowAnonymous]
        [HttpPost("customerswithcoursequalificationrecords")]
        public async Task<IActionResult> CreateCustomersWithCourseQualificationRecords([FromBody] CustomersWithCourseQualificationRecordsViewModel customer)
        {
            var result = await _customerService.CreateCustomersWithCourseQualificationRecords(customer.ToCustomersWithCourseQualificationRecordsModel());
            _memoryCache.Remove("GetAllCustomersWithCourseQualificationRecords"); // Clear cache when data is modified
            return Ok(result);
        }

        [AllowAnonymous]
        [HttpPut("customerswithcoursequalificationrecords/{id}")]
        public async Task<IActionResult> UpdateCustomersWithCourseQualificationRecords(int id, [FromBody] CustomersWithCourseQualificationRecordsViewModel customer)
        {
            var result = await _customerService.UpdateCustomersWithCourseQualificationRecords(customer.ToCustomersWithCourseQualificationRecordsModel());
            _memoryCache.Remove("GetAllCustomersWithCourseQualificationRecords"); // Clear cache when data is modified
            return Ok(result);
        }

        [AllowAnonymous]
        [HttpDelete("customerswithcoursequalificationrecords/{id}")]
        public async Task<IActionResult> DeleteCustomersWithCourseQualificationRecords(int id)
        {
            await _customerService.DeleteCustomersWithCourseQualificationRecords(id);
            _memoryCache.Remove("GetAllCustomersWithCourseQualificationRecords"); // Clear cache when data is modified
            return Ok();
        }

        [AllowAnonymous]
        [HttpGet("get")]
        public async Task<IActionResult> GetCustomer()
        {
            var emailClaim = User.Claims.FirstOrDefault(x => x.Type == "email").Value;

            var result = await _customerService.GetCustomerByEmail(emailClaim);

            return Ok(result);
        }

        [AllowAnonymous]
        [HttpGet("user")]
        public async Task<IActionResult> GetUser()
        {
            var emailClaim = User.Claims.FirstOrDefault(x => x.Type == "email").Value;

            var result = await _customerService.GetBasicCustomerByEmail(emailClaim);

            return Ok(result);
        }

        [HttpGet("get/basic")]
        public async Task<IActionResult> GetBasicCustomer()
        {
            if (User.Claims.FirstOrDefault(x => x.Type == "email") == null)
                return Forbid();

            var emailClaim = User.Claims.FirstOrDefault(x => x.Type == "email").Value;

            var result = await _customerService.GetBasicCustomerByEmail(emailClaim);

            return Ok(result);
        }

        [AllowAnonymous]
        [HttpPost("create")]
        public async Task<IActionResult> AddContact([FromForm] CustomerAddFilesViewModel contact)
        {
            try
            {
                var customer = JsonSerializer.Deserialize<UserViewModel>(contact.Customer, new JsonSerializerOptions
                {
                    PropertyNamingPolicy = new UpperCaseNamingPolicy()
                });

                var files = await FileService.ExtractFiles(contact.Files);

                var result = await _customerService.AddCustomer(FromUserViewModelToCustomerModel(customer, files));

                var options = new JsonSerializerOptions
                {
                    ReferenceHandler = ReferenceHandler.Preserve,
                    WriteIndented = true
                };

                var jsonResult = JsonSerializer.Serialize(result, options);

                _memoryCache.Remove("GetAllBasicContacts"); // Clear cache when data is modified

                return Ok(jsonResult);
            }
            catch (Exception ex)
            {
                return BadRequest();
            }

        }

        [AllowAnonymous]
        [HttpPut("update/{id}")]
        public async Task<IActionResult> UpdateCustomer([FromBody] UserViewModel contact, int id)
        {
            var result = await _customerService.UpdateCustomer(id, FromUserViewModelToCustomerModel(contact));
            _memoryCache.Remove("GetAllBasicContacts"); // Clear cache when data is modified
            return Ok(result);
        }

        [AllowAnonymous]
        [HttpPut("updatewithdocuments/{id}")]
        public async Task<IActionResult> UpdateCustomerWithDocuments([FromForm] CustomerUploadViewModel contact, int id)
        {
            var customer = JsonSerializer.Deserialize<UserViewModel>(contact.Customer, new JsonSerializerOptions
            {
                PropertyNamingPolicy = new UpperCaseNamingPolicy()
            });

            var files = await FileService.ExtractFiles(contact.Files);

            var result = await _customerService.UpdateCustomer(id, FromUserViewModelToCustomerModel(customer, files));

            var options = new JsonSerializerOptions
            {
                ReferenceHandler = ReferenceHandler.Preserve,
                WriteIndented = true
            };

            var jsonResult = JsonSerializer.Serialize(result, options);
            _memoryCache.Remove("GetAllBasicContacts"); // Clear cache when data is modified
            return Ok(jsonResult);
        }

        [AllowAnonymous]
        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteContact(int id)
        {
            await _customerService.DeleteCustomer(id);
            _memoryCache.Remove("GetAllBasicContacts"); // Clear cache when data is modified

            return Ok();
        }

        [AllowAnonymous]
        [HttpPost("batchupload")]
        public async Task<IActionResult> BatchUpload([FromBody] CustomerBatchViewModel contacts)
        {
            try
            {
                var emailClaim = User?.Claims?.FirstOrDefault(x => x.Type == "email")?.Value;

                var result = await _customerService.UploadBatchCandidates(contacts.ToModel(), emailClaim);

                return Ok(result?.ToViewModel());
            }
            catch (Exception ex)
            {
                return BadRequest("Error doing the batch upload");
            }
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

            destination.FileDocuments = source.Files != null ? source.Files.Select(x => new FileModel
            {
                BlobId = x.BlobId,
                Content = x.Content,
                Name = x.Name,
                Url = $"{x.Url}?{_configuration.GetSection("BlobStorageSettings:SharedAccessSignature").Value}"
            }).ToList() : new List<FileModel>();
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

        private CustomerModel FromUserViewModelToCustomerModel(UserViewModel userViewModel, IList<FileModel> files = null)
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
                Ethnicity = userViewModel.Ethnicity,
                Files = files
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
