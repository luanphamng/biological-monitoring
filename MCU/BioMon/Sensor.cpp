#include "Sensor.h"

void Sensor::setup()
{
    // ========== Digital pin ==========
    // Interrupt
    pinMode(SENSOR_MAINFLOW_PIN, INPUT_PULLUP);
    pinMode(SENSOR_GASFLOW_PIN, INPUT_PULLUP);
    pinMode(SENSOR_PIPEFLOW1_PIN, INPUT_PULLUP);
    pinMode(SENSOR_PIPEFLOW2_PIN, INPUT_PULLUP);
    pinMode(SENSOR_PIPEFLOW3_PIN, INPUT_PULLUP);
    // Non-Interrupt
    pinMode(SENSOR_LEVELNUTRIA_PIN, INPUT);
    pinMode(SENSOR_LEVELSTORAGE25_PIN, INPUT);
    pinMode(SENSOR_LEVELSTERILE_PIN, INPUT);
    // Analog pin dont need to setup
}

void Sensor::readGlobalISR(int MainFlow, int gasFlow, int pipe1, int pipe2, int pipe3)
{
    MainFlow_Value = MainFlow;
    GasFlow_Value = gasFlow;
    PipeFlow1_Value = pipe1;
    PipeFlow2_Value = pipe2;
    PipeFlow3_Value = pipe3;
}
void Sensor::read()
{
    #ifdef USE_B3950
    TempStorage25_Value = getB3950Temp(SENSOR_TEMPSTORAGE25_PIN);
    TempDualPipe1_Value = getB3950Temp(SENSOR_TEMPDUALPIPE1_PIN);
    TempDualPipe2_Value = getB3950Temp(SENSOR_TEMPDUALPIPE2_PIN);
    TempBeforeCooler_Value = getB3950Temp(SENSOR_TEMPBEFORECOOLER_PIN);
    #endif

    Turbidity_Value = analogRead(SENSOR_TURBIDITY_PIN);
    Conduct_Value = analogRead(SENSOR_CONDUCT_PIN);
    Light_Value = analogRead(SENSOR_LIGHT_INTENSITY);
    Temp5_Value = analogRead(SENSOR_TEMP5_PIN);
    Temp6_Value = analogRead(SENSOR_TEMP6_PIN);
    
    LevelNutriA_Status = digitalRead(SENSOR_LEVELNUTRIA_PIN);
    LevelStorage25_Status = digitalRead(SENSOR_LEVELSTORAGE25_PIN);
    LevelSterile_Status = digitalRead(SENSOR_LEVELSTERILE_PIN);

    StrCombineData =    String(TempStorage25_Value) + "," +     // 0
                        String(TempDualPipe1_Value) + "," +     // 1
                        String(TempDualPipe2_Value) + "," +     // 2
                        String(TempBeforeCooler_Value) + "," +  // 3                      
                        String(Turbidity_Value) + "," +         // 4
                        String(Conduct_Value) + "," +           // 5
                        String(Light_Value) + "," +             // 6
                        String(MainFlow_Value) + "," +          // 7
                        String(GasFlow_Value) + "," +           // 8
                        String(PipeFlow1_Value) + "," +         // 9
                        String(PipeFlow2_Value) + "," +         // 10
                        String(PipeFlow3_Value) + "," +         // 11
                        String(LevelNutriA_Status) + "," +      // 12
                        String(LevelStorage25_Status) + "," +   // 13
                        String(LevelSterile_Status);            // 14
}

#ifdef USE_DS18B20          
    float Sensor::getDS18B20Temp(DeviceAddress deviceAddress)
    {                
        return sensors.getTempC(deviceAddress);                
    }
#endif

#ifdef USE_B3950
    float Sensor::getB3950Temp(int channel)
    {
        float vo, Rth, tempC, logRth;
        vo = analogRead(channel);
        Rth = (float)(R_ADD * vo) / (1023.0 - vo);
        logRth = log(Rth);
        tempC = (1.0 / (C1 + C2*logRth + C3*logRth*logRth*logRth));
        tempC = tempC - 273.15;
        // TFer = (TFer * 9.0)/ 5.0 + 32.0; 
        // C = (TFer - 32) / 1.8;
        return tempC;
    }
#endif