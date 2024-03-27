using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using System.Text.RegularExpressions;

namespace Heavenly_SMS
{
    public partial class Register : Form
    {
        private string connectionString = "Data Source=localhost;Initial Catalog=qrmdb;User ID=root;Password=aggrey256";
        public Register()
        {
            InitializeComponent();
        }

        private void loginhere_Click(object sender, EventArgs e)
        {
            LogIn loginForm = new LogIn();
            this.Hide();
            loginForm.Show();
        }

        private void lbclose_MouseEnter(object sender, EventArgs e)
        {
            lbclose.ForeColor = Color.White;
        }

        private void lbclose_MouseLeave(object sender, EventArgs e)
        {
            lbclose.ForeColor = Color.Black;
        }

        private void lbclose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }

        private bool IsValidPhoneNumber(string phoneNumber)
        {
            // Use a regular expression to validate the phone number
            // This example uses a simple format: 10 digits without spaces or dashes
            string pattern = @"^\d{10}$";
            Regex regex = new Regex(pattern);

            return regex.IsMatch(phoneNumber);
        }

        private void btnregister_Click(object sender, EventArgs e)
        {
            // Get data from textboxes or other input controls
            string firstName = txtfname.Text;
            string lastName = txtlname.Text;
            string email = txtmail.Text;
            string phonenumber = txttel.Text;
            string username = txtuname.Text;
            string password = txtpwd.Text;
            string role = cmbrole.Text;

            // Validate email
            if (!IsValidEmail(email))
            {
                MessageBox.Show("Please enter a valid email address.");
                return; // Exit the method if email is not valid
            }

            // Validate phone number
            if (!IsValidPhoneNumber(phonenumber))
            {
                MessageBox.Show("Please enter a valid phone number.");
                return; // Exit the method if phone number is not valid
            }

            // Create the SQL query with parameters to prevent SQL injection
            string query = "INSERT INTO reg (FirstName, LastName, Email, Phonenumber, Username, Password, Role) VALUES (@Firstname, @Lastname, @Email, @Phonenumber, @Username, @Password, @Role)";

            // Use a using statement to ensure the connection is properly closed
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();

                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    // Add parameters to the query
                    command.Parameters.AddWithValue("@FirstName", firstName);
                    command.Parameters.AddWithValue("@LastName", lastName);
                    command.Parameters.AddWithValue("@Email", email);
                    command.Parameters.AddWithValue("@Phonenumber", phonenumber);
                    command.Parameters.AddWithValue("@Username", username);
                    command.Parameters.AddWithValue("@Password", password);
                    command.Parameters.AddWithValue("@Role", role);

                    // Execute the query
                    int rowsAffected = command.ExecuteNonQuery();

                    // Check if the query was successful
                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Data inserted successfully!");

                        // Clear textboxes
                        txtfname.Text = "";
                        txtlname.Text = "";
                        txtmail.Text = "";
                        txttel.Text = "";
                        txtuname.Text = "";
                        txtpwd.Text = "";
                        cmbrole.SelectedIndex = -1; // Clear combobox selection
                    }
                    else
                    {
                        MessageBox.Show("Failed to insert data.");
                    }
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                MySqlConnection connection = new MySqlConnection("Server=localhost;port=3306;uid=root;password=aggrey256");
                string updateQuery = "UPDATE qrmdb.reg SET FirstName = @FirstName, LastName = @LastName, Email = @Email, Phonenumber = @Phonenumber, Username = @Username, Password = @Password, Role = @Role WHERE Firstname = @Firstname";

                connection.Open();

                using (MySqlCommand command = new MySqlCommand(updateQuery, connection))
                {
                    // Add parameters to the query
                    command.Parameters.AddWithValue("@FirstName", txtfname.Text);
                    command.Parameters.AddWithValue("@LastName", txtlname.Text);
                    command.Parameters.AddWithValue("@Email", txtmail.Text);
                    command.Parameters.AddWithValue("@Phonenumber", txttel.Text);
                    command.Parameters.AddWithValue("@Username", txtuname.Text);
                    command.Parameters.AddWithValue("@Password", txtpwd.Text);
                    command.Parameters.AddWithValue("@Role", cmbrole.SelectedItem);

                    if (command.ExecuteNonQuery() == 1)
                    {
                        MessageBox.Show("Edited");

                        // Clear textboxes
                        txtfname.Text = "";
                        txtlname.Text = "";
                        txtmail.Text = "";
                        txttel.Text = "";
                        txtuname.Text = "";
                        txtpwd.Text = "";
                        cmbrole.SelectedIndex = -1; // Clear combobox selection
                    }
                    else
                    {
                        MessageBox.Show("Not edited");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                using (MySqlConnection connection = new MySqlConnection("Server=localhost;port=3306;uid=root;password=aggrey256;database=qrmdb"))
                {
                    connection.Open();

                    string deleteQuery = "DELETE FROM reg WHERE Firstname = @Firstname";

                    using (MySqlCommand command = new MySqlCommand(deleteQuery, connection))
                    {
                        command.Parameters.AddWithValue("@Firstname", txtfname.Text);

                        if (command.ExecuteNonQuery() == 1)
                        {
                            MessageBox.Show("Deleted");

                            // Clear textboxes
                            txtfname.Text = "";
                            txtlname.Text = "";
                            txtmail.Text = "";
                            txttel.Text = "";
                            txtuname.Text = "";
                            txtpwd.Text = "";
                            cmbrole.SelectedIndex = -1; // Clear combobox selection
                        }
                        else
                        {
                            MessageBox.Show("Not deleted");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Register_Load(object sender, EventArgs e)
        {
            //the code below will enable combobox to pick from the database in the "role" column
           // try
            //{
              //  MySqlConnection connection = new MySqlConnection("DataSource=localhost;port=3306;uid=root;password=aggrey256");
                //string selectQuery = "SELECT * FROM qrmdb.reg";
                //connection.Open();
                //MySqlCommand command = new MySqlCommand(selectQuery, connection);
                //MySqlDataReader reader = command.ExecuteReader();
                //while (reader.Read())
                //{
                  //  cmbrole.Items.Add(reader.GetString("Role"));
                //}
            //}
            //catch (Exception ex)
            //{
              //  MessageBox.Show(ex.Message);
            //}
        }

        private void guna2PictureBox2_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}

