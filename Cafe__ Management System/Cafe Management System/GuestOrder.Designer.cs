namespace Cafe_Management_System
{
    partial class GuestOrder
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.panel1 = new System.Windows.Forms.Panel();
            this.LabelDate = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.button5 = new System.Windows.Forms.Button();
            this.CustomerNameTb = new System.Windows.Forms.TextBox();
            this.button2 = new System.Windows.Forms.Button();
            this.LabelAmount = new System.Windows.Forms.Label();
            this.OrdersGv = new System.Windows.Forms.DataGridView();
            this.label2 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.ItemGV = new System.Windows.Forms.DataGridView();
            this.GuestTb = new System.Windows.Forms.TextBox();
            this.QtyTb = new System.Windows.Forms.TextBox();
            this.catCb = new System.Windows.Forms.ComboBox();
            this.OrderNumTb = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.CloseBt = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.OrdersGv)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ItemGV)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(253)))), ((int)(((byte)(251)))), ((int)(((byte)(248)))));
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.LabelDate);
            this.panel1.Controls.Add(this.label6);
            this.panel1.Controls.Add(this.button5);
            this.panel1.Controls.Add(this.CustomerNameTb);
            this.panel1.Controls.Add(this.button2);
            this.panel1.Controls.Add(this.LabelAmount);
            this.panel1.Controls.Add(this.OrdersGv);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.button1);
            this.panel1.Controls.Add(this.ItemGV);
            this.panel1.Controls.Add(this.GuestTb);
            this.panel1.Controls.Add(this.QtyTb);
            this.panel1.Controls.Add(this.catCb);
            this.panel1.Controls.Add(this.OrderNumTb);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Location = new System.Drawing.Point(128, 18);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(967, 514);
            this.panel1.TabIndex = 0;
            this.panel1.Paint += new System.Windows.Forms.PaintEventHandler(this.panel1_Paint);
            // 
            // LabelDate
            // 
            this.LabelDate.AutoSize = true;
            this.LabelDate.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.LabelDate.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(120)))), ((int)(((byte)(100)))), ((int)(((byte)(90)))));
            this.LabelDate.Location = new System.Drawing.Point(820, 20);
            this.LabelDate.Name = "LabelDate";
            this.LabelDate.Size = new System.Drawing.Size(42, 20);
            this.LabelDate.TabIndex = 25;
            this.LabelDate.Text = "Date";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.label6.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(29)))), ((int)(((byte)(22)))));
            this.label6.Location = new System.Drawing.Point(26, 295);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(80, 23);
            this.label6.TabIndex = 24;
            this.label6.Text = "Quantity";
            // 
            // button5
            // 
            this.button5.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(5)))), ((int)(((byte)(150)))), ((int)(((byte)(105)))));
            this.button5.Cursor = System.Windows.Forms.Cursors.Hand;
            this.button5.FlatAppearance.BorderSize = 0;
            this.button5.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button5.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
            this.button5.ForeColor = System.Drawing.Color.White;
            this.button5.Location = new System.Drawing.Point(180, 66);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(90, 29);
            this.button5.TabIndex = 23;
            this.button5.Text = "Refresh";
            this.button5.UseVisualStyleBackColor = false;
            this.button5.Click += new System.EventHandler(this.button5_Click);
            // 
            // CustomerNameTb
            // 
            this.CustomerNameTb.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(253)))), ((int)(((byte)(251)))), ((int)(((byte)(248)))));
            this.CustomerNameTb.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.CustomerNameTb.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.CustomerNameTb.Location = new System.Drawing.Point(30, 198);
            this.CustomerNameTb.Name = "CustomerNameTb";
            this.CustomerNameTb.Size = new System.Drawing.Size(193, 29);
            this.CustomerNameTb.TabIndex = 19;
            this.CustomerNameTb.Text = "CustomerName";
            // 
            // button2
            // 
            this.button2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(29)))), ((int)(((byte)(22)))));
            this.button2.Cursor = System.Windows.Forms.Cursors.Hand;
            this.button2.FlatAppearance.BorderSize = 0;
            this.button2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button2.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Bold);
            this.button2.ForeColor = System.Drawing.Color.White;
            this.button2.Location = new System.Drawing.Point(460, 470);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(200, 34);
            this.button2.TabIndex = 18;
            this.button2.Text = "Place Order";
            this.button2.UseVisualStyleBackColor = false;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // LabelAmount
            // 
            this.LabelAmount.AutoSize = true;
            this.LabelAmount.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.LabelAmount.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(198)))), ((int)(((byte)(125)))), ((int)(((byte)(59)))));
            this.LabelAmount.Location = new System.Drawing.Point(680, 475);
            this.LabelAmount.Name = "LabelAmount";
            this.LabelAmount.Size = new System.Drawing.Size(137, 25);
            this.LabelAmount.TabIndex = 17;
            this.LabelAmount.Text = "OrderAmount";
            this.LabelAmount.Click += new System.EventHandler(this.label3_Click);
            // 
            // OrdersGv
            // 
            this.OrdersGv.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.OrdersGv.Location = new System.Drawing.Point(272, 288);
            this.OrdersGv.Name = "OrdersGv";
            this.OrdersGv.RowHeadersWidth = 51;
            this.OrdersGv.RowTemplate.Height = 24;
            this.OrdersGv.Size = new System.Drawing.Size(692, 175);
            this.OrdersGv.TabIndex = 16;
            this.OrdersGv.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView2_CellContentClick);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.label2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(29)))), ((int)(((byte)(22)))));
            this.label2.Location = new System.Drawing.Point(540, 258);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(114, 28);
            this.label2.TabIndex = 15;
            this.label2.Text = "Your Order";
            this.label2.Click += new System.EventHandler(this.label2_Click);
            // 
            // button1
            // 
            this.button1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(198)))), ((int)(((byte)(125)))), ((int)(((byte)(59)))));
            this.button1.Cursor = System.Windows.Forms.Cursors.Hand;
            this.button1.FlatAppearance.BorderSize = 0;
            this.button1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button1.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Bold);
            this.button1.ForeColor = System.Drawing.Color.White;
            this.button1.Location = new System.Drawing.Point(30, 335);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(193, 40);
            this.button1.TabIndex = 14;
            this.button1.Text = "Add To Cart";
            this.button1.UseVisualStyleBackColor = false;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // ItemGV
            // 
            this.ItemGV.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.ItemGV.Location = new System.Drawing.Point(291, 60);
            this.ItemGV.Name = "ItemGV";
            this.ItemGV.RowHeadersWidth = 51;
            this.ItemGV.RowTemplate.Height = 24;
            this.ItemGV.Size = new System.Drawing.Size(659, 195);
            this.ItemGV.TabIndex = 13;
            this.ItemGV.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellContentClick);
            // 
            // GuestTb
            // 
            this.GuestTb.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(253)))), ((int)(((byte)(251)))), ((int)(((byte)(248)))));
            this.GuestTb.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.GuestTb.Enabled = false;
            this.GuestTb.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.GuestTb.Location = new System.Drawing.Point(30, 152);
            this.GuestTb.Name = "GuestTb";
            this.GuestTb.Size = new System.Drawing.Size(193, 29);
            this.GuestTb.TabIndex = 12;
            this.GuestTb.Text = "Guest";
            // 
            // QtyTb
            // 
            this.QtyTb.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(253)))), ((int)(((byte)(251)))), ((int)(((byte)(248)))));
            this.QtyTb.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.QtyTb.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.QtyTb.Location = new System.Drawing.Point(125, 293);
            this.QtyTb.Name = "QtyTb";
            this.QtyTb.Size = new System.Drawing.Size(98, 29);
            this.QtyTb.TabIndex = 11;
            // 
            // catCb
            // 
            this.catCb.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(253)))), ((int)(((byte)(251)))), ((int)(((byte)(248)))));
            this.catCb.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.catCb.FormattingEnabled = true;
            this.catCb.Items.AddRange(new object[] {
            "Food",
            "Coffee",
            "Chai",
            "Beverages"});
            this.catCb.Location = new System.Drawing.Point(30, 67);
            this.catCb.Name = "catCb";
            this.catCb.Size = new System.Drawing.Size(142, 29);
            this.catCb.TabIndex = 10;
            this.catCb.Text = "Category";
            this.catCb.SelectionChangeCommitted += new System.EventHandler(this.catCb_SelectionChangeCommitted);
            // 
            // OrderNumTb
            // 
            this.OrderNumTb.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(253)))), ((int)(((byte)(251)))), ((int)(((byte)(248)))));
            this.OrderNumTb.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.OrderNumTb.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.OrderNumTb.Location = new System.Drawing.Point(136, 108);
            this.OrderNumTb.Name = "OrderNumTb";
            this.OrderNumTb.Size = new System.Drawing.Size(87, 29);
            this.OrderNumTb.TabIndex = 9;
            this.OrderNumTb.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this.label1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(29)))), ((int)(((byte)(22)))));
            this.label1.Location = new System.Drawing.Point(439, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(175, 32);
            this.label1.TabIndex = 8;
            this.label1.Text = "PLACE ORDER";
            this.label1.Click += new System.EventHandler(this.label1_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Cursor = System.Windows.Forms.Cursors.Hand;
            this.label5.Font = new System.Drawing.Font("Segoe UI", 10.5F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))));
            this.label5.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(212)))), ((int)(((byte)(163)))), ((int)(((byte)(115)))));
            this.label5.Location = new System.Drawing.Point(24, 495);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(79, 25);
            this.label5.TabIndex = 7;
            this.label5.Text = "Log-out";
            this.label5.Click += new System.EventHandler(this.label5_Click);
            // 
            // CloseBt
            // 
            this.CloseBt.AutoSize = true;
            this.CloseBt.Cursor = System.Windows.Forms.Cursors.Hand;
            this.CloseBt.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.CloseBt.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(212)))), ((int)(((byte)(163)))), ((int)(((byte)(115)))));
            this.CloseBt.Location = new System.Drawing.Point(1079, 1);
            this.CloseBt.Name = "CloseBt";
            this.CloseBt.Size = new System.Drawing.Size(25, 28);
            this.CloseBt.TabIndex = 8;
            this.CloseBt.Text = "X";
            this.CloseBt.Click += new System.EventHandler(this.label3_Click_1);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(29)))), ((int)(((byte)(22)))));
            this.label3.Location = new System.Drawing.Point(26, 109);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(92, 23);
            this.label3.TabIndex = 29;
            this.label3.Text = "OrderNum";
            // 
            // GuestOrder
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(28)))), ((int)(((byte)(19)))), ((int)(((byte)(14)))));
            this.ClientSize = new System.Drawing.Size(1103, 550);
            this.Controls.Add(this.CloseBt);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "GuestOrder";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "GuestOrder";
            this.Load += new System.EventHandler(this.GuestOrder_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.OrdersGv)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ItemGV)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox catCb;
        private System.Windows.Forms.TextBox OrderNumTb;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DataGridView ItemGV;
        private System.Windows.Forms.TextBox GuestTb;
        private System.Windows.Forms.TextBox QtyTb;
        private System.Windows.Forms.Label LabelAmount;
        private System.Windows.Forms.DataGridView OrdersGv;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.TextBox CustomerNameTb;
        private System.Windows.Forms.Button button5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label LabelDate;
        private System.Windows.Forms.Label CloseBt;
        private System.Windows.Forms.Label label3;
    }
}