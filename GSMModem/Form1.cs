using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Text.RegularExpressions;

using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Tcp;
using GsmComm.PduConverter;
using GsmComm.PduConverter.SmartMessaging;
using GsmComm.GsmCommunication;
using GsmComm.Interfaces;
using GsmComm.Server;
using System.Globalization;

namespace GSMModem
{
    public partial class Form1 : Form
    {
        private GsmCommMain comm;
        private delegate void SetTextCallback(string text);
        private SmsServer smsServer;
        public Form1()
        {
            InitializeComponent();
        }
        TextBox txtNumber = new TextBox();
        private void btnExit_Click(object sender, EventArgs e)
        {
            Close();
        }
        private void btnSend_Click(object sender, EventArgs e)
        {
            Operation code = new Operation();
            try
            {
                string conString = @"Data Source=desktop-ladhvc3\sqlexpress;Initial Catalog=smstest;Integrated Security=True";

                SqlConnection con = new SqlConnection(conString);
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }
                DataTable dt = new DataTable();

                string query = @"select contact from person"; //past your query here for extracting contacts from database
                SqlCommand cmd = new SqlCommand(query, con);
                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                sda.Fill(dt);
                foreach (DataRow myrow in dt.Rows)
                {
                    txtNumber.Text = Convert.ToString(myrow);
                    Cursor.Current = Cursors.WaitCursor;
                    try
                    {
                        SmsSubmitPdu pdu;
                        byte dcs = (byte)DataCodingScheme.GeneralCoding.Alpha7BitDefault;
                        pdu = new SmsSubmitPdu(txtMessage.Text, Convert.ToString(txtNumber.Text), dcs);
                        
                        int times = 3; //set its value according to the quantity of contacts in your database
                        
                        for (int i = 0; i < times; i++)
                        {
                            comm.SendMessage(pdu);
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message.ToString());
                    }
                }
            }
            catch
            {
                MessageBox.Show("SMS not send");
            }

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            cmbCOM.Items.Add("COM1");
            cmbCOM.Items.Add("COM2");
            cmbCOM.Items.Add("COM3");
            cmbCOM.Items.Add("COM4");
            cmbCOM.Items.Add("COM5");
            cmbCOM.Items.Add("COM6");
        }

        private void btnConnect_Click(object sender, EventArgs e)
        {
            if (cmbCOM.Text == "")
            {
                MessageBox.Show("Invalid Port Name");
                return;
            }
             comm = new GsmCommMain(cmbCOM.Text , 9600, 150);
            Cursor.Current = Cursors.Default;

            bool retry;
            do
            {
                retry = false;
                try
                {
                    Cursor.Current = Cursors.WaitCursor;
                    comm.Open();
                    Cursor.Current = Cursors.Default;
                    MessageBox.Show("Modem Connected Sucessfully");
                }
                catch (Exception)
                {
                    Cursor.Current = Cursors.Default;
                    if (MessageBox.Show(this, "GSM Modem is not available", "Check",
                        MessageBoxButtons.RetryCancel, MessageBoxIcon.Warning) == DialogResult.Retry)
                        retry = true;
                    else
                   {

                        //Close();
                        return;
                    }
               }
            }
            while (retry);

        }

     
        }
    }

