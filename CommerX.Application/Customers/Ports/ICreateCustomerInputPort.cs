using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommerX.Application.Customers.DTOs;

namespace CommerX.Application.Customers.Ports
{
    public interface ICreateCustomerInputPort
    {
        // ejecuta el caso de uso de forma asincronica
        Task ExecuteAsync(CreateCustomerRequest request);
    }
}
