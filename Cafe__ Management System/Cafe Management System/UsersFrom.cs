using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Cafe_Management_System
{
    public partial class UsersFrom : Form
    {
        string Con = @"Data Source=DESKTOP-PPPRS94\SQLEXPRESS;Initial Catalog=CafeManagementSystem;Integrated Security=True";

        public UsersFrom()
        {
            InitializeComponent();

            SetupSmartGrid(UsersGV);

            UsersGV.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            UsersGV.MultiSelect = false;

            UsersGV.CellClick += UsersGV_CellContentClick;
        }

        void SetupSmartGrid(DataGridView grid)
        {
            grid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            grid.AllowUserToAddRows = false;
            grid.AllowUserToResizeRows = false;
            grid.RowHeadersVisible = false;

            grid.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(43, 29, 22);
            grid.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            grid.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
            grid.ColumnHeadersHeight = 35;
            grid.EnableHeadersVisualStyles = false;

            grid.DefaultCellStyle.Font = new Font("Segoe UI", 9F);
            grid.DefaultCellStyle.SelectionBackColor = Color.FromArgb(198, 125, 59);
            grid.DefaultCellStyle.SelectionForeColor = Color.White;
            grid.RowTemplate.Height = 30;

            grid.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(250, 247, 243);

            grid.BorderStyle = BorderStyle.None;
            grid.GridColor = Color.FromArgb(230, 222, 214);
            grid.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
        }

        void populate()
        {
            using (SqlConnection connect = new SqlConnection(Con))
            {
                connect.Open();

                string query = "SELECT * FROM UsersF";
                SqlDataAdapter sda = new SqlDataAdapter(query, connect);

                DataTable dt = new DataTable();
                sda.Fill(dt);

                UsersGV.DataSource = dt;
            }
        }

        public delegate void UserActionHandler();

        bool IsPhoneDuplicate(string phone, int excludeId = -1)
        {
            using (SqlConnection connect = new SqlConnection(Con))
            {
                connect.Open();

                string query = excludeId == -1
                    ? "SELECT COUNT(*) FROM UsersF WHERE Phone = @phone"
                    : "SELECT COUNT(*) FROM UsersF WHERE Phone = @phone AND ID <> @id";

                SqlCommand cmd = new SqlCommand(query, connect);
                cmd.Parameters.AddWithValue("@phone", phone);

                if (excludeId != -1)
                    cmd.Parameters.AddWithValue("@id", excludeId);

                int count = Convert.ToInt32(cmd.ExecuteScalar());
                return count > 0;
            }
        }

        bool IsPasswordDuplicate(string password, int excludeId = -1)
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

        private void button4_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(Form1.username) && Form1.username.Equals("admin", StringComparison.OrdinalIgnoreCase))
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

        private void buttonAdmin_Click(object sender, EventArgs e)
        {
            this.Hide();
            AdminForm admin = new AdminForm();
            admin.Show();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            ItemsFrom item = new ItemsFrom();
            item.Show();
            this.Hide();
        }

        private void label5_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to logout?", "Confirmation Message", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)

            {

                Form1 login = new Form1();

                login.Show();

                this.Hide();

            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            UserActionHandler action = AddUser;
            action();
        }

        private void AddUser()
        {
            if (unameTb.Text == "" || UphoneTb.Text == "" || UpassTb.Text == "")
            {
                MessageBox.Show("Please fill all fields");
                return;
            }

            if (UpassTb.Text.Length < 6)
            {
                MessageBox.Show("Password must be at least 6 characters long!");
                return;
            }

            try
            {
                if (IsPhoneDuplicate(UphoneTb.Text))
                {
                    MessageBox.Show("This phone number already exists! Please use a different phone number.");
                    return;
                }

                if (IsPasswordDuplicate(UpassTb.Text))
                {
                    MessageBox.Show("This password is already used by another user! Please choose a different password.");
                    return;
                }

                using (SqlConnection connect = new SqlConnection(Con))
                {
                    connect.Open();

                    string query = "INSERT INTO UsersF(User_Name, Phone, Password) VALUES(@uname, @phone, @pass)";

                    SqlCommand cmd = new SqlCommand(query, connect);
                    cmd.Parameters.AddWithValue("@uname", unameTb.Text);
                    cmd.Parameters.AddWithValue("@phone", UphoneTb.Text);
                    cmd.Parameters.AddWithValue("@pass", UpassTb.Text);

                    cmd.ExecuteNonQuery();

                    MessageBox.Show("User Added Successfully");
                }

                populate();

                unameTb.Text = "";
                UphoneTb.Text = "";
                UpassTb.Text = "";
            }
            catch (SqlException sqlEx)
            {
                MessageBox.Show("Database error: " + sqlEx.Message);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Something went wrong: " + ex.Message);
            }
        }

        private void UsersFrom_Load(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(Form1.username) || !Form1.username.Equals("admin", StringComparison.OrdinalIgnoreCase))
            {
                MessageBox.Show("Access Denied: Only Admin can access Users.", "Access Denied", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                UserOrder uorder = new UserOrder();
                uorder.Show();
                this.Hide();
                return;
            }

            populate();
        }

        private void UsersGV_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                unameTb.Text = UsersGV.Rows[e.RowIndex].Cells[1].Value?.ToString() ?? "";
                UphoneTb.Text = UsersGV.Rows[e.RowIndex].Cells[2].Value?.ToString() ?? "";
                UpassTb.Text = UsersGV.Rows[e.RowIndex].Cells[3].Value?.ToString() ?? "";
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            UserActionHandler action = DeleteUser;
            action();
        }

        private void DeleteUser()
        {
            if (UsersGV.CurrentRow != null)
            {
                try
                {
                    using (SqlConnection connect = new SqlConnection(Con))
                    {
                        connect.Open();

                        string query = "DELETE FROM UsersF WHERE ID = @id";

                        SqlCommand cmd = new SqlCommand(query, connect);
                        cmd.Parameters.AddWithValue("@id", UsersGV.CurrentRow.Cells[0].Value.ToString());

                        cmd.ExecuteNonQuery();

                        MessageBox.Show("User Deleted Successfully");
                    }

                    populate();

                    unameTb.Text = "";
                    UphoneTb.Text = "";
                    UpassTb.Text = "";
                }
                catch (SqlException sqlEx)
                {
                    MessageBox.Show("Database error: " + sqlEx.Message);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Something went wrong: " + ex.Message);
                }
            }
            else
            {
                MessageBox.Show("Please select a user");
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            UserActionHandler action = EditUser;
            action();
        }

        private void EditUser()
        {
            if (UsersGV.CurrentRow != null)
            {
                if (unameTb.Text == "" || UphoneTb.Text == "" || UpassTb.Text == "")
                {
                    MessageBox.Show("Please fill all fields");
                    return;
                }

                if (UpassTb.Text.Length < 6)
                {
                    MessageBox.Show("Password must be at least 6 characters long!");
                    return;
                }

                try
                {
                    int currentId = Convert.ToInt32(UsersGV.CurrentRow.Cells[0].Value);

                    if (IsPhoneDuplicate(UphoneTb.Text, currentId))
                    {
                        MessageBox.Show("This phone number already exists! Please use a different phone number.");
                        return;
                    }

                    if (IsPasswordDuplicate(UpassTb.Text, currentId))
                    {
                        MessageBox.Show("This password is already used by another user! Please choose a different password.");
                        return;
                    }

                    using (SqlConnection connect = new SqlConnection(Con))
                    {
                        connect.Open();

                        string query = "UPDATE UsersF SET User_Name = @uname, Phone = @phone, Password = @pass WHERE ID = @id";

                        SqlCommand cmd = new SqlCommand(query, connect);
                        cmd.Parameters.AddWithValue("@uname", unameTb.Text);
                        cmd.Parameters.AddWithValue("@phone", UphoneTb.Text);
                        cmd.Parameters.AddWithValue("@pass", UpassTb.Text);
                        cmd.Parameters.AddWithValue("@id", currentId);

                        cmd.ExecuteNonQuery();

                        MessageBox.Show("User Updated Successfully");
                    }

                    populate();

                    unameTb.Text = "";
                    UphoneTb.Text = "";
                    UpassTb.Text = "";
                }
                catch (SqlException sqlEx)
                {
                    MessageBox.Show("Database error: " + sqlEx.Message);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Something went wrong: " + ex.Message);
                }
            }
            else
            {
                MessageBox.Show("Please select a user");
            }
        }

        private void Close_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to exit?", "Confirmation Message", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                Application.Exit();
            }
        }
    }
}