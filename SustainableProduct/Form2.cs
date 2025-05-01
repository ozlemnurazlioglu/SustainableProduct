using System;
using System.Data.SqlClient;
using System.Linq;
using System.Windows.Forms;

namespace SustainableProduct
{
    public partial class Form2 : Form
    {
        // Delegate for login event
        public delegate void LoginEventHandler(object sender, EventArgs e);
        public event LoginEventHandler OnLogin;

        // Constructor to initialize the form
        public Form2()
        {
            InitializeComponent();
        }

        // Event handler for button2 click, opens Form1 and hides Form2
        private void button2_Click(object sender, EventArgs e)
        {
            OpenForm<Form1>();
        }

        // Event handler for button1 click, handling the login logic
        private void button1_Click(object sender, EventArgs e)
        {
            string email = textBox1.Text;
            string password = textBox2.Text;

            // Validate inputs using LINQ
            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
            {
                MessageBox.Show("Please enter your email and password.");
                return;
            }

            // Trigger the login event before checking the database
            OnLogin?.Invoke(sender, e);

            // Check login credentials using a helper method and LINQ
            if (ValidateLogin(email, password))
            {
                MessageBox.Show("Login successful!");
                OpenForm<Form3>();
            }
            else
            {
                MessageBox.Show("Email or password is incorrect.");
            }
        }

        // Method to validate login credentials using LINQ
        private bool ValidateLogin(string email, string password)
        {
            string connectionString = @"Server=(localdb)\MSSQLLocalDB;AttachDbFilename=C:\Users\ozlem\Source\Repos\SustainableProduct\SustainableProduct\Registration.mdf;Integrated Security=True;";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    string query = "SELECT COUNT(*) FROM Registration WHERE username = @username AND password = @password";
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@username", email);
                    command.Parameters.AddWithValue("@password", password);

                    // Execute the query using LINQ to fetch the count
                    int count = new[] { command.ExecuteScalar() }.FirstOrDefault() as int? ?? 0;

                    return count > 0; // Returns true if login is successful
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                    return false;
                }
            }
        }

        // Helper method to open a form
        private void OpenForm<T>() where T : Form, new()
        {
            var form = new T();
            form.Show();
            this.Hide();
        }

        // Event handler for link label click, opens Form5 and hides Form2
        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            OpenForm<Form5>();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            // Handle text changes if necessary
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            // Handle text changes if necessary
        }

        private void label1_Click(object sender, EventArgs e)
        {
            // Handle label clicks if necessary
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            // Perform form load actions if necessary
        }
    }
}
