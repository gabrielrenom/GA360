using GA360.DAL.Entities.Entities;
using GA360.DAL.Infrastructure.Interfaces;
using GA360.Domain.Core.Interfaces;
using GA360.Domain.Core.Models;
using Microsoft.Extensions.Logging;
using System.Linq.Expressions;

namespace GA360.Domain.Core.Services;

public class CustomerService : ICustomerService
{
    private readonly ICustomerRepository _customerRepository;
    private readonly ICountryRepository _countryrepository;
    private readonly ITrainingCentreRepository _trainingCentreRepository;
    private readonly ISkillRepository _skillRepository;
    private readonly IEthnicityRepository _ethnicityRepository;
    private readonly ILogger<CustomerService> _logger;

    public CustomerService(ICustomerRepository customerRepository, ILogger<CustomerService> logger, ICountryRepository countryrepository, ISkillRepository skillRepository, IEthnicityRepository ethnicityRepository, ITrainingCentreRepository trainingCentreRepository)
    {
        _customerRepository = customerRepository;
        _logger = logger;
        _countryrepository = countryrepository;
        _skillRepository = skillRepository;
        _ethnicityRepository = ethnicityRepository;
        _trainingCentreRepository = trainingCentreRepository;
    }

    public Customer GetCustomerById(int id)
    {
        return _customerRepository.Get(id);
    }

    public Customer GetCustomerByEmail(string email)
    {
        return _customerRepository.GetCustomerByEmail(email);
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

    public async Task<Customer> AddCustomer(CustomerModel customerModel)
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

            if (customerModel.TrainingCentre != null || customerModel.TrainingCentre>0)
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

            return customer;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "AddCustomer");
        }

        return null;
    }

    public async Task<Customer> UpdateCustomer(int id, Customer customer)
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

        _customerRepository.Update(customerdb);
        await _customerRepository.SaveChangesAsync();

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

    public async Task<List<CustomerModel>> GetAllCustomersWithEntities<TOrderKey>(int? pageNumber, int? pageSize, Expression<Func<Customer, TOrderKey>> orderBy, bool ascending = true)
    {
        var customerList = new List<CustomerModel>();

        var customers = await _customerRepository.GetAllCustomersWithEntities(null, null, c => c.FirstName, true);

        if (customers != null)
        {
            foreach (var customer in customers)
            {
                CustomerModel destination = new CustomerModel();
                Map(customer, destination);
                customerList.Add(destination);
            }
        }

        return customerList;
    }

    public static void Map(Customer source, CustomerModel destination)
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
    }
}