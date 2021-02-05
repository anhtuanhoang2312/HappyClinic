using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HappyClinic.Model
{
    public class DataProvider
    {
        private static DataProvider _instance;
        public static DataProvider Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new DataProvider();
                return _instance;
            }
            set
            {
                _instance = value;
            }
        }

        public ClinicEntities DB { get; set; }

        private DataProvider()
        {
            DB = new ClinicEntities();            
        }
    }
}
