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


namespace BIO_MONITORING
{

    public partial class BioMonitoring : Form
    {
        //Globa var
        string[] G_DataSerial;
        public BioMonitoring()
        {
            InitializeComponent();
            pictureBoxMainPump.Image = new Bitmap(Application.StartupPath + "\\Resources\\pumpRunGreen.png");
            buttonMainPumpStartStop.Text = "Stop";

            
            //Read data calib from file
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
                Debug.WriteLine(lines.ElementAt(0));
                cBoxComPorts.Text = lines.ElementAt(0);
            }
        }

        private void buttonMainPumpStartStop_Click(object sender, EventArgs e)
        {
            
            if (buttonMainPumpStartStop.Text == "Stop")
            {
                buttonMainPumpStartStop.Text = "Start";
                pictureBoxMainPump.Image = new Bitmap(Application.StartupPath + "\\Resources\\pumpStop.png");
                return;
            }

            if (buttonMainPumpStartStop.Text == "Start")
            {
                buttonMainPumpStartStop.Text = "Stop";
                pictureBoxMainPump.Image = new Bitmap(Application.StartupPath + "\\Resources\\pumpRunGreen.png");
                return;
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            string[] ports = SerialPort.GetPortNames();
            cBoxComPorts.Items.AddRange(ports);
        }

        private void btnSerialPort_Click(object sender, EventArgs e)
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
                    btnSerialPort.Text = "Disconnect";
                    return;
    
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }

            if (serialPort1.IsOpen)
            {
                serialPort1.Close();
                btnSerialPort.Text = "Connect";
                return;
            }
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            if(serialPort1.IsOpen)
            {
                serialPort1.Close();
            }
            // Create a string array with the lines of text
            string[] lines = {                
                cBoxComPorts.Text,
                nbrCalibNutriALevel.Value.ToString(),
                nbrCalibNutriAStepper.Value.ToString(),
                nbrCalibStorage25.Value.ToString(),
                nbrCalibStorage25Level.Value.ToString(),
                nbrCalibStorage25PH.Value.ToString(),
                nbrCalibStorage25PPM.Value.ToString(),
                nbrCalibSterileLevel.Value.ToString()

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
            string _tempDataRecieve = null;
            if (serialPort1.IsOpen)
            {
                _tempDataRecieve =  serialPort1.ReadExisting();

            }            
            if (_tempDataRecieve != null)
            {
                Debug.WriteLine(_tempDataRecieve);
                char[] separator = { ',' };

                _tempDataRecieve = _tempDataRecieve.Trim();
                G_DataSerial = _tempDataRecieve.Split(separator);
            }
        }

        //Data Processing

        //Display all processed data on the app.
        public void Display(string[] _input)
        {
            int x = (int)SensorEnum.SensorPosition.DUAL_PIPE_TEMP;
        }

        private void serialPort1_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            string _tempDataRecieve = null;
            if (serialPort1.IsOpen)
            {
                _tempDataRecieve = serialPort1.ReadExisting();

            }            
            if (_tempDataRecieve != null)
            {
                Debug.WriteLine(_tempDataRecieve);
                char[] separator = { ',' };

                _tempDataRecieve = _tempDataRecieve.Trim();
                G_DataSerial = _tempDataRecieve.Split(separator);
            }
            DataProcessing ds = new DataProcessing();
            ds.MainProcessing(G_DataSerial);
            Debug.WriteLine(ds.MainPumpSpeed + " " + ds.StepperSpeed);

        }
    }
}
