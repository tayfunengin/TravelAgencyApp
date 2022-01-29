using Domain.Entities;
using Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Service.CustomerService
{
    public class CustomerService : ICustomerService
    {
        private readonly IRepository<Customer> repository;

        public CustomerService(IRepository<Customer> repository)
        {
            this.repository = repository;
        }

        public string CreateCustomer(Customer customer)
        {
            return this.repository.Insert(customer);
        }

  
    }
}
