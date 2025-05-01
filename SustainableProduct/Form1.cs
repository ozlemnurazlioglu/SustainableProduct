using System;
using System.Data.SqlClient;
using System.Linq;
using System.Windows.Forms;

namespace SustainableProduct
{
    public partial class Form1 : Form
    {
        // Delegate for registration event
        public delegate void RegistrationEventHandler(object sender, EventArgs e);
        public event RegistrationEventHandler OnRegistration;

        // Constructor to initialize the form
        public Form1()
        {
            InitializeComponent();
        }

        // Event handler for form load, currently empty
        private void Form1_Load(object sender, EventArgs e)
        {
        }

        // Event handler for button click to handle user registration
        private void button1_Click(object sender, EventArgs e)
        {
            string username = textBoxUsername.Text;
            string password = textBoxPassword.Text;

            // Validate the input fields using LINQ
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                MessageBox.Show("Username and password cannot be empty!");
                return;
            }

            // Use the delegate to handle the registration process
            OnRegistration?.Invoke(sender, e);

            // Proceed with the database interaction
            RegisterUser(username, password);
        }

        // Method to handle user registration in the database
        private void RegisterUser(string username, string password)
        {
            string connectionString = @"Server=(localdb)\MSSQLLocalDB;AttachDbFilename=C:\Users\ozlem\Source\Repos\SustainableProduct\SustainableProduct\Registration.mdf;Integrated Security=True;";

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    // SQL query to insert registration details into the database
                    string insertQuery = "INSERT INTO dbo.Registration (username, password) VALUES (@username, @password)";
                    SqlCommand insertCommand = new SqlCommand(insertQuery, connection);
                    insertCommand.Parameters.AddWithValue("@username", username);
                    insertCommand.Parameters.AddWithValue("@password", password);

                    // Execute the insert query using LINQ to execute the command
                    var result = new[] { insertCommand }.FirstOrDefault();
                    result?.ExecuteNonQuery();

                    // Show a message indicating success
                    MessageBox.Show("Registration added successfully.");
                }

                // Open Form2 and hide the current form (Form1)
                OpenForm2();
            }
            catch (Exception ex)
            {
                // Show error message in case of an exception
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        // Use LINQ to open Form2
        private void OpenForm2()
        {
            var form2 = new Form2();
            form2.Show();
            this.Hide();
        }

        // Event handler for link label click, opens Form2 and hides Form1
        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            OpenForm2();
        }

        // Event handler for username text changed
        private void textBoxUsername_TextChanged(object sender, EventArgs e)
        {
            // Handle changes if needed
        }

        // Event handler for password text changed
        private void textBoxPassword_TextChanged(object sender, EventArgs e)
        {
            // Handle changes if needed
        }

        private void label2_Click(object sender, EventArgs e)
        {
            // Handle label click if needed
        }
    }
}
