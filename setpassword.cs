using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SQLite;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DBUtility;
using DevExpress.XtraEditors;

namespace folderLocker
{
    public partial class setpassword : XtraForm
    {
        public bool status;
        public string path;

        public setpassword(string path)
        {
            this.path = path;
            status = false;

            InitializeComponent();
            inputpwd.Select();
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            if (inputpwd.Text.Equals(confirmpwd.Text) && inputpwd.Text.Length != 0)
            {
                status = true;
                //the password is valid, save it to sqlite.
                string SQLinsert = "INSERT INTO FolderInfo (folderName,folderpwd) VALUES (@folderName,@folderpwd)";
                SQLiteParameter p1 = new SQLiteParameter("@folderName", path);
                //the password should be encrypted before storing.
                MD5 md5 = new MD5CryptoServiceProvider();
                byte[] plaintext = Encoding.Default.GetBytes(inputpwd.Text);
                byte[] ciphertext = md5.ComputeHash(plaintext);

                string md5Pwd = Convert.ToBase64String(ciphertext);

                //SQLiteParameter p2 = new SQLiteParameter("@folderpwd", inputpwd.Text);
                SQLiteParameter p2 = new SQLiteParameter("@folderpwd", md5Pwd);
                DbHelperSQLite.ExecuteSql(SQLinsert, p1, p2);
                //
                XtraMessageBox.Show(".התיקייה נעולה", "הגדרת סיסמה לתיקייה", MessageBoxButtons.OK, MessageBoxIcon.Information);
                SendKeys.SendWait("{f5}");
                this.Close();
            }
            else
            {
                XtraMessageBox.Show("Passwords don't match or blank password,please retype password");
                //textBox1.Clear();
                confirmpwd.Clear();
                inputpwd.Focus();
            }
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            status = false;
            this.Close();
        }
    }
}
