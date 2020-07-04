#ifndef SENSOR_H
#define SENSOR_H

// #define USE_DS18B20
#define USE_B3950
#include "Arduino.h"
// For DS18B20 Temperature sensor (3 wires)
#include "OneWire.h"
#include "DallasTemperature.h"
#define ONE_WIRE_PIN 55
// For B3950 Temperature sensor (2 wires)
#define R_ADD 10000.0
// float c1 = 1.009249522e-03, c2 = 2.378405444e-04, c3 = 2.019202697e-07;
#define C1 1.009249522e-03
#define C2 2.378405444e-04
#define C3 2.019202697e-07
// ============================== ANALOG SENSORs ======================
// 6 sensor temperature
#define SENSOR_TEMPSTORAGE25_PIN A0
#define SENSOR_TEMPDUALPIPE1_PIN A1
#define SENSOR_TEMPDUALPIPE2_PIN A2
#define SENSOR_TEMPBEFORECOOLER_PIN A3
#define SENSOR_TEMP5_PIN A4
#define SENSOR_TEMP6_PIN A5
// others
#define SENSOR_CONDUCT_PIN A8
#define SENSOR_TURBIDITY_PIN A9
#define SENSOR_LIGHT_INTENSITY A10
// ============================== DIGITAL SENSORs =====================
// Set all flow meter into there interrupt pins 2, 3, 18, 19, 20, 21
#define SENSOR_MAINFLOW_PIN 2
#define SENSOR_GASFLOW_PIN 3
#define SENSOR_PIPEFLOW1_PIN 18
#define SENSOR_PIPEFLOW2_PIN 19
#define SENSOR_PIPEFLOW3_PIN 20

#define SENSOR_LEVELNUTRIA_PIN 14
#define SENSOR_LEVELSTORAGE25_PIN 15
#define SENSOR_LEVELSTERILE_PIN 16

// ====================================================================

class Sensor
{
    public:
        // Value from sensor check water level
        unsigned char LevelNutriA_Status = 0;
        unsigned char LevelStorage25_Status = 0;
        unsigned char LevelSterile_Status = 0;
        // Value for store analog sensor data
        float TempStorage25_Value = 0;
        float TempDualPipe1_Value = 0;
        float TempDualPipe2_Value = 0;
        float TempBeforeCooler_Value = 0;
        float Temp5_Value = 0;
        float Temp6_Value = 0;
        int Conduct_Value = 0;
        int Turbidity_Value = 0;
        int Light_Value = 0;
        // This read from Global Inturrupt, updated by function readGlobalISR
        int MainFlow_Value = 0;
        int GasFlow_Value = 0;
        int PipeFlow1_Value = 0;
        int PipeFlow2_Value = 0;
        int PipeFlow3_Value = 0;
        // Conbined string will send to PC
        String StrCombineData;
        void setup();
        void readGlobalISR(int MainFlow, int gasFlow, int pipe1, int pipe2, int pipe3);
        void read();

        #ifdef USE_DS18B20
            OneWire oneWire(ONE_WIRE_PIN);
            DallasTemperature sensors(&oneWire);
            // arrays to hold device address
            DeviceAddress insideThermometer;
            float getDS18B20Temp(DeviceAddress deviceAddress);
        #endif

        #ifdef USE_B3950
            float getB3950Temp(int channel);
        #endif

};
#endif