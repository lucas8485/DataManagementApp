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
    public partial class FormQuerySingle : Form
    {
        private DataLoad dt;
        private MySql.Data.MySqlClient.MySqlCommand dt_bin;
        private MySql.Data.MySqlClient.MySqlDataReader dt_rd_bin;
        private DataOfClothes _dt;
        public event OnChangedData OnDataChangedEventHandler;
        public FormQuerySingle()
        {
            InitializeComponent();
        }
        public FormQuerySingle(string con_str,DataOfClothes dtofc)
        {
            InitializeComponent();
            dt=new DataLoad(con_str);
            _dt = dtofc;
        }

        private void FormQuerySingle_Load(object sender, EventArgs e)
        {
            dt.initialize();
            txtID.Text = _dt.readid;
            tbxDesc.Text = _dt.readname;
            lblStock.Text = _dt.readstock.ToString();
            using(MySql.Data.MySqlClient.MySqlConnection sqlcon=new MySql.Data.MySqlClient.MySqlConnection(dt.getconnectstr))
            {
                sqlcon.Open();
                string _sql = "SELECT IMAGE FROM clothes WHERE ID = '" + _dt.readid + "'";
                dt_bin = new MySql.Data.MySqlClient.MySqlCommand(_sql, sqlcon);
                dt_rd_bin = dt_bin.ExecuteReader();
                string tempFile = Path.GetTempFileName();
                string ls_fileName = Path.ChangeExtension(tempFile, ".jpg");
                FileStream fs = new FileStream(ls_fileName, FileMode.Create);
                BinaryWriter bw = new BinaryWriter(fs);
                dt_rd_bin.Read();
                byte[] bt = (byte[])dt_rd_bin[0];
                bw.Write(bt);
                bw.Close();
                pbxClothes.Load(ls_fileName);
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            _dt.readstock++;
            string sql = "UPDATE clothes SET STOCK = '" + _dt.readstock.ToString() + "' WHERE ID = '" + _dt.readid + "'";
            dt.run_queries(sql);
            lblStock.Text =_dt.readstock.ToString();
        }

        private void btnMinus_Click(object sender, EventArgs e)
        {

            _dt.readstock--;
            string sql = "UPDATE clothes SET STOCK = '" +_dt.readstock.ToString() + "' WHERE ID = '" + _dt.readid + "'";
            dt.run_queries(sql);
            lblStock.Text = _dt.readstock.ToString();
        }

        private void FormQuerySingle_FormClosing(object sender, FormClosingEventArgs e)
        {
            OnDataChangedEventHandler?.Invoke(this, e);
        }
    }
}
