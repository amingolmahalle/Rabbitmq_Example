using System.Threading.Tasks;
using Consumer.Contracts.User;

namespace Consumer.Repositories
{
    public interface IUserRepository
    {
        Task<GetUserByMobileNumberResponse> GetUserByMobileNumberAsync(string mobileNumber);
    }
}