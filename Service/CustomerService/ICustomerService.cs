using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Service.CustomerService
{
    public interface ICustomerService
    {
        string CreateCustomer(Customer customer);        
    }
}
