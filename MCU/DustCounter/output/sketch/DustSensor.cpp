#include "DustSensor.h"

void DustSensor::setup()
{
    pinMode(ILED, OUTPUT);
}

int DustSensor::Filter(int m)
{
  static int flag_first = 0, _buff[10], sum;
  const int _buff_max = 10;
  int i;
  
  if(flag_first == 0)
  {
    flag_first = 1;

    for(i = 0, sum = 0; i < _buff_max; i++)
    {
      _buff[i] = m;
      sum += _buff[i];
    }
    return m;
    
  }
  else
  {
    sum -= _buff[0];
    for(i = 0; i < (_buff_max - 1); i++)
    {
      _buff[i] = _buff[i + 1];
    }
    _buff[9] = m;
    sum += _buff[9];
    
    i = sum / 10.0;
    return i;
  }
}

void DustSensor::read()
{
    int dustAnalog;
    digitalWrite(ILED, HIGH);
    delayMicroseconds(280);
    dustAnalog = analogRead(SENSOR_OUT);
    digitalWrite(ILED, LOW);
    dustAnalog = Filter(dustAnalog);
    density = Density(dustAnalog);
}


float DustSensor::Density(int vanalogFiltered)
{
    float _v;
    /*
    covert voltage (mv)
    */
    _v = (SYS_VOLTAGE / 1024.0) * vanalogFiltered * 11;
    
    /*
    voltage to density
    */
    if(_v >= NO_DUST_VOLTAGE)
    {
        _v -= NO_DUST_VOLTAGE;
        
        return(_v * COV_RATIO);
    }
    else
        return 0;
    return 0;
}