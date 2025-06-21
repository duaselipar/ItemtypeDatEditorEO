namespace ItemtypeDatEditor
{
    partial class EditorForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Button btnLoad;
        private System.Windows.Forms.Label lblStatus;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(EditorForm));
            dataGridView1 = new DataGridView();
            btnLoad = new Button();
            lblStatus = new Label();
            txtSearch = new TextBox();
            btnSearch = new Button();
            btnSave = new Button();
            btnExport = new Button();
            btnImport = new Button();
            cmbSearch = new ComboBox();
            groupBox1 = new GroupBox();
            btnUptall = new Button();
            btnUptname = new Button();
            btnAdd = new Button();
            btnConnect = new Button();
            label5 = new Label();
            label4 = new Label();
            label3 = new Label();
            label2 = new Label();
            label1 = new Label();
            txtDb = new TextBox();
            txtPass = new TextBox();
            txtUser = new TextBox();
            txtPort = new TextBox();
            txtHost = new TextBox();
            ((System.ComponentModel.ISupportInitialize)dataGridView1).BeginInit();
            groupBox1.SuspendLayout();
            SuspendLayout();
            // 
            // dataGridView1
            // 
            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView1.Location = new Point(8, 53);
            dataGridView1.Margin = new Padding(2);
            dataGridView1.MultiSelect = false;
            dataGridView1.Name = "dataGridView1";
            dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridView1.Size = new Size(1158, 339);
            dataGridView1.TabIndex = 0;
            // 
            // btnLoad
            // 
            btnLoad.Location = new Point(8, 11);
            btnLoad.Margin = new Padding(2);
            btnLoad.Name = "btnLoad";
            btnLoad.Size = new Size(116, 35);
            btnLoad.TabIndex = 1;
            btnLoad.Text = "Load DAT";
            btnLoad.UseVisualStyleBackColor = true;
            btnLoad.Click += btnLoad_Click;
            // 
            // lblStatus
            // 
            lblStatus.Location = new Point(7, 506);
            lblStatus.Margin = new Padding(2, 0, 2, 0);
            lblStatus.Name = "lblStatus";
            lblStatus.Size = new Size(451, 24);
            lblStatus.TabIndex = 6;
            lblStatus.Text = "Ready";
            lblStatus.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // txtSearch
            // 
            txtSearch.Location = new Point(266, 18);
            txtSearch.Name = "txtSearch";
            txtSearch.Size = new Size(253, 23);
            txtSearch.TabIndex = 7;
            // 
            // btnSearch
            // 
            btnSearch.Location = new Point(525, 18);
            btnSearch.Name = "btnSearch";
            btnSearch.Size = new Size(75, 23);
            btnSearch.TabIndex = 8;
            btnSearch.Text = "Search";
            btnSearch.UseVisualStyleBackColor = true;
            btnSearch.Click += btnSearch_Click;
            // 
            // btnSave
            // 
            btnSave.Location = new Point(1051, 11);
            btnSave.Name = "btnSave";
            btnSave.Size = new Size(116, 35);
            btnSave.TabIndex = 9;
            btnSave.Text = "Save";
            btnSave.UseVisualStyleBackColor = true;
            btnSave.Click += btnSave_Click;
            // 
            // btnExport
            // 
            btnExport.Location = new Point(929, 10);
            btnExport.Name = "btnExport";
            btnExport.Size = new Size(116, 35);
            btnExport.TabIndex = 10;
            btnExport.Text = "Export";
            btnExport.UseVisualStyleBackColor = true;
            btnExport.Click += btnExport_Click;
            // 
            // btnImport
            // 
            btnImport.Location = new Point(807, 10);
            btnImport.Name = "btnImport";
            btnImport.Size = new Size(116, 37);
            btnImport.TabIndex = 11;
            btnImport.Text = "Import";
            btnImport.UseVisualStyleBackColor = true;
            btnImport.Click += btnImport_Click;
            // 
            // cmbSearch
            // 
            cmbSearch.FormattingEnabled = true;
            cmbSearch.Location = new Point(139, 18);
            cmbSearch.Name = "cmbSearch";
            cmbSearch.Size = new Size(121, 23);
            cmbSearch.TabIndex = 12;
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(btnUptall);
            groupBox1.Controls.Add(btnUptname);
            groupBox1.Controls.Add(btnAdd);
            groupBox1.Controls.Add(btnConnect);
            groupBox1.Controls.Add(label5);
            groupBox1.Controls.Add(label4);
            groupBox1.Controls.Add(label3);
            groupBox1.Controls.Add(label2);
            groupBox1.Controls.Add(label1);
            groupBox1.Controls.Add(txtDb);
            groupBox1.Controls.Add(txtPass);
            groupBox1.Controls.Add(txtUser);
            groupBox1.Controls.Add(txtPort);
            groupBox1.Controls.Add(txtHost);
            groupBox1.Location = new Point(7, 397);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(1159, 106);
            groupBox1.TabIndex = 13;
            groupBox1.TabStop = false;
            groupBox1.Text = "MySQL Connection";
            // 
            // btnUptall
            // 
            btnUptall.Location = new Point(1008, 36);
            btnUptall.Name = "btnUptall";
            btnUptall.Size = new Size(145, 46);
            btnUptall.TabIndex = 13;
            btnUptall.Text = "Update All Item Stats";
            btnUptall.UseVisualStyleBackColor = true;
            btnUptall.Click += btnUptall_Click;
            // 
            // btnUptname
            // 
            btnUptname.Location = new Point(857, 36);
            btnUptname.Name = "btnUptname";
            btnUptname.Size = new Size(145, 47);
            btnUptname.TabIndex = 12;
            btnUptname.Text = "Update All Item Name";
            btnUptname.UseVisualStyleBackColor = true;
            btnUptname.Click += btnUptname_Click;
            // 
            // btnAdd
            // 
            btnAdd.Location = new Point(706, 35);
            btnAdd.Name = "btnAdd";
            btnAdd.Size = new Size(145, 49);
            btnAdd.TabIndex = 11;
            btnAdd.Text = "Add All Item";
            btnAdd.UseVisualStyleBackColor = true;
            btnAdd.Click += btnAdd_Click;
            // 
            // btnConnect
            // 
            btnConnect.Location = new Point(542, 62);
            btnConnect.Name = "btnConnect";
            btnConnect.Size = new Size(129, 23);
            btnConnect.TabIndex = 10;
            btnConnect.Text = "Connect";
            btnConnect.UseVisualStyleBackColor = true;
            btnConnect.Click += btnConnect_Click;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(475, 38);
            label5.Name = "label5";
            label5.Size = new Size(61, 15);
            label5.TabIndex = 9;
            label5.Text = "Database :";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(253, 65);
            label4.Name = "label4";
            label4.Size = new Size(63, 15);
            label4.TabIndex = 8;
            label4.Text = "Password :";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(280, 36);
            label3.Name = "label3";
            label3.Size = new Size(36, 15);
            label3.TabIndex = 7;
            label3.Text = "User :";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(61, 65);
            label2.Name = "label2";
            label2.Size = new Size(35, 15);
            label2.TabIndex = 6;
            label2.Text = "Port :";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(13, 36);
            label1.Name = "label1";
            label1.Size = new Size(83, 15);
            label1.TabIndex = 5;
            label1.Text = "Hostname/IP :";
            // 
            // txtDb
            // 
            txtDb.Location = new Point(542, 33);
            txtDb.Name = "txtDb";
            txtDb.Size = new Size(129, 23);
            txtDb.TabIndex = 4;
            // 
            // txtPass
            // 
            txtPass.Location = new Point(322, 62);
            txtPass.Name = "txtPass";
            txtPass.Size = new Size(129, 23);
            txtPass.TabIndex = 3;
            // 
            // txtUser
            // 
            txtUser.Location = new Point(322, 33);
            txtUser.Name = "txtUser";
            txtUser.Size = new Size(129, 23);
            txtUser.TabIndex = 2;
            // 
            // txtPort
            // 
            txtPort.Location = new Point(102, 62);
            txtPort.Name = "txtPort";
            txtPort.Size = new Size(129, 23);
            txtPort.TabIndex = 1;
            // 
            // txtHost
            // 
            txtHost.Location = new Point(102, 33);
            txtHost.Name = "txtHost";
            txtHost.Size = new Size(129, 23);
            txtHost.TabIndex = 0;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1178, 532);
            Controls.Add(groupBox1);
            Controls.Add(cmbSearch);
            Controls.Add(btnImport);
            Controls.Add(btnExport);
            Controls.Add(btnSave);
            Controls.Add(btnSearch);
            Controls.Add(txtSearch);
            Controls.Add(lblStatus);
            Controls.Add(btnLoad);
            Controls.Add(dataGridView1);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            Icon = (Icon)resources.GetObject("$this.Icon");
            Margin = new Padding(2);
            MaximizeBox = false;
            Name = "Form1";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Itemtype.dat Editor by DuaSelipar";
            ((System.ComponentModel.ISupportInitialize)dataGridView1).EndInit();
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();

            // ** COLUMN SETUP WAJIB BUAT SELEPAS InitializeComponent() **
            // (letak kat Form1.cs constructor atau method sendiri)
        }

        #endregion

        private TextBox txtSearch;
        private Button btnSearch;
        private Button btnSave;
        private Button btnExport;
        private Button btnImport;
        private ComboBox cmbSearch;
        private GroupBox groupBox1;
        private Button btnConnect;
        private Label label5;
        private Label label4;
        private Label label3;
        private Label label2;
        private Label label1;
        private TextBox txtDb;
        private TextBox txtPass;
        private TextBox txtUser;
        private TextBox txtPort;
        private TextBox txtHost;
        private Button btnAdd;
        private Button btnUptname;
        private Button btnUptall;
    }
}
