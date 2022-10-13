using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace GSMModem
{
    class Operation
    {
        public string ipconfig(string filePath)
        {
            StreamReader streamReader = new StreamReader(filePath);
            string text = streamReader.ReadToEnd();
            streamReader.Close();
            return text;
        }

        public void Execute(string SQL)
        {
                SqlConnection con = Main.GetDBConnection();
                con.Open(); 
                SqlCommand cmd = new SqlCommand(SQL, con);
                cmd.ExecuteNonQuery();
        }
    }
}
