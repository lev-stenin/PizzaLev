using System.IO;
using System.Xml.Serialization;
using PizzaDomain;

namespace PizzaLev
{
    public static class Serializator
    {
        public static void Serialize(Order order, string filename)
        {
            var formatter = new XmlSerializer(typeof(Order));
            using (var fs = new FileStream(filename, FileMode.OpenOrCreate))
            {
                formatter.Serialize(fs, order);
            }
        }

        public static Order Deserialize(string filename)
        {
            var formatter = new XmlSerializer(typeof(Order));
            using (var fs = new FileStream(filename, FileMode.OpenOrCreate))
            {
                return (Order) formatter.Deserialize(fs);
            }
        }
    }
}