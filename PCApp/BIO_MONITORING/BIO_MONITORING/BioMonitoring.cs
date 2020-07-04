using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO.Ports;
using System.IO;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;

namespace BIO_MONITORING
{

    public partial class BioMonitoring : Form
    {
        //Globa var
        public Client client;
        string[] G_DataSerial= { "", "", "", "" };
        string G_DataSerialBuffer;
        public BioMonitoring()
        {
            InitializeComponent();
            pictureBoxMainPump.Image = new Bitmap(Application.StartupPath + "\\Resources\\pumpRunGreen.png");
            btnMainPumpStartStop.Text = "Stop";

            
            // Read user data from previous run
            // Read each line of the file into a string array. Each element
            // of the array is one line of the file.
            if (File.Exists(Application.StartupPath + "\\AppData.txt"))
            {
                string[] lines = System.IO.File.ReadAllLines(Application.StartupPath + "\\AppData.txt");

                // Display the file contents by using a foreach loop.
                Debug.WriteLine("Contents of AppData.txt = ");
                foreach (string line in lines)
                {
                    // Use a tab to indent each line of the file.
                    Debug.WriteLine(line);

                }
                try
                {
                    cBoxComPorts.Text = lines.ElementAt(Constant.COM_PORT);
                    cBoxBaurate.Text = lines.ElementAt(Constant.COM_BAURATE);
                    tBoxNutriAStepper.Text = lines.ElementAt(Constant.STEPPER);
                    tBoxMainPumpSpeed.Text = lines.ElementAt(Constant.MAIN_PUMP_SPEED);                    
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Fail to load config file. All parameters will set to default. Click OK to continue!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);                    
                }                
                
            }
        }

        public void FeedBack(string text)//Update the text on DASHBOARD
        {
            string _tempDataRecieve = null;
            char[] separator = { ',' };
            _tempDataRecieve = text.Trim();
            G_DataSerial = _tempDataRecieve.Split(separator);
            if (G_DataSerial.Length != Constant.TOTAL_FIELD_RECIEVE)
                return;
            SetText(G_DataSerial);
            //if (this.temperatureDisplay.InvokeRequired)
            //{

            //    FeedBackCallback temp = new FeedBackCallback(FeedBack);
            //    this.Invoke(temp, new object[] { text });
            //    string str = "";
            //    str = text;
            //    temperatureDisplay.Text = str;
            //}
            //else
            //{
            //    if (text == "f")
            //    {
            //        Image image = Image.FromFile("off.png");
            //        led.Image = image;
            //    }
            //    else if (text == "o")
            //    {
            //        Image image = Image.FromFile("on.png");
            //        led.Image = image;
            //    }
            //    else
            //    {
            //        temperatureDisplay.Text = "";
            //        string str = "";
            //        str = text;
            //        temperatureDisplay.Text = str;
            //    }
            //}
        }

        private void buttonMainPumpStartStop_Click(object sender, EventArgs e)
        {
            bool mainPumpStatus = false;
            string mainPumpSpeed = null;

            if (btnMainPumpStartStop.Text == "Stop")
            {
                mainPumpStatus = false;
                pictureBoxMainPump.Image = new Bitmap(Application.StartupPath + "\\Resources\\pumpStop.png");
            }

            if (btnMainPumpStartStop.Text == "Start")
            {
                mainPumpStatus = true;
                pictureBoxMainPump.Image = new Bitmap(Application.StartupPath + "\\Resources\\pumpRunGreen.png");
            }

            mainPumpSpeed = mainPumpStatus ? tBoxMainPumpSpeed.Text : "0";

            if (serialPort1.IsOpen) // If use use serial
            {
                try
                {
                    serialPort1.Write(tBoxNutriAStepper.Text + "," + mainPumpSpeed);
                    Debug.WriteLine(tBoxNutriAStepper.Text + "," + mainPumpSpeed);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else // If user use Ethernet
            {
                try
                {
                    if (client == null)
                        client = new Client(this);
                    string _dataSend = tBoxNutriAStepper.Text + "," + mainPumpSpeed;
                    client.Send(Encoding.GetEncoding(Constant.EncodingFormat).GetBytes(_dataSend + '\n'));
                    Debug.WriteLine(tBoxNutriAStepper.Text + "," + mainPumpSpeed);
                }
                catch (SocketException se)
                {
                    MessageBox.Show("Unable to Connect.\r\n" + se.ToString());
                }
            }
            btnMainPumpStartStop.Text = (mainPumpStatus)? ArduinoFormat.buttonStop:ArduinoFormat.buttonStart;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            string[] ports = SerialPort.GetPortNames();
            cBoxComPorts.Items.AddRange(ports);
        }

        private void btnSerialPort_Click(object sender, EventArgs e)
        {
            if(btnSerialPort.Text == ArduinoFormat.Connect)
            {
                if (!serialPort1.IsOpen)
                {
                    try
                    {
                        //User select Port name & BauRate
                        serialPort1.PortName = cBoxComPorts.Text;
                        serialPort1.BaudRate = Convert.ToInt32(cBoxBaurate.Text);
                        //Parity, Stopbits, Databits is default with Arduino Framework
                        serialPort1.Parity = Parity.None;
                        serialPort1.StopBits = StopBits.One;
                        serialPort1.DataBits = 8;

                        serialPort1.Open();
                        if (serialPort1.IsOpen)
                        {
                            UpdateButtonEnableStatus(true);
                            btnConnectEthernet.Enabled = false;
                        }
                        btnSerialPort.Text = ArduinoFormat.Disconnect;
                        return;

                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }
                else
                {
                    return;
                }
            }
            
            if(btnSerialPort.Text == ArduinoFormat.Disconnect)
            {
                if (serialPort1.IsOpen)
                {
                    serialPort1.Close();

                    UpdateButtonEnableStatus(false);
                    btnConnectEthernet.Enabled = true;

                    btnSerialPort.Text = ArduinoFormat.Connect;
                    return;
                }
                else
                {
                    return;
                }
            }            
        }

        // On form close, save all user data
        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            if(serialPort1.IsOpen)
            {
                serialPort1.Close();
            }
            // Create a string array with the lines of text
            string[] lines = {                
                cBoxComPorts.Text,      // 0
                cBoxBaurate.Text,       // 1
                tBoxNutriAStepper.Text, // 2
                tBoxMainPumpSpeed.Text, // 3                                
            };

            // Set a variable to the Documents path.
            string docPath = Application.StartupPath;

            // Write the string array to a new file named "WriteLines.txt".
            using (StreamWriter outputFile = new StreamWriter(Path.Combine(docPath, "AppData.txt")))
            {
                foreach (string line in lines)
                {
                    outputFile.WriteLine(line);
                    Debug.WriteLine(line);
                }
            }
        }

        private void Timer1_Tick(object sender, EventArgs e)
        {
            //string _tempDataRecieve = null;
            //if (serialPort1.IsOpen)
            //{
            //    _tempDataRecieve =  serialPort1.ReadExisting();

            //}            
            //if (_tempDataRecieve != null)
            //{
            //    Debug.WriteLine(_tempDataRecieve);
            //    char[] separator = { ',' };

            //    _tempDataRecieve = _tempDataRecieve.Trim();
            //    G_DataSerial = _tempDataRecieve.Split(separator);
            //}
            if (client != null)
            {
                if (client.isConnected())
                {
                    client.Send(Encoding.GetEncoding(Constant.EncodingFormat).GetBytes(ArduinoFormat.requestSensor + '\n'));
                    Debug.Write("Send Request:" + ArduinoFormat.requestSensor);
                }
            }

        }

        //Data Processing
        //Display all processed data on the app.
        public void Display(string[] _input)
        {
            int x = (int)SensorEnum.SensorPosition.DUAL_PIPE_TEMP;
        }

        // Send data
        private void serialPort1_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            string _tempDataRecieve = null;
            if (serialPort1.IsOpen)
            {
                _tempDataRecieve = serialPort1.ReadExisting();
                Debug.WriteLine("Raw data recieve:" + _tempDataRecieve + " :end raw.");                                                              
            }            
            if (_tempDataRecieve != null)
            {
                if (_tempDataRecieve.Contains(":"))
                    return;
                else if (_tempDataRecieve.Contains("<"))
                {
                    G_DataSerialBuffer = _tempDataRecieve;
                }
                else if(_tempDataRecieve.Contains(">"))
                {
                    G_DataSerialBuffer += _tempDataRecieve;
                    Debug.WriteLine("Conbine buffer data G_DataSerialBuffer: " + G_DataSerialBuffer);
                    string strCorrectData = GetBetween(G_DataSerialBuffer, "<", ">");
                    Debug.WriteLine("Correct data: " + strCorrectData);
                    char[] mySeperator = { ',' };
                    strCorrectData = strCorrectData.Trim();
                    G_DataSerial = strCorrectData.Split(mySeperator);
                    if (G_DataSerial.Length == Constant.TOTAL_FIELD_RECIEVE)
                    {
                        SetText(G_DataSerial);
                        return;
                    }
                    // Empty for next use
                    G_DataSerialBuffer = "";
                }
                else
                {
                    return;
                }
               
            }
        }

        public string GetBetween(string content, string startString, string endString)
        {
            int Start = 0, End = 0;
            if (content.Contains(startString) && content.Contains(endString))
            {
                Start = content.IndexOf(startString, 0) + startString.Length;
                End = content.IndexOf(endString, Start);
                return content.Substring(Start, End - Start);
            }
            else
                return string.Empty;
        }

        // This function safe update value for all unit on dash board through serial.
        delegate void SetTextCallback(string[] text);

        private void SetText(string[] text)
        {
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (this.tBoxStorage25Temp.InvokeRequired)
            {
                SetTextCallback d = new SetTextCallback(SetText);
                this.Invoke(d, new object[] { text });
            }
            else
            {
                this.tBoxStorage25Temp.Text = text[Constant.TEMPSTORAGE25];
            }

            if (this.tBoxStorage25Turbidity.InvokeRequired)
            {
                SetTextCallback d = new SetTextCallback(SetText);
                this.Invoke(d, new object[] { text });
            }
            else
            {
                this.tBoxStorage25Turbidity.Text = text[Constant.TURBIDITY];
            }

            if (this.tBoxDualPipeTemp.InvokeRequired)
            {
                SetTextCallback d = new SetTextCallback(SetText);
                this.Invoke(d, new object[] { text });
            }
            else
            {
                this.tBoxDualPipeTemp.Text = text[Constant.TEMP_DUALPIPE1];
            }

            if (this.tBoxDualPipeTempU.InvokeRequired)
            {
                SetTextCallback d = new SetTextCallback(SetText);
                this.Invoke(d, new object[] { text });
            }
            else
            {
                this.tBoxDualPipeTempU.Text = text[Constant.TEMP_DUALPIPE2];
            }

            if (this.tBoxTempBeforeCooler.InvokeRequired)
            {
                SetTextCallback d = new SetTextCallback(SetText);
                this.Invoke(d, new object[] { text });
            }
            else
            {
                this.tBoxTempBeforeCooler.Text = text[Constant.TEMP_BEFORECOOLER];
            }

            if (this.tBoxLightIntensity.InvokeRequired)
            {
                SetTextCallback d = new SetTextCallback(SetText);
                this.Invoke(d, new object[] { text });
            }
            else
            {
                this.tBoxLightIntensity.Text = text[Constant.LIGHT_INTENSITY];
            }

            if (this.tBoxStorage25Conduct.InvokeRequired)
            {
                SetTextCallback d = new SetTextCallback(SetText);
                this.Invoke(d, new object[] { text });
            }
            else
            {
                this.tBoxStorage25Conduct.Text = text[Constant.CONDUCTIVITY];
            }

            // Pipe flow
            if (this.tBoxPipeFlow1.InvokeRequired)
            {
                SetTextCallback d = new SetTextCallback(SetText);
                this.Invoke(d, new object[] { text });
            }
            else
            {
                this.tBoxPipeFlow1.Text = text[Constant.PIPE_FLOW1];
            }

            if (this.tBoxPipeFlow2.InvokeRequired)
            {
                SetTextCallback d = new SetTextCallback(SetText);
                this.Invoke(d, new object[] { text });
            }
            else
            {
                this.tBoxPipeFlow2.Text = text[Constant.PIPE_FLOW2];
            }

            if (this.tBoxPipeFlow3.InvokeRequired)
            {
                SetTextCallback d = new SetTextCallback(SetText);
                this.Invoke(d, new object[] { text });
            }
            else
            {
                this.tBoxPipeFlow3.Text = text[Constant.PIPE_FLOW3];
            }

            if (this.tBoxMainFlow.InvokeRequired)
            {
                SetTextCallback d = new SetTextCallback(SetText);
                this.Invoke(d, new object[] { text });
            }
            else
            {
                this.tBoxMainFlow.Text = text[Constant.MAIN_FLOW];
            }

            if (this.tBoxGasFlow.InvokeRequired)
            {
                SetTextCallback d = new SetTextCallback(SetText);
                this.Invoke(d, new object[] { text });
            }
            else
            {
                this.tBoxGasFlow.Text = text[Constant.GAS_FLOW];
            }

            //Level sensor 
            if (this.lbLevelNutriA.InvokeRequired)
            {
                SetTextCallback d = new SetTextCallback(SetText);
                this.Invoke(d, new object[] { text });
            }
            else
            {
                if (text[Constant.LEVEL_NUTRIA].Contains("0"))
                {
                    this.lbLevelNutriA.Text = ArduinoFormat.LevelSensorNeedFill;
                    this.lbLevelNutriA.ForeColor = Color.Red;
                }
                else
                {
                    this.lbLevelNutriA.Text = ArduinoFormat.LevelSensorGood;
                    this.lbLevelNutriA.ForeColor = Color.Green;
                }
            }

            if (this.lbStorage25.InvokeRequired)
            {
                SetTextCallback d = new SetTextCallback(SetText);
                this.Invoke(d, new object[] { text });
            }
            else
            {
                if (text[Constant.LEVEL_STORAGE25].Contains("0"))
                {
                    this.lbStorage25.Text = ArduinoFormat.LevelSensorNeedFill;
                    this.lbStorage25.ForeColor = Color.Red;
                }
                else
                {
                    this.lbStorage25.Text = ArduinoFormat.LevelSensorGood;
                    this.lbStorage25.ForeColor = Color.Green;
                }
            }

            if (this.lbSterileWater.InvokeRequired)
            {
                SetTextCallback d = new SetTextCallback(SetText);
                this.Invoke(d, new object[] { text });
            }
            else
            {
                if (text[Constant.LEVEL_STERILE].Contains("0"))
                {
                    this.lbSterileWater.Text = ArduinoFormat.LevelSensorNeedFill;
                    this.lbSterileWater.ForeColor = Color.Red;
                }
                else
                {
                    this.lbSterileWater.Text = ArduinoFormat.LevelSensorGood;
                    this.lbSterileWater.ForeColor = Color.Green;
                }
            }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            string mainPumpSpeed = null;
            if (btnMainPumpStartStop.Text == ArduinoFormat.buttonStart)
                mainPumpSpeed = "0";
            if(btnMainPumpStartStop.Text == ArduinoFormat.buttonStop)
                mainPumpSpeed = tBoxMainPumpSpeed.Text;

            if (serialPort1.IsOpen) // If use use serial
            {
                try
                {
                    serialPort1.Write(tBoxNutriAStepper.Text + "," + mainPumpSpeed);
                    Debug.WriteLine(tBoxNutriAStepper.Text + "," + mainPumpSpeed);
                    return;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }
            else // If user use Ethernet
            {
                try
                {
                    if (client == null)
                        client = new Client(this);
                    string _dataSend = tBoxNutriAStepper.Text + "," + mainPumpSpeed;
                    client.Send(Encoding.GetEncoding(Constant.EncodingFormat).GetBytes(_dataSend + '\n'));
                    Debug.WriteLine(tBoxNutriAStepper.Text + "," + mainPumpSpeed);
                }
                catch (SocketException se)
                {
                    MessageBox.Show("Unable to Connect.\r\n" + se.ToString());
                }
            }
        }

        // Check which action does the VaccumPump button do : Start or Stop
        // Safe send command to Arduino by Serial port if Serial is opened, send by Ethernet if Serial is closed
        private void btnVaccumPump_Click(object sender, EventArgs e)
        {
            bool IOStatus = false;            
            if (btnVaccumPump.Text == ArduinoFormat.buttonStop)
            {
                IOStatus = false;
            }
            else if (btnVaccumPump.Text == ArduinoFormat.buttonStart)
            {
                IOStatus = true;
            }
            btnVaccumPump.Text = IOStatus ? ArduinoFormat.buttonStop : ArduinoFormat.buttonStart;
            if (serialPort1.IsOpen) // If use use serial
            {
                try
                {                    
                    serialPort1.Write(IOStatus ? ArduinoFormat.VacuumPumpON : ArduinoFormat.VacuumPumpOFF);
                    Debug.WriteLine(IOStatus ? ArduinoFormat.VacuumPumpON : ArduinoFormat.VacuumPumpOFF);
                    return;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }
            else // If user use Ethernet
            {
                try
                {
                    if (client == null)
                        client = new Client(this);
                    string _dataSend = IOStatus ? ArduinoFormat.VacuumPumpON : ArduinoFormat.VacuumPumpOFF;
                    client.Send(Encoding.GetEncoding(Constant.EncodingFormat).GetBytes(_dataSend + '\n'));
                    Debug.WriteLine(IOStatus ? ArduinoFormat.VacuumPumpON : ArduinoFormat.VacuumPumpOFF);
                }
                catch (SocketException se)
                {
                    MessageBox.Show("Unable to Connect.\r\n" + se.ToString());
                }
            }            
        }           

        private void btnUVLight_Click(object sender, EventArgs e)
        {
            bool IOStatus = false;
            if (btnUVLight.Text == ArduinoFormat.buttonStop)
            {
                IOStatus = false;
            }
            else if (btnUVLight.Text == ArduinoFormat.buttonStart)
            {
                IOStatus = true;
            }
            btnUVLight.Text = IOStatus ? ArduinoFormat.buttonStop : ArduinoFormat.buttonStart;
            if (serialPort1.IsOpen) // If use use serial
            {
                try
                {
                    serialPort1.Write(IOStatus ? ArduinoFormat.UVLightON : ArduinoFormat.UVLightOFF);
                    Debug.WriteLine(IOStatus ? ArduinoFormat.UVLightON : ArduinoFormat.UVLightOFF);
                    return;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }
            else // If user use Ethernet
            {
                try
                {
                    if (client == null)
                        client = new Client(this);
                    string _dataSend = IOStatus ? ArduinoFormat.UVLightON : ArduinoFormat.UVLightOFF;
                    client.Send(Encoding.GetEncoding(Constant.EncodingFormat).GetBytes(_dataSend + '\n'));
                    Debug.WriteLine(IOStatus ? ArduinoFormat.UVLightON : ArduinoFormat.UVLightOFF);
                }
                catch (SocketException se)
                {
                    MessageBox.Show("Unable to Connect.\r\n" + se.ToString());
                }
            }
        }

        private void btnCooler_Click(object sender, EventArgs e)
        {
            bool IOStatus = false;
            if (btnCooler.Text == ArduinoFormat.buttonStop)
            {
                IOStatus = false;
            }
            else if (btnCooler.Text == ArduinoFormat.buttonStart)
            {
                IOStatus = true;
            }
            btnCooler.Text = IOStatus ? ArduinoFormat.buttonStop : ArduinoFormat.buttonStart;
            if (serialPort1.IsOpen) // If use use serial
            {
                try
                {
                    serialPort1.Write(IOStatus ? ArduinoFormat.CoolerON : ArduinoFormat.CoolerOFF);
                    Debug.WriteLine(IOStatus ? ArduinoFormat.CoolerON : ArduinoFormat.CoolerOFF);
                    return;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }
            else // If user use Ethernet
            {
                try
                {
                    if (client == null)
                        client = new Client(this);
                    string _dataSend = IOStatus ? ArduinoFormat.CoolerON : ArduinoFormat.CoolerOFF;
                    client.Send(Encoding.GetEncoding(Constant.EncodingFormat).GetBytes(_dataSend + '\n'));
                    Debug.WriteLine(IOStatus ? ArduinoFormat.CoolerON : ArduinoFormat.CoolerOFF);
                }
                catch (SocketException se)
                {
                    MessageBox.Show("Unable to Connect.\r\n" + se.ToString());
                }
            }
        }

        private void btnCO2Solenoid_Click(object sender, EventArgs e)
        {
            bool IOStatus = false;
            if (btnCO2Solenoid.Text == ArduinoFormat.buttonStop)
            {
                IOStatus = false;
            }
            else if (btnCO2Solenoid.Text == ArduinoFormat.buttonStart)
            {
                IOStatus = true;
            }
            btnCO2Solenoid.Text = IOStatus ? ArduinoFormat.buttonStop : ArduinoFormat.buttonStart;

            if (serialPort1.IsOpen) // If use use serial
            {
                try
                {
                    serialPort1.Write(IOStatus ? ArduinoFormat.CO2SolenoidON : ArduinoFormat.CO2SolenoidOFF);
                    Debug.WriteLine(IOStatus ? ArduinoFormat.CO2SolenoidON : ArduinoFormat.CO2SolenoidOFF);
                    return;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }
            else // If user use Ethernet
            {
                try
                {
                    if (client == null)
                        client = new Client(this);
                    string _dataSend = IOStatus ? ArduinoFormat.CO2SolenoidON : ArduinoFormat.CO2SolenoidOFF;
                    client.Send(Encoding.GetEncoding(Constant.EncodingFormat).GetBytes(_dataSend + '\n'));
                    Debug.WriteLine(IOStatus ? ArduinoFormat.CO2SolenoidON : ArduinoFormat.CO2SolenoidOFF);
                }
                catch (SocketException se)
                {
                    MessageBox.Show("Unable to Connect.\r\n" + se.ToString());
                }
            }
        }

        private void btnLEDGrow_Click(object sender, EventArgs e)
        {
            bool IOStatus = false;
            if (btnLEDGrow.Text == ArduinoFormat.buttonStop)
            {
                IOStatus = false;
            }
            else if (btnLEDGrow.Text == ArduinoFormat.buttonStart)
            {
                IOStatus = true;
            }
            btnLEDGrow.Text = IOStatus ? ArduinoFormat.buttonStop : ArduinoFormat.buttonStart;

            if (serialPort1.IsOpen) // If use use serial
            {
                try
                {
                    serialPort1.Write(IOStatus ? ArduinoFormat.LEDGrowON : ArduinoFormat.LEDGrowOFF);
                    Debug.WriteLine(IOStatus ? ArduinoFormat.LEDGrowON : ArduinoFormat.LEDGrowOFF);
                    return;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }
            else // If user use Ethernet
            {
                try
                {
                    if (client == null)
                        client = new Client(this);
                    string _dataSend = IOStatus ? ArduinoFormat.LEDGrowON : ArduinoFormat.LEDGrowOFF;
                    client.Send(Encoding.GetEncoding(Constant.EncodingFormat).GetBytes(_dataSend + '\n'));
                    Debug.WriteLine(IOStatus ? ArduinoFormat.LEDGrowON : ArduinoFormat.LEDGrowOFF);
                }
                catch (SocketException se)
                {
                    MessageBox.Show("Unable to Connect.\r\n" + se.ToString());
                }
            }
        }

        private void btnRelay1n_Click(object sender, EventArgs e)
        {
            bool IOStatus = false;
            if (btnRelay1n.Text == ArduinoFormat.buttonStop)
            {
                IOStatus = false;
            }
            else if (btnRelay1n.Text == ArduinoFormat.buttonStart)
            {
                IOStatus = true;
            }
            btnRelay1n.Text = IOStatus ? ArduinoFormat.buttonStop : ArduinoFormat.buttonStart;

            if (serialPort1.IsOpen) // If use use serial
            {
                try
                {
                    serialPort1.Write(IOStatus ? ArduinoFormat.Relay1nON : ArduinoFormat.Relay1nOFF);
                    Debug.WriteLine(IOStatus ? ArduinoFormat.Relay1nON : ArduinoFormat.Relay1nOFF);
                    return;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }
            else // If user use Ethernet
            {
                try
                {
                    if (client == null)
                        client = new Client(this);
                    string _dataSend = IOStatus ? ArduinoFormat.Relay1nON : ArduinoFormat.Relay1nOFF;
                    client.Send(Encoding.GetEncoding(Constant.EncodingFormat).GetBytes(_dataSend + '\n'));
                    Debug.WriteLine(IOStatus ? ArduinoFormat.Relay1nON : ArduinoFormat.Relay1nOFF);
                }
                catch (SocketException se)
                {
                    MessageBox.Show("Unable to Connect.\r\n" + se.ToString());
                }
            }
        }

        private void btnRelay2n_Click(object sender, EventArgs e)
        {
            bool IOStatus = false;
            if (btnRelay2n.Text == ArduinoFormat.buttonStop)
            {
                IOStatus = false;
            }
            else if (btnRelay2n.Text == ArduinoFormat.buttonStart)
            {
                IOStatus = true;
            }
            btnRelay2n.Text = IOStatus ? ArduinoFormat.buttonStop : ArduinoFormat.buttonStart;
            if (serialPort1.IsOpen) // If use use serial
            {
                try
                {
                    serialPort1.Write(IOStatus ? ArduinoFormat.Relay2nON : ArduinoFormat.Relay2nOFF);
                    Debug.WriteLine(IOStatus ? ArduinoFormat.Relay2nON : ArduinoFormat.Relay2nOFF);
                    return;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }
            else // If user use Ethernet
            {
                try
                {
                    if (client == null)
                        client = new Client(this);
                    string _dataSend = IOStatus ? ArduinoFormat.Relay2nON : ArduinoFormat.Relay2nOFF;
                    client.Send(Encoding.GetEncoding(Constant.EncodingFormat).GetBytes(_dataSend + '\n'));
                    Debug.WriteLine(IOStatus ? ArduinoFormat.Relay2nON : ArduinoFormat.Relay2nOFF);
                }
                catch (SocketException se)
                {
                    MessageBox.Show("Unable to Connect.\r\n" + se.ToString());
                }
            }
        }

        private void btnRelay3n_Click(object sender, EventArgs e)
        {
            bool IOStatus = false;
            if (btnRelay3n.Text == ArduinoFormat.buttonStop)
            {
                IOStatus = false;
            }
            else if (btnRelay3n.Text == ArduinoFormat.buttonStart)
            {
                IOStatus = true;
            }
            btnRelay3n.Text = IOStatus ? ArduinoFormat.buttonStop : ArduinoFormat.buttonStart;

            if (serialPort1.IsOpen) // If use use serial
            {
                try
                {
                    serialPort1.Write(IOStatus ? ArduinoFormat.Relay3nON : ArduinoFormat.Relay3nOFF);
                    Debug.WriteLine(IOStatus ? ArduinoFormat.Relay3nON : ArduinoFormat.Relay3nOFF);
                    return;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }
            else // If user use Ethernet
            {
                try
                {
                    if (client == null)
                        client = new Client(this);
                    string _dataSend = IOStatus ? ArduinoFormat.Relay3nON : ArduinoFormat.Relay3nOFF;
                    client.Send(Encoding.GetEncoding(Constant.EncodingFormat).GetBytes(_dataSend + '\n'));
                    Debug.WriteLine(IOStatus ? ArduinoFormat.Relay3nON : ArduinoFormat.Relay3nOFF);
                }
                catch (SocketException se)
                {
                    MessageBox.Show("Unable to Connect.\r\n" + se.ToString());
                }
            }
        }

        //Check IP and Port then allow PC app connect to Arduino
        private void btnConnectEthernet_Click(object sender, EventArgs e)
        {
            string ipAddr = tBoxIPAddr.Text;
            string port = tBoxPort.Text;

            if(btnConnectEthernet.Text == ArduinoFormat.Connect)
            {
                if (IsValidIPAddress(ipAddr) == true)
                {
                    try
                    {
                        if (client == null)
                            client = new Client(this);

                        client.Connect(ipAddr, port);
                        //  client.Send(Encoding.GetEncoding(Constant.EncodingFormat).GetBytes("c"+'\n'));
                        btnConnectEthernet.Text = ArduinoFormat.Disconnect;
                        UpdateButtonEnableStatus(true);
                        btnSerialPort.Enabled = false;
                        return;
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

            if(btnConnectEthernet.Text == ArduinoFormat.Disconnect)
            {
                client.Disconnect();
                btnConnectEthernet.Text = ArduinoFormat.Connect;
                UpdateButtonEnableStatus(false);
                btnSerialPort.Enabled = true;
            }
            
        }
        // Check IP Address is valid or not
        private bool IsValidIPAddress(string ipaddr)
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

        //Enable or Disable button base on connectivity
        void UpdateButtonEnableStatus(bool connectStatus)
        {
                btnCO2Solenoid.Enabled = connectStatus;
                btnCooler.Enabled = connectStatus;
                btnLEDGrow.Enabled = connectStatus;
                btnRelay1n.Enabled = connectStatus;
                btnOK.Enabled = connectStatus;
                btnRelay2n.Enabled = connectStatus;
                btnRelay3n.Enabled = connectStatus;
                btnUVLight.Enabled = connectStatus;
                btnVaccumPump.Enabled = connectStatus;
                btnMainPumpStartStop.Enabled = connectStatus;
                return;
        }

        private void tBoxNutriAStepper_KeyNumber_Only(object sender, KeyPressEventArgs e)
        {
            if(!Char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void tBoxMainPumpSpeed_KeyNumberOnly(object sender, KeyPressEventArgs e)
        {

            if (!Char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }
    }
}
