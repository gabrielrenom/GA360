using GA360.DAL.Entities.Entities;
using GA360.DAL.Infrastructure.Interfaces;
using GA360.DAL.Infrastructure.Repositories;
using GA360.Domain.Core.Interfaces;
using GA360.Domain.Core.Models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System.Linq.Expressions;
using System.Net.Http.Headers;
using System.Security.Cryptography.X509Certificates;

namespace GA360.Domain.Core.Services;

public class CustomerService : ICustomerService
{
    private readonly ICustomerRepository _customerRepository;
    private readonly ICountryRepository _countryrepository;
    private readonly ITrainingCentreRepository _trainingCentreRepository;
    private readonly ISkillRepository _skillRepository;
    private readonly IEthnicityRepository _ethnicityRepository;
    private readonly ILogger<CustomerService> _logger;
    private readonly IFileService _fileService;
    private readonly IDocumentRepository _documentRepository;
    private readonly IConfiguration _configuration;

    public CustomerService(ICustomerRepository customerRepository, ILogger<CustomerService> logger, ICountryRepository countryrepository, ISkillRepository skillRepository, IEthnicityRepository ethnicityRepository, ITrainingCentreRepository trainingCentreRepository, IFileService fileService, IDocumentRepository documentRepository, IConfiguration configuration)
    {
        _customerRepository = customerRepository;
        _logger = logger;
        _countryrepository = countryrepository;
        _skillRepository = skillRepository;
        _ethnicityRepository = ethnicityRepository;
        _trainingCentreRepository = trainingCentreRepository;
        _fileService = fileService;
        _documentRepository = documentRepository;
        _configuration = configuration;
    }

    public DAL.Entities.Entities.Customer GetCustomerById(int id)
    {
        return _customerRepository.Get(id);
    }

    public async Task<CustomerModel> GetCustomerByEmail(string email)
    {
        var result = await _customerRepository.GetCustomerByEmail(email);
        CustomerModel destination = new CustomerModel();
        if (result != null)
            Map(result, destination);

        return destination;
    }

    public async Task<object> GetBasicCustomerByEmail(string email)
    {
        var result = await _customerRepository.GetBasicCustomerByEmail(email);
        return new
        {
            Email = result.Email,
            FirstName = result.FirstName,
            LastName = result.LastName,
            Role = result.Roles!=null? result.Roles.FirstOrDefault().Role.Name:null,
            RoleId = result.Roles != null ? result.Roles.FirstOrDefault().RoleId:0,
            CustomerId= result.Id,
            TrainingCentreId= result.TrainingCentreId,
            Contact = result.Contact,
            City = result.Address!=null?result.Address.City:string.Empty,
            AvatarImage = result.AvatarImage,
            EmployeeStatus = result.EmploymentStatus,

        } ;
    }

    public IEnumerable<Customer> GetCustomersByCountry(int countryId)
    {
        return _customerRepository.GetCustomersByCountry(countryId);
    }

    public async Task<Customer> AddCustomer(Customer customer)
    {
        try
        {
            if (customer.CountryId == 0)
            {
                var country = await _customerRepository.Get<Country>(x => x.Name.ToLower() == customer.Country.Name.ToLower());
                customer.CountryId = country.Id;
            }

            _customerRepository.Add(customer);
            await _customerRepository.SaveChangesAsync();
            return customer;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "AddCustomer");
        }

        return null;
    }

    public async Task<Customer> AddCustomer(Models.CustomerModel customerModel)
    {
        try
        {
            var address = new Address
            {
                City = customerModel.City,
                Number = customerModel.Number,
                Postcode = customerModel.Postcode,
                Street = customerModel.Street,
            };

            var countryId = 0;
            if (customerModel.Country != null)
            {
                var country = await _customerRepository.Get<Country>(x => x.Name.ToLower() == customerModel.Country.ToLower());
                countryId = country.Id;
            }

            var ethnicOriginId = await _ethnicityRepository.Get<EthnicOrigin>(x => x.Name.ToLower() == customerModel.Ethnicity.ToLower());

            var customer = new Customer
            {
                About = customerModel.About,
                Address = address,
                Contact = customerModel.Contact,
                DOB = customerModel.DOB,
                CountryId = countryId,
                Description = customerModel.Description,
                Disability = customerModel.Disability,
                Email = customerModel.Email,
                FirstName = customerModel.FirstName,
                LastName = customerModel.LastName,
                Employer = customerModel.Employer,
                ePortfolio = customerModel.ePortfolio,
                EmploymentStatus = customerModel.EmploymentStatus,
                EthnicOriginId = ethnicOriginId.Id,
                FatherName = customerModel.FatherName,
                Gender = customerModel.Gender,
                Location = customerModel.Location,
                NI = customerModel.NI,
                Status = DAL.Entities.Enums.StatusEnum.Status.ProcessingRequest,
                Role = customerModel.Role,
                AvatarImage = customerModel.AvatarImage,
            };

            if (customerModel.TrainingCentre != null && customerModel.TrainingCentre > 0)
            {
                customer.TrainingCentreId = customerModel.TrainingCentre;
            }
            else
            {
                customer.TrainingCentreId = null;
            }

            _customerRepository.Context.Customers.Add(customer);
            //_customerRepository.Add(customer);
            await _customerRepository.SaveChangesAsync();

            var role = await _customerRepository.Context.Roles.FirstOrDefaultAsync(x => x.Name.ToLower() == customer.Role.ToLower());

            _customerRepository.Context.UserRoles.Add(new UserRole
            {
                RoleId = role.Id,
                CustomerId = customer.Id
            });

            await _customerRepository.SaveChangesAsync();


            var skills = new List<CustomerSkills>();
            var dbSkills = await _skillRepository.GetAll();

            foreach (var skill in customerModel.Skills)
            {
                if (dbSkills.FirstOrDefault(x => x.Name.ToLower() == skill.ToLower()) != null)
                {
                    skills.Add(new CustomerSkills
                    {
                        CustomerId = customer.Id,
                        SkillId = dbSkills.FirstOrDefault(x => x.Name.ToLower() == skill.ToLower()).Id
                    });
                }
            }

            var result = await _skillRepository.AddCustomerSkills(skills);

            // FILES
            var fileResult = await _fileService.UploadDocumentsAsync(customerModel.Files, customer.Id.ToString());

            var documents = new List<DocumentCustomer>();
            var documentEntities = fileResult.Select(x => new Document
            {
                BlobId = x.BlobId,
                Title = x.Name,
                Path = x.Url,
                FileSize = x.ByteArrayContent != null ? x.ByteArrayContent.Length.ToString() : null,
            }).ToList();

            await _documentRepository.UpsertDocuments(customer.Id, documentEntities);

            return customer;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "AddCustomer");
        }

        return null;
    }

    public async Task<DAL.Entities.Entities.Customer> UpdateCustomer(int id, DAL.Entities.Entities.Customer customer)
    {
        var customerdb = _customerRepository.Get(id);

        customerdb.About = customer.About;
        customerdb.Contact = customer.Contact;
        customerdb.Description = customer.Description;
        customerdb.Location = customer.Location;
        customerdb.Email = customer.Email;
        customerdb.FatherName = customer.FatherName;
        customerdb.FirstName = customer.FirstName;
        customerdb.LastName = customer.LastName;
        customerdb.Role = customer.Role;
        
        if (customer.TrainingCentreId != null && customer.TrainingCentreId >1)
        {
            customerdb.TrainingCentreId = customer.TrainingCentreId;
        }

        _customerRepository.Update(customerdb);
        await _customerRepository.SaveChangesAsync();

        return customerdb;
    }

    public async Task<Customer> UpdateCustomer(int id, Models.CustomerModel customer)
    {
        
        var ethnicOrigin = await _ethnicityRepository.Get<EthnicOrigin>(x => x.Name.ToLower() == customer.Ethnicity.ToLower());

        var customerdb = await _customerRepository.GetWithAllEntitiesById(id);

        try
        {
            if ((customer.TrainingCentre != null || customer.TrainingCentre > 0) && customer.TrainingCentre!=0)
            {
                var trainingCentreEntity = await _customerRepository.Get<TrainingCentre>(x => x.Id == customer.TrainingCentre);
                customerdb.TrainingCentre = trainingCentreEntity;
                customerdb.TrainingCentreId = customer.TrainingCentre;
            }

            if (customer.Country != null)
            {
                var country = await _customerRepository.Get<Country>(x => x.Name.ToLower() == customer.Country.ToLower());
                customerdb.Country = country;
                customerdb.CountryId = country.Id;
            }

            customerdb.About = customer.About;
            customerdb.Contact = customer.Contact;
            customerdb.Description = customer.Description;
            customerdb.Location = customer.Location;
            customerdb.Email = customer.Email;
            customerdb.FatherName = customer.FatherName;
            customerdb.FirstName = customer.FirstName;
            customerdb.LastName = customer.LastName;
            customerdb.Role = customer.Role;
            customerdb.DOB = customer.DOB;
            customerdb.EthnicOriginId = ethnicOrigin.Id;
            customerdb.Disability = customer.Disability;
            customerdb.Employer = customer.Employer;
            customerdb.EmploymentStatus = customer.EmploymentStatus;
            customerdb.Employer = customerdb.Employer;
            customerdb.NI = customer.NI;
            customerdb.NI = customer.NationalInsurance;
            customerdb.Role = customer.Role;
            customerdb.Gender = customer.Gender;
            customerdb.Status = (DAL.Entities.Enums.StatusEnum.Status)Enum.ToObject(typeof(DAL.Entities.Enums.StatusEnum.Status), customer.Status); ;
            customerdb.Contact = customer.Contact;
            customerdb.Address.City = customer.City;
            customerdb.Address.Street = customer.Street;
            customerdb.Address.Number = customer.Number;
            customerdb.Address.Postcode = customer.Postcode;
            customerdb.Location = customer.Location;
            customerdb.ePortfolio = customer.ePortfolio;
            customerdb.EthnicOrigin = ethnicOrigin;
            customerdb.AvatarImage = customer.AvatarImage;

            _customerRepository.Update(customerdb);
            await _customerRepository.SaveChangesAsync();


            var skills = new List<CustomerSkills>();
            await _skillRepository.Remove(customerdb.Id);
            var dbSkills = await _skillRepository.GetAll();
            foreach (var skill in customer.Skills)
            {
                if (dbSkills.FirstOrDefault(x => x.Name.ToLower() == skill.ToLower()) != null)
                {
                    skills.Add(new CustomerSkills
                    {
                        CustomerId = customerdb.Id,
                        SkillId = dbSkills.FirstOrDefault(x => x.Name.ToLower() == skill.ToLower()).Id
                    });
                }
            }

            var result = await _skillRepository.AddCustomerSkills(skills);

            var fileResult = await _fileService.UploadDocumentsAsync(customer.Files, customerdb.Id.ToString());

            var documents = new List<DocumentCustomer>();
            var documentEntities = fileResult.Select(x => new Document
            {
                BlobId = x.BlobId,
                Title = x.Name,
                Path = x.Url,
                FileSize = x.ByteArrayContent != null ? x.ByteArrayContent.Length.ToString() : null,
            }).ToList();

            await _documentRepository.UpsertDocuments(customerdb.Id, documentEntities);

            var docOrphansRemoved = await _fileService.CleanOrphans(customer.Files, customerdb.Id.ToString());
            var haveOrphansRemovedFromDb = await _documentRepository.CleanOrphans(customerdb.Id, documentEntities);
        }
        catch(Exception ex)
        {
            _logger.LogError("Error updating customer", ex);
        }

        return customerdb;
    }

    public async Task DeleteCustomer(int id)
    {
        try
        {
            var customer = _customerRepository.Get(id);
            if (customer != null)
            {
                // Delete related records in QualificationCustomerCourseCertificates table
                var relatedQualifications = _customerRepository.Context.QualificationCustomerCourseCertificates
                    .Where(qccc => qccc.CustomerId == id)
                    .ToList();
                foreach (var record in relatedQualifications)
                {
                    _customerRepository.Context.QualificationCustomerCourseCertificates.Remove(record);
                }

                // Delete related records in CustomerSkills table
                var relatedSkills = _customerRepository.Context.CustomerSkills
                    .Where(cs => cs.CustomerId == id)
                    .ToList();
                foreach (var record in relatedSkills)
                {
                    _customerRepository.Context.CustomerSkills.Remove(record);
                }

                // Delete related records in DocumentCustomers table
                var relatedDocuments = _customerRepository.Context.DocumentCustomer
                    .Where(dc => dc.CustomerId == id)
                    .ToList();
                foreach (var record in relatedDocuments)
                {
                    _customerRepository.Context.DocumentCustomer.Remove(record);
                }

                // Delete related records in UserRoles table
                var relatedRoles = _customerRepository.Context.UserRoles
                    .Where(ur => ur.CustomerId == id)
                    .ToList();
                foreach (var record in relatedRoles)
                {
                    _customerRepository.Context.UserRoles.Remove(record);
                }

                // Save changes to delete related records
                await _customerRepository.Context.SaveChangesAsync();

                // Now delete the customer
                _customerRepository.Delete(customer);
                await _customerRepository.SaveChangesAsync();
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting customer");
            throw;
        }
    }




    public async Task<List<Customer>> GetAll()
    {
        var result = await _customerRepository.GetAll((c => c.CustomerSkills, new Expression<Func<object, object>>[] { cs => ((CustomerSkills)cs).Skill }));

        return result;
    }

    public async Task<List<Customer>> GetAllCustomersWithEntities<TOrderKey>(int? pageNumber, int? pageSize, Expression<Func<DAL.Entities.Entities.Customer, TOrderKey>> orderBy, bool ascending = true)
    {
        var customerList = new List<CustomerModel>();

        //var customers = await _customerRepository.GetAllCustomersWithEntities(null, null, c => c.FirstName, true);
        var customers = await _customerRepository.GetAllCustomersWithEntitiesFast(pageNumber, pageSize, c => c.FirstName, true);

        //if (customers != null)
        //{
        //    foreach (var customer in customers)
        //    {
        //        Models.CustomerModel destination = new Models.CustomerModel();
        //        Map(customer, destination);
        //        customerList.Add(destination);
        //    }
        //}

        return customers;
    }

    public async Task<List<Customer>> GetAllCustomerWithCourseQualificationRecords<TOrderKey>(string email, int? pageNumber, int? pageSize, Expression<Func<Customer, TOrderKey>> orderBy, bool ascending = true)
    {
        var customerList = new List<Customer>();

        var customers = await _customerRepository.GetAllCustomerWithCourseQualificationRecords(email, pageNumber, pageSize, c => c.FirstName, true);

        if (customers != null)
        {
            foreach (var customer in customers)
            {
                customerList.Add(customer);
            }
        }

        return customerList;
    }

    public async Task<List<Customer>> GetAllCustomerWithCourseQualificationRecordsByCustomerId(int customerId)
    {
        var customerList = new List<Customer>();

        var customers = await _customerRepository.Context.Customers
               .Include(x => x.QualificationCustomerCourseCertificates)
               .ThenInclude(x => x.Course)
               .Include(x => x.QualificationCustomerCourseCertificates)
               .ThenInclude(x => x.Qualification)
               .Include(x => x.QualificationCustomerCourseCertificates)
               .ThenInclude(x => x.Certificate)
               .Include(x => x.QualificationCustomerCourseCertificates)
               .ThenInclude(x => x.QualificationStatus)
               .Include(x => x.TrainingCentre)
               .Where(x => x.Id == customerId)
               .ToListAsync();

        if (customers != null)
        {
            foreach (var customer in customers)
            {
                customerList.Add(customer);
            }
        }

        return customerList;
    }

    public async Task<CustomerModel> GetCustomerByIdWithAllEntities(int id)
    {
        var destination = new Models.CustomerModel();
        var customer =  await _customerRepository.GetWithAllPossibleEntitiesById(id);

        Map(customer, destination);

        return destination;
    }

    public async Task<List<Customer>> GetAllCustomersWithCourseQualificationRecords<TOrderKey>(int? pageNumber, int? pageSize, Expression<Func<Customer, TOrderKey>> orderBy, bool ascending = true)
    {
        var customerList = new List<Customer>();

        //var customers = await _customerRepository.GetAllCustomersWithCourseQualificationRecords(pageNumber, pageSize, c => c.FirstName, true);
        //var customers = await _customerRepository.GetAllCustomersWithCourseQualificationRecords(pageNumber, pageSize, "FirstName" , true);
        var customers = await _customerRepository.GetAllCustomersWithCourseQualificationRecords();

        return customers;
    }



    public async Task<bool> DeleteCustomersWithCourseQualificationRecords(int id)
    {
        var result = await _customerRepository.DeleteCustomersWithCourseQualificationRecords(id);

        return result;
    }

    public async Task<CustomersWithCourseQualificationRecordsModel> UpdateCustomersWithCourseQualificationRecords(CustomersWithCourseQualificationRecordsModel customer)
    {
        var result = await _customerRepository.UpdateCustomersWithCourseQualificationRecords(new QualificationCustomerCourseCertificate
        {
            CertificateId = customer.CertificateId,
            QualificationStatusId = customer.QualificationStatusId,
            QualificationId = customer.QualificationId,
            QualificationProgression = customer.Progression,
            CourseProgression = customer.Progression,
            CustomerId = customer.CustomerId,
            CourseId = customer.CourseId,
            Id = customer.Id
        });

        if (result == null)
            return null;

        var qualificationRecord = await _customerRepository.GetCustomerWithCourseQualificationRecordById(result.Id);

        int trainingCentreId;
        if (int.TryParse(customer.TrainingCentre, out trainingCentreId))
        {
            qualificationRecord.TrainingCentreId = trainingCentreId;

            await UpdateCustomer(qualificationRecord.Id, qualificationRecord);

            qualificationRecord = await _customerRepository.GetCustomerWithCourseQualificationRecordById(result.Id);
        }

        var customerRecord = new CustomersWithCourseQualificationRecordsModel();
        customerRecord.CustomerId = customer.CustomerId;
        customerRecord.CourseId = customer?.CourseId;
        customerRecord.QualificationId = customer?.QualificationId;
        customerRecord.Email = qualificationRecord?.Email;
        customerRecord.CertificateId = customer?.CertificateId;
        customerRecord.TrainingCentre = qualificationRecord?.TrainingCentre?.Name ?? string.Empty;
        customerRecord.CertificateName = qualificationRecord?.QualificationCustomerCourseCertificates.FirstOrDefault(x => x.Id == result.Id)?.Certificate?.Name;
        customerRecord.CourseName = qualificationRecord?.QualificationCustomerCourseCertificates.FirstOrDefault(x => x.Id == result.Id)?.Course?.Name;
        customerRecord.Progression = customer.Progression;
        customerRecord.QualificationName = qualificationRecord?.QualificationCustomerCourseCertificates.FirstOrDefault(x => x.Id == result.Id)?.Qualification?.Name;
        customerRecord.TrainingCentreId = customer?.TrainingCentreId;
        customerRecord.Id = result.Id;
        customerRecord.QualificationStatus = qualificationRecord?.QualificationCustomerCourseCertificates.FirstOrDefault(x => x.Id == result.Id)?.QualificationStatus?.Name ?? string.Empty;
        customerRecord.QualificationStatusId = result?.QualificationStatusId;

        return customerRecord;
    }

    public async Task<CustomersWithCourseQualificationRecordsModel> CreateCustomersWithCourseQualificationRecords(CustomersWithCourseQualificationRecordsModel customer)
    {
        var entity = new QualificationCustomerCourseCertificate
        {
            QualificationId = customer.QualificationId,
            CertificateId = customer.CertificateId,
            QualificationProgression = customer.Progression,
            CustomerId = customer.CustomerId,
            QualificationStatusId = customer.QualificationStatusId,
            CourseId = customer.CourseId
        };

        var result = await _customerRepository.CreateCustomersWithCourseQualificationRecords(entity);

        if (result == null)
            return null;

        // Detach any existing tracked instance of the Customer entity
        var existingCustomerEntity = _customerRepository.Context.Customers.Local.FirstOrDefault(x => x.Id == customer.CustomerId);
        if (existingCustomerEntity != null)
        {
            _customerRepository.Context.Entry(existingCustomerEntity).State = EntityState.Detached;
        }

        var customerEntity = await _customerRepository.Context.Customers.FirstOrDefaultAsync(x => x.Id == customer.CustomerId);

        var customerWithQualificationRecord = await _customerRepository.GetCustomerWithCourseQualificationRecordById(result.Id);
        customerWithQualificationRecord.TrainingCentreId = customerEntity.TrainingCentreId;

        await UpdateCustomer(customerWithQualificationRecord.Id, customerWithQualificationRecord);
        customerWithQualificationRecord = await _customerRepository.GetCustomerWithCourseQualificationRecordById(result.Id);

        if (!customer.TrainingCentre.IsNullOrEmpty())
        {
            // Detach any existing tracked instance of the Customer entity
            existingCustomerEntity = _customerRepository.Context.Customers.Local.FirstOrDefault(x => x.Id == customer.CustomerId);
            if (existingCustomerEntity != null)
            {
                _customerRepository.Context.Entry(existingCustomerEntity).State = EntityState.Detached;
            }

            customerEntity.TrainingCentreId = Convert.ToInt32(customer.TrainingCentre);
            _customerRepository.Context.Customers.Update(customerEntity);
            await _customerRepository.SaveChangesAsync();
        }

        var customerRecord = new CustomersWithCourseQualificationRecordsModel
        {
            CustomerId = customer.CustomerId,
            CourseId = customer.CourseId,
            QualificationId = customer.QualificationId,
            Email = customerWithQualificationRecord?.Email,
            CertificateId = customer.CertificateId,
            TrainingCentre = customerWithQualificationRecord?.TrainingCentre?.Name ?? string.Empty,
            CertificateName = customerWithQualificationRecord?.QualificationCustomerCourseCertificates.FirstOrDefault(x => x.Id == result.Id)?.Certificate?.Name,
            CourseName = customerWithQualificationRecord?.QualificationCustomerCourseCertificates.FirstOrDefault(x => x.Id == result.Id)?.Course?.Name,
            Progression = customer.Progression,
            QualificationName = customerWithQualificationRecord?.QualificationCustomerCourseCertificates.FirstOrDefault(x => x.Id == result.Id)?.Qualification?.Name,
            TrainingCentreId = customerEntity.TrainingCentreId,
            Id = result.Id,
            QualificationStatus = customerWithQualificationRecord?.QualificationCustomerCourseCertificates.FirstOrDefault(x => x.Id == result.Id)?.QualificationStatus?.Name ?? string.Empty,
            QualificationStatusId = result.QualificationStatusId
        };

        return customerRecord;
    }


    public void Map(Customer source, CustomerModel destination)
    {
        destination.FirstName = source.FirstName;
        destination.LastName = source.LastName;
        destination.Contact = source.Contact;
        destination.FatherName = source.FatherName;
        destination.About = source.About;
        destination.Gender = source.Gender;
        destination.Email = source.Email;
        destination.Role = source.Role;
        destination.Description = source.Description;
        destination.Location = source.Location;
        destination.CountryId = source.CountryId;
        destination.Country = source.Country?.Name;
        destination.TenantId = source.TenantId;
        destination.Id = source.Id;
        destination.AddressId = source.AddressId;
        destination.DOB = source.DOB;
        destination.NI = source.NI;
        destination.NationalInsurance = source.NI;
        destination.Disability = source.Disability;
        destination.EmploymentStatus = source.EmploymentStatus;
        destination.Employer = source.Employer;
        destination.ePortfolio = source.ePortfolio;
        destination.Portfolio = source.ePortfolio;
        destination.EthnicOriginId = source.EthnicOriginId;
        destination.EthnicOrigin = source.EthnicOrigin?.Name;
        destination.AvatarImage = source.AvatarImage;
        destination.Status = (int)source.Status;
        destination.TrainingCentre = source.TrainingCentreId;
        destination.Street = source.Address?.Street;
        destination.City = source.Address?.City;
        destination.Number = source.Address?.Number;
        destination.Postcode = source.Address?.Postcode;
        destination.Skills = source.CustomerSkills?.Select(cs => cs.Skill.Name).ToArray();

        //TOdo
        destination.Qualifications = source.QualificationCustomerCourseCertificates != null ?
            source.QualificationCustomerCourseCertificates.Where(x => x.QualificationId != null)
            .Select(x => new QualificationModel
            {
                CertificateDate = x.Qualification.CertificateDate,
                CertificateNumber = x.Qualification.CertificateNumber,
                ExpectedDate = x.Qualification.ExpectedDate,
                Id = x.Qualification.Id,
                Name = x.Qualification.Name,
                RegistrationDate = x.Qualification.RegistrationDate,
                Status = x.QualificationStatus == null ? string.Empty : x.QualificationStatus.Name,
                Progression = x.QualificationProgression

            }).ToList() : new List<QualificationModel>();

        destination.Files = source.DocumentCustomers?.Select(x => new FileModel
        {
            BlobId = x.Document.BlobId,
            Url = $"{x.Document.Path}?{_configuration.GetSection("BlobStorageSettings:SharedAccessSignature").Value}",
            Name = x.Document.Title
        }).ToList();

        destination.Certificates = source.QualificationCustomerCourseCertificates != null ?
            source.QualificationCustomerCourseCertificates
            .Where(x => x.CertificateId != null)
            .Select(x => new CertificateModel
            {
                Charge = x.Certificate.Charge,
                Id = x.Certificate.Id,
                Name = x.Certificate.Name,
                Type = x.Certificate.Type,
                Date = x.CreatedAt
            }).ToList()
            : new List<CertificateModel>();

        destination.Courses = source.QualificationCustomerCourseCertificates != null ?
            source.QualificationCustomerCourseCertificates
            .Where(x => x.CourseId != null)
            .Select(x => new CourseModel
            {
                Description = x.Course.Description,
                Id = x.Course.Id,
                Name = x.Course.Name,
                Status = x.Course.Status,
                Progression = x.CourseProgression,
                Assesor = x.Assesor,
                Duration = x.Course.Duration,
                Date = x.Course.RegistrationDate != null ? x.Course.RegistrationDate.ToShortDateString() : string.Empty,
            }).ToList()
            : new List<CourseModel>();
    }

    public async Task<CustomerBatchModel> UploadBatchCandidates(CustomerBatchModel batch, string trainincCentreUser)
    {
        var result = new CustomerBatchModel 
        {
             Customers = new List<CustomerBatchItemModel>(),
        };

        var trainingCentreUserEntity = await GetCustomerByEmail(trainincCentreUser);

        try
        {
            foreach (var customerModel in batch.Customers)
            {
                try
                {
                    var existingUser = await GetCustomerByEmail(customerModel.Email);

                    if (existingUser.Id > 0 || customerModel.Email.IsNullOrEmpty())
                        continue;

                    var address = new Address
                    {
                        City = customerModel.City,
                        Number = customerModel.Number.ToString(),
                        Postcode = customerModel.Postcode,
                        Street = customerModel.Street,
                    };

                    var countryId = 0;
                    if (customerModel.Country != null)
                    {
                        var country = await _customerRepository.Get<Country>(x => x.Name.ToLower() == customerModel.Country.ToLower());
                        if (country == null)
                            continue;

                        countryId = country.Id;
                    }

                    var ethnicOrigin = await _ethnicityRepository.Get<EthnicOrigin>(x => x.Name.ToLower() == customerModel.Ethnicity.ToLower());

                    if (ethnicOrigin == null)
                        continue;

                    var trainingCentre = await _trainingCentreRepository.Get<TrainingCentre>(x => x.Name.ToLower() == customerModel.TrainingCentre.ToLower());

                    var customer = new Customer
                    {
                        About = customerModel.About,
                        Address = address,
                        Contact = customerModel.Contact,
                        DOB = customerModel.DOB,
                        CountryId = countryId,
                        Disability = customerModel.Disability,
                        Email = customerModel.Email,
                        FirstName = customerModel.FirstName,
                        LastName = customerModel.LastName,
                        Employer = customerModel.Employer,
                        ePortfolio = customerModel.Portfolio,
                        EmploymentStatus = customerModel.EmployeeStatus,
                        EthnicOriginId = ethnicOrigin.Id,
                        FatherName = customerModel.FatherName,
                        Gender = customerModel.Gender,
                        Location = customerModel.Location,
                        NI = customerModel.NationalInsurance,
                        Status = DAL.Entities.Enums.StatusEnum.Status.ProcessingRequest,
                        Role = customerModel.Role,
                        //TrainingCentreId =  trainingCentreUserEntity.TrainingCentre:trainingCentre.Id,
                        TrainingCentreId =  trainingCentreUserEntity.TrainingCentre,
                    };

                    _customerRepository.Add(customer);

                    await _customerRepository.SaveChangesAsync();

                    var role = _customerRepository.GetDbContext().Set<Role>().Where(x => x.Name.ToLower() == "candidate").FirstOrDefault();

                    _customerRepository.GetDbContext().Add(new UserRole
                    {
                         RoleId = role.Id,
                         CustomerId = customer.Id
                    });
                    await _customerRepository.SaveChangesAsync();

                    result.Customers.Add(customerModel);
                }
                catch (Exception ex)
                {
                    _logger.LogError($"Error creating client {customerModel.Email}", ex);
                }
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "AddCustomer");
        }

        return result;
    }

    public async Task<CustomerBatchModel> UploadBatchCandidates(CustomerBatchModel batch)
    {
        var result = new CustomerBatchModel
        {
            Customers = new List<CustomerBatchItemModel>(),
        };

        try
        {
            foreach (var customerModel in batch.Customers)
            {
                try
                {
                    var existingUser = await GetCustomerByEmail(customerModel.Email);

                    if (existingUser.Id > 0 || customerModel.Email.IsNullOrEmpty())
                        continue;

                    var address = new Address
                    {
                        City = customerModel.City,
                        Number = customerModel.Number.ToString(),
                        Postcode = customerModel.Postcode,
                        Street = customerModel.Street,
                    };

                    var countryId = 0;
                    if (customerModel.Country != null)
                    {
                        var country = await _customerRepository.Get<Country>(x => x.Name.ToLower() == customerModel.Country.ToLower());
                        if (country == null)
                            continue;

                        countryId = country.Id;
                    }

                    var ethnicOrigin = await _ethnicityRepository.Get<EthnicOrigin>(x => x.Name.ToLower() == customerModel.Ethnicity.ToLower());

                    if (ethnicOrigin == null)
                        continue;

                    var trainingCentre = await _trainingCentreRepository.Get<TrainingCentre>(x => x.Name.ToLower() == customerModel.TrainingCentre.ToLower());

                    var customer = new Customer
                    {
                        About = customerModel.About,
                        Address = address,
                        Contact = customerModel.Contact,
                        DOB = customerModel.DOB,
                        CountryId = countryId,
                        Disability = customerModel.Disability,
                        Email = customerModel.Email,
                        FirstName = customerModel.FirstName,
                        LastName = customerModel.LastName,
                        Employer = customerModel.Employer,
                        ePortfolio = customerModel.Portfolio,
                        EmploymentStatus = customerModel.EmployeeStatus,
                        EthnicOriginId = ethnicOrigin.Id,
                        FatherName = customerModel.FatherName,
                        Gender = customerModel.Gender,
                        Location = customerModel.Location,
                        NI = customerModel.NationalInsurance,
                        Status = DAL.Entities.Enums.StatusEnum.Status.ProcessingRequest,
                        Role = customerModel.Role,
                        //TrainingCentreId =  trainingCentreUserEntity.TrainingCentre:trainingCentre.Id,
                        TrainingCentreId = trainingCentre.Id,
                    };

                    _customerRepository.Add(customer);

                    await _customerRepository.SaveChangesAsync();

                    var role = _customerRepository.GetDbContext().Set<Role>().Where(x => x.Name.ToLower() == "candidate").FirstOrDefault();

                    _customerRepository.GetDbContext().Add(new UserRole
                    {
                        RoleId = role.Id,
                        CustomerId = customer.Id
                    });
                    await _customerRepository.SaveChangesAsync();

                    result.Customers.Add(customerModel);
                }
                catch (Exception ex)
                {
                    _logger.LogError($"Error creating client {customerModel.Email}", ex);
                }
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "AddCustomer");
        }

        return result;
    }


    public async Task<List<CustomerModelHighPerformance>> GetAllUltraHighPerfomance(int? trainingCentreId)
    {
        var customers = new List<CustomerModelHighPerformance>();

        using (var connection = new SqlConnection(_customerRepository.GetDbContext().Database.GetConnectionString()))
        {
            // Base query
            var query = @"
            SELECT 
                c.Id,
                c.FirstName,
                c.LastName,
                CONCAT(c.FirstName, ' ', c.LastName) AS Name,
                c.Contact,
                c.Email,
                co.Name AS Country,
                c.Location,
                c.Status,
                c.CountryId,
                c.DOB,
                c.DOB AS DateOfBirth,
                c.TrainingCentreId,
                tc.Name AS TrainingCentreName
            FROM 
                Customers c
            LEFT JOIN 
                Countries co ON c.CountryId = co.Id
            LEFT JOIN 
                TrainingCentres tc ON c.TrainingCentreId = tc.Id";

            // Add WHERE clause if trainingCentreId is provided
            if (trainingCentreId.HasValue)
            {
                query += " WHERE c.TrainingCentreId = @TrainingCentreId";
            }

            query += " ORDER BY c.Id";

            var command = new SqlCommand(query, connection);

            if (trainingCentreId.HasValue)
            {
                command.Parameters.AddWithValue("@TrainingCentreId", trainingCentreId.Value);
            }

            await connection.OpenAsync();
            using (var reader = await command.ExecuteReaderAsync())
            {
                while (await reader.ReadAsync())
                {
                    var customer = new CustomerModelHighPerformance
                    {
                        Id = reader.GetInt32(reader.GetOrdinal("Id")),
                        FirstName = reader.GetString(reader.GetOrdinal("FirstName")),
                        LastName = reader.GetString(reader.GetOrdinal("LastName")),
                        Name = reader.GetString(reader.GetOrdinal("Name")),
                        Contact = reader.GetString(reader.GetOrdinal("Contact")),
                        Email = reader.GetString(reader.GetOrdinal("Email")),
                        Country = reader.GetString(reader.GetOrdinal("Country")),
                        Location = reader.GetString(reader.GetOrdinal("Location")),
                        Status = reader.GetInt32(reader.GetOrdinal("Status")),
                        Avatar = 0, // Assuming Avatar is not mapped from Customer
                        CountryId = reader.GetInt32(reader.GetOrdinal("CountryId")),
                        DOB = reader.GetString(reader.GetOrdinal("DOB")),
                        DateOfBirth = reader.GetString(reader.GetOrdinal("DateOfBirth")),
                        TrainingCentreId = reader.IsDBNull(reader.GetOrdinal("TrainingCentreId")) ? 0 : reader.GetInt32(reader.GetOrdinal("TrainingCentreId")),
                        TrainingCentre = reader.IsDBNull(reader.GetOrdinal("TrainingCentreName")) ? null : reader.GetString(reader.GetOrdinal("TrainingCentreName"))
                    };

                    customers.Add(customer);
                }
            }
        }

        return customers;
    }

    public async Task<List<FileModel>> GetCustomerDocumentsByEmail(string email)
    {
        var documents = new List<FileModel>();

        using (var connection = new SqlConnection(_customerRepository.Context.Database.GetConnectionString()))
        {
            await connection.OpenAsync();

            var query = @"
        SELECT 
            d.Title AS Name,
            d.Path AS Url,
            d.BlobId,
            d.Id
        FROM 
            [dbo].[Customers] c
        JOIN 
            [dbo].[DocumentCustomer] dc ON c.Id = dc.CustomerId
        JOIN 
            [dbo].[Documents] d ON dc.DocumentId = d.Id
        WHERE 
            c.Email = @Email";

            using (var command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@Email", email);

                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        var document = new FileModel
                        {
                            Name = reader.GetString(reader.GetOrdinal("Name")),
                            Url = reader.GetString(reader.GetOrdinal("Url")),
                            BlobId = reader.GetString(reader.GetOrdinal("BlobId")),
                        };

                        documents.Add(document);
                    }
                }
            }
        }

        return documents;
    }
}