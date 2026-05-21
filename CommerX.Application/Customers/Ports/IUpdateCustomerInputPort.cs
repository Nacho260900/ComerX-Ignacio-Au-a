using System.Threading.Tasks;
using CommerX.Application.Customers.DTOs;

namespace CommerX.Application.Customers.Ports
{
    public interface IUpdateCustomerInputPort
    {
        //define el metodo que iniciara el caso de uso
        Task ExecuteAsync(UpdateCustomerRequest request);
    }
}