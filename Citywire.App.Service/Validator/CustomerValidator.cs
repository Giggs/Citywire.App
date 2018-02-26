using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Citywire.App.Service
{
    using Model;

    public class CustomerValidator : IValidator<Customer>
    {
        private readonly Dictionary<string, string> _errors;

        public CustomerValidator(Dictionary<string, string> errors)
        {
            _errors = errors;
        }

        public bool Validate(Customer customer)
        {
            if (string.IsNullOrEmpty(customer.Firstname))
                _errors.Add("Firstname", "Please enter FirstName.");

            if (string.IsNullOrEmpty(customer.Surname))
                _errors.Add("Surname", "Please enter Surname.");

            if (!customer.EmailAddress.Contains("@") && !customer.EmailAddress.Contains("."))
                _errors.Add("EmailAddress", "Please enter valid email address.");

            var now = DateTime.Now;
            int age = now.Year - customer.DateOfBirth.Year;
            if (now.Month < customer.DateOfBirth.Month || (now.Month == customer.DateOfBirth.Month && now.Day < customer.DateOfBirth.Day)) age--;

            if (age < 21)
                _errors.Add("DateOfBirth", "Your still not old enough.");

            return _errors.Count == 0;
        }
    }
}
