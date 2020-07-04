/*
    Support function communication with PC
 */
#ifndef COMM_H
#define COMM_H
#include "Arduino.h"
#include <Ethernet.h>

#define COMM_BAU_RATE 115200
#define CO2SOLENOID_OFF "CO2Solenoid:OFF"
#define CO2SOLENOID_ON "CO2Solenoid:ON"
#define COOLER_OFF "Cooler:OFF"
#define COOLER_ON "Cooler:ON"
#define LEGROW_OFF "LEDGrow:OFF"
#define LEGROW_ON "LEDGrow:ON"
#define RELAY1N_OFF "Relay1n:OFF"
#define RELAY1N_ON "Relay1n:ON"
#define RELAY2N_OFF "Relay2n:OFF"
#define RELAY2N_ON "Relay2n:ON"
#define RELAY3N_OFF "Relay3n:OFF"
#define RELAY3N_ON "Relay3n:ON"
#define UVLIGHT_OFF "UVLight:OFF"
#define UVLIGHT_ON "UVLight:ON"
#define VACCUUMPUMP_OFF "VacuumPump:OFF"
#define VACCUUMPUMP_ON "VacuumPump:ON"
#define REQUEST_SENSOR "requestSensor"
#define LOGOUT "*"

class Comm
{
    public:
        bool VaccumPump;
        bool UVLight;
        bool Cooler;
        bool CO2Solenoid;
        bool LEDGrow;
        bool Relay1n;
        bool Relay2n;
        bool Relay3n;
        bool RequestSensor;
        bool IsChangeSpeed;
        unsigned int stepperSpeed;
        unsigned int mainPumpSpeed;  
        String strEthernetReceived;;    

        // ==================== Serial function =========================
        // setup baurate, bitdata = 8, stop bit 1, none parity
        void serialSetup();
        // Send data to computer by Serial 9600
        void sendData(String data);
        // Split data
        String getValue(String data, char separator, int index);
        // Recieve data from computer Serial 9600
        String recieveData();
        // validate speed
        unsigned int validateSpeed(unsigned int _speed, unsigned int _limit);
        // ==================== Ethernet function =======================
        void waitIncommingConnection();
};
#endif