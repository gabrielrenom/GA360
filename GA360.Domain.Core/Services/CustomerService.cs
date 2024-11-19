using GA360.DAL.Entities.Entities;
using GA360.DAL.Infrastructure.Interfaces;
using GA360.Domain.Core.Interfaces;
using GA360.Domain.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Linq.Expressions;
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
        Map(result, destination);

        return destination;
    }

    public async Task<CustomerModel> GetBasicCustomerByEmail(string email)
    {
        var result = await _customerRepository.GetCustomerBasicByEmail(email);
        CustomerModel destination = new CustomerModel();
        Map(result, destination);

        return destination;
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

            if (customerModel.TrainingCentre != null || customerModel.TrainingCentre > 0)
            {
                customer.TrainingCentreId = customerModel.TrainingCentre;
            }

            _customerRepository.Add(customer);
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

        if (customer.TrainingCentre != null || customer.TrainingCentre > 0)
        {
            customerdb.TrainingCentre = null;
            customerdb.TrainingCentreId = customer.TrainingCentre;
        }

        if (customer.Country != null)
        {
            var country = await _customerRepository.Get<Country>(x => x.Name.ToLower() == customer.Country.ToLower());
            customerdb.Country = null;
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

        return customerdb;
    }

    public async Task DeleteCustomer(int id)
    {
        var customer = _customerRepository.Get(id);
        if (customer != null)
        {
            _customerRepository.Delete(customer);
            await _customerRepository.SaveChangesAsync();
        }
    }

    public async Task<List<Customer>> GetAll()
    {
        var result = await _customerRepository.GetAll((c => c.CustomerSkills, new Expression<Func<object, object>>[] { cs => ((CustomerSkills)cs).Skill }));

        return result;
    }

    public async Task<List<CustomerModel>> GetAllCustomersWithEntities<TOrderKey>(int? pageNumber, int? pageSize, Expression<Func<DAL.Entities.Entities.Customer, TOrderKey>> orderBy, bool ascending = true)
    {
        var customerList = new List<CustomerModel>();

        var customers = await _customerRepository.GetAllCustomersWithEntities(null, null, c => c.FirstName, true);

        if (customers != null)
        {
            foreach (var customer in customers)
            {
                Models.CustomerModel destination = new Models.CustomerModel();
                Map(customer, destination);
                customerList.Add(destination);
            }
        }

        return customerList;
    }

    public async Task<List<Customer>> GetAllCustomersWithCourseQualificationRecords<TOrderKey>(int? pageNumber, int? pageSize, Expression<Func<Customer, TOrderKey>> orderBy, bool ascending = true)
    {
        var customerList = new List<Customer>();

        var customers = await _customerRepository.GetAllCustomersWithCourseQualificationRecords(pageNumber, pageSize, c => c.FirstName, true);

        if (customers != null)
        {
            foreach (var customer in customers)
            {
                customerList.Add(customer);
            }
        }

        return customerList;
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
            CustomerId = customer.CustomerId,
            CourseId = customer.CourseId,
            Id = customer.Id
        });

        return new CustomersWithCourseQualificationRecordsModel
        {
            Email = customer.Email,
            CertificateName = customer.CertificateName,
            CourseName = customer.CourseName,
            CertificateId = result.CertificateId,
            CourseId = result.CourseId,
            CustomerId = customer.CustomerId,
            Progression = customer.Progression,
            QualificationId = result.QualificationId,
            QualificationName = customer.QualificationName,
            TrainingCentre = customer.TrainingCentre,
            TrainingCentreId = customer.TrainingCentreId,
            Id = result.Id,
            QualificationStatusId = result.QualificationStatusId,
            QualificationStatus = customer.QualificationStatus
        };
    }

    public async Task<CustomersWithCourseQualificationRecordsModel> CreateCustomersWithCourseQualificationRecords(CustomersWithCourseQualificationRecordsModel customer)
    {
        var entity = (new QualificationCustomerCourseCertificate
        {
            QualificationId = customer.QualificationId,
            CertificateId = customer.CertificateId,
            CourseProgression = customer.Progression,
            CustomerId = customer.CustomerId,
            QualificationStatusId = customer.QualificationStatusId,
            CourseId = customer.CourseId
        });

        var result = await _customerRepository.CreateCustomersWithCourseQualificationRecords(entity);
       

        var qualificationRecord = await _customerRepository.GetCustomerWithCourseQualificationRecordById(result.Id);
        qualificationRecord.TrainingCentreId = Convert.ToInt32(customer.TrainingCentre);
        await UpdateCustomer(qualificationRecord.Id, qualificationRecord);
        qualificationRecord = await _customerRepository.GetCustomerWithCourseQualificationRecordById(result.Id);

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
}