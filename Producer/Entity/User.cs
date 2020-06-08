using System;

namespace Producer.Entity
{
    public class User
    {
        public int Id { get; set; }
        
        public string Fullname { get; set; }
        
        public string MobileNumber { get; set; }
        
        public string Email { get; set; }
        
        public bool IsActive { get; set; }
        
        public DateTime BirthDate { get; set; }
    }
}