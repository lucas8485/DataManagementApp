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

namespace DataManagementApp
{
    public partial class Form2 : Form
    {
        private DataLoad dlm;
        private Form1 frm_caller;
        private int call_id;
        public Form2()
        {
            InitializeComponent();
            call_id = 0;
        }
        public Form2(DataLoad dx,Form1 caller)
        {
            InitializeComponent();
            dlm = new DataLoad(dx.getconnectstr);
            frm_caller = caller;
            call_id = 0;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            call_id = 1;
            frm_caller.Show();
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            FormQueryAll frm_qry = new DataManagementApp.FormQueryAll(dlm);
            frm_qry.Show();
        }

        private void Form2_FormClosing(object sender, FormClosingEventArgs e)
        {
            if(call_id==0)
                 Environment.Exit(0);
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            if (dlm == null)
                MessageBox.Show("Error:\nDataMember DataLoad is NULL!","Exception",MessageBoxButtons.OK,MessageBoxIcon.Error);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            frmAddStock frmAStk = new frmAddStock(dlm.getconnectstr);
            frmAStk.Show();
        }

        private void 关于AToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AboutThisProgram frmAbout = new DataManagementApp.AboutThisProgram();
            frmAbout.Show();
        }

        private void 总库存QToolStripMenuItem_Click(object sender, EventArgs e)
        {
            button1_Click(null, null);
        }

        private void 断开数据库连接EToolStripMenuItem_Click(object sender, EventArgs e)
        {
            button4_Click(null, null);
        }

        private void 入库IToolStripMenuItem_Click(object sender, EventArgs e)
        {
            button2_Click(null, null);
        }

        private void 出库OToolStripMenuItem_Click(object sender, EventArgs e)
        {
            button3_Click(null, null);
        }

        private void button3_Click(object sender, EventArgs e)
        {

            frmRemoveStock frmRStk = new frmRemoveStock(dlm.getconnectstr);
            frmRStk.Show();
        }


        private void 退出QToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
