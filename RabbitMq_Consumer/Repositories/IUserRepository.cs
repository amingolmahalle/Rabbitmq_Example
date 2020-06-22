using System.Threading.Tasks;
using RabbitMq_Consumer.Contracts.Users;

namespace RabbitMq_Consumer.Repositories
{
    public interface IUserRepository
    {
        Task<GetUserByMobileNumberResponse> GetUserByMobileNumberAsync(string mobileNumber);
    }
}