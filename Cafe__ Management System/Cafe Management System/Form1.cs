using System;
using System.Data.SqlClient;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Button;

namespace Cafe_Management_System
{
    public partial class Form1 : Form
    {
        string Con = "Data Source=DESKTOP-PPPRS94\\SQLEXPRESS;Initial Catalog=CafeManagementSystem;Integrated Security=True";

        public static string username = "";

        public Form1()
        {
            InitializeComponent();
        }

        private void label2_Click(object sender, EventArgs e)
        {
        }

        private void label5_Click(object sender, EventArgs e)
        {
            this.Hide();
            GuestOrder guest = new GuestOrder();
            guest.Show();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (UserNameTb.Text == "" || PasswordTb.Text == "")
            {
                MessageBox.Show("Please enter username and password");
            }
            else
            {
                try
                {
                    SqlConnection connect = new SqlConnection(Con);
                    connect.Open();

                    string query = "SELECT User_Name FROM UsersF WHERE LOWER(User_Name)=LOWER(@username) AND Password=@password";

                    SqlCommand cmd = new SqlCommand(query, connect);
                    cmd.Parameters.AddWithValue("@username", UserNameTb.Text);
                    cmd.Parameters.AddWithValue("@password", PasswordTb.Text);

                    object result = cmd.ExecuteScalar();

                    if (result != null)
                    {
                        username = result.ToString();

                        MessageBox.Show("Login Successfully");

                        if (username.Equals("admin", StringComparison.OrdinalIgnoreCase))
                        {
                            AdminForm admin = new AdminForm();
                            admin.Show();
                        }
                        else
                        {
                            UserOrder uorder = new UserOrder();
                            uorder.Show();
                        }

                        this.Hide();
                    }
                    else
                    {
                        MessageBox.Show("Invalid Username or Password");
                    }

                    connect.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
        }

        private void label1_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            PasswordTb.PasswordChar = checkBox1.Checked ? '\0' : '*';

        }

        private void forgotPasswordLbl_Click(object sender, EventArgs e)
        {
            this.Hide();
            ForgotPasswordForm forgotForm = new ForgotPasswordForm();
            forgotForm.Show();
        }
    }
}