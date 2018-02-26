using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Citywire.App.DataLayer
{
    using Model;

    public class CustomerRepository : IRepository<Customer>
    {
        public Customer GetById(int id)
        {
            throw new NotImplementedException();
        }

        public bool Create(Customer customer)
        {
            var flag = true;
            try
            {
                CustomerDataAccess.AddCustomer(customer);
            }
            catch (SqlException)
            {
                flag = false;
            }
            return flag;
        }
    }
}
