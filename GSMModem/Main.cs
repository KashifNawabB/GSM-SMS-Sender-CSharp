using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.IO;

namespace GSMModem
{
    class Main
    {
        public static SqlConnection GetDBConnection()
        {
            Operation ipconf = new Operation();
            string path = Directory.GetCurrentDirectory();
            string path1 = ipconf.ipconfig(path + "\\" + "IPCONfG.txt");
            string strerverIP = path1;

            // Define the Access Database driver and the filename of the database
            SqlConnection conn = new SqlConnection(
        "Data Source=" + strerverIP + @"\CRMIS;Initial Catalog=PSH;User ID=sa;Password=###Reno321");
            return conn;
        }
    }
}
