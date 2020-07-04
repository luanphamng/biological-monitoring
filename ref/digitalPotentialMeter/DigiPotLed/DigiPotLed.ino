/*
   DigiPot.pde - Example sketch for Arduino library for managing digital potentiometers X9C1xxx (xxx = 102,103,104,503).
   By Timo Fager, Jul 29, 2011.
   Released to public domain.

   For this example, connect your X9C103P (or the like) as follows:
   1 - INC - Arduino pin 4
   2 - U/D - Arduino pin 5
   3 - VH  - 5V
   4 - VSS - GND
   5 - VW  - Output: 150 Ohm resistor -> LED -> GND
   6 - VL  - GND
   7 - CS  - Arduino pin 6
   8 - VCC - 5V

   thank for original code writer , i'm edit it later (joeGTEC)
 **/


#include <DigiPotX9Cxxx.h>

DigiPot pot(50, 51, 52);
int rst = 2;
volatile int i = 0;
int wiperPos = 0;
long PotOhm = 100000 ; //x9c104

void setup() {
  pinMode(rst, INPUT_PULLUP);
  Serial.begin(9600);
  while (!Serial) {
    ; // wait for serial port to connect. Needed for native USB
  }

  attachInterrupt(digitalPinToInterrupt(rst), makereset, FALLING);
}

void makereset() {
  delayMicroseconds(50000);  // debounce time 50ms
  if (digitalRead(rst) == LOW) {
     Serial.println("reset");
    pot.reset();
    delayMicroseconds(50);
    i = pot.get();
    delayMicroseconds(50);
  }
}

void loop() {
  Serial.println("Starting");

  for ( i = 1; i < 100; i++) {

    Serial.print("Increasing, WiperPos = ");
    wiperPos = constrain(pot.get(), 0, 99);
    Serial.print(wiperPos);
    Serial.print(", Ohm = " + String(getOhm(wiperPos)));
    Serial.print(", Volt = "); Serial.print(getVolt(wiperPos), 2);
    Serial.println();
    delay(10);
    pot.increase(1);
    delay(150);
  }

  for ( i = 1; i < 100; i++) {
    Serial.print("Decreasing, WiperPos = ");
    wiperPos = constrain(pot.get(), 0, 99);
    Serial.print(wiperPos);
    Serial.print(", Ohm = " + String(getOhm(wiperPos)));
    Serial.print(", Volt = "); Serial.print(getVolt(wiperPos), 2);
    Serial.println();

    pot.decrease(1);
    delay(150);
  }

}


unsigned long getOhm(int wiperPosition) {
  unsigned long Ohm ;
  Ohm = (long) wiperPosition * (PotOhm / 99);
  return Ohm;
}

float getVolt(int wiperPosition) {
  float Vcc = 5.00;
  float Rwl = (float)getOhm(wiperPosition);
  float Vw = Vcc * (Rwl / (float)PotOhm);
  return Vw;
}
