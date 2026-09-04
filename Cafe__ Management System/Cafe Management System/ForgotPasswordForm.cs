using System;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace Cafe_Management_System
{
    public partial class ForgotPasswordForm : Form
    {
        string Con = @"Data Source=DESKTOP-PPPRS94\SQLEXPRESS;Initial Catalog=CafeManagementSystem;Integrated Security=True";

        public ForgotPasswordForm()
        {
            InitializeComponent();
        }

        private void label1_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            Form1 login = new Form1();
            login.Show();
            this.Hide();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            NewPasswordTb.PasswordChar = checkBox1.Checked ? '\0' : '*';
            ConfirmPasswordTb.PasswordChar = checkBox1.Checked ? '\0' : '*';
        }

        private bool IsPasswordDuplicate(string password, int excludeId = -1)
        {
            using (SqlConnection connect = new SqlConnection(Con))
            {
                connect.Open();
                string query = excludeId == -1
                    ? "SELECT COUNT(*) FROM UsersF WHERE Password = @pass"
                    : "SELECT COUNT(*) FROM UsersF WHERE Password = @pass AND ID <> @id";

                SqlCommand cmd = new SqlCommand(query, connect);
                cmd.Parameters.AddWithValue("@pass", password);

                if (excludeId != -1)
                    cmd.Parameters.AddWithValue("@id", excludeId);

                int count = Convert.ToInt32(cmd.ExecuteScalar());
                return count > 0;
            }
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            string username = UserNameTb.Text.Trim();
            string phone = PhoneTb.Text.Trim();
            string newPass = NewPasswordTb.Text;
            string confirmPass = ConfirmPasswordTb.Text;

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(phone) ||
                string.IsNullOrEmpty(newPass) || string.IsNullOrEmpty(confirmPass))
            {
                MessageBox.Show("Please fill all fields.", "Missing Information", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (newPass.Length < 6)
            {
                MessageBox.Show("Password must be at least 6 characters long!", "Invalid Password", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (newPass != confirmPass)
            {
                MessageBox.Show("Passwords do not match. Please re-enter.", "Mismatch", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                using (SqlConnection connect = new SqlConnection(Con))
                {
                    connect.Open();

                    // 1. Verify user exists with matching username and phone number
                    string checkQuery = "SELECT ID FROM UsersF WHERE LOWER(User_Name) = LOWER(@uname) AND Phone = @phone";
                    SqlCommand checkCmd = new SqlCommand(checkQuery, connect);
                    checkCmd.Parameters.AddWithValue("@uname", username);
                    checkCmd.Parameters.AddWithValue("@phone", phone);

                    object result = checkCmd.ExecuteScalar();

                    if (result == null)
                    {
                        MessageBox.Show("No account found matching this Username and Phone number!", "Verification Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    int userId = Convert.ToInt32(result);

                    // 2. Check if password is used by another account
                    if (IsPasswordDuplicate(newPass, userId))
                    {
                        MessageBox.Show("This password is already in use. Please choose a different password.", "Duplicate Password", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    // 3. Update the password
                    string updateQuery = "UPDATE UsersF SET Password = @newpass WHERE ID = @id";
                    SqlCommand updateCmd = new SqlCommand(updateQuery, connect);
                    updateCmd.Parameters.AddWithValue("@newpass", newPass);
                    updateCmd.Parameters.AddWithValue("@id", userId);

                    int rowsAffected = updateCmd.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Password has been reset successfully! You can now log in with your new password.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        Form1 login = new Form1();
                        login.Show();
                        this.Hide();
                    }
                    else
                    {
                        MessageBox.Show("Failed to reset password. Please try again.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (SqlException sqlEx)
            {
                MessageBox.Show("Database error: " + sqlEx.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show("An unexpected error occurred: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
