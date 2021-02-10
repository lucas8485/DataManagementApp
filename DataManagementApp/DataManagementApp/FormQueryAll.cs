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
    public delegate void OnChangedData(object sender, EventArgs e);
    public partial class FormQueryAll : Form
    {
        private DataLoad dm;
        public FormQueryAll()
        {
            InitializeComponent();
        }
        public FormQueryAll(DataLoad dx)
        {
            InitializeComponent();
            dm = new DataLoad(dx.getconnectstr);
            dm.initialize();
        }
        private void FormQueryAll_Load(object sender, EventArgs e)
        {
            dataGridView1.DataSource = dm.get_dts.Tables["clothes"];
            using(MySql.Data.MySqlClient.MySqlConnection sqlcon = new MySql.Data.MySqlClient.MySqlConnection(dm.getconnectstr))
            {
                sqlcon.Open();
                string query_sql = "SELECT DISTINCT ID FROM clothes";
                MySql.Data.MySqlClient.MySqlCommand cmd = new MySql.Data.MySqlClient.MySqlCommand(query_sql,sqlcon);
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
            dm.run_queries(query_sql);
            dataGridView1.DataSource = dm.get_new_dts.Tables["clothes"];
        }
        
        private void setdata(object sender,EventArgs e)
        {
            string query_sql = "SELECT ID AS 货号,NAME AS 描述,STOCK AS 库存 FROM clothes WHERE ID like '%" + comboBox1.Text.Trim() + "%'";
            dm.run_queries(query_sql);
            dataGridView1.DataSource = dm.get_new_dts.Tables["clothes"];
        }

        private void dataGridView1_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            DataGridViewRow dgvr = dataGridView1.Rows[e.RowIndex];
            DataGridViewCellCollection c = dgvr.Cells;
            DataOfClothes obj = new DataManagementApp.DataOfClothes((string)c[0].Value, (string)c[1].Value, (int)c[2].Value);
            FormQuerySingle fqs = new DataManagementApp.FormQuerySingle(dm.getconnectstr, obj);
            fqs.OnDataChangedEventHandler += new OnChangedData(setdata);
            fqs.Show();
        }
    }
}
