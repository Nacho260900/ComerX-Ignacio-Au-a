using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommerX.Application.Customers.DTOs
{
    public class CreateCustomerRequest
    {
        // required: el compilador obliga a inicializar esta propiedad
        // init: solo puede asignarse durante la construcción del objeto
        public required string FirstName { get; init; }
        public required string LastName { get; init; }
        public required string Document { get; init; }
        public required string Email { get; init; }
        public required string Phone { get; init; }
        public required string Address { get; init; }
        // DateOnly: representa solo la fecha sin la hora
        public required DateOnly BirthDate { get; init; }
    }
}
