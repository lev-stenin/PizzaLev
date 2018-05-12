using System;
using System.ComponentModel;

namespace PizzaDomain
{
    [Serializable]
    public class Address
    {
        public Address(string street, int building)
        {
            Street = street;
            Building = building;
        }

        public Address()
        {
        }

        [Description("Улица")]
        public string Street { get; set; } = "";

        [Description("Дом")]
        public int Building { get; set; }

        public override string ToString()
        {
            return $"{Street}, {Building}";
        }
    }
}