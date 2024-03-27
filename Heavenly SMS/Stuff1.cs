using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Heavenly_SMS.BusinessTire;
using System.IO;
using MySql.Data.MySqlClient;
using System.Drawing.Printing;

namespace Heavenly_SMS
{
    public partial class Stuff1 : Form
    {
        private string connectionString = "Data Source=localhost;Initial Catalog=qrmdb;User ID=root;Password=aggrey256";
        private MySqlCommand cmd;
        private MySqlDataReader reader;
        private MySqlDataAdapter da;
        private string imagelocation;

        public object BarcodeDrawFactory { get; private set; }

        public Stuff1()
        {
            InitializeComponent();
        }

        public void Uploadfiles(string file)
        {
            try
            {
                byte[] contents;

                using (FileStream fstream = File.OpenRead(file))
                {
                    contents = new byte[fstream.Length];
                    fstream.Read(contents, 0, (int)fstream.Length);
                }

                using (MySqlConnection con = new MySqlConnection(connectionString))
                using (MySqlCommand cmd = new MySqlCommand("INSERT INTO documents (id, files) VALUES (@id, @files)", con))
                {
                    con.Open();

                    cmd.Parameters.AddWithValue("@id", txtid.Text);
                    cmd.Parameters.AddWithValue("@files", contents);

                    cmd.ExecuteNonQuery();

                    MessageBox.Show("Upload done!", "PDF Format", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (MySqlException ex)
            {
                MessageBox.Show($"Database Error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void Downloadfiles(string file)
        {
            using (MySqlConnection con = new MySqlConnection(connectionString))
            {
                con.Open();
                bool em = false;
                using (cmd = new MySqlCommand("select files from documents where id = @id", con))
                {
                    cmd.Parameters.AddWithValue("@id", txtid.Text);

                    using (reader = cmd.ExecuteReader(CommandBehavior.Default))
                    {
                        if (reader.Read())
                        {
                            em = true;
                            byte[] fileData = (byte[])reader.GetValue(0);
                            using (FileStream fs = new FileStream(file, FileMode.Create, FileAccess.ReadWrite))
                            using (BinaryWriter bw = new BinaryWriter(fs))
                            {
                                bw.Write(fileData);
                            }
                            MessageBox.Show("Download complete!", "PDF Formart", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        if (em == false)
                        {
                            MessageBox.Show("Try Again", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                        reader.Close();
                    }
                }
            }
        }

        private void DisplayDataForm(DataTable dataTable)
        {
            // For example, create a new form, set its DataSource property to the DataTable, and show the form.
            DataGridView dataGridView = new DataGridView();
            dataGridView.DataSource = dataTable;

            Form dataForm = new Form();
            dataForm.Controls.Add(dataGridView);

            // Optionally set other properties of the form
            dataForm.Text = "Data from File";
            dataForm.Size = new Size(600, 400);

            // Show the form
            dataForm.ShowDialog();
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

        private void btncalculate_Click(object sender, EventArgs e)
        {
            try
            {
                // Check validation
                if (FormValidation.IsEmpty(txtname.Text) || FormValidation.IsEmpty(dtdate.Text) ||
                    FormValidation.IsEmpty(txtclients.Text) || FormValidation.IsEmpty(txttotal.Text))
                {
                    MessageBox.Show("Validation failed. All fields are required.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    // Prepare result for displaying
                    txtname.Enabled = false;
                    dtdate.Enabled = false;
                    txtclients.Enabled = false;
                    rbbarber.Enabled = false;
                    rbmassage.Enabled = false;
                    rbloan.Enabled = false;
                    txttotal.Enabled = false;

                    richTextResult.Visible = true;

                    StringBuilder resultText = new StringBuilder();

                    if (rbbarber.Checked)
                    {
                        Massage_Therapist newtherapist = new Massage_Therapist(txtname.Text, dtdate.Text, txtclients.Text,
                            double.TryParse(txttotal.Text, out double total) ? total : 0, true);

                        resultText.AppendLine("Barber Commission Income Details");
                        resultText.AppendLine($"Total Commission: {newtherapist.CalculateCommission(true).ToString("C")}");
                        resultText.AppendLine($"Commission Rate: {newtherapist.GetCommissionRate()}");
                        resultText.AppendLine("\nNote: Barber has no loan to deduct");
                    }
                    else if (rbmassage.Checked)
                    {
                        Barber abarb = new Barber(txtname.Text, dtdate.Text, txtclients.Text,
                            double.TryParse(txttotal.Text, out double total) ? total : 0);

                        resultText.AppendLine("Massage therapist Commission Income Details");
                        resultText.AppendLine($"Total Commission: {abarb.CalculateCommission(true).ToString("C")}");
                        resultText.AppendLine($"Commission Rate: {abarb.GetCommissionRate()}");
                    }
                    else
                    {
                        Loan aloan = new Loan(txtname.Text, dtdate.Text, txtclients.Text,
                            double.TryParse(txttotal.Text, out double total) ? total : 0);

                        resultText.AppendLine("Employee loan Deduction Details");
                        resultText.AppendLine($"Total Deduction: {aloan.CalculateCommission(true).ToString("C")}");
                        resultText.AppendLine($"Deduction Rate: {aloan.GetCommissionRate()}");
                    }

                    richTextResult.Text = resultText.ToString();
                    //this.Width = 750;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnupload_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog dlg = new OpenFileDialog() { Filter = "Text Documents (*.pdf;*.txt;*.docx;*.rtf;*.xlsx) |*.pdf;*.txt;*.docx;*.rtf;*.xlsx", ValidateNames = true })
            {
                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    DialogResult dialog = MessageBox.Show("Are you sure?", "PDF Formart", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (dialog == DialogResult.Yes)
                    {
                        string filename = dlg.FileName;
                        Uploadfiles(filename);
                    }
                }
            }
        }

        private void btndownload_Click(object sender, EventArgs e)
        {
            using (SaveFileDialog sfd = new SaveFileDialog() { Filter = "Text Document (Text Documents (*.pdf;*.txt;*.docx;*.rtf;*.xlsx) |*.pdf;*.txt;*.docx;*.rtf;*.xlsx", ValidateNames = true })
            {
                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    DialogResult dialog = MessageBox.Show("Are you sure!?", "PDF Formart", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (dialog == DialogResult.Yes)
                    {
                        string filename = sfd.FileName;
                        Downloadfiles(filename);
                    }
                }
            }
        }

        private void btndocs_Click(object sender, EventArgs e)
        {
            try
            {
                string connectionString = "Server=localhost;Database=qrmdb;Uid=root;Pwd=aggrey256;";

                using (MySqlConnection con = new MySqlConnection(connectionString))
                {
                    con.Open();
                    string query = "SELECT * FROM documents";

                    using (MySqlDataAdapter adapter = new MySqlDataAdapter(query, con))
                    {
                        DataTable table = new DataTable();
                        adapter.Fill(table);
                        dataGridView1.DataSource = table;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Choose image (*.jpg;*.jpeg;*.png;*.gif)|*.jpg;*.jpeg;*.png;*.gif";
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                imagelocation = ofd.FileName.ToString();
                pbpic.ImageLocation = imagelocation;
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            try
            {
                using (MySqlConnection con = new MySqlConnection(connectionString))
                {
                    if (txtpicid.Text == "" || pbpic == null)
                    {
                        MessageBox.Show("No data", "Denied format", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    else
                    {
                        byte[] img = null;
                        FileStream stream = new FileStream(imagelocation, FileMode.Open, FileAccess.Read);
                        BinaryReader brs = new BinaryReader(stream);
                        img = brs.ReadBytes((int)stream.Length);

                        con.Open();
                        cmd = new MySqlCommand("INSERT INTO receipts(id, picture) VALUES(@id, @picture)", con);
                        cmd.Parameters.AddWithValue("@id", txtpicid.Text); // Add this line to set the ID parameter
                        cmd.Parameters.AddWithValue("@picture", img);
                        cmd.ExecuteNonQuery();
                        con.Close(); // Corrected line to close the connection
                        MessageBox.Show("Success!", "Picture format", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            try
            {
                OpenFileDialog ofd = new OpenFileDialog();
                ofd.Filter = "Choose Image(*.jpg;*.jpeg;*.png;*.gif) |*.jpg;*.jpeg;*.png;*.gif";
                DialogResult dialogResult = ofd.ShowDialog();

                if (dialogResult != DialogResult.OK)
                {
                    return;
                }

                imagelocation = ofd.FileName.ToString();
                pbpic.ImageLocation = imagelocation;

                byte[] img = null;
                using (FileStream stream = new FileStream(imagelocation, FileMode.Open, FileAccess.Read))
                using (BinaryReader brs = new BinaryReader(stream))
                {
                    img = brs.ReadBytes((int)stream.Length);
                }

                using (MySqlConnection con = new MySqlConnection(connectionString))
                {
                    con.Open();
                    using (MySqlCommand cmd = new MySqlCommand("UPDATE receipts SET picture=@picture WHERE id=@id", con))
                    {
                        cmd.Parameters.AddWithValue("@picture", img);
                        cmd.Parameters.AddWithValue("@id", txtpicid.Text);
                        cmd.ExecuteNonQuery();
                    }
                }

                MessageBox.Show("Update complete!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                string connectionString = "Server=localhost;Database=qrmdb;Uid=root;Pwd=aggrey256;";

                using (MySqlConnection con = new MySqlConnection(connectionString))
                {
                    con.Open();
                    string query = "SELECT * FROM receipts";

                    using (MySqlDataAdapter adapter = new MySqlDataAdapter(query, con))
                    {
                        DataTable table = new DataTable();
                        adapter.Fill(table);
                        dataGridView2.DataSource = table;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button8_Click(object sender, EventArgs e)
        {
            try
            {
                using (TextWriter text1 = new StreamWriter("E:\\PRINT.txt"))
                {
                    // Write header with column names
                    foreach (DataGridViewColumn column in dataGridView1.Columns)
                    {
                        text1.Write(column.HeaderText + "\t");
                    }
                    text1.WriteLine();

                    // Write data rows
                    foreach (DataGridViewRow row in dataGridView1.Rows)
                    {
                        foreach (DataGridViewCell cell in row.Cells)
                        {
                            text1.Write(cell.Value?.ToString() + "\t"); // Handle null values
                        }
                        text1.WriteLine();
                    }
                }

                MessageBox.Show("Done");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                using (TextWriter text1 = new StreamWriter("E:\\PRINT.txt"))
                {
                    // Write header with column names
                    foreach (DataGridViewColumn column in dataGridView1.Columns)
                    {
                        text1.Write(column.HeaderText + "\t");
                    }
                    text1.WriteLine();

                    // Write data rows
                    foreach (DataGridViewRow row in dataGridView1.Rows)
                    {
                        foreach (DataGridViewCell cell in row.Cells)
                        {
                            text1.Write(cell.Value?.ToString() + "\t"); // Handle null values
                        }
                        text1.WriteLine();
                    }
                }

                MessageBox.Show("Done");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                using (StreamReader reader = new StreamReader("E:\\PRINT.txt"))
                {
                    string line;

                    // Read header
                    line = reader.ReadLine();
                    string[] headers = line.Split('\t');

                    // Create a DataTable to store the data
                    DataTable dataTable = new DataTable();

                    // Add columns to the DataTable
                    foreach (string header in headers)
                    {
                        dataTable.Columns.Add(header.Trim());
                    }

                    // Read data rows
                    while ((line = reader.ReadLine()) != null)
                    {
                        string[] values = line.Split('\t');
                        dataTable.Rows.Add(values);
                    }

                    // Display the data in a new form or any other way you prefer
                    DisplayDataForm(dataTable);
                }

                MessageBox.Show("Read complete!");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            try
            {
                using (StreamReader reader = new StreamReader("E:\\PRINT.txt"))
                {
                    string line;

                    // Read header
                    line = reader.ReadLine();
                    string[] headers = line.Split('\t');

                    // Create a DataTable to store the data
                    DataTable dataTable = new DataTable();

                    // Add columns to the DataTable
                    foreach (string header in headers)
                    {
                        dataTable.Columns.Add(header.Trim());
                    }

                    // Read data rows
                    while ((line = reader.ReadLine()) != null)
                    {
                        string[] values = line.Split('\t');
                        dataTable.Rows.Add(values);
                    }

                    // Display the data in a new form or any other way you prefer
                    DisplayDataForm(dataTable);
                }

                MessageBox.Show("Read complete!");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button11_Click(object sender, EventArgs e)
        {
            // Get data from textboxes or other input controls
            string itemno = txtitemno.Text.Trim();  // Trim to remove leading and trailing whitespace
            string date = dTPdate.Value.ToString("yyyy-MM-dd");
            string item = txtitem.Text;
            string qty = txtQty.Text;
            string price = txtprice.Text;
            string receipts = rbyes.Checked ? "Yes" : "No";

            bool checkBox1Value = cbhair.Checked;
            bool checkBox2Value = cbmass.Checked;
            bool checkBox3Value = cbnails.Checked;
            string purpose = $"{(checkBox1Value ? "Hair" : "")} {(checkBox2Value ? "Massage" : "")} {(checkBox3Value ? "Nails" : "")}";

            if (string.IsNullOrWhiteSpace(itemno))
            {
                MessageBox.Show("ItemNo cannot be empty. Please enter a valid ItemNo.");
                return;  // Stop further processing
            }

            // Create the SQL query with parameters to prevent SQL injection
            string query = "INSERT INTO purchases (ItemNo, Date, Item, Qty, Price, Receipts, Purpose) VALUES (@ItemNo, @Date, @Item, @Qty, @Price, @Receipts, @Purpose)";

            // Use a using statement to ensure the connection is properly closed
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();

                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    try
                    {
                        // Add parameters to the query
                        command.Parameters.AddWithValue("@ItemNo", itemno);
                        command.Parameters.AddWithValue("@Date", date);
                        command.Parameters.AddWithValue("@Item", item);
                        command.Parameters.AddWithValue("@Qty", qty);
                        command.Parameters.AddWithValue("@Price", price);
                        command.Parameters.AddWithValue("@Receipts", receipts);
                        command.Parameters.AddWithValue("@Purpose", purpose);

                        // Execute the query
                        int rowsAffected = command.ExecuteNonQuery();

                        // Check if the query was successful
                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Report sent!");

                            // clear controls
                            txtitem.Text = "";
                            txtitemno.Text = "";
                            txtprice.Text = "";
                            txtQty.Text = "";
                            rbyes.Checked = false;
                            cbhair.Checked = false;
                            cbmass.Checked = false;
                            cbnails.Checked = false;
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

        private void button10_Click(object sender, EventArgs e)
        {
            try
            {
                using (TextWriter text1 = new StreamWriter("E:\\PRINT.txt"))
                {
                    // Write header with column names
                    foreach (DataGridViewColumn column in dataGridView1.Columns)
                    {
                        text1.Write(column.HeaderText + "\t");
                    }
                    text1.WriteLine();

                    // Write data rows
                    foreach (DataGridViewRow row in dataGridView1.Rows)
                    {
                        foreach (DataGridViewCell cell in row.Cells)
                        {
                            text1.Write(cell.Value?.ToString() + "\t"); // Handle null values
                        }
                        text1.WriteLine();
                    }
                }

                MessageBox.Show("Done");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button9_Click(object sender, EventArgs e)
        {
            try
            {
                using (StreamReader reader = new StreamReader("E:\\PRINT.txt"))
                {
                    string line;

                    // Read header
                    line = reader.ReadLine();
                    string[] headers = line.Split('\t');

                    // Create a DataTable to store the data
                    DataTable dataTable = new DataTable();

                    // Add columns to the DataTable
                    foreach (string header in headers)
                    {
                        dataTable.Columns.Add(header.Trim());
                    }

                    // Read data rows
                    while ((line = reader.ReadLine()) != null)
                    {
                        string[] values = line.Split('\t');
                        dataTable.Rows.Add(values);
                    }

                    // Display the data in a new form
                    DisplayDataForm(dataTable);
                }

                MessageBox.Show("Read complete!");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

        private void btnpwdchnge_Click(object sender, EventArgs e)
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

        private void prtpayment_Click(object sender, EventArgs e)
        {
            PrintDialog p1 = new PrintDialog();
            PrintDocument p2 = new PrintDocument();

            p2.DocumentName = "Print Document";
            p1.Document = p2;
            p1.AllowSelection = true;
            p1.AllowSelection = true;

            if (p1.ShowDialog() == DialogResult.OK)
                p2.Print();
        }

        private void btnclr_Click(object sender, EventArgs e)
        {
            // Clear textboxes
            txtname.Text = "";
            dtdate.Value = DateTime.Now; // Set date to current date
            txtclients.Text = "";
            txttotal.Text = "";
            txtid.Text = "";

            // Clear rich text box
            richTextResult.Clear();

            // Enable input controls
            txtname.Enabled = true;
            dtdate.Enabled = true;
            txtclients.Enabled = true;
            rbbarber.Enabled = true;
            rbmassage.Enabled = true;
            rbloan.Enabled = true;
            txttotal.Enabled = true;

            // Hide rich text box
            richTextResult.Visible = true;

            // Clear DataGridViews
            dataGridView1.DataSource = null;
            dataGridView2.DataSource = null;
            guna2DataGridView1.DataSource = null;
            guna2DataGridView2.DataSource = null;

            // Clear picture box
            pbpic.Image = null;
            imagelocation = "";

            // Clear checkboxes
            cbhair.Checked = false;
            cbmass.Checked = false;
            cbnails.Checked = false;

            // Clear radio buttons
            rbbarber.Checked = false;
            rbmassage.Checked = false;
            rbloan.Checked = false;
        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            
        }
    }
}
