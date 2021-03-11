using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HappyClinic.Model
{
    public class CurrentUser
    {
        private static CurrentUser _instance;
        public static CurrentUser Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new CurrentUser();
                return _instance;
            }
            set
            {
                _instance = value;
            }
        }

        public Account User { get; set; }

        private CurrentUser()
        {
            User = new Account();
        }
    }
}
