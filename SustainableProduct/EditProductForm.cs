using System;
using System.Windows.Forms;

namespace SustainableProduct
{
    public partial class EditProductForm : Form
    {
        public string ProductName => txtName.Text;
        public string Description => txtDescription.Text;
        public decimal Price => decimal.TryParse(txtPrice.Text, out var val) ? val : 0;
        public string Brand => txtBrand.Text;
        public string Category => txtCategory.Text;
        public string Tags => txtTags.Text;
        public double Footprint => double.TryParse(txtFootprint.Text, out var val) ? val : 0;

        public EditProductForm(string name, string desc, string price, string brand, string category, string tags, string footprint)
        {
            InitializeComponent();


            txtName.Text = name;
            txtDescription.Text = desc;
            txtPrice.Text = price;
            txtBrand.Text = brand;
            txtCategory.Text = category;
            txtTags.Text = tags;
            txtFootprint.Text = footprint;
        }
        private void EditProductForm_Load(object sender, EventArgs e)
        {

            label1.Text = "Name:";
            label2.Text = "Description:";
            label3.Text = "Price:";
            label4.Text = "Brand:";
            label5.Text = "Category:";
            label6.Text = "Sustainability Tags:";
            label7.Text = "Carbon Footprint:";
            btnSave.Text = "Save";
        }


        private void btnSave_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void btnSave_Click_1(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Close();

        }

        private void button1_Click(object sender, EventArgs e)
        {
            Form4 form4 = new Form4();
            form4.Show();
            this.Hide();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}
