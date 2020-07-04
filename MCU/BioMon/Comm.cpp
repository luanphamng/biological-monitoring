#include "Comm.h"

// setup baurate, bitdata = 8, stop bit 1, none parity
void Comm::serialSetup()
{
    Serial.begin(COMM_BAU_RATE);
}

void Comm::sendData(String data)
{
    Serial.println("<" + data + ">");
}

String Comm::getValue(String data, char separator, int index)
{
    int found = 0;
    int strIndex[] = { 0, -1 };
    int maxIndex = data.length() - 1;

    for (int i = 0; i <= maxIndex && found <= index; i++) {
        if (data.charAt(i) == separator || i == maxIndex) {
            found++;
            strIndex[0] = strIndex[1] + 1;
            strIndex[1] = (i == maxIndex) ? i+1 : i;
            Serial.println(i);
            break;
        }
    }    
    return (index == 0)?data.substring(strIndex[0], strIndex[1]) : data.substring(strIndex[1] + 1, maxIndex + 1);
}

String Comm::recieveData()
{
    unsigned int _stepperSpeed = 0;
    unsigned int _mainPumpSpeed = 0;
    String _data = Serial.readString();
    if(_data == CO2SOLENOID_OFF)
        CO2Solenoid = false;
    else if(_data == CO2SOLENOID_ON)
        CO2Solenoid = true;
    else if(_data == COOLER_OFF)
        Cooler = false;
    else if(_data == COOLER_ON)
        Cooler = true;
    else if(_data == LEGROW_OFF)
        LEDGrow = false;
    else if(_data == LEGROW_ON)
        LEDGrow = true;
    else if(_data == RELAY1N_OFF)
        Relay1n = false;
    else if(_data == RELAY1N_ON)
        Relay1n = true;
    else if(_data == RELAY2N_OFF)
        Relay2n = false;
    else if(_data == RELAY2N_ON)
        Relay2n = true;
    else if(_data == RELAY3N_OFF)
        Relay3n = false;
    else if(_data == RELAY3N_ON)
        Relay3n = true;
    else if(_data == UVLIGHT_OFF)
        UVLight = false;
    else if(_data == UVLIGHT_ON)
        UVLight = true;
    else if(_data == VACCUUMPUMP_OFF)
        VaccumPump = false;
    else if(_data == VACCUUMPUMP_ON)
        VaccumPump = true;
    else //Example stepperSpeed,mainPumpSpeed = "50,80"
    {
        _stepperSpeed =  getValue(_data,',', 0).toInt();
        _mainPumpSpeed = getValue(_data,',', 1).toInt();
        if(stepperSpeed != _stepperSpeed || mainPumpSpeed != _mainPumpSpeed)
        {
            IsChangeSpeed = true;
            stepperSpeed = validateSpeed(_stepperSpeed, (unsigned int)255);
            mainPumpSpeed = validateSpeed(_mainPumpSpeed, (unsigned int)255);
        }
        else
        {
            IsChangeSpeed = false;
        }
    }
    //Serial.println(_data);
    return Serial.readString();
}

// check incomming data from PC and set status for output or motor
void Comm::waitIncommingConnection()
{ 
    unsigned int _stepperSpeed = 0;
    unsigned int _mainPumpSpeed = 0;
    Serial.println(strEthernetReceived);         
    if(strEthernetReceived == CO2SOLENOID_OFF)
        CO2Solenoid = false;
    else if(strEthernetReceived == CO2SOLENOID_ON)
        CO2Solenoid = true;
    else if(strEthernetReceived == COOLER_OFF)
        Cooler = false;
    else if(strEthernetReceived == COOLER_ON)
        Cooler = true;
    else if(strEthernetReceived == LEGROW_OFF)
        LEDGrow = false;
    else if(strEthernetReceived == LEGROW_ON)
        LEDGrow = true;
    else if(strEthernetReceived == RELAY1N_OFF)
        Relay1n = false;
    else if(strEthernetReceived == RELAY1N_ON)
        Relay1n = true;
    else if(strEthernetReceived == RELAY2N_OFF)
        Relay2n = false;
    else if(strEthernetReceived == RELAY2N_ON)
        Relay2n = true;
    else if(strEthernetReceived == RELAY3N_OFF)
        Relay3n = false;
    else if(strEthernetReceived == RELAY3N_ON)
        Relay3n = true;
    else if(strEthernetReceived == UVLIGHT_OFF)
        UVLight = false;
    else if(strEthernetReceived == UVLIGHT_ON)
        UVLight = true;
    else if(strEthernetReceived == VACCUUMPUMP_OFF)
    {
        VaccumPump = false;
        Serial.println("Pump OFF");  
    }
    else if(strEthernetReceived == VACCUUMPUMP_ON)
    {
        VaccumPump = true;
        Serial.println("Pump ON");  
    }
    else if(strEthernetReceived == REQUEST_SENSOR)
    {
        RequestSensor = true;
        Serial.println("Request sensor true!");                                    
    }
    else if(strEthernetReceived == LOGOUT)
    {
        //client.print('x');
        //client.stop();
    }
    else //Example stepperSpeed,mainPumpSpeed = "50,80"
    {
        _stepperSpeed =  getValue(strEthernetReceived,',', 0).toInt();
        _mainPumpSpeed = getValue(strEthernetReceived,',', 1).toInt();
        if(stepperSpeed != _stepperSpeed || mainPumpSpeed != _mainPumpSpeed)
        {
            IsChangeSpeed = true;
            stepperSpeed = validateSpeed(_stepperSpeed, (unsigned int)255);
            mainPumpSpeed = validateSpeed(_mainPumpSpeed, (unsigned int)255);
        }
        else
        {
            IsChangeSpeed = false;
        }
    }
    strEthernetReceived = ""; // Clear recieve buffer for using next time   
}

// Check speed overrange
unsigned int Comm::validateSpeed(unsigned int _speed, unsigned int _limit)
{
    if(_speed > _limit)
        return _limit;
    else
        return _speed;    
}