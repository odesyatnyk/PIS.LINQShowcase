using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LINQShowcase.UI
{
    public partial class MainForm : Form
    {
        Order Order1;
        Order Order2;
        public MainForm()
        {
            InitializeComponent();

            Product prod1 = new Product()
            {
                IdProduct = 1, Manufacturer = "China", Price = 100, ProductName = "Keyboard"
            };

            Product prod2 = new Product()
            {
                IdProduct = 2, Manufacturer = "USA", Price = 400, ProductName = "Mouse"
            };

            Product prod3 = new Product()
            {
                IdProduct = 3, Manufacturer = "Ukraine", Price = 1000, ProductName = "Screen"
            };

            Order1 = new Order()
            {
                IdOrder = 1,
                Client = "KPI"
            }.AddProduct(prod1)
             .AddProduct(prod2);

            Order2 = new Order()
            {
                IdOrder = 2,
                Client = "NAU"
            }.AddProduct(prod1)
             .AddProduct(prod2)
             .AddProduct(prod3);
        }

        private void departments_tbBindingNavigatorSaveItem_Click(object sender, EventArgs e)
        {
            this.Validate();
            this.departments_tbBindingSource.EndEdit();
            this.tableAdapterManager.UpdateAll(this.kraftHeinzDataSet);
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            this.departments_tbTableAdapter.Fill(this.kraftHeinzDataSet.departments_tb);
        }

        /// <summary>
        /// Query from
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {
            var result = from departments in kraftHeinzDataSet.departments_tb
                         where departments.departmentName.ToLower().Contains(textBox1.Text.Trim().ToLower())
                         || departments.departmentDescription.ToLower().Contains(textBox1.Text.Trim().ToLower())
                         orderby departments.departmentName ascending
                         select new { departments.idDepartment, departments.departmentName, departments.departmentDescription };

            dataGridView1.DataSource = result.ToList();
        }

        /// <summary>
        /// Extension method form
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button2_Click(object sender, EventArgs e)
        {
            dataGridView1.DataSource = kraftHeinzDataSet.departments_tb.Where(x => x.departmentName.ToLower().Trim().Contains(textBox1.Text.Trim().ToLower()) ||
                                                                                   x.departmentDescription.ToLower().Trim().Contains(textBox1.Text.Trim().ToLower()))
                                                                       .OrderBy(x => x.departmentName)
                                                                       .Select(x => new { x.idDepartment, x.departmentName, x.departmentDescription })
                                                                       .ToList();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            string[] strings = new string[] { "FIRST", "second", "Math", "Desiatnyk", "", "Wine" };
            string result = strings.Where(x => x.Length <= 5 && !string.IsNullOrWhiteSpace(x))
                                   .Take(2)
                                   .Aggregate((x, y) => x.ToLower() + " + " + y.ToLower());

            MessageBox.Show("Result: " + result);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            List<Order> orders = new List<Order> { Order1, Order2 };

            Order orderWithMaxTotalPrice = orders.FirstOrDefault(x => x.TotalPrice == orders.Select(y => y.TotalPrice).Max());

            List<Product> allOrderedProducts = orders.SelectMany(x => x.OrderedProducts.Select(y => y))
                                                     .Distinct()
                                                     .ToList();

            var productsInBothOrders = orders.SelectMany(z => orders, (x, y) => x.OrderedProducts.Intersect(y.OrderedProducts)).First().ToList();
        }
    }
}
