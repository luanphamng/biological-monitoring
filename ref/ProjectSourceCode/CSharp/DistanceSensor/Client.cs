using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace DistanceSensor
{
    public class Client
    {
        private Socket client;
        private Thread clientListener;
        private bool isEndClientListener;

        private Form1 form1;

        public Client(Form1 f)
        {
            form1 = f;
        }

        Boolean isConnected()
        {
            if (client == null)
                return false;
            else
                return true;
        }

        public void Send(byte[] data)
        {
            if (client != null && data.Length > 0)
            {
                try
                {
                    client.Send(data);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Invaild Password or Username, please press Disconnect button!", "Invaild Password or Username, please press Disconnect button!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    Disconnect();
                }
            }
            else
            {

            }
        }

        public void Connect(string ipAddr, string port)
        {
            client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            IPEndPoint ipe = new IPEndPoint(IPAddress.Parse(ipAddr), Convert.ToInt32(port));
            client.Connect(ipe);

            clientListener = new Thread(OnDataReceived);
            isEndClientListener = false;
            clientListener.Start();
        }
        public void Disconnect()
        {
            if (client != null)
            {
                isEndClientListener = true;
                client.Close();
                client = null;
                clientListener.Abort();
            }
        }

        private void OnDataReceived()//For Client Mode
        {
            try
            {
                while (!isEndClientListener)
                {
                    byte[] receiveData = new byte[client.ReceiveBufferSize];

                    int iRx = client.Receive(receiveData);
                    string szData = "";

                    if (iRx != 0)
                    {

                        if (iRx < receiveData.Length)
                        {
                            byte[] tempData = new byte[iRx];
                            for (int i = 0; i < iRx; i++)
                            {

                                tempData[i] = receiveData[i];
                            }
                            receiveData = tempData;


                        }

                        for (int i = 0; i < receiveData.Length; i++)
                        {
                            szData += char.ConvertFromUtf32(receiveData[i]).ToString();

                        }
                  
                        if (szData.Length > 0)
                        {
                            if (szData == "x")
                            {
                                Disconnect();
                            }
                            else 
                            {
                                if (szData == "f" || szData == "o")
                                {
                                    form1.FeedBack(szData);
                                }
                                else
                                {
                                    form1.FeedBack(szData + "°C");
                                }
                            }
                      
                        }

                    }
                }
            }
            catch (Exception e)
            {//Socket has beenn closed
               
            }

        }

        private static string GetStringFromAsciiHex(String input)
        {
            if (input.Length % 2 != 0)
                throw new ArgumentException("input");

            byte[] bytes = new byte[input.Length / 2];

            for (int i = 0; i < input.Length; i += 2)
            {
                // Split the string into two-bytes strings which represent a hexadecimal value, and convert each value to a byte
                String hex = input.Substring(i, 2);
                bytes[i / 2] = Convert.ToByte(hex, 16);
            }

            return System.Text.ASCIIEncoding.ASCII.GetString(bytes);
        }


    }
}
