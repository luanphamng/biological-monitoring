/*
 * Update value for all outputs
 */
#include "Output.h"
#include "Comm.h"

void Output::setup()
{
    // General IO
    pinMode(VACCUMPUMP_OUTPUT, OUTPUT);
    pinMode(UVLIGHT_OUTPUT, OUTPUT);
    pinMode(COOLER_OUTPUT, OUTPUT);
    pinMode(CO2SOLENOID_OUTPUT, OUTPUT);
    pinMode(LEDGROW_OUTPUT, OUTPUT);
    pinMode(RELAY1N_OUTPUT, OUTPUT);
    pinMode(RELAY2N_OUTPUT, OUTPUT);
    pinMode(RELAY3N_OUTPUT, OUTPUT);
    // Anaglog out
    pinMode(STEPPER_PWM, OUTPUT);
    // pinMode(MAINPUMP_PWM, OUTPUT);
}

void Output::updateOutputValue(Comm Output_comm)
{
    // Output write
    digitalWrite(VACCUMPUMP_OUTPUT, Output_comm.VaccumPump);
    digitalWrite(UVLIGHT_OUTPUT, Output_comm.UVLight);
    digitalWrite(COOLER_OUTPUT, Output_comm.Cooler);
    digitalWrite(CO2SOLENOID_OUTPUT, Output_comm.CO2Solenoid);
    digitalWrite(LEDGROW_OUTPUT, Output_comm.LEDGrow);
    digitalWrite(RELAY1N_OUTPUT, Output_comm.Relay1n);
    digitalWrite(RELAY2N_OUTPUT, Output_comm.Relay2n);
    digitalWrite(RELAY3N_OUTPUT, Output_comm.Relay3n);
    // PWM write
    analogWrite(STEPPER_PWM, Output_comm.stepperSpeed);
    //Main Pump
    analogWrite(MAINPUMP_PWM, Output_comm.mainPumpSpeed);
}