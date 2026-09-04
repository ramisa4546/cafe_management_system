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
    public partial class AdminForm : Form
    {
        string Con = @"Data Source=DESKTOP-PPPRS94\SQLEXPRESS;Initial Catalog=CafeManagementSystem;Integrated Security=True";

        int num = 0;
        decimal price = 0;
        decimal total = 0;
        string item = "";
        string cat = "";
        int flag = 0;
        decimal sum = 0;

        DataTable table = new DataTable();

        public AdminForm()
        {
            InitializeComponent();

            SetupSmartGrid(ItemGV);
            SetupSmartGrid(OrdersGv);

            ItemGV.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            ItemGV.MultiSelect = false;
            ItemGV.ReadOnly = true;

            ItemGV.CellClick += ItemGV_CellContentClick;

            table.Columns.Add("Num", typeof(int));
            table.Columns.Add("Item", typeof(string));
            table.Columns.Add("Category", typeof(string));
            table.Columns.Add("Quantity", typeof(int));
            table.Columns.Add("UnitPrice", typeof(decimal));
            table.Columns.Add("TotalAmount", typeof(decimal));

            OrdersGv.AutoGenerateColumns = true;
            OrdersGv.DataSource = table;

            StyleOrdersGvColumns();
        }

        void SetupSmartGrid(DataGridView grid)
        {
            grid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            grid.AllowUserToAddRows = false;
            grid.AllowUserToResizeRows = false;
            grid.RowHeadersVisible = false;

            grid.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(43, 29, 22);
            grid.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            grid.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            grid.ColumnHeadersHeight = 35;
            grid.EnableHeadersVisualStyles = false;

            grid.DefaultCellStyle.Font = new Font("Segoe UI", 9.5F);
            grid.DefaultCellStyle.SelectionBackColor = Color.FromArgb(198, 125, 59);
            grid.DefaultCellStyle.SelectionForeColor = Color.White;
            grid.RowTemplate.Height = 30;

            grid.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(250, 247, 243);

            grid.BorderStyle = BorderStyle.None;
            grid.GridColor = Color.FromArgb(230, 222, 214);
            grid.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
        }

        void StyleOrdersGvColumns()
        {
            if (OrdersGv.Columns["Num"] != null)
            {
                OrdersGv.Columns["Num"].FillWeight = 30;
                OrdersGv.Columns["Num"].HeaderText = "No";
            }
            if (OrdersGv.Columns["Item"] != null)
                OrdersGv.Columns["Item"].FillWeight = 100;
            if (OrdersGv.Columns["Category"] != null)
                OrdersGv.Columns["Category"].FillWeight = 100;
            if (OrdersGv.Columns["Quantity"] != null)
            {
                OrdersGv.Columns["Quantity"].FillWeight = 60;
                OrdersGv.Columns["Quantity"].HeaderText = "Qty";
            }
            if (OrdersGv.Columns["UnitPrice"] != null)
            {
                OrdersGv.Columns["UnitPrice"].FillWeight = 80;
                OrdersGv.Columns["UnitPrice"].HeaderText = "Price";
                OrdersGv.Columns["UnitPrice"].DefaultCellStyle.Format = "N2";
            }
            if (OrdersGv.Columns["TotalAmount"] != null)
            {
                OrdersGv.Columns["TotalAmount"].FillWeight = 90;
                OrdersGv.Columns["TotalAmount"].HeaderText = "Total";
                OrdersGv.Columns["TotalAmount"].DefaultCellStyle.Format = "N2";
            }
        }

        void populate()
        {
            try
            {
                using (SqlConnection connect = new SqlConnection(Con))
                {
                    connect.Open();

                    string query = "SELECT Itemnum, Item_name, Catagory, Item_price, ItemImage FROM Items";
                    SqlDataAdapter sda = new SqlDataAdapter(query, connect);

                    DataTable dt = new DataTable();
                    sda.Fill(dt);

                    LoadItemsIntoGrid(dt);
                }
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

        void filterbycategory()
        {
            if (catCb.SelectedIndex == -1)
            {
                populate();
                return;
            }

            try
            {
                using (SqlConnection connect = new SqlConnection(Con))
                {
                    connect.Open();

                    string query = "SELECT Itemnum, Item_name, Catagory, Item_price, ItemImage FROM Items WHERE Catagory = @cat";
                    SqlCommand cmd = new SqlCommand(query, connect);
                    cmd.Parameters.AddWithValue("@cat", catCb.SelectedItem.ToString());

                    SqlDataAdapter sda = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    sda.Fill(dt);

                    LoadItemsIntoGrid(dt);
                }
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

        void LoadItemsIntoGrid(DataTable dt)
        {
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

        private void button4_Click(object sender, EventArgs e)
        {
            this.Hide();
            ItemsFrom Items = new ItemsFrom();
            Items.Show();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Hide();
            UsersFrom user = new UsersFrom();
            user.Show();
        }

        int GetNextOrderNumber()
        {
            try
            {
                using (SqlConnection connect = new SqlConnection(Con))
                {
                    connect.Open();
                    string query = "SELECT ISNULL(MAX(OrderNum), 0) + 1 FROM Orders";
                    SqlCommand cmd = new SqlCommand(query, connect);
                    object result = cmd.ExecuteScalar();
                    if (result != null && int.TryParse(result.ToString(), out int nextNum))
                    {
                        return nextNum;
                    }
                }
            }
            catch
            {
            }
            return 1;
        }

        private void AdminForm_Load(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(Form1.username) || !Form1.username.Equals("admin", StringComparison.OrdinalIgnoreCase))
            {
                MessageBox.Show("Access Denied: Only Admin can access Admin Panel.", "Access Denied", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                UserOrder uorder = new UserOrder();
                uorder.Show();
                this.Hide();
                return;
            }

            populate();
            LabelDate.Text = DateTime.Now.ToString("dd-MM-yyyy") + "\n" + DateTime.Now.ToString("hh:mm:ss tt");
            SellerNameTb.Text = Form1.username;
            OrderNumTb.ReadOnly = true;
            OrderNumTb.Text = GetNextOrderNumber().ToString();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (QtyTb.Text == "")
            {
                MessageBox.Show("What is The Quantity of item?");
            }
            else if (!int.TryParse(QtyTb.Text, out int qty))
            {
                MessageBox.Show("Please enter a valid number!");
            }
            else if (qty <= 0)
            {
                MessageBox.Show("Please enter a valid quantity!");
            }
            else if (flag == 0)
            {
                MessageBox.Show("Please select an item!");
            }
            else
            {
                num = num + 1;
                total = price * qty;

                table.Rows.Add(num, item, cat, qty, price, total);

                OrdersGv.DataSource = null;
                OrdersGv.AutoGenerateColumns = true;
                OrdersGv.DataSource = table;

                StyleOrdersGvColumns();

                flag = 0;
                QtyTb.Text = "";

                MessageBox.Show("Item Added To Cart");

                sum = sum + total;

                LabelAmount.Text = "Amount: " + sum + "TK";
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

        private void catCb_SelectionChangeCommitted(object sender, EventArgs e)
        {
            filterbycategory();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            populate();
            OrderNumTb.Text = GetNextOrderNumber().ToString();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (CustomerNameTb.Text == "")
            {
                MessageBox.Show("Please enter customer name");
            }
            else if (OrdersGv.Rows.Count == 0)
            {
                MessageBox.Show("Please add item to cart");
            }
            else if (MessageBox.Show("Are you sure you want to place this order?", "Order Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                try
                {
                    using (SqlConnection connect = new SqlConnection(Con))
                    {
                        connect.Open();

                        string sellerName = SellerNameTb.Text == "" ? "Guest" : SellerNameTb.Text;

                        string orderQuery = "INSERT INTO Orders(SellerName, CustomerName, Time, TotalAmount, Status) " +
                                             "OUTPUT INSERTED.OrderNum " +
                                             "VALUES(@seller, @customer, GETDATE(), @total, 'Unpaid')";

                        SqlCommand orderCmd = new SqlCommand(orderQuery, connect);
                        orderCmd.Parameters.AddWithValue("@seller", sellerName);
                        orderCmd.Parameters.AddWithValue("@customer", CustomerNameTb.Text);
                        orderCmd.Parameters.AddWithValue("@total", sum);

                        int newOrderNum = (int)orderCmd.ExecuteScalar();

                        foreach (DataRow row in table.Rows)
                        {
                            string detailQuery = "INSERT INTO OrderDetails(OrderNum, ItemName, Category, Quantity, UnitPrice, TotalAmount) " +
                                                  "VALUES(@orderNum, @item, @cat, @qty, @price, @total)";

                            SqlCommand detailCmd = new SqlCommand(detailQuery, connect);
                            detailCmd.Parameters.AddWithValue("@orderNum", newOrderNum);
                            detailCmd.Parameters.AddWithValue("@item", row["Item"]);
                            detailCmd.Parameters.AddWithValue("@cat", row["Category"]);
                            detailCmd.Parameters.AddWithValue("@qty", row["Quantity"]);
                            detailCmd.Parameters.AddWithValue("@price", row["UnitPrice"]);
                            detailCmd.Parameters.AddWithValue("@total", row["TotalAmount"]);

                            detailCmd.ExecuteNonQuery();
                        }

                        MessageBox.Show("Order Placed Successfully");
                    }

                    SellerNameTb.Text = "";
                    CustomerNameTb.Text = "";

                    table.Clear();
                    num = 0;
                    sum = 0;

                    LabelAmount.Text = "Amount: 0 TK";
                    OrderNumTb.Text = GetNextOrderNumber().ToString();
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

        private void button6_Click(object sender, EventArgs e)
        {
            ViewOrders view = new ViewOrders();
            view.Show();
        }

        private void ItemGV_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                ItemGV.Rows[e.RowIndex].Selected = true;

                item = ItemGV.Rows[e.RowIndex].Cells["Item_name"].Value?.ToString() ?? "";
                cat = ItemGV.Rows[e.RowIndex].Cells["Catagory"].Value?.ToString() ?? "";
                price = Convert.ToDecimal(ItemGV.Rows[e.RowIndex].Cells["Item_price"].Value);

                flag = 1;

                MessageBox.Show("Selected Item: " + item);
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            this.Hide();
            AdminView aview = new AdminView();
            aview.Show();
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