using System;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace SustainableProduct
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string username = textBoxUsername.Text;
            string password = textBoxPassword.Text;

            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                MessageBox.Show("Kullanıcı adı ve şifre boş olamaz!");
                return;
            }

            try
            {
                string connectionString = @"Server=(localdb)\MSSQLLocalDB;AttachDbFilename=C:\Users\ozlem\Source\Repos\SustainableProduct\SustainableProduct\Registration.mdf;Integrated Security=True;";

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    string insertQuery = "INSERT INTO dbo.Registration (username, password) VALUES (@username, @password)";
                    SqlCommand insertCommand = new SqlCommand(insertQuery, connection);
                    insertCommand.Parameters.AddWithValue("@username", username);
                    insertCommand.Parameters.AddWithValue("@password", password);
                    insertCommand.ExecuteNonQuery();

                    string countQuery = "SELECT COUNT(*) FROM dbo.Registration";
                    SqlCommand countCommand = new SqlCommand(countQuery, connection);
                    int count = (int)countCommand.ExecuteScalar();

                    MessageBox.Show("Kayıt başarıyla eklendi.");
                }
                Form2 form2 = new Form2();
                form2.Show();
                this.Hide();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hata: " + ex.Message);
            }
        }



        private void textBoxUsername_TextChanged(object sender, EventArgs e)
        {
            string username = textBoxUsername.Text;
            string connectionString = @"Server=(localdb)\MSSQLLocalDB;AttachDbFilename=C:\Users\ozlem\Source\Repos\SustainableProduct\SustainableProduct\Registration.mdf;Integrated Security=True;";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    string query = "INSERT INTO dbo.Registration (username) VALUES (@username)";
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@username", username);
                    command.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Username kayıt hatası: " + ex.Message);
                }
            }
        }


        private void textBoxPassword_TextChanged(object sender, EventArgs e)
        {

        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Form2 form2 = new Form2();
            form2.Show();
            this.Hide();
        }
    }
}
