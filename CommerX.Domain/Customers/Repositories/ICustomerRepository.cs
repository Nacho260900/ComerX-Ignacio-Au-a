using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommerX.Domain.Customers.Entities;

namespace CommerX.Domain.Customers.Repositories
{
    public interface ICustomerRepository
    {
        //busca un cliente por su numero de documento - devuelve null si no existe
        Task<Customer?> FindByDocumentAsync(string document);

        //persiste un nuevo cliente en el repositorio
        Task AddAsync(Customer customer);

        //busca un cliente por su ID único - devuelve null si no existe
        Task<Customer?> FindByIdAsync(Guid id);

        //verifica si ya existe un cliente con el mismo email excluyendo al cliente actual
        Task<bool> ExistsByEmailExcludeAsync(string email, Guid currentCustomerId);

        //persiste la actualización de un cliente en el repositorio
        Task UpdateAsync(Customer customer);
    }
}
