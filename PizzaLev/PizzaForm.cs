using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using PizzaDomain;
using PizzaDomain.Infrastructure;

namespace PizzaLev
{
    public partial class PizzaForm : Form
    {
        private readonly KeyPressEventHandler allowOnlyDigits = (sender, e) =>
        {
            if (!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar))
                e.Handled = true;
        };

        private readonly KeyPressEventHandler allowOnlyLetters = (sender, e) =>
        {
            if (!char.IsLetter(e.KeyChar) && !char.IsControl(e.KeyChar) && !char.IsSeparator(e.KeyChar))
                e.Handled = true;
        };

        private const string Extension = "Файл заказа | *.pzz";
        private readonly TabPage clientPage = new TabPage("Информация о клиенте");
        private readonly TabPage pizzaPage = new TabPage("Выбор Пиццы");
        private readonly TabControl tabsControl = new TabControl();
        private Order order = new Order(new Client(), new Pizza());

        public PizzaForm()
        {
            InitializeComponent();
            StartPosition = FormStartPosition.CenterScreen;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            Width = 650;
            Height = 400;
            ClientSize = new Size(Width, Height);
            Text = @"Заказ пиццы: ";

            tabsControl.Size = new Size(ClientSize.Width, (int) (ClientSize.Height * 0.85));
            tabsControl.Dock = DockStyle.Top;
            FillPizzaPage();
            FillClientPage();
            var table = new TableLayoutPanel
            {
                Dock = DockStyle.Bottom,
                Size = new Size(ClientSize.Width, (int) (ClientSize.Height * 0.15))
            };
            var saveButton = new Button
            {
                Text = @"Сохранить заказ",
                Anchor = AnchorStyles.Right | AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left
            };
            saveButton.Click += SaveClick;
            var openButton = new Button
            {
                Text = @"Открыть заказ",
                Anchor = AnchorStyles.Right | AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left
            };
            openButton.Click += OpenClick;
            table.Controls.Add(openButton);
            table.Controls.Add(saveButton);
            Controls.Add(tabsControl);
            Controls.Add(table);
        }

        private void SaveClick(object sender, EventArgs e)
        {
            var saveFileDialog = new SaveFileDialog {Filter = Extension };
            if (saveFileDialog.ShowDialog() == DialogResult.Cancel)
                return;
            Serializator.Serialize(order, saveFileDialog.FileName);
        }

        private void OpenClick(object sender, EventArgs e)
        {
            var openFileDialog = new OpenFileDialog {Filter = Extension };
            if (openFileDialog.ShowDialog() == DialogResult.Cancel)
                return;
            order = Serializator.Deserialize(openFileDialog.FileName);
            Updator.UpdatePizzaControls(order.Pizza, pizzaPage.Controls[0].Controls);
            Updator.UpdateClientControls(order.Client, clientPage.Controls[0].Controls);
        }

        private void FillPizzaPage()
        {
            var table = new TableLayoutPanel
            {
                Location = new Point((int) (ClientSize.Width * 0.6), ClientSize.Height / 4),
                AutoSize = true
            };
            var row = 0;
            foreach (Ingredients ingredient in Enum.GetValues(typeof(Ingredients)))
                MakeLabeledCheckbox(table, ingredient, row++);

            var size = new NumericUpDown
            {
                Maximum = Constraints.MaxPizzaSize,
                Minimum = Constraints.MinPizzaSize,
                Anchor = AnchorStyles.Left
            };
            size.ValueChanged += (sender, args) =>
            {
                order.Pizza.Size = (int) (sender as NumericUpDown).Value;
                pizzaPage.Invalidate();
            };
            table.Controls.Add(size, 1, row);
            table.Controls.Add(new Label {Text = @"Размер пиццы: ", AutoSize = true, Anchor = AnchorStyles.Right}, 0,
                row);
            pizzaPage.Controls.Add(table);
            tabsControl.Controls.Add(pizzaPage);
            pizzaPage.Paint += PizzaPaint;
        }

        private void PizzaPaint(object sender, PaintEventArgs paintEventArgs)
        {
            const int pizzaCoefficient = 5;
            var x = pizzaPage.Height / 2;
            var y = pizzaPage.Width / 4;
            var centerH = x - pizzaCoefficient * order.Pizza.Size / 2;
            var centerW = y - pizzaCoefficient * order.Pizza.Size / 2;
            var graphics = paintEventArgs.Graphics;
            graphics.FillEllipse(new SolidBrush(Color.BurlyWood), centerW, centerH, order.Pizza.Size * pizzaCoefficient,
                order.Pizza.Size * pizzaCoefficient);
            paintEventArgs.Graphics.SmoothingMode = SmoothingMode.HighQuality;
        }

        private void MakeLabeledCheckbox(TableLayoutPanel table, Ingredients ingredient, int startRow)
        {
            var checkBox = new CheckBox {Name = ingredient.GetDescription()};
            checkBox.CheckStateChanged += (sender, evt) =>
            {
                var box = sender as CheckBox;
                if (box.Checked)
                {
                    order.Pizza.AddIngredient(ingredient);
                    Text += $@"{ingredient.GetDescription()} ";
                }
                else
                {
                    order.Pizza.RemoveIngredient(ingredient);
                    Text = Text.Replace($"{ingredient.GetDescription()} ", "");
                }
            };

            var label = new Label {Text = ingredient.GetDescription(), AutoSize = true, Anchor = AnchorStyles.Right};
            table.Controls.Add(checkBox, 1, startRow);
            table.Controls.Add(label, 0, startRow);
        }

        private void FillClientPage()
        {
            var table = new TableLayoutPanel
            {
                AutoSize = true,
                Anchor = AnchorStyles.Left
            };
            var row = 0;
            MakeLabeledTextBoxForType(table, typeof(Name), ref row);
            MakeLabeledTextBoxForType(table, typeof(Address), ref row);
            MakeLabeledTextBoxForPhone(table, "Номер телефона", row);
            clientPage.Controls.Add(table);
            tabsControl.Controls.Add(clientPage);
        }

        private void MakeLabeledTextBoxForPhone(TableLayoutPanel table, string text, int row)
        {
            var input = new MaskedTextBox("8 (999) 000-00-00") {Name = "Номер"};
            input.Leave += (sender, args) => { order.Client.PhoneNumber = (sender as MaskedTextBox).Text; };
            var label = new Label {Text = text, AutoSize = true, Anchor = AnchorStyles.Right};
            table.Controls.Add(input, 1, row);
            table.Controls.Add(label, 0, row);
        }

        private void MakeLabeledTextBoxForType(TableLayoutPanel table, Type t, ref int startRow)
        {
            foreach (var fieldInfo in t.GetProperties())
            {
                KeyPressEventHandler onKeyPressed = null;
                if (fieldInfo.PropertyType == typeof(string))
                    onKeyPressed = allowOnlyLetters;
                else if (fieldInfo.PropertyType == typeof(int))
                    onKeyPressed = allowOnlyDigits;

                void EventHandler(object sender, EventArgs args)
                {
                    var textBox = sender as TextBox;
                    var val = textBox.Text;
                    if (t == typeof(Name))
                        fieldInfo.SetValue(order.Client.Name, val);
                    else if (t == typeof(Address))
                        if (fieldInfo.PropertyType == typeof(int))
                            fieldInfo.SetValue(order.Client.Address, int.Parse(val));
                        else
                            fieldInfo.SetValue(order.Client.Address, val);
                }

                MakeLabeledTextBox(table, fieldInfo.GetCustomDescription(), startRow++, onKeyPressed, EventHandler);
            }
        }


        private static void MakeLabeledTextBox(TableLayoutPanel table, string text, int startRow,
            KeyPressEventHandler onKeyPress, EventHandler onLeave)
        {
            var nameLabel = new Label {Text = text, AutoSize = true, Anchor = AnchorStyles.Right};
            var lineedit = new TextBox {Name = text, AutoSize = true};
            lineedit.KeyPress += onKeyPress;
            lineedit.Leave += onLeave;
            table.Controls.Add(nameLabel, 0, startRow);
            table.Controls.Add(lineedit, 1, startRow);
        }
    }
}