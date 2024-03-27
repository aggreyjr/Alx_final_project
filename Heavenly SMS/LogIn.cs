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

namespace Heavenly_SMS
{
    public partial class LogIn : Form
    {
        public LogIn()
        {
            InitializeComponent();

            //size of the textbox area of password
            //this.pswd.AutoSize = false;
            //this.pswd.Size = new Size(this.Size.Width, 35);
            //this.pswd.Size = new Size(this.Size., 50);
        }
        //multi user login
        private void btnlogin_Click(object sender, EventArgs e)
        {
            string connectionString = "Server=localhost;Database=qrmdb;Uid=root;Pwd=aggrey256;";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    string username = uname.Text;
                    string password = pswd.Text;

                    string query = "SELECT * FROM reg WHERE Username = @username";
                    MySqlCommand command = new MySqlCommand(query, connection);
                    command.Parameters.AddWithValue("@Username", username);

                    MySqlDataReader reader = command.ExecuteReader();

                    if (reader.Read())
                    {
                        string storedPassword = reader["Password"].ToString();
                        string Role = reader["Role"].ToString(); 

                        if (VerifyPassword(password, storedPassword))
                        if (Role == "Admin") 
                        {
                            MessageBox.Show("Login successful.");
                            Admins adminForm = new Admins();
                            adminForm.Show();
                            this.Hide();
                        }
                        else if (Role == "Staff")
                            {
                                MessageBox.Show("Login successful.");
                                Stuff1 staffForm = new Stuff1();
                                staffForm.Show();
                                this.Hide();
                            }
                        else
                            {
                                MessageBox.Show("Login successful.");
                                Client homeForm = new Client();
                                homeForm.Show();
                                this.Hide();
                            }
                        else
                        {
                            MessageBox.Show("Incorrect password.");
                        }
                    }
                    else
                    {
                        MessageBox.Show("User not found.");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
            }

            if (checkBox1.Checked == true)
            {
                Properties.Settings.Default.Username = uname.Text;
                Properties.Settings.Default.Password = pswd.Text;
                Properties.Settings.Default.Save();
            }else
            {
                Properties.Settings.Default.Username = "";
                Properties.Settings.Default.Password = "";
                Properties.Settings.Default.Save();
            }
        }

        private bool VerifyPassword(string inputPassword, string hashedPassword)
        {
            // Implement password verification logic here.
            return inputPassword == hashedPassword;
        }

        private void registerhere_Click(object sender, EventArgs e)
        {
            Register regForm = new Register();
            this.Hide();
            regForm.Show();
        }

        private void lblclose_MouseEnter(object sender, EventArgs e)
        {
            lblclose.ForeColor = Color.White;
        }

        private void lblclose_MouseLeave(object sender, EventArgs e)
        {
            lblclose.ForeColor = Color.Black;
        }

        private void lblclose_Click(object sender, EventArgs e)
        {
            //this.Close();
            Application.Exit();
        }

        private void guna2PictureBox2_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            
        }

        private void LogIn_Load(object sender, EventArgs e)
        {
            if(Properties.Settings.Default.Username != string.Empty)
            {
                uname.Text = Properties.Settings.Default.Username;
                pswd.Text = Properties.Settings.Default.Password;
            }
        }
    }
    
}
