# 1 "d:\\05_Projects\\Fiverr\\2019\\freelancercode2019\\July_scada_arduino\\MCU\\DustCounter\\DustCounter.ino"
# 1 "d:\\05_Projects\\Fiverr\\2019\\freelancercode2019\\July_scada_arduino\\MCU\\DustCounter\\DustCounter.ino"
# 2 "d:\\05_Projects\\Fiverr\\2019\\freelancercode2019\\July_scada_arduino\\MCU\\DustCounter\\DustCounter.ino" 2
# 3 "d:\\05_Projects\\Fiverr\\2019\\freelancercode2019\\July_scada_arduino\\MCU\\DustCounter\\DustCounter.ino" 2
# 4 "d:\\05_Projects\\Fiverr\\2019\\freelancercode2019\\July_scada_arduino\\MCU\\DustCounter\\DustCounter.ino" 2
LiquidCrystal_I2C lcd(0x27,20,4); // set the LCD address to 0x27 for a 16 chars and 2 line display
DustSensor dustsensor;
void setup()
{

  lcd.init(); // initialize the lcd   
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
