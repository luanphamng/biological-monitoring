// Add 3 custom libraries
#include "Sensor.h" // Sensor
#include "Comm.h"   // Communication
#include "Output.h" // Output relay, pwm
#include <LiquidCrystal_I2C.h>
#define USE_ETHERNET 1 // 1: Use ethernet, 0: use serial
// ========== All global variable here ==========
// Custom library
Sensor sensor;
Comm comm;
Output output;
// Ethernet connection
EthernetServer server(80);
// Display
LiquidCrystal_I2C lcd(0x27,16,2);
// Serial connection
unsigned long previousMillis = 0;     // will store last time LED was updated
const long interval = 1000;           // interval at which to blink (milliseconds)

// Global variable used for interrupt
int Calc;
String Glb_strDebug = "";
unsigned int Glb_MainFlow = 0;
unsigned int Glb_GasFlow = 0;
unsigned int Glb_PipeFlow1 = 0;
unsigned int Glb_PipeFlow2 = 0;
unsigned int Glb_PipeFlow3 = 0;

// ========== All setup code here ==========
void setup() 
{  
  sensor.setup();
  comm.serialSetup();
  output.setup();
  // Main pump Digital PotentialMeter pin setup

  lcd.init();                      // initialize the lcd 
  // Print a message to the LCD.
  lcd.backlight();
  lcd.setCursor(1,0);
  lcd.print("Running...");
  if(!USE_ETHERNET)
  {
    lcd.setCursor(0,1);
    lcd.print("Serial 115200");
  }
  // Ethernet server setup  
  if(USE_ETHERNET)
  {
    IPAddress ip(192,168,1,110);
    byte mac[] = {0xDE,0xAE,0xBE,0xEF,0xFE,0xED};
    Ethernet.begin(mac, ip);
    lcd.setCursor(0,1);
    lcd.print(Ethernet.localIP());
    server.begin();
  }

  // Global interrupt for flow sensor meters
  attachInterrupt(digitalPinToInterrupt(SENSOR_MAINFLOW_PIN), waterFlowISR, RISING);
  attachInterrupt(digitalPinToInterrupt(SENSOR_GASFLOW_PIN),gasFlowISR, RISING);
  attachInterrupt(digitalPinToInterrupt(SENSOR_PIPEFLOW1_PIN), pipeFlow1ISR, RISING);
  attachInterrupt(digitalPinToInterrupt(SENSOR_PIPEFLOW2_PIN), pipeFlow2ISR, RISING);
  attachInterrupt(digitalPinToInterrupt(SENSOR_PIPEFLOW3_PIN), pipeFlow3ISR, RISING);
}

// ========== All code will run repeatly ==========  
void loop() 
{
  unsigned long currentMillis = millis();
  if(!USE_ETHERNET)
  {
    if (currentMillis - previousMillis >= interval) 
    {
      // Read all sensor datas       
      sensor.read();
      sensor.readGlobalISR(Glb_MainFlow, Glb_GasFlow, Glb_PipeFlow1, Glb_PipeFlow2, Glb_PipeFlow3);    
      Glb_MainFlow = 0;
      Glb_GasFlow = 0;
      Glb_PipeFlow1 = 0;
      Glb_PipeFlow2 = 0;
      Glb_PipeFlow3 = 0;
      Glb_strDebug =  sensor.StrCombineData;
      // Send all to serial
      if(!USE_ETHERNET)
      {        
        comm.sendData(Glb_strDebug);
      }
      // save the last time
      previousMillis = currentMillis;
    }
  // ========== If serial has incomming data from PC ==========
    if(Serial.available() > 0)  
    {
      comm.recieveData();
      output.updateOutputValue(comm);
    }
  }
  else
  {
    // ========== If use Ethernet ==========
    // Create new session
    EthernetClient client = server.available(); 
    while (client.connected()) 
    { 
      currentMillis = millis();
      if ( client.available() > 0)
      { 
        char recieved = client.read();
        if(recieved != '\n')
        {
          comm.strEthernetReceived += recieved;                       
        }
        if (recieved == '\n')
          comm.waitIncommingConnection();
      }
      else
      {
        // Close socket already
      }
    // If client request sensor data
    if(comm.RequestSensor)
    {
      sensor.read();
      if(currentMillis - previousMillis >= interval)
      {    
      sensor.readGlobalISR(Glb_MainFlow, Glb_GasFlow, Glb_PipeFlow1, Glb_PipeFlow2, Glb_PipeFlow3);
      // Reset flow sensor each second
      Glb_MainFlow = 0;
      Glb_GasFlow = 0;
      Glb_PipeFlow1 = 0;
      Glb_PipeFlow2 = 0;
      Glb_PipeFlow3 = 0;
      //Reset currentMillis and continue run      
      previousMillis = currentMillis;
      }          
      Glb_strDebug =  sensor.StrCombineData;    
      client.println(Glb_strDebug);  
      comm.RequestSensor = false;
    }
    // Update value for output and motor    
    output.updateOutputValue(comm);  
    if(comm.IsChangeSpeed)
    {
      comm.IsChangeSpeed = false;
    }
    }
  }
}
// ========== Interrupts function ==========
void waterFlowISR()
{
  Glb_MainFlow++;
}

void gasFlowISR()
{
  Glb_GasFlow++;
}

void pipeFlow1ISR()
{
  Glb_PipeFlow1++;
}

void pipeFlow2ISR()
{
  Glb_PipeFlow2++;
}

void pipeFlow3ISR()
{
  Glb_PipeFlow3++;
}