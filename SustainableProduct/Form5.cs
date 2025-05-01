using System;
using System.Data.SqlClient;
using System.Linq;
using System.Windows.Forms;

namespace SustainableProduct
{
    public partial class Form5 : Form
    {
        // Delegate for login logic
        public delegate void LoginEventHandler(object sender, EventArgs e);

        // Constructor to initialize the form
        public Form5()
        {
            InitializeComponent();
        }

        // Event handler for the button2 click, navigates to Form1
        private void button2_Click(object sender, EventArgs e)
        {
            NavigateToForm<Form1>();
        }

        // Event handler for the button1 click, handles login logic
        private void button1_Click(object sender, EventArgs e)
        {
            string email = textBox1.Text; // Email textbox
            string password = textBox2.Text; // Password textbox

            // Check if the email or password fields are empty or whitespace
            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
            {
                MessageBox.Show("Lütfen e-posta ve şifre girin.");
                return; // Stop execution if fields are empty
            }

            string connectionString = @"Server=(localdb)\MSSQLLocalDB;AttachDbFilename=C:\Users\ozlem\Source\Repos\SustainableProduct\SustainableProduct\Registration.mdf;Integrated Security=True;";

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open(); // Open the database connection

                    // SQL query to check if the user exists with the given email and password using LINQ query for matching users
                    string query = "SELECT * FROM Registration WHERE username = @username AND password = @password";
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@username", email); // Add email as parameter to the query
                    command.Parameters.AddWithValue("@password", password); // Add password as parameter to the query

                    var userExists = command.ExecuteReader()
                                             .Cast<System.Data.SqlClient.SqlDataReader>()
                                             .Any(reader => reader["username"].ToString() == email && reader["password"].ToString() == password);

                    // Check if any user was found with the provided email and password
                    if (userExists)
                    {
                        MessageBox.Show("Login successful!"); // Show a success message
                        NavigateToForm<Form3>(); // Navigate to Form3 on successful login
                    }
                    else
                    {
                        MessageBox.Show("E-posta veya şifre yanlış."); // Show an error message if the login failed
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hata: " + ex.Message); // Display an error message in case of an exception
            }
        }

        // Helper method to navigate to a new form
        private void NavigateToForm<T>() where T : Form, new()
        {
            var form = new T();
            form.Show();
            this.Hide();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            // Currently no action when the text in textBox1 changes
        }

        private void label1_Click(object sender, EventArgs e)
        {
            // Currently no action when label1 is clicked
        }
    }
}
