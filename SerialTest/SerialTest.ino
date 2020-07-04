
// Global var send to app
int NutriA_iLevel;
int NutriA_iStepper;
int Storage25_iLevel;
float Storage25_fTemp;
float Storage25_fPH;
float Storage25_fPPM;
int Sterile_iLevel;
int MainPump_iSpeed;

// Global tranfer data for Serial
String G_sSerialTransfer;



void setup() {
  // put your setup code here, to run once:
  Serial.begin(9600);
}

void loop() {
  // put your main code here, to run repeatedly:

  NutriA_iLevel = random(0, 100);
  NutriA_iStepper = random(0, 100);
  Storage25_iLevel = random(0, 100);
  Storage25_fTemp = random(0, 100);
  Storage25_fPH = random(0, 100);
  Storage25_fPPM = random(0, 100);
  Sterile_iLevel = random(0, 100);
  MainPump_iSpeed = random(0, 100);

  G_sSerialTransfer = String(NutriA_iLevel) + "," 
                    + String(NutriA_iStepper) + ","
                    + String(Storage25_iLevel) + ","
                    + String(Storage25_fTemp) + ","
                    + String(Storage25_fPH) + ","                                        
                    + String(Storage25_fPPM) + ","
                    + String(Sterile_iLevel) + ","
                    + String(MainPump_iSpeed);
                                                                                                                       
  Serial.println(G_sSerialTransfer);
  delay(2000);
}
