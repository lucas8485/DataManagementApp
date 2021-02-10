using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Data;
using MySql.Data.MySqlClient;

namespace DataManagementApp
{
    public class DataLoad
    {
        private string StrConnection;
        private MySqlDataAdapter myda;
        private DataSet dts;
        private DataSet new_dts;
        public DataLoad(string strconnect)
        {
            StrConnection = strconnect;
            dts = new DataSet();
            new_dts = new DataSet();
        }
        public string getconnectstr
        {
            get{
                return StrConnection;
            }
        }
        public bool initialize()
        {
            try
            {
                //System.Windows.Forms.MessageBox.Show(StrConnection);
                using (MySqlConnection sqlcon = new MySqlConnection(StrConnection))
                { 
                    sqlcon.Open();
                    //System.Windows.Forms.MessageBox.Show("Initialize successful!");
                    string sql = "SELECT ID AS 货号,NAME AS 描述,STOCK AS 库存 FROM clothes";
                    MySqlCommand comm = new MySqlCommand(sql, sqlcon);
                    myda = new MySqlDataAdapter( comm);
                    myda.Fill(dts,"clothes");
                    if (dts == null)
                        System.Windows.Forms.MessageBox.Show("Dts is null!");
                 }
                 return true;
            }
            catch(System.Exception)
            {
                return false;
            }
        }
        public void run_queries(string sqlstr)
        {
            try
            {
                using (MySqlConnection sqlcon = new MySqlConnection(StrConnection))
                {
                    new_dts = new DataSet();
                    sqlcon.Open();
                    MySqlCommand comm = new MySqlCommand(sqlstr, sqlcon);
                    myda = new MySqlDataAdapter(comm);
                    myda.Fill(new_dts, "clothes");
                }
            }
            finally
            {

            }
        }
        public DataSet get_dts
        {
            get
            {
                return dts;
            }
        }
        public DataSet get_new_dts
        {
            get
            {
                return new_dts;
            }
        }
    }
}
