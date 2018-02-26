using System;
using System.Net.Configuration;
using Moq;
using NUnit.Framework;

namespace Citywire.App.Tests
{
    using Service;
    using Model;
    using DataLayer;

    [TestFixture]
    public class CustomerServiceTest
    {
        private CustomerService _customerService;

        private Mock<IValidator<Customer>> _customerValidator;
        private Mock<IRepository<Customer>> _customerRepoMock;
        private Mock<IRepository<Company>> _companyRepoMock;

        private Mock<ICustomerCreditService> _creditService;


        [SetUp]
        public void Setup()
        {
            this._customerValidator = new Mock<IValidator<Customer>>();
            this._customerRepoMock = new Mock<IRepository<Customer>>();
            this._companyRepoMock = new Mock<IRepository<Company>>();

            this._creditService = new Mock<ICustomerCreditService>();

            this._customerService = new CustomerService(_customerValidator.Object, _companyRepoMock.Object,
                _customerRepoMock.Object, _creditService.Object);
        }

        [TestCase(5)]
        [TestCase(157)]
        [TestCase(1000)]
        public void CreateCustomer_WithInValid_CustomerData(int companyId)
        {
            var customer = new Customer()
            {
                Firstname = "",
                Surname = "",
                DateOfBirth = new DateTime(2010, 10, 17),
                EmailAddress = "test.hon"
            };

            var company = new Company()
            {
                Id = companyId,
                Classification = Classification.Gold,
                Name = "comapny"
            };

            _companyRepoMock.Setup(x => x.GetById(It.IsAny<int>())).Returns(company);
            _customerRepoMock.Setup(x => x.Create(It.IsAny<Customer>())).Returns(true);

            _creditService.Setup(x => x.GetCreditLimit(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<DateTime>())).Returns(1000);

            var result = _customerService.CreateCustomer(customer, companyId);

            Assert.IsFalse(result);

        }

        [TestCase(5, "Client", 10)]
        [TestCase(157, "ImportantClient", 100)]
        [TestCase(1000, "test", 499)]
        public void CreateCustomer_WithInValid_CreditScore(int companyId, string companyName, int creditScore)
        {
            var customer = new Customer()
            {
                Firstname = "test",
                Surname = "hon",
                DateOfBirth = new DateTime(1980, 10, 17),
                EmailAddress = "test.hon@gmail.com"
            };

            var company = new Company()
            {
                Id = companyId,
                Classification = Classification.Gold,
                Name = companyName
            };

            _customerValidator.Setup(x => x.Validate(It.IsAny<Customer>())).Returns(true);
            _companyRepoMock.Setup(x => x.GetById(It.IsAny<int>())).Returns(company);
            _customerRepoMock.Setup(x => x.Create(It.IsAny<Customer>())).Returns(true);

            _creditService.Setup(x => x.GetCreditLimit(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<DateTime>())).Returns(creditScore);

            var result = _customerService.CreateCustomer(customer, companyId);

            Assert.IsFalse(result);

        }


        [TestCase(5, "Client")]
        [TestCase(157, "ImportantClient")]
        [TestCase(1000, "VeryImportantClient")]
        public void CreateCustomer_WithValidData(int companyId, string companyName)
        {
            var customer = new Customer()
            {
                Firstname = "test",
                Surname = "hon",
                DateOfBirth = new DateTime(1980, 10, 17),
                EmailAddress = "test.hon@gmail.com"
            };

            var company = new Company()
            {
                Id = companyId,
                Classification = Classification.Gold,
                Name = companyName
            };

            _customerValidator.Setup(x => x.Validate(It.IsAny<Customer>())).Returns(true);
            _companyRepoMock.Setup(x => x.GetById(It.IsAny<int>())).Returns(company);
            _customerRepoMock.Setup(x => x.Create(It.IsAny<Customer>())).Returns(true);

            _creditService.Setup(x => x.GetCreditLimit(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<DateTime>())).Returns(1000);

            var result = _customerService.CreateCustomer(customer, companyId);

            Assert.IsTrue(result);
        }
    }
}
