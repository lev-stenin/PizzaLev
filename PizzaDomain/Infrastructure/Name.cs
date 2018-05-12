using System;
using System.ComponentModel;

namespace PizzaDomain.Infrastructure
{
    [Serializable]
    public class Name
    {
        public Name(string firstName, string lastName)
        {
            FirstName = firstName;
            LastName = lastName;
        }

        public Name()
        {
        }

        [Description("Имя")]
        public string FirstName { get; set; } = "";

        [Description("Фамилия")]
        public string LastName { get; set; } = "";

        public override string ToString()
        {
            return $"{FirstName} {LastName}";
        }
    }
}