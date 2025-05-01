using System;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Windows.Forms;

namespace SustainableProduct
{
    public partial class Form4 : Form
    {
        // Connection string to the local database containing product information
        private string connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\ozlem\source\repos\SustainableProduct\SustainableProduct\Product.mdf;Integrated Security=True";

        // Constructor to initialize the form
        public Form4()
        {
            InitializeComponent();

        }

        // Event handler for form load, triggers the product loading process
        private void Form4_Load(object sender, EventArgs e)
        {
            LoadProducts(); // Load all products into the data grid view
        }

        // Method to load products from the database and display them in a DataGridView using LINQ
        private void LoadProducts()
        {
            string query = "SELECT ProductName, Description, Price, Brand, Category, SustainabilityTags, EstimatedCarbonFootprint FROM Products";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlDataAdapter adapter = new SqlDataAdapter(query, conn);
                DataTable dt = new DataTable(); // Create a DataTable to hold the data
                adapter.Fill(dt); // Fill the DataTable with the query result

                var filteredData = dt.AsEnumerable()
                                     .Where(row => row.Field<decimal>("Price") > 10) // Example filter using LINQ (filtering products with price > 10)
                                     .Select(row => new
                                     {
                                         ProductName = row.Field<string>("ProductName"),
                                         Description = row.Field<string>("Description"),
                                         Price = row.Field<decimal>("Price"),
                                         Brand = row.Field<string>("Brand"),
                                         Category = row.Field<string>("Category"),
                                         SustainabilityTags = row.Field<string>("SustainabilityTags"),
                                         EstimatedCarbonFootprint = row.Field<decimal>("EstimatedCarbonFootprint")
                                     }).ToList();

                // Convert the LINQ result to DataTable
                var filteredDataTable = new DataTable();
                filteredDataTable.Columns.Add("ProductName");
                filteredDataTable.Columns.Add("Description");
                filteredDataTable.Columns.Add("Price");
                filteredDataTable.Columns.Add("Brand");
                filteredDataTable.Columns.Add("Category");
                filteredDataTable.Columns.Add("SustainabilityTags");
                filteredDataTable.Columns.Add("EstimatedCarbonFootprint");

                foreach (var item in filteredData)
                {
                    filteredDataTable.Rows.Add(item.ProductName, item.Description, item.Price, item.Brand, item.Category, item.SustainabilityTags, item.EstimatedCarbonFootprint);
                }

                // Bind the filtered DataTable to the DataGridView
                dataGridView1.DataSource = filteredDataTable;
            }
        }

        // Event handler for button2 click, deletes the selected product from the database
        private void button2_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow != null)
            {
                string productName = dataGridView1.CurrentRow.Cells["ProductName"].Value.ToString();

                string query = "DELETE FROM Products WHERE ProductName = @name";

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@name", productName);
                    cmd.ExecuteNonQuery();
                }

                LoadProducts(); // Reload the products after deletion
                MessageBox.Show("Product deleted.");
            }
        }

        // Event handler for button1 click, opens the EditProductForm to modify the selected product
        private void button1_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow != null)
            {
                DataGridViewRow row = dataGridView1.CurrentRow;

                // Use null-coalescing operator to provide a default value if the cell value is null
                var form = new EditProductForm(
                    row.Cells["ProductName"].Value?.ToString() ?? string.Empty,
                    row.Cells["Description"].Value?.ToString() ?? string.Empty,
                    row.Cells["Price"].Value?.ToString() ?? string.Empty,
                    row.Cells["Brand"].Value?.ToString() ?? string.Empty,
                    row.Cells["Category"].Value?.ToString() ?? string.Empty,
                    row.Cells["SustainabilityTags"].Value?.ToString() ?? string.Empty,
                    row.Cells["EstimatedCarbonFootprint"].Value?.ToString() ?? string.Empty
                );

                if (form.ShowDialog() == DialogResult.OK)
                {
                    string oldName = row.Cells["ProductName"].Value.ToString();
                    string query = @"UPDATE Products 
                             SET ProductName = @newName,
                                 Description = @desc,
                                 Price = @price,
                                 Brand = @brand,
                                 Category = @category,
                                 SustainabilityTags = @tags,
                                 EstimatedCarbonFootprint = @footprint
                             WHERE ProductName = @oldName";

                    using (SqlConnection conn = new SqlConnection(connectionString))
                    {
                        conn.Open();
                        SqlCommand cmd = new SqlCommand(query, conn);
                        cmd.Parameters.AddWithValue("@newName", form.ProductName);
                        cmd.Parameters.AddWithValue("@desc", form.Description);
                        cmd.Parameters.AddWithValue("@price", form.Price);
                        cmd.Parameters.AddWithValue("@brand", form.Brand);
                        cmd.Parameters.AddWithValue("@category", form.Category);
                        cmd.Parameters.AddWithValue("@tags", form.Tags);
                        cmd.Parameters.AddWithValue("@footprint", form.Footprint);
                        cmd.Parameters.AddWithValue("@oldName", oldName);
                        cmd.ExecuteNonQuery();
                    }

                    LoadProducts();
                    MessageBox.Show("Product successfully modified.");
                }
            }
        }


        // Event handler for button3 click, navigates to Form3
        private void button3_Click(object sender, EventArgs e)
        {
            Form3 form3 = new Form3();
            form3.Show();
            this.Hide();
        }
    }
}
