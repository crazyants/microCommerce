using System;

namespace microCommerce.Domain.Customers
{
    public class CustomerPassword : BaseEntity
    {
        public int CustomerId { get; set; }
        public string Password { get; set; }
        public string PasswordSalt { get; set; }
        public int PasswordFormatId { get; set; }
        public DateTime CreatedDateUtc { get; set; }

        public PasswordFormat PasswordFormat
        {
            get { return (PasswordFormat)PasswordFormatId; }
            set { PasswordFormatId = (int)value; }
        }
    }
}