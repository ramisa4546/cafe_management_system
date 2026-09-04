using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Cafe_Management_System
{
    public partial class ItemsFrom : Form
    {
        string Con = @"Data Source=DESKTOP-PPPRS94\SQLEXPRESS;Initial Catalog=CafeManagementSystem;Integrated Security=True";

        byte[] selectedImageBytes = null;

        public ItemsFrom()
        {
            InitializeComponent();

            SetupSmartGrid(ItemGV);

            ItemGV.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            ItemGV.MultiSelect = false;

            ItemGV.CellClick += ItemGV_CellContentClick;

            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox1.Click += pictureBox1_Click;
            pictureBox1.Cursor = Cursors.Hand;
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

                string query = "SELECT Itemnum, Item_name, Catagory, Item_price, ItemImage FROM Items";
                SqlDataAdapter sda = new SqlDataAdapter(query, connect);

                DataTable dt = new DataTable();
                sda.Fill(dt);

                DataTable displayTable = new DataTable();
                displayTable.Columns.Add("Itemnum", typeof(string));
                displayTable.Columns.Add("Item_name", typeof(string));
                displayTable.Columns.Add("Catagory", typeof(string));
                displayTable.Columns.Add("Item_price", typeof(string));

                List<Image> images = new List<Image>();

                foreach (DataRow row in dt.Rows)
                {
                    Image itemImage = null;

                    if (row["ItemImage"] != DBNull.Value)
                    {
                        byte[] imageBytes = (byte[])row["ItemImage"];
                        using (MemoryStream ms = new MemoryStream(imageBytes))
                        {
                            itemImage = Image.FromStream(ms);
                        }
                    }

                    images.Add(itemImage);

                    displayTable.Rows.Add(
                        row["Itemnum"].ToString(),
                        row["Item_name"].ToString(),
                        row["Catagory"].ToString(),
                        row["Item_price"].ToString()
                    );
                }

                ItemGV.DataSource = displayTable;

                if (ItemGV.Columns["Itemnum"] != null)
                    ItemGV.Columns["Itemnum"].HeaderText = "Item No";
                if (ItemGV.Columns["Item_name"] != null)
                    ItemGV.Columns["Item_name"].HeaderText = "Name";
                if (ItemGV.Columns["Catagory"] != null)
                    ItemGV.Columns["Catagory"].HeaderText = "Category";
                if (ItemGV.Columns["Item_price"] != null)
                    ItemGV.Columns["Item_price"].HeaderText = "Price";

                if (!ItemGV.Columns.Contains("Photo"))
                {
                    DataGridViewImageColumn photoColumn = new DataGridViewImageColumn();
                    photoColumn.Name = "Photo";
                    photoColumn.HeaderText = "Photo";
                    photoColumn.Width = 70;
                    photoColumn.ImageLayout = DataGridViewImageCellLayout.Zoom;
                    ItemGV.Columns.Add(photoColumn);
                }

                for (int i = 0; i < images.Count; i++)
                {
                    ItemGV.Rows[i].Cells["Photo"].Value = images[i];
                }

                ItemGV.RowTemplate.Height = 70;
                foreach (DataGridViewRow row in ItemGV.Rows)
                {
                    row.Height = 70;
                }
            }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp;*.gif";
                ofd.Title = "Select Item Photo";

                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        selectedImageBytes = File.ReadAllBytes(ofd.FileName);
                        pictureBox1.Image = Image.FromFile(ofd.FileName);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Could not load image: " + ex.Message);
                    }
                }
            }
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

        private void button4_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(Form1.username) && Form1.username.Equals("admin", StringComparison.OrdinalIgnoreCase))
            {
                AdminForm admin = new AdminForm();
                admin.Show();
            }
            else
            {
                UserOrder order = new UserOrder();
                order.Show();
            }
            this.Hide();
        }

        private void buttonAdmin_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(Form1.username) && Form1.username.Equals("admin", StringComparison.OrdinalIgnoreCase))
            {
                this.Hide();
                AdminForm admin = new AdminForm();
                admin.Show();
            }
            else
            {
                MessageBox.Show("Access Denied: Only Admin can access Admin Panel.", "Access Denied", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        bool IsItemNameDuplicate(string itemName, string excludeItemNum = "")
        {
            using (SqlConnection connect = new SqlConnection(Con))
            {
                connect.Open();

                string query = string.IsNullOrEmpty(excludeItemNum)
                    ? "SELECT COUNT(*) FROM Items WHERE LOWER(Item_name) = LOWER(@name)"
                    : "SELECT COUNT(*) FROM Items WHERE LOWER(Item_name) = LOWER(@name) AND Itemnum <> @num";

                SqlCommand cmd = new SqlCommand(query, connect);
                cmd.Parameters.AddWithValue("@name", itemName.Trim());

                if (!string.IsNullOrEmpty(excludeItemNum))
                    cmd.Parameters.AddWithValue("@num", excludeItemNum);

                int count = Convert.ToInt32(cmd.ExecuteScalar());
                return count > 0;
            }
        }

        public delegate void ItemActionHandler();

        private void button1_Click(object sender, EventArgs e)
        {
            ItemActionHandler action = AddItem;
            action();
        }

        private void AddItem()
        {
            if (ItemNumTb.Text == "" || ItemNameTb.Text == "" || ItemPriceTb.Text == "" || CatCb.SelectedIndex == -1)
            {
                MessageBox.Show("Please fill all fields");
            }
            else if (!decimal.TryParse(ItemPriceTb.Text, out decimal priceValue))
            {
                MessageBox.Show("Please enter a valid price!");
            }
            else
            {
                try
                {
                    if (IsItemNameDuplicate(ItemNameTb.Text))
                    {
                        MessageBox.Show("This item name already exists! Please use a different item name.");
                        return;
                    }

                    using (SqlConnection connect = new SqlConnection(Con))
                    {
                        connect.Open();
                        
                        string query = "INSERT INTO Items(Itemnum, Item_name, Catagory, Item_price, ItemImage) " +
                                       "VALUES(@num, @name, @cat, @price, @image)";

                        SqlCommand cmd = new SqlCommand(query, connect);
                        cmd.Parameters.AddWithValue("@num", ItemNumTb.Text);
                        cmd.Parameters.AddWithValue("@name", ItemNameTb.Text);
                        cmd.Parameters.AddWithValue("@cat", CatCb.SelectedItem.ToString());
                        cmd.Parameters.AddWithValue("@price", priceValue);

                        if (selectedImageBytes != null)
                            cmd.Parameters.AddWithValue("@image", selectedImageBytes);
                        else
                            cmd.Parameters.AddWithValue("@image", DBNull.Value);

                        cmd.ExecuteNonQuery();

                        MessageBox.Show("Item Added Successfully");
                    }

                    populate();

                    ItemNumTb.Text = "";
                    ItemNameTb.Text = "";
                    ItemPriceTb.Text = "";
                    CatCb.SelectedIndex = -1;
                    pictureBox1.Image = null;
                    selectedImageBytes = null;
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
        }

        private void ItemsFrom_Load(object sender, EventArgs e)
        {
            populate();

            bool isAdmin = !string.IsNullOrEmpty(Form1.username) && Form1.username.Equals("admin", StringComparison.OrdinalIgnoreCase);
            button3.Visible = isAdmin;
            buttonAdmin.Visible = isAdmin;
        }

        private void ItemGV_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                ItemNumTb.Text = ItemGV.Rows[e.RowIndex].Cells["Itemnum"].Value?.ToString() ?? "";
                ItemNameTb.Text = ItemGV.Rows[e.RowIndex].Cells["Item_name"].Value?.ToString() ?? "";
                CatCb.Text = ItemGV.Rows[e.RowIndex].Cells["Catagory"].Value?.ToString() ?? "";
                ItemPriceTb.Text = ItemGV.Rows[e.RowIndex].Cells["Item_price"].Value?.ToString() ?? "";

                LoadItemImage(ItemNumTb.Text);
            }
        }

        void LoadItemImage(string itemNum)
        {
            try
            {
                using (SqlConnection connect = new SqlConnection(Con))
                {
                    connect.Open();

                    string query = "SELECT ItemImage FROM Items WHERE Itemnum = @num";
                    SqlCommand cmd = new SqlCommand(query, connect);
                    cmd.Parameters.AddWithValue("@num", itemNum);

                    object result = cmd.ExecuteScalar();

                    if (result != null && result != DBNull.Value)
                    {
                        byte[] imageBytes = (byte[])result;
                        selectedImageBytes = imageBytes;

                        using (MemoryStream ms = new MemoryStream(imageBytes))
                        {
                            pictureBox1.Image = Image.FromStream(ms);
                        }
                    }
                    else
                    {
                        pictureBox1.Image = null;
                        selectedImageBytes = null;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Could not load item image: " + ex.Message);
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            ItemActionHandler action = DeleteItem;
            action();
        }

        private void DeleteItem()
        {
            if (ItemGV.CurrentRow != null)
            {
                try
                {
                    using (SqlConnection connect = new SqlConnection(Con))
                    {
                        connect.Open();

                        string query = "DELETE FROM Items WHERE Itemnum = @num";

                        SqlCommand cmd = new SqlCommand(query, connect);
                        cmd.Parameters.AddWithValue("@num", ItemGV.CurrentRow.Cells["Itemnum"].Value.ToString());

                        cmd.ExecuteNonQuery();

                        MessageBox.Show("Item Deleted Successfully");
                    }

                    populate();

                    ItemNumTb.Text = "";
                    ItemNameTb.Text = "";
                    CatCb.SelectedIndex = -1;
                    ItemPriceTb.Text = "";
                    pictureBox1.Image = null;
                    selectedImageBytes = null;
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
                MessageBox.Show("Please select an item");
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            ItemActionHandler action = EditItem;
            action();
        }

        private void EditItem()
        {
            if (ItemGV.CurrentRow != null)
            {
                if (ItemNameTb.Text == "" || ItemPriceTb.Text == "" || CatCb.SelectedIndex == -1)
                {
                    MessageBox.Show("Please fill all fields");
                    return;
                }

                if (!decimal.TryParse(ItemPriceTb.Text, out decimal priceValue))
                {
                    MessageBox.Show("Please enter a valid price!");
                    return;
                }

                try
                {
                    string currentNum = ItemGV.CurrentRow.Cells["Itemnum"].Value.ToString();

                    if (IsItemNameDuplicate(ItemNameTb.Text, currentNum))
                    {
                        MessageBox.Show("This item name already exists! Please use a different item name.");
                        return;
                    }

                    using (SqlConnection connect = new SqlConnection(Con))
                    {
                        connect.Open();

                        string query = "UPDATE Items SET Item_name = @name, Catagory = @cat, Item_price = @price, ItemImage = @image " +
                                       "WHERE Itemnum = @num";

                        SqlCommand cmd = new SqlCommand(query, connect);
                        cmd.Parameters.AddWithValue("@name", ItemNameTb.Text);
                        cmd.Parameters.AddWithValue("@cat", CatCb.SelectedItem.ToString());
                        cmd.Parameters.AddWithValue("@price", priceValue);
                        cmd.Parameters.AddWithValue("@num", ItemGV.CurrentRow.Cells["Itemnum"].Value.ToString());

                        if (selectedImageBytes != null)
                            cmd.Parameters.AddWithValue("@image", selectedImageBytes);
                        else
                            cmd.Parameters.AddWithValue("@image", DBNull.Value);

                        cmd.ExecuteNonQuery();

                        MessageBox.Show("Item Updated Successfully");
                    }

                    populate();

                    ItemNumTb.Text = "";
                    ItemNameTb.Text = "";
                    CatCb.SelectedIndex = -1;
                    ItemPriceTb.Text = "";
                    pictureBox1.Image = null;
                    selectedImageBytes = null;
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
                MessageBox.Show("Please select an item");
            }
        }

        private void Close_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to exit?", "Confirmation Message", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                Application.Exit();
            }
        }

        private void label3_Click(object sender, EventArgs e)
        {
            label3.Text = "Add Item\n Picture";

        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(Form1.username) && Form1.username.Equals("admin", StringComparison.OrdinalIgnoreCase))
            {
                this.Hide();
                UsersFrom ufrom = new UsersFrom();
                ufrom.Show();
            }
            else
            {
                MessageBox.Show("Access Denied: Only Admin can access Users.", "Access Denied", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
    }
}