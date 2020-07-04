/*  
    all outputs configuration and update
*/
#ifndef _OUTPUT_H
#define _OUTPUT_H

#include "Arduino.h"
#include "Comm.h"

// Digital
#define VACCUMPUMP_OUTPUT 32
#define UVLIGHT_OUTPUT 33
#define COOLER_OUTPUT 34
#define CO2SOLENOID_OUTPUT 35
#define LEDGROW_OUTPUT 36
#define RELAY1N_OUTPUT 37
#define RELAY2N_OUTPUT 38
#define RELAY3N_OUTPUT 39
// Analog
#define STEPPER_PWM 8
#define MAINPUMP_PWM 9
class Output 
{
    public:
        // unsigned int mainPumpSpeed;                           
        // int pin1, pin2, pin3;
        void setup();
        void updateOutputValue(Comm Output_comm);        
        // digital potentimeter    
        
        


};

#endif