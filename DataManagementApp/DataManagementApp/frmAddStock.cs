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
    public partial class frmAddStock : Form
    {
        private DataLoad _dm;
        private byte[] pic;
        private string path;
        private string id;
        private List<string> _lst;
        private bool onchanged_text;
        public frmAddStock()
        {
            InitializeComponent();
            _dm = null;
            _lst = null;
        }
        public frmAddStock(string constr)
        {
            InitializeComponent();
            _dm = new DataLoad(constr);
            _dm.initialize();
            _lst = new List<string>();

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                path = openFileDialog1.FileName;
                pictureBox1.Load(path);
            }
        }
        private void byte_to_string()
        {
            FileInfo fi = new FileInfo(path);
            pic = new byte[fi.Length];
            FileStream fs = fi.OpenRead();
            fs.Read(pic, 0, Convert.ToInt32(fi.Length));
            fs.Close();
        }
        private void button2_Click(object sender, EventArgs e)
        {
            id = comboBox1.Text;
            if (_lst.Contains(id))
            {
                if (onchanged_text)
                {
                    byte_to_string();
                    string sql = "UPDATE clothes SET NAME = '" + textBox1.Text + "',IMAGE = ?image ,STOCK = '" + numericUpDown1.Value.ToString() + "' WHERE ID='" + id.ToString() + "'";
                    using (MySqlConnection sqlcon = new MySqlConnection(_dm.getconnectstr))
                    {
                        MySqlCommand comm = new MySqlCommand(sql, sqlcon);
                        comm.Parameters.Add("?image", MySqlDbType.LongBlob).Value = pic;
                        sqlcon.Open();
                        comm.ExecuteNonQuery();
                    }
                }
                else
                {
                    string sql = "UPDATE clothes SET STOCK = '" + numericUpDown1.Value.ToString() + "' WHERE ID='" + id.ToString() + "'";
                    _dm.run_queries(sql);
                }
            }
            else
            {
                byte_to_string();
                string sql = "INSERT INTO clothes VALUES('"+id.ToString()+"','"+textBox1.Text+"',?image ,'"+numericUpDown1.Value.ToString()+"')";
                using (MySqlConnection sqlcon = new MySqlConnection(_dm.getconnectstr))
                {
                    MySqlCommand comm = new MySqlCommand(sql, sqlcon);
                    comm.Parameters.Add("?image", MySqlDbType.LongBlob).Value = pic;
                    sqlcon.Open();
                    comm.ExecuteNonQuery();
                }
            }
            MessageBox.Show("入库操作成功！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            Close();
        }

        private void frmAddStock_Load(object sender, EventArgs e)
        {
            using (MySqlConnection sqlcon = new MySql.Data.MySqlClient.MySqlConnection(_dm.getconnectstr))
            {
                sqlcon.Open();
                string query_sql = "SELECT DISTINCT ID FROM clothes";
                MySqlCommand cmd = new MySql.Data.MySqlClient.MySqlCommand(query_sql, sqlcon);
                MySqlDataReader mrd = cmd.ExecuteReader();
                while (mrd.Read())
                {
                    comboBox1.Items.Add(mrd[0]);
                    _lst.Add(mrd[0].ToString());
                }
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            onchanged_text = true;
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            id = comboBox1.Text;
            if (_lst.Contains(id))
            {
                using (MySqlConnection sqlcon = new MySql.Data.MySqlClient.MySqlConnection(_dm.getconnectstr))
                {
                    sqlcon.Open();
                    MySql.Data.MySqlClient.MySqlCommand dt_bin;
                    MySql.Data.MySqlClient.MySqlDataReader dt_rd_bin;
                    string _sql = "SELECT IMAGE FROM clothes WHERE ID = '" + id + "'";
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
                    path = ls_fileName;
                    _sql = "SELECT NAME,STOCK FROM clothes WHERE ID='"+id+"'";
                    dt_bin = new MySql.Data.MySqlClient.MySqlCommand(_sql, sqlcon);
                    dt_rd_bin = dt_bin.ExecuteReader();
                    dt_rd_bin.Read();
                    textBox1.Text = (string)dt_rd_bin[0];
                    numericUpDown1.Value = (int)dt_rd_bin[1];
                }
            }
        }
    }
}
