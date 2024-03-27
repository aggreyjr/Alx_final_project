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
using System.IO;

namespace Heavenly_SMS
{
    public partial class Client : Form
    {
        private string connectionString = "Data Source=localhost;Initial Catalog=qrmdb;User ID=root;Password=aggrey256";
        private MySqlCommand cmd;
        private MySqlDataReader reader;
        private MySqlDataAdapter da;
        private string imagelocation;

        public Client()
        {
            InitializeComponent();
        }

        private void guna2HtmlLabel9_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void guna2HtmlLabel9_MouseHover(object sender, EventArgs e)
        {
            guna2HtmlLabel9.ForeColor = Color.White;
        }

        private void guna2HtmlLabel9_MouseLeave(object sender, EventArgs e)
        {
            guna2HtmlLabel9.ForeColor = Color.Black;
        }

        private void guna2GradientButton1_Click(object sender, EventArgs e)
        {
            LogIn lgnForm = new LogIn();
            lgnForm.Show();
            this.Hide();
        }

        private void tabPage3_Click(object sender, EventArgs e)
        {

        }

        private void tabPage7_Click(object sender, EventArgs e)
        {

        }

        private void btnRequest_Click(object sender, EventArgs e)
        {
            string name = txtname.Text.Trim();  // Trim to remove leading and trailing whitespace
            string email = txtmail.Text;
            string date = dtpdate.Value.ToString("yyyy-MM-dd");
            string phone = txtphone.Text;
            string time = cbtime.Text;
            string stylist = cbstylist.Text;
            string therapist = cbtherapist.Text;
            string service = cbservice.Text;
            string preferred = rbmail.Checked ? "Mail" : "Phone";

            bool checkBox1Value = cbyes.Checked;
            bool checkBox2Value = cbno.Checked;
            string firsttime = $"{(checkBox1Value ? "Yes" : "")} {(checkBox2Value ? "No" : "")}";

            if (string.IsNullOrWhiteSpace(name))
            {
                MessageBox.Show("Name cannot be empty. Please enter a valid Name.");
                return;  // Stop further processing
            }

            // Create the SQL query with parameters to prevent SQL injection
            string query = "INSERT INTO appointments (Name, Email, Date, Phone, Time, Stylist, Therapist, Preferred, Firsttime, Service) VALUES (@Name, @Email, @Date, @Phone, @Time, @Stylist, @Therapist, @Preferred, @Firsttime, @Service)";

            // Use a using statement to ensure the connection is properly closed
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();

                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    try
                    {
                        // Add parameters to the query
                        command.Parameters.AddWithValue("@Name", name);
                        command.Parameters.AddWithValue("@Date", date);
                        command.Parameters.AddWithValue("@Email", email);
                        command.Parameters.AddWithValue("@Phone", phone);
                        command.Parameters.AddWithValue("@Time", time);
                        command.Parameters.AddWithValue("@Stylist", stylist);
                        command.Parameters.AddWithValue("@Therapist", therapist);
                        command.Parameters.AddWithValue("@Preferred", preferred);
                        command.Parameters.AddWithValue("@Firsttime", firsttime);
                        command.Parameters.AddWithValue("@Service", service);

                        // Execute the query
                        int rowsAffected = command.ExecuteNonQuery();

                        // Check if the query was successful
                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Report sent!");
                        }
                        else
                        {
                            MessageBox.Show("Request aborted.");
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnchwn_Click(object sender, EventArgs e)
        {
            try
            {
                MySqlConnection connection = new MySqlConnection("Server=localhost;port=3306;uid=root;password=aggrey256");
                string updateQuery = "UPDATE qrmdb.reg SET Username = @Username, Password = @Password WHERE Username = @Username";

                connection.Open();

                using (MySqlCommand command = new MySqlCommand(updateQuery, connection))
                {
                    // Add parameters to the query
                    command.Parameters.AddWithValue("@Username", txtuname.Text);
                    command.Parameters.AddWithValue("@Password", txtpwd.Text);

                    if (command.ExecuteNonQuery() == 1)
                    {
                        MessageBox.Show("Edited");

                        // Clear textboxes
                        txtuname.Text = "";
                        txtpwd.Text = "";
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

        private void btnimgr_Click(object sender, EventArgs e)
        {
            //Graphics g = Graphics.FromHwnd(Handle);
            Graphics g = tabPage6.CreateGraphics();

            imageList1.ColorDepth = ColorDepth.Depth32Bit;
            imageList1.ImageSize = new Size(255, 255);

            imageList1.Images.Add(Image.FromFile(@"C:\wamp64\www\Heavenly SMS\Heavenly SMS\pics\haircut.png"));
            imageList1.Images.Add(Image.FromFile(@"C:\wamp64\www\Heavenly SMS\Heavenly SMS\pics\hr.jpeg"));
            imageList1.Images.Add(Image.FromFile(@"C:\wamp64\www\Heavenly SMS\Heavenly SMS\pics\logo.png"));
            imageList1.Images.Add(Image.FromFile(@"C:\wamp64\www\Heavenly SMS\Heavenly SMS\pics\mani.png"));
            imageList1.Images.Add(Image.FromFile(@"C:\wamp64\www\Heavenly SMS\Heavenly SMS\pics\mass.png"));
            imageList1.Images.Add(Image.FromFile(@"C:\wamp64\www\Heavenly SMS\Heavenly SMS\pics\scrub.jpeg"));

            for (int i=0; i<imageList1.Images.Count;i++)
            {
                imageList1.Draw(g, new Point(40, 40), i);
                System.Threading.Thread.Sleep(1000);
            }

        }
    }
}
