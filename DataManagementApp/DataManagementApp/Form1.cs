using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data;
using System.IO;

namespace DataManagementApp
{
    public partial class Form1 : Form
    {
        private string s="";
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            if(File.Exists(Application.StartupPath + @"\connection.bin"))
            {
                FileStream fs = new FileStream(Application.StartupPath + @"\connection.bin", FileMode.Open);
                StreamReader br = new StreamReader(fs);
                string[] x = new string[3];
                x[0] = br.ReadLine();
                x[1] = br.ReadLine();
                x[2] = br.ReadLine();
                txtDbIP.Text = x[0];
                txtUsrName.Text = x[1];
                txtPasswd.Text = x[2];
                chkRemPasswd.Checked = true;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            s = @"server=" + txtDbIP.Text + ";User Id=" + txtUsrName.Text + ";password=" + txtPasswd.Text + ";database=datamanagement;Character Set=utf8";
            DataLoad dtload;
            dtload = new DataManagementApp.DataLoad(s);
            if (dtload.initialize())
            {
                if (chkRemPasswd.Checked)
                {
                    FileStream fs = new FileStream(Application.StartupPath + @"\connection.bin",FileMode.OpenOrCreate);
                    StreamWriter bw = new StreamWriter(fs);
                    bw.Write(txtDbIP.Text+"\n"+txtUsrName.Text+"\n"+txtPasswd.Text);
                    bw.Close();
                }
                Form2 mainForm = new DataManagementApp.Form2(new DataManagementApp.DataLoad(dtload.getconnectstr),this);
                mainForm.Show();
                this.Hide();
            }
            else
            {
                MessageBox.Show("数据库初始化失败！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtPasswd.Focus();
                txtPasswd.SelectAll();
                s = "";
            }
        }
    }
}
