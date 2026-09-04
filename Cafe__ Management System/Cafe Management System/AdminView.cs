using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Cafe_Management_System
{
    public partial class AdminView : Form
    {
        string Con = @"Data Source=DESKTOP-PPPRS94\SQLEXPRESS;Initial Catalog=CafeManagementSystem;Integrated Security=True";

        public AdminView()
        {
            InitializeComponent();

            SetupSmartGrid(DailyTotalOrdersGV);
            SetupSmartGrid(EveryMonthIncomeGV);
            SetupSmartGrid(EmployeeCommissionGV);

            DailyTotalOrdersGV.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            DailyTotalOrdersGV.MultiSelect = false;
            DailyTotalOrdersGV.ReadOnly = true;

            EveryMonthIncomeGV.ReadOnly = true;
            EmployeeCommissionGV.ReadOnly = true;

            populateDailyOrders();
            populateMonthlyIncome();
            populateEmployeeCommission();
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

        void populateDailyOrders(string searchKeyword = "")
        {
            try
            {
                using (SqlConnection connect = new SqlConnection(Con))
                {
                    connect.Open();

                    string trimmed = searchKeyword?.Trim() ?? "";
                    string query;
                    string incomeQuery;
                    SqlCommand cmd;
                    SqlCommand incomeCmd;
                    string summaryLabel = "Daily Total Income";

                    if (string.IsNullOrWhiteSpace(trimmed))
                    {
                        query = "SELECT OrderNum, SellerName, CustomerName, Time AS [Date], TotalAmount, ISNULL(Status, 'Unpaid') AS Status " +
                                "FROM Orders WHERE CAST(Time AS DATE) = CAST(GETDATE() AS DATE) " +
                                "ORDER BY OrderNum DESC";
                        cmd = new SqlCommand(query, connect);

                        incomeQuery = "SELECT ISNULL(SUM(TotalAmount), 0) FROM Orders WHERE CAST(Time AS DATE) = CAST(GETDATE() AS DATE) AND Status = 'Paid'";
                        incomeCmd = new SqlCommand(incomeQuery, connect);
                        summaryLabel = "Daily Total Income";
                    }
                    else
                    {
                        string dayCandidate = trimmed.ToLower().Replace("day", "").Replace("st", "").Replace("nd", "").Replace("rd", "").Replace("th", "").Trim();

                        if (DateTime.TryParse(trimmed, out DateTime parsedDate))
                        {
                            query = "SELECT OrderNum, SellerName, CustomerName, Time AS [Date], TotalAmount, ISNULL(Status, 'Unpaid') AS Status " +
                                    "FROM Orders WHERE CAST(Time AS DATE) = CAST(@targetDate AS DATE) " +
                                    "ORDER BY OrderNum DESC";
                            cmd = new SqlCommand(query, connect);
                            cmd.Parameters.AddWithValue("@targetDate", parsedDate.Date);

                            incomeQuery = "SELECT ISNULL(SUM(TotalAmount), 0) FROM Orders WHERE CAST(Time AS DATE) = CAST(@targetDate AS DATE) AND Status = 'Paid'";
                            incomeCmd = new SqlCommand(incomeQuery, connect);
                            incomeCmd.Parameters.AddWithValue("@targetDate", parsedDate.Date);
                            summaryLabel = "Total Income (" + parsedDate.ToString("dd-MM-yyyy") + ")";
                        }
                        else if (int.TryParse(dayCandidate, out int dayOfMonth) && dayOfMonth >= 1 && dayOfMonth <= 31)
                        {
                            query = "SELECT OrderNum, SellerName, CustomerName, Time AS [Date], TotalAmount, ISNULL(Status, 'Unpaid') AS Status " +
                                    "FROM Orders " +
                                    "WHERE DAY(Time) = @day AND MONTH(Time) = MONTH(GETDATE()) AND YEAR(Time) = YEAR(GETDATE()) " +
                                    "ORDER BY OrderNum DESC";
                            cmd = new SqlCommand(query, connect);
                            cmd.Parameters.AddWithValue("@day", dayOfMonth);

                            incomeQuery = "SELECT ISNULL(SUM(TotalAmount), 0) FROM Orders " +
                                          "WHERE DAY(Time) = @day AND MONTH(Time) = MONTH(GETDATE()) AND YEAR(Time) = YEAR(GETDATE()) AND Status = 'Paid'";
                            incomeCmd = new SqlCommand(incomeQuery, connect);
                            incomeCmd.Parameters.AddWithValue("@day", dayOfMonth);
                            summaryLabel = "Total Income (Day " + dayOfMonth + ")";
                        }
                        else
                        {
                            query = "SELECT OrderNum, SellerName, CustomerName, Time AS [Date], TotalAmount, ISNULL(Status, 'Unpaid') AS Status " +
                                    "FROM Orders " +
                                    "WHERE MONTH(Time) = MONTH(GETDATE()) AND YEAR(Time) = YEAR(GETDATE()) " +
                                    "AND (LOWER(SellerName) LIKE @kw " +
                                    "     OR LOWER(CustomerName) LIKE @kw " +
                                    "     OR CAST(OrderNum AS NVARCHAR) LIKE @kw) " +
                                    "ORDER BY OrderNum DESC";
                            cmd = new SqlCommand(query, connect);
                            cmd.Parameters.AddWithValue("@kw", "%" + trimmed.ToLower() + "%");

                            incomeQuery = "SELECT ISNULL(SUM(TotalAmount), 0) FROM Orders " +
                                          "WHERE MONTH(Time) = MONTH(GETDATE()) AND YEAR(Time) = YEAR(GETDATE()) " +
                                          "AND (LOWER(SellerName) LIKE @kw " +
                                          "     OR LOWER(CustomerName) LIKE @kw " +
                                          "     OR CAST(OrderNum AS NVARCHAR) LIKE @kw) " +
                                          "AND Status = 'Paid'";
                            incomeCmd = new SqlCommand(incomeQuery, connect);
                            incomeCmd.Parameters.AddWithValue("@kw", "%" + trimmed.ToLower() + "%");
                            summaryLabel = "Monthly Total Income";
                        }
                    }

                    SqlDataAdapter sda = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    sda.Fill(dt);

                    object incomeResult = incomeCmd.ExecuteScalar();
                    decimal totalIncome = 0;
                    if (incomeResult != null && decimal.TryParse(incomeResult.ToString(), out decimal inc))
                    {
                        totalIncome = inc;
                    }

                    DataTable displayTable = new DataTable();
                    displayTable.Columns.Add("OrderNum", typeof(string));
                    displayTable.Columns.Add("SellerName", typeof(string));
                    displayTable.Columns.Add("CustomerName", typeof(string));
                    displayTable.Columns.Add("Date", typeof(string));
                    displayTable.Columns.Add("TotalAmount", typeof(decimal));
                    displayTable.Columns.Add("Status", typeof(string));

                    foreach (DataRow row in dt.Rows)
                    {
                        displayTable.Rows.Add(
                            row["OrderNum"].ToString(),
                            row["SellerName"].ToString(),
                            row["CustomerName"].ToString(),
                            row["Date"] != DBNull.Value ? Convert.ToDateTime(row["Date"]).ToString("yyyy-MM-dd HH:mm:ss") : "",
                            row["TotalAmount"] != DBNull.Value ? Convert.ToDecimal(row["TotalAmount"]) : 0m,
                            row["Status"].ToString()
                        );
                    }

                    // Add extra row for Total Income (sum of paid orders)
                    displayTable.Rows.Add(
                        "",
                        summaryLabel,
                        "",
                        "",
                        totalIncome,
                        "Paid"
                    );

                    DailyTotalOrdersGV.DataSource = displayTable;

                    if (DailyTotalOrdersGV.Columns["OrderNum"] != null)
                    {
                        DailyTotalOrdersGV.Columns["OrderNum"].HeaderText = "Order No";
                        DailyTotalOrdersGV.Columns["OrderNum"].FillWeight = 40;
                    }
                    if (DailyTotalOrdersGV.Columns["SellerName"] != null)
                    {
                        DailyTotalOrdersGV.Columns["SellerName"].HeaderText = "Employee";
                        DailyTotalOrdersGV.Columns["SellerName"].FillWeight = 125;
                    }
                    if (DailyTotalOrdersGV.Columns["CustomerName"] != null)
                    {
                        DailyTotalOrdersGV.Columns["CustomerName"].HeaderText = "Customer";
                        DailyTotalOrdersGV.Columns["CustomerName"].FillWeight = 80;
                    }
                    if (DailyTotalOrdersGV.Columns["Date"] != null)
                    {
                        DailyTotalOrdersGV.Columns["Date"].HeaderText = "Date & Time";
                        DailyTotalOrdersGV.Columns["Date"].FillWeight = 85;
                    }
                    if (DailyTotalOrdersGV.Columns["TotalAmount"] != null)
                    {
                        DailyTotalOrdersGV.Columns["TotalAmount"].HeaderText = "Total (TK)";
                        DailyTotalOrdersGV.Columns["TotalAmount"].FillWeight = 65;
                        DailyTotalOrdersGV.Columns["TotalAmount"].DefaultCellStyle.Format = "N2";
                    }
                    if (DailyTotalOrdersGV.Columns["Status"] != null)
                    {
                        DailyTotalOrdersGV.Columns["Status"].HeaderText = "Status";
                        DailyTotalOrdersGV.Columns["Status"].FillWeight = 45;
                        DailyTotalOrdersGV.Columns["Status"].DefaultCellStyle.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
                    }

                    if (DailyTotalOrdersGV.Rows.Count > 0)
                    {
                        DataGridViewRow summaryRow = DailyTotalOrdersGV.Rows[DailyTotalOrdersGV.Rows.Count - 1];
                        summaryRow.DefaultCellStyle.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
                        summaryRow.DefaultCellStyle.BackColor = Color.FromArgb(235, 225, 215);
                        summaryRow.DefaultCellStyle.ForeColor = Color.FromArgb(43, 29, 22);
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

        void populateMonthlyIncome()
        {
            try
            {
                using (SqlConnection connect = new SqlConnection(Con))
                {
                    connect.Open();

                    string query = "SELECT DATENAME(MONTH, Time) AS MonthName, " +
                                   "SUM(TotalAmount) AS TotalIncome, " +
                                   "YEAR(Time) AS Year " +
                                   "FROM Orders " +
                                   "WHERE LOWER(LTRIM(RTRIM(ISNULL(Status, '')))) = 'paid' " +
                                   "GROUP BY YEAR(Time), MONTH(Time), DATENAME(MONTH, Time) " +
                                   "ORDER BY YEAR(Time), MONTH(Time)";

                    SqlDataAdapter sda = new SqlDataAdapter(query, connect);

                    DataTable dt = new DataTable();
                    sda.Fill(dt);

                    EveryMonthIncomeGV.DataSource = dt;

                    if (EveryMonthIncomeGV.Columns["MonthName"] != null)
                        EveryMonthIncomeGV.Columns["MonthName"].HeaderText = "Month";
                    if (EveryMonthIncomeGV.Columns["TotalIncome"] != null)
                    {
                        EveryMonthIncomeGV.Columns["TotalIncome"].HeaderText = "Total Income (TK)";
                        EveryMonthIncomeGV.Columns["TotalIncome"].DefaultCellStyle.Format = "N2";
                    }
                    if (EveryMonthIncomeGV.Columns["Year"] != null)
                        EveryMonthIncomeGV.Columns["Year"].HeaderText = "Year";
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

        void populateEmployeeCommission()
        {
            try
            {
                using (SqlConnection connect = new SqlConnection(Con))
                {
                    connect.Open();

                    string query = "SELECT SellerName AS EmployeeName, " +
                                   "SUM(TotalAmount) AS TotalSell, " +
                                   "SUM(TotalAmount) * 0.01 AS Commission " +
                                   "FROM Orders " +
                                   "WHERE MONTH(Time) = MONTH(GETDATE()) AND YEAR(Time) = YEAR(GETDATE()) " +
                                   "GROUP BY SellerName " +
                                   "ORDER BY TotalSell DESC";

                    SqlDataAdapter sda = new SqlDataAdapter(query, connect);

                    DataTable dt = new DataTable();
                    sda.Fill(dt);

                    EmployeeCommissionGV.DataSource = dt;

                    if (EmployeeCommissionGV.Columns["EmployeeName"] != null)
                        EmployeeCommissionGV.Columns["EmployeeName"].HeaderText = "Employee Name";
                    if (EmployeeCommissionGV.Columns["TotalSell"] != null)
                    {
                        EmployeeCommissionGV.Columns["TotalSell"].HeaderText = "Total Sell (TK)";
                        EmployeeCommissionGV.Columns["TotalSell"].DefaultCellStyle.Format = "N2";
                    }
                    if (EmployeeCommissionGV.Columns["Commission"] != null)
                    {
                        EmployeeCommissionGV.Columns["Commission"].HeaderText = "Commission (TK)";
                        EmployeeCommissionGV.Columns["Commission"].DefaultCellStyle.Format = "N2";
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

        private void AdminView_Load(object sender, EventArgs e)
        {
        }

        private void SearchTb_TextChanged(object sender, EventArgs e)
        {
            populateDailyOrders(SearchTb.Text);
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            populateDailyOrders(SearchTb.Text);
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            SearchTb.Text = "";
            populateDailyOrders();
        }

        private void SearchTb_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                populateDailyOrders(SearchTb.Text);
                e.Handled = true;
                e.SuppressKeyPress = true;
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

        private void button7_Click(object sender, EventArgs e)
        {
            this.Hide();
            AdminForm admin = new AdminForm();
            admin.Show();
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

        private void DailyTotalOrdersGV_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                string orderNumStr = Convert.ToString(DailyTotalOrdersGV.Rows[e.RowIndex].Cells[0].Value);
                if (!int.TryParse(orderNumStr, out _))
                {
                    return;
                }

                DailyTotalOrdersGV.Rows[e.RowIndex].Selected = true;

                try
                {
                    if (printPreviewDialog1.ShowDialog() == DialogResult.OK)
                    {
                        printDocument1.Print();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Print error: " + ex.Message);
                }
            }
        }

        private void printPreviewDialog1_Load(object sender, EventArgs e)
        {

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

            if (DailyTotalOrdersGV.CurrentRow != null)
            {
                string orderNumStr = Convert.ToString(DailyTotalOrdersGV.CurrentRow.Cells[0].Value);
                if (!int.TryParse(orderNumStr, out int orderNum))
                {
                    return;
                }
                string sellerName = Convert.ToString(DailyTotalOrdersGV.CurrentRow.Cells[1].Value);
                string customerName = Convert.ToString(DailyTotalOrdersGV.CurrentRow.Cells[2].Value);
                string time = Convert.ToString(DailyTotalOrdersGV.CurrentRow.Cells[3].Value);
                string totalAmount = Convert.ToString(DailyTotalOrdersGV.CurrentRow.Cells[4].Value);

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

        private void EveryMonthIncomeGV_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void EmployeeCommissionGV_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {
        }

        private void Close_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to exit?", "Confirmation Message", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                Application.Exit();
            }
        }

        private void AdminView_Load_1(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(Form1.username) || !Form1.username.Equals("admin", StringComparison.OrdinalIgnoreCase))
            {
                MessageBox.Show("Access Denied: Only Admin can access Admin View.", "Access Denied", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                UserOrder uorder = new UserOrder();
                uorder.Show();
                this.Hide();
                return;
            }
        }
    }
}