#include "Arduino.h"


#define SENSOR_OUT A0
#define ILED 7

#define        COV_RATIO                       0.2            //ug/mmm / mv
#define        NO_DUST_VOLTAGE                 400            //mv
#define        SYS_VOLTAGE                     5000   

class DustSensor
{
    public:
    float density;
        void setup();
        void read();
        int Filter(int m);
        float Density(int vanalogFiltered);
};