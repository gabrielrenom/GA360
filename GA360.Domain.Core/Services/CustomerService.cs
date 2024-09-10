using GA360.DAL.Entities.Entities;
using GA360.DAL.Infrastructure.Interfaces;
using GA360.Domain.Core.Interfaces;
using Microsoft.Extensions.Logging;
using System.Linq.Expressions;

namespace GA360.Domain.Core.Services;

public class CustomerService : ICustomerService
{
    private readonly ICustomerRepository _customerRepository;
    private readonly ILogger<CustomerService> _logger;

    public CustomerService(ICustomerRepository customerRepository, ILogger<CustomerService> logger)
    {
        _customerRepository = customerRepository;
        _logger = logger;
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
            _logger.LogError(ex,"AddCustomer");
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
}