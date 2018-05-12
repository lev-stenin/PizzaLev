using System;

namespace PizzaDomain
{
    [Serializable]
    public class Order
    {
        public Client Client = new Client();
        public Pizza Pizza = new Pizza();

        public Order()
        {
        }

        public Order(Client client, Pizza pizza)
        {
            Client = client;
            Pizza = pizza;
        }
    }
}