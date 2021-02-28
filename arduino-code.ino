void setup() {
  // put your setup code here, to run once:
    Serial.begin(9600); 
}
 
void loop() {
  // put your main code here, to run repeatedly:
 
  // It checks if faze 1 has any voltage and then waits for it to drop and
  // then it checks if the faze after(2) or the one before(3) has any voltage
  // to determine if the disc is spinning foward or backward.
    if(analogRead(0)>2){
      while(analogRead(0)>0){
        // waits for the voltage to drop
        delay(1);
      }
      if(analogRead(2)>2){
        Serial.print("1");
        Serial.println(analogRead(2));
      }
      if(analogRead(1)>2){
        Serial.print("0");
        Serial.println(analogRead(1));
      }
    }
  // The output shows 1(foward) or 0(backward) and the voltage 
}