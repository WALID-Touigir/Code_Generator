using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Windows.Forms;
using static System.Windows.Forms.AxHost;
using System.Diagnostics;
using Code_Generator.TemplateProcessor;
using System.IO;




namespace Code_Generator
{
    public partial class frmMainForm : Form
    {
        //enum enMode { enUpload =1 , enConnect =2}
        //enMode Mode = enMode.enConnect;

        DataTable _dt = new DataTable();

        Stopwatch stopwatch1 = new Stopwatch();
        public frmMainForm()
        {
            InitializeComponent();

            clsBusinessManager.ProgressBar += UpdateProgressBarValue;
        }

        private void UpdateProgressBarValue(int value)
        {
            if (progressBar1.InvokeRequired)
            {
                progressBar1.BeginInvoke(new Action<int>(UpdateProgressBarValue), value);
                return;
            }

                
            if(value == 100)
            { 
                stopwatch1.Stop();

                progressBar1.Value = value;
                lblProgressCount.Text = value.ToString() + "%";

                
            }else
            {
                progressBar1.Value = value;
                lblProgressCount.Text = value.ToString() + "%";
            }

        }

        private void btnConnectSQLserver_Click(object sender, EventArgs e)
        {



            if (!this.ValidateChildren(ValidationConstraints.Enabled))
            {
                
                MessageBox.Show("Please correct the highlighted input errors before connecting.","Validation Error",MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            Thread t = new Thread(()=> _dt = clsConnectionManager.GetDBNames());
            t.Start();

            if (clsUtils.IsValidDatabaseName(txtProjectName.Text.Trim()))
                clsGlobal.ProjectName = txtProjectName.Text.Trim();

            

            clsGlobal.SetCredentials(txtUserName.Text.Trim(), txtPassword.Text.Trim());

            cbDataBaseName.DisplayMember = "name";
            t.Join(); // waiting before if statment

            if (_dt == null || !(_dt.Rows.Count >0))
            {
                MessageBox.Show("UserName or Password Invalid", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
           
            if (chkRememberMe.Checked) // as a tread
            {
                clsGlobal.RememberUsernameAndPassword(txtUserName.Text.Trim(), txtPassword.Text.Trim());
            }
            else
                clsGlobal.RememberUsernameAndPassword("", "");


            cbDataBaseName.Items.Clear();
            cbDataBaseName.DataSource = _dt;
            cbDataBaseName.SelectedIndex = 0;

            
            gbConnectionToSQLserver.Enabled = false;
            gbChooseDB.Enabled = true;
            cbDataBaseName.Focus();


        }

        private void txtPassword_Validating(object sender, CancelEventArgs e)
        {
            TextBox txtB = sender as TextBox;

            if (string.IsNullOrEmpty(txtB.Text))
            {
                errorProvider1.SetError(txtB, "Enter a valid info");
                e.Cancel = true;
            }
            else
                errorProvider1.SetError(txtB, "");


        }
        private void txtProjectName_Validating(object sender, CancelEventArgs e)
        {
            TextBox txtB = sender as TextBox;

            if (string.IsNullOrEmpty(txtB.Text))
            {
                errorProvider1.SetError(txtB, "Enter a valid info");
                e.Cancel = true;
            }
            else
                errorProvider1.SetError(txtB, "");


        }
        private void txtUserName_Validating(object sender, CancelEventArgs e)
        {
            TextBox txtB = sender as TextBox;

            if (string.IsNullOrEmpty(txtB.Text))
            {
                errorProvider1.SetError(txtB, "Enter a valid info");
                e.Cancel = true;
            }
            else
                errorProvider1.SetError(txtB, "");


        }

        private void btnPath_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                string selectedPath = folderBrowserDialog1.SelectedPath;

                if (clsUtils.IsValidPath(selectedPath))
                {
                    txtPath.Text = selectedPath;

                    clsGlobal.PathFilesToGenerate = selectedPath;
                }
                else
                {
                    MessageBox.Show("The path does not exist. Please select a valid path.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnSelectDB_Click(object sender, EventArgs e)
        {
            if (!clsUtils.IsValidDatabaseName(cbDataBaseName.Text))
            {
                MessageBox.Show("The DB Name is not Valid", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (!clsUtils.IsValidFolderName(txtPath.Text))
            {
                MessageBox.Show("Folder Name is not Valid", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                errorProvider1.SetError(txtPath, "choose a correct output path !");
                return;
            }
            else
            {
                errorProvider1.SetError(txtPath, "");
            }

            clsGlobal.DataBaseName = cbDataBaseName.Text;
            clsGlobal.PathFilesToGenerate = txtPath.Text.Trim();
            LoadCheckListWithTables();
            
            gbChooseDB.Enabled = false;
            chkSelectAll.Checked = false;
            gbTables.Enabled = true;
            btnGenerate.Enabled = true;
            DBTablesList.Focus();
            
        }

        private void LoadCheckListWithTables()
        {
            DataTable tablesName = clsBusinessManager.GetAllDBTables();

            if (tablesName.Rows.Count > 0)
            {
                DBTablesList.Items.Clear();
                foreach (DataRow row in tablesName.Rows)
                {
                    string tableName = row[0].ToString();

                    DBTablesList.Items.Add(tableName);
                }

            }
            else
            {
                MessageBox.Show("DataBase Dosn't contain any table", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {   
            if (chkSelectAll.Checked)
                SetAllItemsInListChecked(true);
            else
                SetAllItemsInListChecked(false);
        }

        public void SetAllItemsInListChecked( bool state)
        {
            DBTablesList.BeginUpdate();

            try
            {

                for (int i = 0; i < DBTablesList.Items.Count; i++)
                {
                    DBTablesList.SetItemChecked(i, state);
                }
            }
            finally
            {
                DBTablesList.EndUpdate();
            }
        }

        private void btnGenerate_Click(object sender, EventArgs e)
        {

            if(DBTablesList.CheckedItems.Count == 0)
            {
                MessageBox.Show("There is No Tabel Selected.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return; 
            }

            stopwatch1.Start();

            gbTables.Enabled = false;
            progresPanel.Visible = true;


            Task.Run(() => { 
                foreach (var TableName in DBTablesList.CheckedItems)
                {
                    clsBusinessManager.checkedTables.Add(TableName.ToString());

                }

                if (!clsBusinessManager.GetAllSelectedTables())
                {
                    MessageBox.Show("There is No Tabel Selected.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (!clsBusinessManager.GenerateAllLayers())
                     MessageBox.Show("Generating Layers Failed", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                TimeSpan ts1 = stopwatch1.Elapsed;
                string formattedTime = String.Format("{0:00}:{1:00}.{2:000}", ts1.Minutes, ts1.Seconds, ts1.Milliseconds);


                MessageBox.Show($"Total Time for Generating Layers is {formattedTime}", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);

                if ( MessageBox.Show($"Do You want to Open your {clsGlobal.ProjectName} Folder ?", "Question", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                    return;
                System.Diagnostics.Process.Start(Path.Combine(clsGlobal.PathFilesToGenerate,clsGlobal.ProjectName));
            });

                   
                    



        }


       private void frmMainForm_Load(object sender, EventArgs e)
        {
            string username = "", password = "";
            clsGlobal.GetStoredCredential(ref username, ref password );
            chkRememberMe.Checked = (username != "" && password != "");

            if (chkRememberMe.Checked)
            {
                txtUserName.Text = username;
                txtPassword.Text = password;
            }
            else
            {
                txtUserName.Text = "";
                txtPassword.Text = "";
                clsGlobal.RememberUsernameAndPassword("","");

            }

            //Mode = enMode.enConnect;
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            DBTablesList.Items.Clear();

            gbTables.Enabled = false;
            gbChooseDB.Enabled = true;
        }

        private void rbAddingStaticMethodsYes_CheckedChanged(object sender, EventArgs e)
        {
            clsGlobal.AddingDTOclass = rbAddingDTOclassYes.Checked;
        }


        private void btnUpload_Click(object sender, EventArgs e)
        {
            string sourceFilePath = "";
            string DBname;

            openFileDialog1.Filter = "Database Files (*.bak;*.mdf;*.ldf;)|*.bak;*.mdf;*.ldf;";
            openFileDialog1.FileName = "";

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                sourceFilePath = openFileDialog1.FileName;

                if(clsBusinessManager.RestoreDB(sourceFilePath, out DBname))
                {                    
                    cbDataBaseName.DataSource = clsConnectionManager.GetDBNames();
                    cbDataBaseName.SelectedIndex = cbDataBaseName.FindStringExact(DBname);

                }
                else
                {
                    MessageBox.Show("SQL Server requires permission to access the uploaded file. Please move it to the SQL ‘Backup’ directory or run this app as Administrator.",
                        "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }


                   // Mode = enMode.enUpload; 
                
            }
            

           
        }

        private void chkRememberMe_CheckedChanged(object sender, EventArgs e)
        {

            if(chkRememberMe.Checked == false)
                clsGlobal.RememberUsernameAndPassword("", "");
            
        }

        private void rbAddingStaticMethodsNo_CheckedChanged(object sender, EventArgs e)
        {
            clsGlobal.AddingDTOclass = !rbAddingStaticMethodsNo.Checked;
        }
    }
}
