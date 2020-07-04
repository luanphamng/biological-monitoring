#include <Wire.h> 
#include <LiquidCrystal_I2C.h>
#include "DustSensor.h"
LiquidCrystal_I2C lcd(0x27,20,4);  // set the LCD address to 0x27 for a 16 chars and 2 line display
DustSensor dustsensor;
void setup()
{
  
  lcd.init();                      // initialize the lcd   
  Serial.begin(9600);
  // Print a message to the LCD.
  lcd.backlight();    
  lcd.setCursor(0,0);
  lcd.print("Dust Counter:");
}


void loop() {
  dustsensor.read();
  lcd.setCursor(0,1);
  lcd.print(dustsensor.density);
  Serial.print("The current dust concentration is: ");
  Serial.print(dustsensor.density);
  Serial.print(" ug/m3\n");  
  Serial.println("");
  delay(1000);
}