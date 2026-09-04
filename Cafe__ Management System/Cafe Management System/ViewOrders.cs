using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Printing;
using System.Windows.Forms;

namespace Cafe_Management_System
{
    public partial class ViewOrders : Form
    {
        string Con = @"Data Source=DESKTOP-PPPRS94\SQLEXPRESS;Initial Catalog=CafeManagementSystem;Integrated Security=True";

        public ViewOrders()
        {
            InitializeComponent();

            SetupSmartGrid(OrdersGV);

            OrdersGV.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            OrdersGV.MultiSelect = false;
            OrdersGV.ReadOnly = true;

            if (FilterCb.Items.Count > 0)
            {
                FilterCb.SelectedIndex = 0; // Default to "Unpaid Orders"
            }
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

        void populate(string searchKeyword = "")
        {
            try
            {
                using (SqlConnection connect = new SqlConnection(Con))
                {
                    connect.Open();

                    string filterOption = FilterCb.SelectedItem?.ToString() ?? "Unpaid Orders";
                    string statusClause = "";

                    if (filterOption == "Unpaid Orders")
                        statusClause = " (Status = 'Unpaid' OR Status IS NULL) ";
                    else if (filterOption == "Paid Orders")
                        statusClause = " Status = 'Paid' ";
                    else
                        statusClause = " 1=1 ";

                    string query;
                    SqlCommand cmd;

                    if (string.IsNullOrWhiteSpace(searchKeyword))
                    {
                        query = $"SELECT OrderNum, SellerName, CustomerName, Time, TotalAmount, ISNULL(Status, 'Unpaid') AS Status FROM Orders WHERE {statusClause} ORDER BY OrderNum DESC";
                        cmd = new SqlCommand(query, connect);
                    }
                    else
                    {
                        string kw = searchKeyword.Trim().ToLower();
                        query = "SELECT OrderNum, SellerName, CustomerName, Time, TotalAmount, ISNULL(Status, 'Unpaid') AS Status FROM Orders " +
                                "WHERE (CAST(OrderNum AS NVARCHAR) LIKE @kw " +
                                "       OR LOWER(CustomerName) LIKE @kw " +
                                "       OR LOWER(SellerName) LIKE @kw " +
                                "       OR LOWER(ISNULL(Status, 'Unpaid')) LIKE @kw) " +
                                "ORDER BY OrderNum DESC";
                        cmd = new SqlCommand(query, connect);
                        cmd.Parameters.AddWithValue("@kw", "%" + kw + "%");
                    }

                    SqlDataAdapter sda = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    sda.Fill(dt);

                    OrdersGV.DataSource = dt;

                    if (OrdersGV.Columns["OrderNum"] != null)
                    {
                        OrdersGV.Columns["OrderNum"].HeaderText = "Order No";
                        OrdersGV.Columns["OrderNum"].FillWeight = 50;
                    }
                    if (OrdersGV.Columns["SellerName"] != null)
                    {
                        OrdersGV.Columns["SellerName"].HeaderText = "Employee";
                        OrdersGV.Columns["SellerName"].FillWeight = 75;
                    }
                    if (OrdersGV.Columns["CustomerName"] != null)
                    {
                        OrdersGV.Columns["CustomerName"].HeaderText = "Customer";
                        OrdersGV.Columns["CustomerName"].FillWeight = 85;
                    }
                    if (OrdersGV.Columns["Time"] != null)
                    {
                        OrdersGV.Columns["Time"].HeaderText = "Date & Time";
                        OrdersGV.Columns["Time"].FillWeight = 90;
                    }
                    if (OrdersGV.Columns["TotalAmount"] != null)
                    {
                        OrdersGV.Columns["TotalAmount"].HeaderText = "Total (TK)";
                        OrdersGV.Columns["TotalAmount"].FillWeight = 65;
                        OrdersGV.Columns["TotalAmount"].DefaultCellStyle.Format = "N2";
                    }
                    if (OrdersGV.Columns["Status"] != null)
                    {
                        OrdersGV.Columns["Status"].HeaderText = "Status";
                        OrdersGV.Columns["Status"].FillWeight = 55;
                        OrdersGV.Columns["Status"].DefaultCellStyle.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
                    }
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

        private void SearchTb_TextChanged(object sender, EventArgs e)
        {
            populate(SearchTb.Text);
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            populate(SearchTb.Text);
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            SearchTb.Text = "";
            populate();
        }

        private void FilterCb_SelectedIndexChanged(object sender, EventArgs e)
        {
            populate(SearchTb.Text);
        }

        private void SearchTb_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                populate(SearchTb.Text);
                e.Handled = true;
                e.SuppressKeyPress = true;
            }
        }

        void MarkOrderAsPaid(int orderNum)
        {
            try
            {
                using (SqlConnection connect = new SqlConnection(Con))
                {
                    connect.Open();

                    string query = "UPDATE Orders SET Status = 'Paid' WHERE OrderNum = @orderNum";

                    SqlCommand cmd = new SqlCommand(query, connect);
                    cmd.Parameters.AddWithValue("@orderNum", orderNum);

                    cmd.ExecuteNonQuery();
                }
            }
            catch (SqlException sqlEx)
            {
                MessageBox.Show("Database error while updating status: " + sqlEx.Message);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Something went wrong: " + ex.Message);
            }
        }

        private void ViewOrders_Load(object sender, EventArgs e)
        {
            populate();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        private void OrdersGV_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                OrdersGV.Rows[e.RowIndex].Selected = true;

                int orderNum = Convert.ToInt32(OrdersGV.CurrentRow.Cells["OrderNum"].Value);

                try
                {
                    printPreviewDialog1.ShowDialog();

                    if (MessageBox.Show("Has the order been printed? Will you mark it as Paid?", "Confirm Payment", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        MarkOrderAsPaid(orderNum);
                        populate();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Print error: " + ex.Message);
                }
            }
        }

        private void printDocument1_PrintPage(object sender, PrintPageEventArgs e)
        {
            Font titleFont = new Font("Arial", 20, FontStyle.Bold);
            Font headingFont = new Font("Arial", 14, FontStyle.Bold);
            Font normalFont = new Font("Arial", 12, FontStyle.Regular);
            Font boldNormalFont = new Font("Arial", 12, FontStyle.Bold);

            int x = 80;
            int y = 50;

            e.Graphics.DrawString("Cafe Management System", titleFont, Brushes.Black, x + 80, y);
            y += 50;

            e.Graphics.DrawString("Order Details", headingFont, Brushes.Black, x, y);
            y += 40;

            if (OrdersGV.CurrentRow != null)
            {
                string orderNumStr = Convert.ToString(OrdersGV.CurrentRow.Cells["OrderNum"].Value);
                string sellerName = Convert.ToString(OrdersGV.CurrentRow.Cells["SellerName"].Value);
                string customerName = Convert.ToString(OrdersGV.CurrentRow.Cells["CustomerName"].Value);
                string time = Convert.ToString(OrdersGV.CurrentRow.Cells["Time"].Value);
                string totalAmount = Convert.ToString(OrdersGV.CurrentRow.Cells["TotalAmount"].Value);

                e.Graphics.DrawString("Order Number: " + orderNumStr, normalFont, Brushes.Black, x, y);
                y += 30;

                e.Graphics.DrawString("Seller Name: " + sellerName, normalFont, Brushes.Black, x, y);
                y += 30;

                e.Graphics.DrawString("Customer Name: " + customerName, normalFont, Brushes.Black, x, y);
                y += 30;

                e.Graphics.DrawString("Time: " + time, normalFont, Brushes.Black, x, y);
                y += 30;

                e.Graphics.DrawString("Total Amount: " + totalAmount + " TK", normalFont, Brushes.Black, x, y);
                y += 50;

                e.Graphics.DrawString("Order Information", headingFont, Brushes.Black, x, y);
                y += 40;

                int xItem = x;
                int xCategory = x + 130;
                int xQty = x + 250;
                int xPrice = x + 310;
                int xTotal = x + 390;

                e.Graphics.DrawString("Item", boldNormalFont, Brushes.Black, xItem, y);
                e.Graphics.DrawString("Category", boldNormalFont, Brushes.Black, xCategory, y);
                e.Graphics.DrawString("Qty", boldNormalFont, Brushes.Black, xQty, y);
                e.Graphics.DrawString("Price", boldNormalFont, Brushes.Black, xPrice, y);
                e.Graphics.DrawString("Total", boldNormalFont, Brushes.Black, xTotal, y);
                y += 25;

                e.Graphics.DrawLine(Pens.Black, x, y, x + 470, y);
                y += 15;

                int orderNum = Convert.ToInt32(orderNumStr);
                decimal grandTotal = 0;

                try
                {
                    using (SqlConnection connect = new SqlConnection(Con))
                    {
                        connect.Open();

                        string detailQuery = "SELECT ItemName, Category, Quantity, UnitPrice, TotalAmount " +
                                              "FROM OrderDetails WHERE OrderNum = @orderNum";

                        SqlCommand cmd = new SqlCommand(detailQuery, connect);
                        cmd.Parameters.AddWithValue("@orderNum", orderNum);

                        SqlDataAdapter sda = new SqlDataAdapter(cmd);
                        DataTable dt = new DataTable();
                        sda.Fill(dt);

                        foreach (DataRow row in dt.Rows)
                        {
                            e.Graphics.DrawString(row["ItemName"].ToString(), normalFont, Brushes.Black, xItem, y);
                            e.Graphics.DrawString(row["Category"].ToString(), normalFont, Brushes.Black, xCategory, y);
                            e.Graphics.DrawString(row["Quantity"].ToString(), normalFont, Brushes.Black, xQty, y);
                            e.Graphics.DrawString(row["UnitPrice"].ToString(), normalFont, Brushes.Black, xPrice, y);
                            e.Graphics.DrawString(row["TotalAmount"].ToString(), normalFont, Brushes.Black, xTotal, y);

                            y += 25;

                            grandTotal += Convert.ToDecimal(row["TotalAmount"]);
                        }
                    }
                }
                catch (SqlException sqlEx)
                {
                    MessageBox.Show("Database error while loading order details: " + sqlEx.Message);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Something went wrong: " + ex.Message);
                }

                y += 10;
                e.Graphics.DrawLine(Pens.Black, x, y, x + 470, y);
                y += 20;

                e.Graphics.DrawString("Grand Total: " + grandTotal + " TK", boldNormalFont, Brushes.Black, xTotal - 100, y);
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