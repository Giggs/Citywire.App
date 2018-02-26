using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Citywire.App.Service
{
    using DataLayer;
    using Model;

    public class CustomerService : ICustomerService
    {
        private readonly IValidator<Customer> _customerValidator;
        private readonly IRepository<Company> _companyRepository;
        private readonly IRepository<Customer> _customerRepository;

        private readonly ICustomerCreditService _customerCreditService;

        public CustomerService(IValidator<Customer> customerValidator , IRepository<Company> companyRepository, IRepository<Customer> customerRepository, ICustomerCreditService customerCreditService)
        {
            _customerValidator = customerValidator;
            _companyRepository = companyRepository;
            _customerRepository = customerRepository;
            _customerCreditService = customerCreditService;
        }

        /// <summary>
        /// Create customer with valid credit score
        /// </summary>
        /// <param name="customer">Customer details</param>
        /// <param name="companyId">reference company id</param>
        /// <returns>True - If able to add customer / False - if NOT able to add customer </returns>
        public bool CreateCustomer(Customer customer, int companyId)
        {
            try
            {
                // Input Property Validation
                if (!_customerValidator.Validate(customer))
                    return false;

                //Check Credit Application score
                if (!IsValidCreditScore(customer, companyId))
                    return false;

                // Add Customer
                return _customerRepository.Create(customer);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }


        // ***We can move this as part of NEW/independent Comapnyservice

        // Check customer Credit score before creating them into system
        private bool IsValidCreditScore(Customer customer, int companyId)
        {
            // Get Company Details
            customer.Company = _companyRepository.GetById(companyId);

            // Based on customer company profile get credit limit
            switch (customer.Company.Name)
            {
                case "VeryImportantClient":
                    // Skip credit check
                    customer.HasCreditLimit = false;
                    break;

                case "ImportantClient":
                    // Do credit check and double credit limit
                    customer.HasCreditLimit = true;
                    var creditLimit = _customerCreditService.GetCreditLimit(customer.Firstname, customer.Surname, customer.DateOfBirth);
                    customer.CreditLimit = creditLimit * 2;
                    break;

                default:
                    // Do credit check
                    customer.HasCreditLimit = true;
                    customer.CreditLimit = _customerCreditService.GetCreditLimit(customer.Firstname, customer.Surname, customer.DateOfBirth);
                    break;
            }

            return !customer.HasCreditLimit || customer.CreditLimit >= 500;
        }
    }
}
