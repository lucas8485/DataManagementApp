using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataManagementApp
{
    public class DataOfClothes
    {
        private string _id;
        private string _name;
        private int _stock;
        public DataOfClothes(string id, string name, int stock)
        {
            _id = id;
            _name = name;
            _stock = stock;
        }
        public string readid
        {
            get
            {
                return _id;
            }
        }
        public string readname
        {
            get
            {
                return _name;
            }
        }
        public int readstock
        {
            get
            {
                return _stock;
            }
            set
            {
                _stock = value;
            }
        }
    }
}
