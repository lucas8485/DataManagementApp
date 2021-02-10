using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using MySql.Data.MySqlClient;

namespace DataManagementApp
{
    public partial class frmRemoveStock : Form
    {

        private DataLoad dt;
        public frmRemoveStock()
        {
            InitializeComponent();
        }
        public frmRemoveStock(string con_str)
        {
            InitializeComponent();
            dt = new DataLoad(con_str);
            dt.initialize();
        }

        private void frmRemoveStock_Load(object sender, EventArgs e)
        {
            dataGridView1.DataSource = dt.get_dts.Tables["clothes"];
            using (MySql.Data.MySqlClient.MySqlConnection sqlcon = new MySql.Data.MySqlClient.MySqlConnection(dt.getconnectstr))
            {
                sqlcon.Open();
                string query_sql = "SELECT DISTINCT ID FROM clothes";
                MySql.Data.MySqlClient.MySqlCommand cmd = new MySql.Data.MySqlClient.MySqlCommand(query_sql, sqlcon);
                MySql.Data.MySqlClient.MySqlDataReader mrd = cmd.ExecuteReader();
                while (mrd.Read())
                {
                    comboBox1.Items.Add(mrd[0]);
                }
            }
        }

        private void comboBox1_TextChanged(object sender, EventArgs e)
        {
            string query_sql = "SELECT ID AS 货号,NAME AS 描述,STOCK AS 库存 FROM clothes WHERE ID like '%" + comboBox1.Text.Trim() + "%'";
            dt.run_queries(query_sql);
            dataGridView1.DataSource = dt.get_new_dts.Tables["clothes"];
        }
        

        private void dataGridView1_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            DataGridViewRow dgvr = dataGridView1.Rows[e.RowIndex];
            DataGridViewCellCollection c = dgvr.Cells;
            textBox1.Text = (string)c[0].Value;
            textBox2.Text = (string)c[1].Value;
            numericUpDown1.Value = (int)c[2].Value;
            using (MySqlConnection sqlcon = new MySql.Data.MySqlClient.MySqlConnection(dt.getconnectstr))
            {
                sqlcon.Open();
                MySql.Data.MySqlClient.MySqlCommand dt_bin;
                MySql.Data.MySqlClient.MySqlDataReader dt_rd_bin;
                string _sql = "SELECT IMAGE FROM clothes WHERE ID = '" + textBox1.Text + "'";
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
                dt_rd_bin.Close();
                pictureBox1.Load(ls_fileName);
            }
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            string sql = "UPDATE clothes SET STOCK = '" + numericUpDown1.Value.ToString() + "' WHERE ID='" + textBox1.Text + "'";
            dt.run_queries(sql);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
