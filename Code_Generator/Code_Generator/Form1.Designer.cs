namespace Code_Generator
{
    partial class frmMainForm
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMainForm));
            this.gbConnectionToSQLserver = new System.Windows.Forms.GroupBox();
            this.label8 = new System.Windows.Forms.Label();
            this.txtProjectName = new System.Windows.Forms.TextBox();
            this.btnConnectSQLserver = new System.Windows.Forms.Button();
            this.lblLogin = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.chkRememberMe = new System.Windows.Forms.CheckBox();
            this.txtPassword = new System.Windows.Forms.TextBox();
            this.txtUserName = new System.Windows.Forms.TextBox();
            this.gbChooseDB = new System.Windows.Forms.GroupBox();
            this.btnUpload = new System.Windows.Forms.Button();
            this.btnSelectDB = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.txtPath = new System.Windows.Forms.TextBox();
            this.btnPath = new System.Windows.Forms.Button();
            this.cbDataBaseName = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.gbTables = new System.Windows.Forms.GroupBox();
            this.progresPanel = new System.Windows.Forms.Panel();
            this.lblProgressCount = new System.Windows.Forms.Label();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.rbAddingStaticMethodsNo = new System.Windows.Forms.RadioButton();
            this.rbAddingDTOclassYes = new System.Windows.Forms.RadioButton();
            this.btnGenerate = new System.Windows.Forms.Button();
            this.label6 = new System.Windows.Forms.Label();
            this.chkSelectAll = new System.Windows.Forms.CheckBox();
            this.DBTablesList = new System.Windows.Forms.CheckedListBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.errorProvider1 = new System.Windows.Forms.ErrorProvider(this.components);
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.gbConnectionToSQLserver.SuspendLayout();
            this.gbChooseDB.SuspendLayout();
            this.gbTables.SuspendLayout();
            this.progresPanel.SuspendLayout();
            this.groupBox4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).BeginInit();
            this.SuspendLayout();
            // 
            // gbConnectionToSQLserver
            // 
            this.gbConnectionToSQLserver.Controls.Add(this.label8);
            this.gbConnectionToSQLserver.Controls.Add(this.txtProjectName);
            this.gbConnectionToSQLserver.Controls.Add(this.btnConnectSQLserver);
            this.gbConnectionToSQLserver.Controls.Add(this.lblLogin);
            this.gbConnectionToSQLserver.Controls.Add(this.label2);
            this.gbConnectionToSQLserver.Controls.Add(this.label1);
            this.gbConnectionToSQLserver.Controls.Add(this.chkRememberMe);
            this.gbConnectionToSQLserver.Controls.Add(this.txtPassword);
            this.gbConnectionToSQLserver.Controls.Add(this.txtUserName);
            this.gbConnectionToSQLserver.Location = new System.Drawing.Point(12, 12);
            this.gbConnectionToSQLserver.Name = "gbConnectionToSQLserver";
            this.gbConnectionToSQLserver.Size = new System.Drawing.Size(597, 297);
            this.gbConnectionToSQLserver.TabIndex = 0;
            this.gbConnectionToSQLserver.TabStop = false;
            this.gbConnectionToSQLserver.Text = "Connection";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.label8.Location = new System.Drawing.Point(11, 184);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(121, 20);
            this.label8.TabIndex = 18;
            this.label8.Text = "ProJect Name:";
            // 
            // txtProjectName
            // 
            this.txtProjectName.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.txtProjectName.Location = new System.Drawing.Point(161, 178);
            this.txtProjectName.Name = "txtProjectName";
            this.txtProjectName.Size = new System.Drawing.Size(279, 26);
            this.txtProjectName.TabIndex = 17;
            this.txtProjectName.Validating += new System.ComponentModel.CancelEventHandler(this.txtProjectName_Validating);
            // 
            // btnConnectSQLserver
            // 
            this.btnConnectSQLserver.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(71)))), ((int)(((byte)(136)))), ((int)(((byte)(199)))));
            this.btnConnectSQLserver.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnConnectSQLserver.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnConnectSQLserver.ForeColor = System.Drawing.Color.Black;
            this.btnConnectSQLserver.Location = new System.Drawing.Point(256, 254);
            this.btnConnectSQLserver.Name = "btnConnectSQLserver";
            this.btnConnectSQLserver.Size = new System.Drawing.Size(104, 37);
            this.btnConnectSQLserver.TabIndex = 16;
            this.btnConnectSQLserver.Text = "Connect";
            this.btnConnectSQLserver.UseVisualStyleBackColor = false;
            this.btnConnectSQLserver.Click += new System.EventHandler(this.btnConnectSQLserver_Click);
            // 
            // lblLogin
            // 
            this.lblLogin.BackColor = System.Drawing.Color.Transparent;
            this.lblLogin.Font = new System.Drawing.Font("Microsoft Tai Le", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblLogin.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(71)))), ((int)(((byte)(136)))), ((int)(((byte)(199)))));
            this.lblLogin.Location = new System.Drawing.Point(68, 18);
            this.lblLogin.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblLogin.Name = "lblLogin";
            this.lblLogin.Size = new System.Drawing.Size(522, 44);
            this.lblLogin.TabIndex = 14;
            this.lblLogin.Text = "Connection To SQL Server";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.label2.Location = new System.Drawing.Point(44, 132);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(88, 20);
            this.label2.TabIndex = 4;
            this.label2.Text = "Password:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.label1.Location = new System.Drawing.Point(41, 84);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(91, 20);
            this.label1.TabIndex = 3;
            this.label1.Text = "Username:";
            // 
            // chkRememberMe
            // 
            this.chkRememberMe.AutoSize = true;
            this.chkRememberMe.Location = new System.Drawing.Point(161, 228);
            this.chkRememberMe.Name = "chkRememberMe";
            this.chkRememberMe.Size = new System.Drawing.Size(119, 20);
            this.chkRememberMe.TabIndex = 2;
            this.chkRememberMe.Text = "Remember Me";
            this.chkRememberMe.UseVisualStyleBackColor = true;
            this.chkRememberMe.CheckedChanged += new System.EventHandler(this.chkRememberMe_CheckedChanged);
            // 
            // txtPassword
            // 
            this.txtPassword.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.txtPassword.Location = new System.Drawing.Point(161, 132);
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.PasswordChar = '*';
            this.txtPassword.Size = new System.Drawing.Size(279, 26);
            this.txtPassword.TabIndex = 1;
            this.txtPassword.Validating += new System.ComponentModel.CancelEventHandler(this.txtPassword_Validating);
            // 
            // txtUserName
            // 
            this.txtUserName.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.txtUserName.Location = new System.Drawing.Point(161, 84);
            this.txtUserName.Name = "txtUserName";
            this.txtUserName.Size = new System.Drawing.Size(279, 26);
            this.txtUserName.TabIndex = 0;
            this.txtUserName.Validating += new System.ComponentModel.CancelEventHandler(this.txtUserName_Validating);
            // 
            // gbChooseDB
            // 
            this.gbChooseDB.Controls.Add(this.btnUpload);
            this.gbChooseDB.Controls.Add(this.btnSelectDB);
            this.gbChooseDB.Controls.Add(this.label5);
            this.gbChooseDB.Controls.Add(this.txtPath);
            this.gbChooseDB.Controls.Add(this.btnPath);
            this.gbChooseDB.Controls.Add(this.cbDataBaseName);
            this.gbChooseDB.Controls.Add(this.label3);
            this.gbChooseDB.Controls.Add(this.label4);
            this.gbChooseDB.Enabled = false;
            this.gbChooseDB.Location = new System.Drawing.Point(12, 309);
            this.gbChooseDB.Name = "gbChooseDB";
            this.gbChooseDB.Size = new System.Drawing.Size(597, 235);
            this.gbChooseDB.TabIndex = 1;
            this.gbChooseDB.TabStop = false;
            this.gbChooseDB.Text = "Select DB";
            // 
            // btnUpload
            // 
            this.btnUpload.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(71)))), ((int)(((byte)(136)))), ((int)(((byte)(199)))));
            this.btnUpload.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnUpload.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnUpload.ForeColor = System.Drawing.Color.Black;
            this.btnUpload.Location = new System.Drawing.Point(465, 86);
            this.btnUpload.Name = "btnUpload";
            this.btnUpload.Size = new System.Drawing.Size(104, 37);
            this.btnUpload.TabIndex = 19;
            this.btnUpload.Text = "Upload";
            this.btnUpload.UseVisualStyleBackColor = false;
            this.btnUpload.Click += new System.EventHandler(this.btnUpload_Click);
            // 
            // btnSelectDB
            // 
            this.btnSelectDB.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(71)))), ((int)(((byte)(136)))), ((int)(((byte)(199)))));
            this.btnSelectDB.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnSelectDB.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnSelectDB.ForeColor = System.Drawing.Color.Black;
            this.btnSelectDB.Location = new System.Drawing.Point(237, 188);
            this.btnSelectDB.Name = "btnSelectDB";
            this.btnSelectDB.Size = new System.Drawing.Size(147, 37);
            this.btnSelectDB.TabIndex = 18;
            this.btnSelectDB.Text = "Select DB";
            this.btnSelectDB.UseVisualStyleBackColor = false;
            this.btnSelectDB.Click += new System.EventHandler(this.btnSelectDB_Click);
            // 
            // label5
            // 
            this.label5.BackColor = System.Drawing.Color.Transparent;
            this.label5.Font = new System.Drawing.Font("Microsoft Tai Le", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(71)))), ((int)(((byte)(136)))), ((int)(((byte)(199)))));
            this.label5.Location = new System.Drawing.Point(205, 18);
            this.label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(213, 44);
            this.label5.TabIndex = 17;
            this.label5.Text = "Choose DB";
            // 
            // txtPath
            // 
            this.txtPath.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.txtPath.Location = new System.Drawing.Point(180, 140);
            this.txtPath.Name = "txtPath";
            this.txtPath.Size = new System.Drawing.Size(260, 26);
            this.txtPath.TabIndex = 17;
            // 
            // btnPath
            // 
            this.btnPath.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(71)))), ((int)(((byte)(136)))), ((int)(((byte)(199)))));
            this.btnPath.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnPath.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnPath.ForeColor = System.Drawing.Color.Black;
            this.btnPath.Location = new System.Drawing.Point(465, 134);
            this.btnPath.Name = "btnPath";
            this.btnPath.Size = new System.Drawing.Size(104, 37);
            this.btnPath.TabIndex = 17;
            this.btnPath.Text = "Browse";
            this.btnPath.UseVisualStyleBackColor = false;
            this.btnPath.Click += new System.EventHandler(this.btnPath_Click);
            // 
            // cbDataBaseName
            // 
            this.cbDataBaseName.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbDataBaseName.FormattingEnabled = true;
            this.cbDataBaseName.Location = new System.Drawing.Point(180, 89);
            this.cbDataBaseName.Name = "cbDataBaseName";
            this.cbDataBaseName.Size = new System.Drawing.Size(260, 24);
            this.cbDataBaseName.TabIndex = 7;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.label3.Location = new System.Drawing.Point(6, 141);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(146, 20);
            this.label3.TabIndex = 6;
            this.label3.Text = "OutPut Files Path:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.label4.Location = new System.Drawing.Point(6, 93);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(143, 20);
            this.label4.TabIndex = 5;
            this.label4.Text = "Data Base Name:";
            // 
            // gbTables
            // 
            this.gbTables.Controls.Add(this.progresPanel);
            this.gbTables.Controls.Add(this.groupBox4);
            this.gbTables.Controls.Add(this.btnGenerate);
            this.gbTables.Controls.Add(this.label6);
            this.gbTables.Controls.Add(this.chkSelectAll);
            this.gbTables.Controls.Add(this.DBTablesList);
            this.gbTables.Controls.Add(this.pictureBox1);
            this.gbTables.Enabled = false;
            this.gbTables.Location = new System.Drawing.Point(615, 12);
            this.gbTables.Name = "gbTables";
            this.gbTables.Size = new System.Drawing.Size(517, 532);
            this.gbTables.TabIndex = 1;
            this.gbTables.TabStop = false;
            this.gbTables.Text = "Select Tabels";
            // 
            // progresPanel
            // 
            this.progresPanel.Controls.Add(this.lblProgressCount);
            this.progresPanel.Controls.Add(this.progressBar1);
            this.progresPanel.Location = new System.Drawing.Point(44, 465);
            this.progresPanel.Name = "progresPanel";
            this.progresPanel.Size = new System.Drawing.Size(442, 57);
            this.progresPanel.TabIndex = 17;
            this.progresPanel.Visible = false;
            // 
            // lblProgressCount
            // 
            this.lblProgressCount.AutoSize = true;
            this.lblProgressCount.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.lblProgressCount.Location = new System.Drawing.Point(355, 14);
            this.lblProgressCount.Name = "lblProgressCount";
            this.lblProgressCount.Size = new System.Drawing.Size(41, 25);
            this.lblProgressCount.TabIndex = 28;
            this.lblProgressCount.Text = "0%";
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(13, 14);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(327, 23);
            this.progressBar1.TabIndex = 29;
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.rbAddingStaticMethodsNo);
            this.groupBox4.Controls.Add(this.rbAddingDTOclassYes);
            this.groupBox4.Font = new System.Drawing.Font("Microsoft Tai Le", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox4.Location = new System.Drawing.Point(18, 315);
            this.groupBox4.Margin = new System.Windows.Forms.Padding(4);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Padding = new System.Windows.Forms.Padding(4);
            this.groupBox4.Size = new System.Drawing.Size(256, 84);
            this.groupBox4.TabIndex = 26;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Adding DTO class:";
            // 
            // rbAddingStaticMethodsNo
            // 
            this.rbAddingStaticMethodsNo.AutoSize = true;
            this.rbAddingStaticMethodsNo.Checked = true;
            this.rbAddingStaticMethodsNo.Font = new System.Drawing.Font("Microsoft Tai Le", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rbAddingStaticMethodsNo.Location = new System.Drawing.Point(97, 35);
            this.rbAddingStaticMethodsNo.Margin = new System.Windows.Forms.Padding(4);
            this.rbAddingStaticMethodsNo.Name = "rbAddingStaticMethodsNo";
            this.rbAddingStaticMethodsNo.Size = new System.Drawing.Size(60, 29);
            this.rbAddingStaticMethodsNo.TabIndex = 22;
            this.rbAddingStaticMethodsNo.TabStop = true;
            this.rbAddingStaticMethodsNo.Text = "No";
            this.rbAddingStaticMethodsNo.UseVisualStyleBackColor = true;
            this.rbAddingStaticMethodsNo.CheckedChanged += new System.EventHandler(this.rbAddingStaticMethodsNo_CheckedChanged);
            // 
            // rbAddingDTOclassYes
            // 
            this.rbAddingDTOclassYes.AutoSize = true;
            this.rbAddingDTOclassYes.Font = new System.Drawing.Font("Microsoft Tai Le", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rbAddingDTOclassYes.Location = new System.Drawing.Point(13, 34);
            this.rbAddingDTOclassYes.Margin = new System.Windows.Forms.Padding(4);
            this.rbAddingDTOclassYes.Name = "rbAddingDTOclassYes";
            this.rbAddingDTOclassYes.Size = new System.Drawing.Size(62, 29);
            this.rbAddingDTOclassYes.TabIndex = 23;
            this.rbAddingDTOclassYes.Text = "Yes";
            this.rbAddingDTOclassYes.UseVisualStyleBackColor = true;
            this.rbAddingDTOclassYes.CheckedChanged += new System.EventHandler(this.rbAddingStaticMethodsYes_CheckedChanged);
            // 
            // btnGenerate
            // 
            this.btnGenerate.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(71)))), ((int)(((byte)(136)))), ((int)(((byte)(199)))));
            this.btnGenerate.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnGenerate.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnGenerate.ForeColor = System.Drawing.Color.Black;
            this.btnGenerate.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnGenerate.Location = new System.Drawing.Point(195, 406);
            this.btnGenerate.Name = "btnGenerate";
            this.btnGenerate.Size = new System.Drawing.Size(147, 37);
            this.btnGenerate.TabIndex = 19;
            this.btnGenerate.Text = "Generate";
            this.btnGenerate.UseVisualStyleBackColor = false;
            this.btnGenerate.Click += new System.EventHandler(this.btnGenerate_Click);
            // 
            // label6
            // 
            this.label6.BackColor = System.Drawing.Color.Transparent;
            this.label6.Font = new System.Drawing.Font("Microsoft Tai Le", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(71)))), ((int)(((byte)(136)))), ((int)(((byte)(199)))));
            this.label6.Location = new System.Drawing.Point(86, 18);
            this.label6.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(355, 44);
            this.label6.TabIndex = 18;
            this.label6.Text = "Select DB Tables";
            // 
            // chkSelectAll
            // 
            this.chkSelectAll.AutoSize = true;
            this.chkSelectAll.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.chkSelectAll.Location = new System.Drawing.Point(20, 78);
            this.chkSelectAll.Name = "chkSelectAll";
            this.chkSelectAll.Size = new System.Drawing.Size(90, 22);
            this.chkSelectAll.TabIndex = 17;
            this.chkSelectAll.Text = "Select All";
            this.chkSelectAll.UseVisualStyleBackColor = true;
            this.chkSelectAll.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
            // 
            // DBTablesList
            // 
            this.DBTablesList.FormattingEnabled = true;
            this.DBTablesList.Location = new System.Drawing.Point(17, 106);
            this.DBTablesList.Name = "DBTablesList";
            this.DBTablesList.Size = new System.Drawing.Size(493, 191);
            this.DBTablesList.TabIndex = 0;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::Code_Generator.Properties.Resources.Next_32;
            this.pictureBox1.Location = new System.Drawing.Point(468, 61);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(42, 39);
            this.pictureBox1.TabIndex = 27;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.Click += new System.EventHandler(this.pictureBox1_Click);
            // 
            // errorProvider1
            // 
            this.errorProvider1.ContainerControl = this;
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // frmMainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1144, 551);
            this.Controls.Add(this.gbTables);
            this.Controls.Add(this.gbChooseDB);
            this.Controls.Add(this.gbConnectionToSQLserver);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmMainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Walid\'s Code Generator";
            this.Load += new System.EventHandler(this.frmMainForm_Load);
            this.gbConnectionToSQLserver.ResumeLayout(false);
            this.gbConnectionToSQLserver.PerformLayout();
            this.gbChooseDB.ResumeLayout(false);
            this.gbChooseDB.PerformLayout();
            this.gbTables.ResumeLayout(false);
            this.gbTables.PerformLayout();
            this.progresPanel.ResumeLayout(false);
            this.progresPanel.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox gbConnectionToSQLserver;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox chkRememberMe;
        private System.Windows.Forms.TextBox txtPassword;
        private System.Windows.Forms.TextBox txtUserName;
        private System.Windows.Forms.GroupBox gbChooseDB;
        private System.Windows.Forms.GroupBox gbTables;
        private System.Windows.Forms.Button btnConnectSQLserver;
        private System.Windows.Forms.Label lblLogin;
        private System.Windows.Forms.ComboBox cbDataBaseName;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button btnSelectDB;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtPath;
        private System.Windows.Forms.Button btnPath;
        private System.Windows.Forms.Button btnGenerate;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.CheckBox chkSelectAll;
        private System.Windows.Forms.CheckedListBox DBTablesList;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.ErrorProvider errorProvider1;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
        private System.Windows.Forms.Panel progresPanel;
        private System.Windows.Forms.Label lblProgressCount;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox txtProjectName;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.RadioButton rbAddingStaticMethodsNo;
        private System.Windows.Forms.RadioButton rbAddingDTOclassYes;
        private System.Windows.Forms.Button btnUpload;
    }
}

