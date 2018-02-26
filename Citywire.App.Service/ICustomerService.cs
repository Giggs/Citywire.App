
namespace Citywire.App.Service
{
    using Model;

    public interface ICustomerService
    {
        bool CreateCustomer(Customer customer, int companyId);
    }
}