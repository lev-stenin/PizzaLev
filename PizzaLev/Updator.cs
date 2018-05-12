using System;
using System.Linq;
using System.Windows.Forms;
using PizzaDomain;
using PizzaDomain.Infrastructure;

namespace PizzaLev
{
    public static class Updator
    {
        public static void UpdateClientControls(Client client, Control.ControlCollection controls)
        {
            UpdateLabelsForType(typeof(Name), client, controls);
            UpdateLabelsForType(typeof(Address), client, controls);
            if (controls.Find("Номер", false)[0] is MaskedTextBox phoneControl)
                phoneControl.Text = client.PhoneNumber;
        }

        private static void UpdateLabelsForType(Type t, Client client, Control.ControlCollection controls)
        {
            foreach (var fi in t.GetProperties())
            {
                var name = fi.GetCustomDescription();
                foreach (var control in controls)
                {
                    if (!(control is TextBox textBox))
                        continue;
                    if (name != textBox.Name)
                        continue;
                    if (t == typeof(Name))
                        textBox.Text = fi.GetValue(client.Name).ToString();
                    else if (t == typeof(Address))
                        textBox.Text = fi.GetValue(client.Address).ToString();
                }
            }
        }

        public static void UpdatePizzaControls(Pizza pizza, Control.ControlCollection controls)
        {
            var ingridientsDescriptions = pizza.Ingredients.Select(i => EnumExtensions.GetDescription(i)).ToList();
            foreach (var control in controls)
            {
                if (control is NumericUpDown selector)
                {
                    selector.Value = pizza.Size;
                    continue;
                }

                if (!(control is CheckBox checkBox))
                    continue;
                checkBox.Checked = ingridientsDescriptions.Contains(checkBox.Name);
            }
        }
    }
}