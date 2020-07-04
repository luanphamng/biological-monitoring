using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Windows.Forms;

namespace DistanceSensor
{
    public partial class Form1 : Form
    {
        public Client client;
        public delegate void FeedBackCallback(string text);
        public delegate void UpdateMessageBoxCallback(string text);

        public Form1()
        {
            InitializeComponent();
        }

        public void FeedBack(string text)//Update the text on textBox1
        {
            
            if (this.temperatureDisplay.InvokeRequired)
            {

                FeedBackCallback temp = new FeedBackCallback(FeedBack);
                this.Invoke(temp, new object[] { text });
                string str = "";
                str = text;
                temperatureDisplay.Text = str;
            }
           else
           {
               if (text == "f")
               {
                   Image image = Image.FromFile("off.png");
                   led.Image = image;
               }
               else if (text == "o")
               {
                   Image image = Image.FromFile("on.png");
                   led.Image = image;
               }
               else
               {
                   temperatureDisplay.Text = "";
                   string str = "";
                   str = text;
                   temperatureDisplay.Text = str;
               }
            }
        }

        public void UpdateMessageBox(string text)//Update the text on textBox1
        {
            this.temperatureDisplay.AppendText(text);
        }



        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void connect_Click(object sender, EventArgs e)
        {
            string ipAddr = ip1.Text + "." + ip2.Text + "." + ip3.Text + "." + ip4.Text;
            string port = portInput.Text;
            
            if (IsValidIPAddress(ipAddr) == true)
            {
                    try
                    {
                        if (client == null)
                            client = new Client(this);

                        client.Connect(ipAddr, port);
                      //  client.Send(Encoding.GetEncoding(Constant.EncodingFormat).GetBytes("c"+'\n'));
                        disconect.Enabled = true;
                        connect.Enabled = false;
                        on.Enabled = true;
                    }
                    catch (SocketException se)
                    {
                        MessageBox.Show("Unable to Connect.\r\n" + se.ToString());
                    }
            }
            else
            {
                MessageBox.Show("Invaild Ip Adrress", "Invaild Ip Adrress", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private bool IsEmptyUserNamePasswordFields(string userName, string password)
        {

            if (userName.Length == 0 || password.Length == 0)
            {
                MessageBox.Show("Password and Username field is required!", "Required Fileds Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            else
            {
                return true;
            }
        }


        private bool IsValidIPAddress(string ipaddr)//Validate the input IP address
        {
            try
            {
                IPAddress.Parse(ipaddr);
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {

        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void on_Click(object sender, EventArgs e)
        {
            disconect.PerformClick();
            connect.PerformClick();
            try
            {
                if (client == null)
                    client = new Client(this);

      
                client.Send(Encoding.GetEncoding(Constant.EncodingFormat).GetBytes("o" + '\n'));                
                off.Enabled = true;
                on.Enabled = false;
            }
            catch (SocketException se)
            {
                MessageBox.Show("Unable to Connect.\r\n" + se.ToString());
            }

        }

        private void off_Click(object sender, EventArgs e)
        {
            disconect.PerformClick();
            connect.PerformClick();
            try
            {
                if (client == null)
                    client = new Client(this);

                client.Send(Encoding.GetEncoding(Constant.EncodingFormat).GetBytes("f" + '\n'));
                on.Enabled = true;
                off.Enabled = false;
            }
            catch (SocketException se)
            {
                MessageBox.Show("Unable to Connect.\r\n" + se.ToString());
            }
        }


        private void disconect_Click(object sender, EventArgs e)
        {
            connect.Enabled = true;
            disconect.Enabled = false;
            client.Disconnect();
        }

        private void temperature_Click(object sender, EventArgs e)
        {
            disconect.PerformClick();
            connect.PerformClick();
            try
            {
                if (client == null)
                    client = new Client(this);

                client.Send(Encoding.GetEncoding(Constant.EncodingFormat).GetBytes("c" + '\n'));
            }
            catch (SocketException se)
            {
                MessageBox.Show("Unable to Connect.\r\n" + se.ToString());
            }
        }
    }
}
