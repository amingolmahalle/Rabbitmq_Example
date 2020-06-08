using System;
using Common.RabbitMq;
using Newtonsoft.Json;

namespace Consumer.Services.Commands.GetAllUser
{
    public class GetAllUserCommand : ICommand
    {
        public int Id { get; set; }

        public string Fullname { get; set; }

        public string MobileNumber { get; set; }

        public string Email { get; set; }

        //  public int Age { get; } = DateTimeHelper.CalculateAge(BirthDate);

        public bool IsActive { get; set; }

        [JsonIgnore] public DateTime BirthDate { get; set; }
    }
}