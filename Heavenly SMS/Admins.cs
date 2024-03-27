using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using MySql.Data.MySqlClient;
using Heavenly_SMS.BusinessTire;

namespace Heavenly_SMS
{
    public partial class Admins : Form
    {
        private string connectionString = "Data Source=localhost;Initial Catalog=qrmdb;User ID=root;Password=aggrey256";
        private MySqlCommand cmd;
        private MySqlDataReader reader;
        private MySqlDataAdapter da;
        private string imagelocation;

        public Admins()
        {
            InitializeComponent();
        }

        private void tabPage12_Click(object sender, EventArgs e)
        {

        }

        private void guna2GradientButton1_Click(object sender, EventArgs e)
        {
            LogIn lgnForm = new LogIn();
            lgnForm.Show();
            this.Hide();
        }

        private void guna2HtmlLabel9_MouseHover(object sender, EventArgs e)
        {
            guna2HtmlLabel9.ForeColor = Color.White;
        }

        private void guna2HtmlLabel9_MouseLeave(object sender, EventArgs e)
        {
            guna2HtmlLabel9.ForeColor = Color.Black;
        }

        private void guna2HtmlLabel9_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void guna2ContainerControl2_Click(object sender, EventArgs e)
        {

        }

        private void guna2ToggleSwitch3_CheckedChanged(object sender, EventArgs e)
        {
            if (guna2ToggleSwitch3.Checked) // Toggle switch is on
            {
                string query = "SELECT * FROM reg";

                try
                {
                    using (MySqlConnection connection = new MySqlConnection(connectionString))
                    {
                        MySqlCommand command = new MySqlCommand(query, connection);
                        connection.Open();
                        using (MySqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.HasRows)
                            {
                                DataTable dataTable = new DataTable();
                                dataTable.Load(reader);

                                // Bind the DataTable to your DataGridView
                                guna2DataGridView3.DataSource = dataTable;
                            }
                            else
                            {
                                MessageBox.Show("No data found.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else // Toggle switch is off
            {
                guna2DataGridView3.DataSource = null; // Clear the DataGridView
            }
        }

        private void guna2ToggleSwitch2_CheckedChanged(object sender, EventArgs e)
        {
            if (guna2ToggleSwitch2.Checked) // Toggle switch is on
            {
                string query = "SELECT * FROM purchases";

                try
                {
                    using (MySqlConnection connection = new MySqlConnection(connectionString))
                    {
                        MySqlCommand command = new MySqlCommand(query, connection);
                        connection.Open();
                        using (MySqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.HasRows)
                            {
                                DataTable dataTable = new DataTable();
                                dataTable.Load(reader);

                                // Bind the DataTable to your DataGridView
                                guna2DataGridView1.DataSource = dataTable;
                            }
                            else
                            {
                                MessageBox.Show("No data found.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else // Toggle switch is off
            {
                guna2DataGridView1.DataSource = null; // Clear the DataGridView
            }
        }

        private void guna2ToggleSwitch1_CheckedChanged(object sender, EventArgs e)
        {
            if (guna2ToggleSwitch1.Checked) // Toggle switch is on
            {
                string query = "SELECT * FROM appointments";

                try
                {
                    using (MySqlConnection connection = new MySqlConnection(connectionString))
                    {
                        MySqlCommand command = new MySqlCommand(query, connection);
                        connection.Open();
                        using (MySqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.HasRows)
                            {
                                DataTable dataTable = new DataTable();
                                dataTable.Load(reader);

                                // Bind the DataTable to your DataGridView
                                guna2DataGridView2.DataSource = dataTable;
                            }
                            else
                            {
                                MessageBox.Show("No data found.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else // Toggle switch is off
            {
                guna2DataGridView2.DataSource = null; // Clear the DataGridView
            }
        }

        private void guna2ToggleSwitch4_CheckedChanged(object sender, EventArgs e)
        {
            if (guna2ToggleSwitch4.Checked) // Toggle switch is on
            {
                string query = "SELECT * FROM receipts";

                try
                {
                    using (MySqlConnection connection = new MySqlConnection(connectionString))
                    {
                        MySqlCommand command = new MySqlCommand(query, connection);
                        connection.Open();
                        using (MySqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.HasRows)
                            {
                                DataTable dataTable = new DataTable();
                                dataTable.Load(reader);

                                // Bind the DataTable to your DataGridView
                                guna2DataGridView4.DataSource = dataTable;
                            }
                            else
                            {
                                MessageBox.Show("No data found.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else // Toggle switch is off
            {
                guna2DataGridView4.DataSource = null; // Clear the DataGridView
            }
        }

        private void guna2ToggleSwitch5_CheckedChanged(object sender, EventArgs e)
        {
            if (guna2ToggleSwitch5.Checked) // Toggle switch is on
            {
                string query = "SELECT * FROM payment";

                try
                {
                    using (MySqlConnection connection = new MySqlConnection(connectionString))
                    {
                        MySqlCommand command = new MySqlCommand(query, connection);
                        connection.Open();
                        using (MySqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.HasRows)
                            {
                                DataTable dataTable = new DataTable();
                                dataTable.Load(reader);

                                // Bind the DataTable to your DataGridView
                                guna2DataGridView5.DataSource = dataTable;
                            }
                            else
                            {
                                MessageBox.Show("No data found.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else // Toggle switch is off
            {
                guna2DataGridView5.DataSource = null; // Clear the DataGridView
            }
        }

        private void guna2ToggleSwitch6_CheckedChanged(object sender, EventArgs e)
        {
            if (guna2ToggleSwitch6.Checked) // Toggle switch is on
            {
                string query = "SELECT * FROM documents";

                try
                {
                    using (MySqlConnection connection = new MySqlConnection(connectionString))
                    {
                        MySqlCommand command = new MySqlCommand(query, connection);
                        connection.Open();
                        using (MySqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.HasRows)
                            {
                                DataTable dataTable = new DataTable();
                                dataTable.Load(reader);

                                // Bind the DataTable to your DataGridView
                                guna2DataGridView6.DataSource = dataTable;
                            }
                            else
                            {
                                MessageBox.Show("No data found.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else // Toggle switch is off
            {
                guna2DataGridView6.DataSource = null; // Clear the DataGridView
            }
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
    }
}
