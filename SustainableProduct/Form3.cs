using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SustainableProduct
{
    public partial class Form3 : Form
    {
        // Constructor to initialize the form and set up events for pictureBox6 (image display)
        public Form3()
        {
            InitializeComponent();
            pictureBox6.Paint += pictureBox6_Paint; // Subscribe to the Paint event to display text if no image is present
            pictureBox6.AllowDrop = true; // Enable drag-and-drop functionality for pictureBox6
            pictureBox6.DragEnter += pictureBox6_DragEnter; // Handle the drag-enter event
            pictureBox6.DragDrop += pictureBox6_DragDrop; // Handle the drag-drop event
        }

        // Event handler for pictureBox6 click, allows the user to select an image file from their computer
        private void pictureBox6_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Images|*.jpg;*.jpeg;*.png;*.bmp"; // Filter to allow image files only

            // Show the file dialog and check if the user selects a file
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                pictureBox6.Image = Image.FromFile(ofd.FileName); // Set the selected image to pictureBox6
                pictureBox6.Invalidate(); // Force the pictureBox to be redrawn
            }
        }

        // Event handler for the Paint event of pictureBox6, displays a message if no image is loaded
        private void pictureBox6_Paint(object sender, PaintEventArgs e)
        {
            if (pictureBox6.Image == null)
            {
                string message = "You can add Photo";
                Font font = new Font("Arial", 10, FontStyle.Italic); // Define the font for the message
                SizeF textSize = e.Graphics.MeasureString(message, font); // Measure the size of the message text
                PointF location = new PointF(
                    (pictureBox6.Width - textSize.Width) / 2, // Center the message horizontally
                    (pictureBox6.Height - textSize.Height) / 2 // Center the message vertically
                );

                e.Graphics.DrawString(message, font, Brushes.Gray, location); // Draw the message on the pictureBox
            }
        }

        // Event handler for the DragEnter event, determines whether the dragged item is an image
        private void pictureBox6_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
                e.Effect = DragDropEffects.Copy; // Allow copy of the image file
            else
                e.Effect = DragDropEffects.None; // Prevent drag-and-drop if not an image file
        }

        // Event handler for the DragDrop event, handles dropping of image files onto pictureBox6
        private void pictureBox6_DragDrop(object sender, DragEventArgs e)
        {
            string[] files = (string[])e.Data.GetData(DataFormats.FileDrop); // Get the dropped file(s)
            if (files.Length > 0)
            {
                try
                {
                    pictureBox6.Image = Image.FromFile(files[0]); // Set the dropped image to pictureBox6
                    pictureBox6.Invalidate(); // Force the pictureBox to be redrawn
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Invalid image folder: " + ex.Message); // Show an error message if the image is invalid
                }
            }
        }

        // Event handlers for the TextChanged events of multiple textboxes (currently not in use)
        private void textBox1_TextChanged(object sender, EventArgs e) { }
        private void textBox2_TextChanged(object sender, EventArgs e) { }
        private void textBox3_TextChanged(object sender, EventArgs e) { }
        private void textBox4_TextChanged(object sender, EventArgs e) { }
        private void textBox5_TextChanged(object sender, EventArgs e) { }
        private void textBox6_TextChanged(object sender, EventArgs e) { }
        private void textBox7_TextChanged(object sender, EventArgs e) { }

        // Event handler for button1 click, validates the product data and saves it to the database
        private void button1_Click(object sender, EventArgs e)
        {
            // Get the values entered in the textboxes
            string name = textBox1.Text.Trim();
            string description = textBox2.Text.Trim();

            // Validate and parse the price
            decimal price = 0;
            if (string.IsNullOrWhiteSpace(textBox3.Text) || !decimal.TryParse(textBox3.Text.Trim(), out price))
            {
                MessageBox.Show("Please enter a valid price.");
                return; // Stop further execution if the price is invalid
            }

            string brand = textBox4.Text.Trim();
            string category = textBox5.Text.Trim();
            string tags = textBox6.Text.Trim();

            // Validate and parse the carbon footprint
            decimal carbon = 0;
            if (string.IsNullOrWhiteSpace(textBox7.Text) || !decimal.TryParse(textBox7.Text.Trim(), out carbon))
            {
                MessageBox.Show("Please enter a valid estimated carbon footprint.");
                return; // Stop further execution if the carbon footprint is invalid
            }

            byte[] imageBytes = null;
            // If an image is selected, convert it to a byte array for storage in the database
            if (pictureBox6.Image != null)
            {
                using (var ms = new System.IO.MemoryStream())
                {
                    pictureBox6.Image.Save(ms, pictureBox6.Image.RawFormat); // Save image to memory stream
                    imageBytes = ms.ToArray(); // Convert image to byte array
                }
            }

            // Database connection string
            string connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\ozlem\source\repos\SustainableProduct\SustainableProduct\Product.mdf;Integrated Security=True";

            // Insert product data into the Products table in the database
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = @"INSERT INTO Products 
            (ProductName, Description, Price, Brand, Category, SustainabilityTags, EstimatedCarbonFootprint, Image)
            VALUES (@name, @desc, @price, @brand, @cat, @tags, @carbon, @image)";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    // Add parameters to the SQL command to prevent SQL injection
                    cmd.Parameters.AddWithValue("@name", name);
                    cmd.Parameters.AddWithValue("@desc", description);
                    cmd.Parameters.AddWithValue("@price", price);
                    cmd.Parameters.AddWithValue("@brand", brand);
                    cmd.Parameters.AddWithValue("@cat", category);
                    cmd.Parameters.AddWithValue("@tags", tags);
                    cmd.Parameters.AddWithValue("@carbon", carbon);
                    if (imageBytes != null)
                        cmd.Parameters.Add("@image", SqlDbType.VarBinary).Value = imageBytes; // Add the image data
                    else
                        cmd.Parameters.Add("@image", SqlDbType.VarBinary).Value = DBNull.Value; // Add DBNull if no image is selected

                    conn.Open(); // Open the database connection
                    cmd.ExecuteNonQuery(); // Execute the insert query
                    conn.Close(); // Close the database connection
                }
            }

            // Show a message that the product was successfully added
            MessageBox.Show("Product successfully added!");
        }

        // Event handler for button2 click, opens Form4 and hides Form3
        private void button2_Click(object sender, EventArgs e)
        {
            Form4 form4 = new Form4();
            form4.Show();
            this.Hide();
        }

        // Event handler for label1 click (currently not used)
        private void label1_Click(object sender, EventArgs e) { }
    }
}
