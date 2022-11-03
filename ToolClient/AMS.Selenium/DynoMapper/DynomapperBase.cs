using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AMS.Selenium.DynoMapper
{
    public class DynomapperBase
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public DynomapperBase(string userName,string password)
        {
            this.UserName = userName;
            this.Password = password;
        }
    }
}
