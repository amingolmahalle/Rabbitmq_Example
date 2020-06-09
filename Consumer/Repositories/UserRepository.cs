using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Common.Helpers;
using Consumer.Contracts.User;
using Producer.Entity;

namespace Consumer.Repositories
{
    public class UserRepository : IUserRepository
    {
        public async Task<GetUserByMobileNumberResponse> GetUserByMobileNumberAsync(string mobileNumber)
        {
            var user = MockData().Single(c => c.MobileNumber == mobileNumber);

            return Map(user);
        }

        private IEnumerable<User> MockData()
        {
            var listData = new List<User>
            {
                new User()
                {
                    Id = 1,
                    Fullname = "Amin Golmahalle",
                    Email = "amin@gmail.com",
                    MobileNumber = "09111111111",
                    IsActive = true,
                    BirthDate = Convert.ToDateTime("1991-01-01")
                },
                new User
                {
                    Id = 2,
                    Fullname = "Hamed Naeemaei",
                    Email = "hamed@gmail.com",
                    MobileNumber = "09222222222",
                    IsActive = true,
                    BirthDate = Convert.ToDateTime("1992-02-02")
                },
                new User
                {
                    Id = 3,
                    Fullname = "Reza Jenabi",
                    Email = "reza@gmail.com",
                    MobileNumber = "09100000000",
                    IsActive = true,
                    BirthDate = Convert.ToDateTime("1993-03-03")
                }
            };

            return listData;
        }

        private GetUserByMobileNumberResponse Map(User user)
        {
            return new GetUserByMobileNumberResponse
            {
                Id = user.Id,
                Fullname = user.Fullname,
                Email = user.Email,
                MobileNumber = user.MobileNumber,
                IsActive = user.IsActive,
                Age = DateTimeHelper.CalculateAge(user.BirthDate)
            };
        }
    }
}