using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace CustomerCrudApp
{
    public partial class Form1 : Form
    {
        private string connectionString =
    @"Server=(localdb)\MSSQLLocalDB;Database=CustomerDB;Trusted_Connection=True;TrustServerCertificate=True;";

        private void LoadCustomers()
        {
            string sql = "SELECT Id, CustomerId, CustomerName, PhoneNumber, Email, City FROM Customers ORDER BY Id DESC";

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlDataAdapter adapter = new SqlDataAdapter(sql, connection))
                    {
                        DataTable table = new DataTable();
                        adapter.Fill(table);

                        dgvCustomers.DataSource = table;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading customers.\n\n" + ex.Message,
                                "Database Error",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
            }
        }
        private bool ValidateInput()
        {
            if (txtCustomerId.Text.Trim() == "")
            {
                MessageBox.Show("Please enter the Customer ID.",
                                "Missing Customer ID",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Warning);

                txtCustomerId.Focus();
                return false;
            }

            if (txtCustomerName.Text.Trim() == "")
            {
                MessageBox.Show("Please enter the Customer Name.",
                                "Missing Customer Name",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Warning);

                txtCustomerName.Focus();
                return false;
            }

            if (txtPhone.Text.Trim() == "")
            {
                MessageBox.Show("Please enter the phone number.",
                                "Missing Phone Number",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Warning);

                txtPhone.Focus();
                return false;
            }

            if (txtEmail.Text.Trim() == "")
            {
                MessageBox.Show("Please enter the email address.",
                                "Missing Email",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Warning);

                txtEmail.Focus();
                return false;
            }

            if (!txtEmail.Text.Contains("@") || !txtEmail.Text.Contains("."))
            {
                MessageBox.Show("Please enter a valid email address.",
                                "Invalid Email",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Warning);

                txtEmail.Focus();
                return false;
            }

            if (txtCity.Text.Trim() == "")
            {
                MessageBox.Show("Please enter the city.",
                                "Missing City",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Warning);

                txtCity.Focus();
                return false;
            }

            return true;
        }

        private void ClearInputFields()
        {
            txtId.Clear();
            txtCustomerId.Clear();
            txtCustomerName.Clear();
            txtPhone.Clear();
            txtEmail.Clear();
            txtCity.Clear();

            txtCustomerId.Focus();
        }

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            LoadCustomers();
            ClearInputFields();

        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (!ValidateInput())
            {
                return;
            }

            string sql = @"INSERT INTO Customers
                   (CustomerId, CustomerName, PhoneNumber, Email, City)
                   VALUES
                   (@CustomerId, @CustomerName, @PhoneNumber, @Email, @City)";

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@CustomerId", txtCustomerId.Text.Trim());
                        command.Parameters.AddWithValue("@CustomerName", txtCustomerName.Text.Trim());
                        command.Parameters.AddWithValue("@PhoneNumber", txtPhone.Text.Trim());
                        command.Parameters.AddWithValue("@Email", txtEmail.Text.Trim());
                        command.Parameters.AddWithValue("@City", txtCity.Text.Trim());

                        connection.Open();

                        int rowsAffected = command.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Customer added successfully.",
                                            "Record Added",
                                            MessageBoxButtons.OK,
                                            MessageBoxIcon.Information);

                            LoadCustomers();
                            ClearInputFields();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error adding customer.\n\n" + ex.Message,
                                "Database Error",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
            }

        }

        private void dgvCustomers_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
            {
                return;
            }

            DataGridViewRow row = dgvCustomers.Rows[e.RowIndex];

            txtId.Text = row.Cells["Id"].Value.ToString();
            txtCustomerId.Text = row.Cells["CustomerId"].Value.ToString();
            txtCustomerName.Text = row.Cells["CustomerName"].Value.ToString();
            txtPhone.Text = row.Cells["PhoneNumber"].Value.ToString();
            txtEmail.Text = row.Cells["Email"].Value.ToString();
            txtCity.Text = row.Cells["City"].Value.ToString();

        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (txtId.Text.Trim() == "")
            {
                MessageBox.Show("Please select a customer record to update.",
                                "No Record Selected",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Warning);

                return;
            }

            if (!ValidateInput())
            {
                return;
            }

            string sql = @"UPDATE Customers
                   SET CustomerId = @CustomerId,
                       CustomerName = @CustomerName,
                       PhoneNumber = @PhoneNumber,
                       Email = @Email,
                       City = @City
                   WHERE Id = @Id";

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@Id", Convert.ToInt32(txtId.Text));
                        command.Parameters.AddWithValue("@CustomerId", txtCustomerId.Text.Trim());
                        command.Parameters.AddWithValue("@CustomerName", txtCustomerName.Text.Trim());
                        command.Parameters.AddWithValue("@PhoneNumber", txtPhone.Text.Trim());
                        command.Parameters.AddWithValue("@Email", txtEmail.Text.Trim());
                        command.Parameters.AddWithValue("@City", txtCity.Text.Trim());

                        connection.Open();

                        int rowsAffected = command.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Customer updated successfully.",
                                            "Record Updated",
                                            MessageBoxButtons.OK,
                                            MessageBoxIcon.Information);

                            LoadCustomers();
                            ClearInputFields();
                        }
                        else
                        {
                            MessageBox.Show("No record was updated.",
                                            "Update Failed",
                                            MessageBoxButtons.OK,
                                            MessageBoxIcon.Warning);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error updating customer.\n\n" + ex.Message,
                                "Database Error",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
            }

        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (txtId.Text.Trim() == "")
            {
                MessageBox.Show("Please select a customer record to delete.",
                                "No Record Selected",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Warning);

                return;
            }

            DialogResult result = MessageBox.Show("Are you sure you want to delete this customer?",
                                                  "Confirm Delete",
                                                  MessageBoxButtons.YesNo,
                                                  MessageBoxIcon.Question);

            if (result != DialogResult.Yes)
            {
                return;
            }

            string sql = "DELETE FROM Customers WHERE Id = @Id";

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@Id", Convert.ToInt32(txtId.Text));

                        connection.Open();

                        int rowsAffected = command.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Customer deleted successfully.",
                                            "Record Deleted",
                                            MessageBoxButtons.OK,
                                            MessageBoxIcon.Information);

                            LoadCustomers();
                            ClearInputFields();
                        }
                        else
                        {
                            MessageBox.Show("No record was deleted.",
                                            "Delete Failed",
                                            MessageBoxButtons.OK,
                                            MessageBoxIcon.Warning);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error deleting customer.\n\n" + ex.Message,
                                "Database Error",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
            }

        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            ClearInputFields();
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            LoadCustomers();
            ClearInputFields();
            txtSearch.Clear();

        }

        private void lblSearch_Click(object sender, EventArgs e)
        {

        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            string searchText = txtSearch.Text.Trim();

            if (searchText == "")
            {
                MessageBox.Show("Please enter a customer name to search.",
                                "Missing Search Text",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Warning);

                txtSearch.Focus();
                return;
            }

            string sql = @"SELECT Id, CustomerId, CustomerName, PhoneNumber, Email, City
                   FROM Customers
                   WHERE CustomerName LIKE @SearchText
                   ORDER BY Id DESC";

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlDataAdapter adapter = new SqlDataAdapter(sql, connection))
                    {
                        adapter.SelectCommand.Parameters.AddWithValue("@SearchText", "%" + searchText + "%");

                        DataTable table = new DataTable();
                        adapter.Fill(table);

                        dgvCustomers.DataSource = table;

                        if (table.Rows.Count == 0)
                        {
                            MessageBox.Show("No matching customer found.",
                                            "Search Result",
                                            MessageBoxButtons.OK,
                                            MessageBoxIcon.Information);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error searching customers.\n\n" + ex.Message,
                                "Database Error",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
            }

        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Are you sure you want to exit?",
                                      "Confirm Exit",
                                      MessageBoxButtons.YesNo,
                                      MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                Application.Exit();
            }

        }
    }
}
    

