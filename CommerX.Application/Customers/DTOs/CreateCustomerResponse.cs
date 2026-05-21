using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommerX.Application.Customers.DTOs
{
    public class CreateCustomerResponse
    {
        //identificar unico generado por el sistema
        public required Guid CustomerID { get; init; }
    }
}
