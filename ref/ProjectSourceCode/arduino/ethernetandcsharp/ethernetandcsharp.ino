//////////INCLUDE HEADER FILES //////////////////
#include <SPI.h>
#include <Ethernet.h>

EthernetServer server(80);
String content;
int ledPin = 7;
int  outputpin = 0;
void setup()
{
  IPAddress ip( 192, 168,1,110);
  byte mac[] = { 
    0xDE,0xAE,0xBE,0xEF,0xFE,0xED
  };

  Serial.begin(9600);
  while (!Serial) {
    ; // wait for serial port to connect. Needed for native USB port only
  }
  
  if (Ethernet.begin(mac) == 0) {
    Serial.println("Failed to configure Ethernet using DHCP");
    // Check for Ethernet hardware present
    if (Ethernet.hardwareStatus() == EthernetNoHardware) {
      Serial.println("Ethernet shield was not found.  Sorry, can't run without hardware. :(");
      while (true) {
        delay(1); // do nothing, no point running without Ethernet hardware
      }
    }
    if (Ethernet.linkStatus() == LinkOFF) {
      Serial.println("Ethernet cable is not connected.");
    }
    // try to congifure using IP address instead of DHCP:
    Ethernet.begin(mac, ip);
  } else {
    Serial.print("DHCP assigned IP ");
    Serial.println(Ethernet.localIP());
  }
  //server.begin();

  
  Serial.print("DHCP assigned IP:");
  Serial.println(Ethernet.localIP());
}
///////////////////////////////////////////////


void loop()
{
  waitIncommingConnection();   
}



void waitIncommingConnection()
{

  String pwd            = "";
  String inData         = "";
  EthernetClient client = server.available();
  
  if ( client  )
  {
    while ( client.connected() ) 
    {      
      if ( client.available() > 0)
      { 
        char recieved = client.read();
        inData += recieved;                       
        if (recieved == '\n')
        {                           
          switch( inData[0] )
          {                            
            case (char)'o' : 
              client.println("o");
              digitalWrite(ledPin, HIGH);
            break;

            case (char)'f' :
              client.println("f");
              digitalWrite(ledPin, LOW);
            break;
  
             case (char)'c' :
                 client.println(getTemperature());
            break;
            
            case (char)'*' :
              Logout(client);  
            break;
          
            default:
               client.println('d');
            break;
          }
          inData = ""; 
        }

      }
    }
  }
  else
  {
   client.println('v');
  }
}

float getTemperature()
{
  int rawvoltage= analogRead(outputpin);
  float millivolts= (rawvoltage/1024.0) * 4800;
  float kelvin= (millivolts/10);
  
  
  float celsius= kelvin - 273.15;

  return celsius;


}

void Logout(EthernetClient client )
{ 
  client.print('x');
  client.stop(); 

}
