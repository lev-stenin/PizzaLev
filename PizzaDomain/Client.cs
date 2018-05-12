using System;
using PizzaDomain.Infrastructure;

namespace PizzaDomain
{
    [Serializable]
    public class Client
    {
        public Client(Name name, Address address, string phoneNumber)
        {
            Name = name;
            Address = address;
            PhoneNumber = phoneNumber;
        }

        public Client()
        {
        }

        public Name Name { get; set; } = new Name();
        public Address Address { get; set; } = new Address();
        public string PhoneNumber { get; set; } = "";

        public override string ToString()
        {
            return $"{Name}\n{Address}\n{PhoneNumber}";
        }
    }
}