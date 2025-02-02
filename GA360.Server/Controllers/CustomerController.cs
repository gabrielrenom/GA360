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
using GA360.DAL.Infrastructure.Interfaces;
using Microsoft.IdentityModel.Tokens;
using static GA360.Domain.Core.Interfaces.IAuditTrailService;

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
        private readonly ICustomerRepository _customerRepository;
        private readonly IPermissionService _permissionService;
        private readonly IAuditTrailService _auditTrailService;


        public CustomerController(ILogger<CustomerController> logger,
            ICustomerService customerService,
            IConfiguration configuration,
            IMemoryCache memoryCache,
            ICustomerRepository customerRepository,
            IPermissionService permissionService,
            IAuditTrailService auditTrailService)
        {
            _logger = logger;
            _customerService = customerService;
            _configuration = configuration;
            _memoryCache = memoryCache;
            _customerRepository = customerRepository;
            _permissionService = permissionService;
            _auditTrailService = auditTrailService;
        }

        //[AllowAnonymous]
        //[Authorize]
        //[HttpGet("list")]
        //public async Task<IActionResult> GetAllContacts()
        //{

        //    var result = await _customerService.GetAllCustomersWithEntities(1, 10, c => c.Email, true);

        //    return Ok(result == null ? new List<UserViewModel>() : result.Select(x => FromUserModelToViewModel(x)).ToList());
        //}
        //[Authorize]
        //[HttpGet("list")]
        //public async Task<IActionResult> GetAllContacts(int page = 1, int pageSize = 10)
        //{
        //    //var result = await _customerService.GetAllCustomersWithEntities(page, pageSize, c => c.Email, true);
        //    _logger.LogError($"STARTING GetAllContacts :: {DateTime.Now}");
        //    var result = await _customerService.GetAllCustomersWithEntities(null, null, c => c.Email, true);
        //    _logger.LogError($"AFTER DB CALL GetAllContacts :: {DateTime.Now}");

        //    var final = result == null ? new List<UserViewModel>() : result.Select(x => x.ToUserViewModel()).ToList();
        //    _logger.LogError($"END GetAllContacts :: {DateTime.Now}");

        //    return Ok(final);
        //}

        [Authorize]
        [HttpGet("list")]
        public async Task<IActionResult> GetAllContacts(int page = 1, int pageSize = 10, int? trainingCentreId = null)
        {
            var emailClaim = User?.Claims?.FirstOrDefault(x => x.Type == "email")?.Value;

            var permissions = await _permissionService.GetPermissions(emailClaim);

            return Ok(await _customerService.GetAllUltraHighPerformance(trainingCentreId));
        }

        [Authorize]
        [HttpGet("list/leads")]
        public async Task<IActionResult> GetAllLeads(int page = 1, int pageSize = 10, int? trainingCentreId = null)
        {
            var emailClaim = User?.Claims?.FirstOrDefault(x => x.Type == "email")?.Value;

            var permissions = await _permissionService.GetPermissions(emailClaim);

            await _auditTrailService.InsertAudit(IAuditTrailService.AuditTrailArea.Leads, IAuditTrailService.AuditTrailType.Information, "Getting Leads...", emailClaim);

            return Ok(await _customerService.GetLeadsAllUltraHighPerformance(trainingCentreId));
        }

        [Authorize]
        [HttpGet("learners/month/{trainingCentreId?}")]
        public async Task<IActionResult> GetActiveLearnersPerMonth(int? trainingCentreId = null)
        {
            var emailClaim = User?.Claims?.FirstOrDefault(x => x.Type == "email")?.Value;

            var permissions = await _permissionService.GetPermissions(emailClaim);

            await _auditTrailService.InsertAudit(IAuditTrailService.AuditTrailArea.Learners, IAuditTrailService.AuditTrailType.Information, $"Getting active learner per month TC:{trainingCentreId}.", emailClaim);

            return Ok(await _customerService.GetActiveLearnersPerMonth(trainingCentreId));
        }

        [Authorize]
        [HttpGet("list/leads/expiration/{trainingCentreId?}")]
        public async Task<IActionResult> GetAllLeadsApproachingExpiration(int? trainingCentreId = null)
        {
            var emailClaim = User?.Claims?.FirstOrDefault(x => x.Type == "email")?.Value;

            var permissions = await _permissionService.GetPermissions(emailClaim);

            await _auditTrailService.InsertAudit(IAuditTrailService.AuditTrailArea.Leads, IAuditTrailService.AuditTrailType.Information, $"Getting leads by expiration date TC:{trainingCentreId}.", emailClaim);

            return Ok(await _customerService.GetAllLeadsApproachingExpiration(trainingCentreId));
        }

        [Authorize]
        [HttpGet("get/full/{id}")]
        public async Task<IActionResult> GetCustomerById(int id)
        {
            var emailClaim = User?.Claims?.FirstOrDefault(x => x.Type == "email")?.Value;

            var result = await _customerService.GetCustomerByIdWithAllEntities(id);

            await _auditTrailService.InsertAudit(IAuditTrailService.AuditTrailArea.Learners, IAuditTrailService.AuditTrailType.Information, $"Getting full details from learner L:{id}", emailClaim);

            return Ok(FromUserModelToViewModel(result));
        }

        [Authorize]
        [HttpGet("get/profile/{id}")]
        public async Task<IActionResult> GetCustomerProfileHighPerfomance(int id)
        {
            var emailClaim = User?.Claims?.FirstOrDefault(x => x.Type == "email")?.Value;

            var result = await _customerService.GetCustomerProfileHighPerformance(id);

            await _auditTrailService.InsertAudit(IAuditTrailService.AuditTrailArea.Learners, IAuditTrailService.AuditTrailType.Information, $"Get learner profile High Perfomance L:{id}", emailClaim);

            return Ok(result);
        }

        //[AllowAnonymous]
        [HttpGet("list/basic")]
        public async Task<IActionResult> GetAllBasicContacts()
        {
            var emailClaim = User?.Claims?.FirstOrDefault(x => x.Type == "email")?.Value;

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

            await _auditTrailService.InsertAudit(IAuditTrailService.AuditTrailArea.Learners, IAuditTrailService.AuditTrailType.Information, $"Getting basic learners", emailClaim);

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

            await _auditTrailService.InsertAudit(IAuditTrailService.AuditTrailArea.Learners, IAuditTrailService.AuditTrailType.Information, $"Getting course qualification records.", emailClaim);

            return Ok(result!=null?
                result?.SelectMany(c => c.ToCustomersWithCourseQualificationRecordsViewModel()).ToList():
                new List<CustomersWithCourseQualificationRecordsViewModel>());
        }

        [AllowAnonymous]
        [HttpGet("customerwithcoursequalificationrecordsbycustomerid/{customerid}")]
        public async Task<IActionResult> GetCustomerWithCourseQualificationRecordsByCustomerId(int customerid)
        {
            var emailClaim = User?.Claims?.FirstOrDefault(x => x.Type == "email")?.Value;

            var result = await _customerService.GetAllCustomerWithCourseQualificationRecordsByCustomerId(customerid);

            await _auditTrailService.InsertAudit(IAuditTrailService.AuditTrailArea.Learners, IAuditTrailService.AuditTrailType.Information, $"Getting course qualification records by learner id L:{customerid}", emailClaim);

            return Ok(result != null ?
                result?.SelectMany(c => c.ToCustomersWithCourseQualificationRecordsViewModel()).ToList() :
                new List<CustomersWithCourseQualificationRecordsViewModel>());
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("customerswithcoursequalificationrecords")]
        public async Task<IActionResult> GetAllCustomersWithCourseQualificationRecords(
            int? pageNumber, int? pageSize, string orderBy = "Email", bool ascending = true)
        {
            var emailClaim = User?.Claims?.FirstOrDefault(x => x.Type == "email")?.Value;

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

            await _auditTrailService.InsertAudit(IAuditTrailService.AuditTrailArea.Learners, IAuditTrailService.AuditTrailType.Information, $"Getting course qualification records with pagination", emailClaim);

            return Ok(cachedResult);
        }


        [AllowAnonymous]
        [HttpPost("customerswithcoursequalificationrecords")]
        public async Task<IActionResult> CreateCustomersWithCourseQualificationRecords([FromBody] CustomersWithCourseQualificationRecordsViewModel customer)
        {
            var emailClaim = User?.Claims?.FirstOrDefault(x => x.Type == "email")?.Value;

            if (customer.QualificationId == null && customer.QualificationStatusId == null && customer.CourseId == null && customer.CertificateId == null && customer.Email.IsNullOrEmpty())
                return Ok();

            var result = await _customerService.CreateCustomersWithCourseQualificationRecords(customer.ToCustomersWithCourseQualificationRecordsModel());
            // Convert the customer object to a detailed message

            string auditMessage = $"Created new qualification with course. " +
                                  $"QualificationId: {customer.QualificationId}, " +
                                  $"QualificationStatusId: {customer.QualificationStatusId}, " +
                                  $"CourseId: {customer.CourseId}, " +
                                  $"CertificateId: {customer.CertificateId}, " +
                                  $"Email: {customer.Email}";

            await _auditTrailService.InsertAudit(IAuditTrailService.AuditTrailArea.Learners, IAuditTrailService.AuditTrailType.Information, auditMessage, emailClaim);

            _memoryCache.Remove("GetAllCustomersWithCourseQualificationRecords"); // Clear cache when data is modified
            return Ok(result);
        }

        [AllowAnonymous]
        [HttpPut("customerswithcoursequalificationrecords/{id}")]
        public async Task<IActionResult> UpdateCustomersWithCourseQualificationRecords(int id, [FromBody] CustomersWithCourseQualificationRecordsViewModel customer)
        {
            var emailClaim = User?.Claims?.FirstOrDefault(x => x.Type == "email")?.Value;

            var result = await _customerService.UpdateCustomersWithCourseQualificationRecords(customer.ToCustomersWithCourseQualificationRecordsModel());

            string auditMessage = $"Updating qualification with course. " +
                                  $"QualificationId: {customer.QualificationId}, " +
                                  $"QualificationStatusId: {customer.QualificationStatusId}, " +
                                  $"CourseId: {customer.CourseId}, " +
                                  $"CertificateId: {customer.CertificateId}, " +
                                  $"Email: {customer.Email}";

            await _auditTrailService.InsertAudit(IAuditTrailService.AuditTrailArea.Learners, IAuditTrailService.AuditTrailType.Information, auditMessage, emailClaim);

            _memoryCache.Remove("GetAllCustomersWithCourseQualificationRecords"); // Clear cache when data is modified
            return Ok(result);
        }

        [AllowAnonymous]
        [HttpDelete("customerswithcoursequalificationrecords/{id}")]
        public async Task<IActionResult> DeleteCustomersWithCourseQualificationRecords(int id)
        {
            var emailClaim = User?.Claims?.FirstOrDefault(x => x.Type == "email")?.Value;

            await _customerService.DeleteCustomersWithCourseQualificationRecords(id);
            _memoryCache.Remove("GetAllCustomersWithCourseQualificationRecords"); // Clear cache when data is modified

            await _auditTrailService.InsertAudit(AuditTrailArea.Learners, AuditTrailType.Information, $"Qualification removed for ID: {id}", emailClaim);

            return Ok();
        }

        [AllowAnonymous]
        [HttpGet("get")]
        public async Task<IActionResult> GetCustomer()
        {
            var emailClaim = User?.Claims?.FirstOrDefault(x => x.Type == "email")?.Value;

            var result = await _customerService.GetCustomerByEmail(emailClaim);

            await _auditTrailService.InsertAudit(AuditTrailArea.Learners, AuditTrailType.Information, "Customer information retrieved.", emailClaim);

            return Ok(result);
        }

        [HttpGet("get/documents/{email}")]
        public async Task<IActionResult> GetDocuments(string email)
        {
            var emailClaim = User?.Claims?.FirstOrDefault(x => x.Type == "email")?.Value;

            var result = await _customerService.GetCustomerDocumentsByEmail(email);

            await _auditTrailService.InsertAudit(AuditTrailArea.Learners, AuditTrailType.Information, $"Documents retrieved for email: {email}", emailClaim);

            return Ok(result);
        }

        [AllowAnonymous]
        [HttpGet("user")]
        public async Task<IActionResult> GetUser()
        {
            var emailClaim = User?.Claims?.FirstOrDefault(x => x.Type == "email")?.Value;

            var result = await _customerService.GetBasicCustomerByEmail(emailClaim);

            await _auditTrailService.InsertAudit(AuditTrailArea.Learners, AuditTrailType.Information, "Basic customer information retrieved.", emailClaim);

            return Ok(result);
        }

        [HttpGet("get/basic")]
        public async Task<IActionResult> GetBasicCustomer()
        {
            var emailClaim = User?.Claims?.FirstOrDefault(x => x.Type == "email")?.Value;

            if (emailClaim == null)
                return Forbid();

            var result = await _customerService.GetBasicCustomerByEmail(emailClaim);

            await _auditTrailService.InsertAudit(AuditTrailArea.Learners, AuditTrailType.Information, "Basic customer information retrieved.", emailClaim);

            return Ok(result);
        }

        [AllowAnonymous]
        [HttpPost("create")]
        public async Task<IActionResult> AddContact([FromForm] CustomerAddFilesViewModel contact)
        {
            var emailClaim = User?.Claims?.FirstOrDefault(x => x.Type == "email")?.Value;

            try
            {
                if (emailClaim == null)
                    return Forbid();

                var permissions = await _permissionService.GetPermissions(emailClaim);

                var customer = JsonSerializer.Deserialize<UserViewModel>(contact.Customer, new JsonSerializerOptions
                {
                    PropertyNamingPolicy = new UpperCaseNamingPolicy()
                });

                var files = await FileService.ExtractFiles(contact.Files);

                if (permissions.Role == "Training Centre")
                    customer.Role = "Candidate";

                if (customer.Role == "Super Admin")
                {
                    customer.TrainingCentre = 0;
                }
                if (string.IsNullOrEmpty(customer.Role))
                {
                    customer.Role = "Super Admin";
                }

                var result = await _customerService.AddCustomer(FromUserViewModelToCustomerModel(customer, files));

                var options = new JsonSerializerOptions
                {
                    ReferenceHandler = ReferenceHandler.Preserve,
                    WriteIndented = true
                };

                var jsonResult = JsonSerializer.Serialize(result, options);

                await _auditTrailService.InsertAudit(AuditTrailArea.Learners, AuditTrailType.Information, $"Contact created: {jsonResult}", emailClaim);

                _memoryCache.Remove("GetAllBasicContacts"); // Clear cache when data is modified

                return Ok(jsonResult);
            }
            catch (Exception ex)
            {
                await _auditTrailService.InsertAudit(AuditTrailArea.Learners, AuditTrailType.Error, $"Error creating contact: {ex.Message}", emailClaim);
                return BadRequest();
            }
        }

        [AllowAnonymous]
        [HttpPut("update/{id}")]
        public async Task<IActionResult> UpdateCustomer([FromBody] UserViewModel contact, int id)
        {
            var emailClaim = User?.Claims?.FirstOrDefault(x => x.Type == "email")?.Value;

            var result = await _customerService.UpdateCustomer(id, FromUserViewModelToCustomerModel(contact));
            _memoryCache.Remove("GetAllBasicContacts"); // Clear cache when data is modified

            await _auditTrailService.InsertAudit(AuditTrailArea.Learners, AuditTrailType.Information, $"Customer updated with ID: {id}", emailClaim);

            return Ok(result);
        }

        [AllowAnonymous]
        [HttpPut("updatewithdocuments/{id}")]
        public async Task<IActionResult> UpdateCustomerWithDocuments([FromForm] CustomerUploadViewModel contact, int id)
        {
            var emailClaim = User?.Claims?.FirstOrDefault(x => x.Type == "email")?.Value;

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

            await _auditTrailService.InsertAudit(AuditTrailArea.Learners, AuditTrailType.Information, $"Customer with documents updated with ID: {id}", emailClaim);

            return Ok(jsonResult);
        }

        [AllowAnonymous]
        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteContact(int id)
        {
            var emailClaim = User?.Claims?.FirstOrDefault(x => x.Type == "email")?.Value;

            await _customerService.DeleteCustomer(id);
            _memoryCache.Remove("GetAllBasicContacts"); // Clear cache when data is modified

            await _auditTrailService.InsertAudit(AuditTrailArea.Learners, AuditTrailType.Information, $"Contact deleted with ID: {id}", emailClaim);

            return Ok();
        }

        [AllowAnonymous]
        [HttpPost("batchupload")]
        public async Task<IActionResult> BatchUpload([FromBody] CustomerBatchViewModel contacts)
        {
            var emailClaim = User?.Claims?.FirstOrDefault(x => x.Type == "email")?.Value;

            try
            {
                var permissions = await _permissionService.GetPermissions(emailClaim);

                var result = (permissions.Role == "Training Centre") ?
                    await _customerService.UploadBatchCandidates(contacts.ToModel(), emailClaim) :
                    await _customerService.UploadBatchCandidates(contacts.ToModel());

                var jsonResult = JsonSerializer.Serialize(result?.ToViewModel());

                await _auditTrailService.InsertAudit(AuditTrailArea.Learners, AuditTrailType.Information, $"Batch upload completed: {jsonResult}", emailClaim);

                return Ok(result?.ToViewModel());
            }
            catch (Exception ex)
            {
                await _auditTrailService.InsertAudit(AuditTrailArea.Learners, AuditTrailType.Error, $"Error during batch upload: {ex.Message}", emailClaim);
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
                Url = x.Url// $"{x.Url}?{_configuration.GetSection("BlobStorageSettings:SharedAccessSignature").Value}"
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
