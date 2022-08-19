#include <ESP8266WiFi.h>
#include <ESP8266WebServer.h>
#include <EEPROM.h>
#include <WiFiUdp.h>
ESP8266WebServer    server(80);

WiFiUDP Udp;
unsigned int port = 12345;
char packet[255];

int isReady = 1;
#define motor1 D1
#define motor2 D2
#define motor3 D3
#define motor4 D4
#define motor5 D5
#define motor6 D6
#define motor7 D7
#define motor8 D8

struct settings {
  char ssid[30];
  char password[30];
} user_wifi = {};

IPAddress local_IP(192, 168, 1, 195);
IPAddress gateway(192, 168, 1, 1);
IPAddress subnet(255, 255, 255, 0);

String rcv = "";

void setup() {

    pinMode(motor1, OUTPUT);
    pinMode(motor2, OUTPUT);
    pinMode(motor3, OUTPUT);
    pinMode(motor4, OUTPUT);
    pinMode(motor5, OUTPUT);
    pinMode(motor6, OUTPUT);
    pinMode(motor7, OUTPUT);
    pinMode(motor8, OUTPUT);
    digitalWrite(motor1, LOW);
    digitalWrite(motor2, LOW);
    digitalWrite(motor3, LOW);
    digitalWrite(motor4, LOW);
    digitalWrite(motor5, LOW);
    digitalWrite(motor6, LOW);
    digitalWrite(motor7, LOW);
    digitalWrite(motor8, LOW);
  
  EEPROM.begin(sizeof(struct settings) );
  EEPROM.get( 0, user_wifi );
    Serial.begin(115200);
   if(!WiFi.config(local_IP,gateway,subnet)){
    Serial.println("WiFi config failed!");
    }
  WiFi.mode(WIFI_STA);
  WiFi.begin(user_wifi.ssid, user_wifi.password);

  delay(500);
    

  byte tries = 0;
  while (WiFi.status() != WL_CONNECTED) {
    delay(1000);
    if (tries++ > 30) {
      WiFi.mode(WIFI_AP);
      WiFi.softAP("Shudder Chest");
      isReady=0;
      break;
    }
  }

  if(isReady == 0){
  server.on("/",  handlePortal);
  server.begin();
  }else
  {
    
    Serial.println("Wifi Connected!");
    Serial.println("IP address: ");
    Serial.println(WiFi.localIP());
    Serial.println("Connection Successful");
    Udp.begin(port);
    Serial.printf("Listener started at IP %s, at port %d", WiFi.localIP().toString().c_str(), port);
    Serial.println();
    Serial.println("started server");
    }

}


void loop() {
  if(isReady){

    int packetSize = Udp.parsePacket();
  if (packetSize)
  {
    Serial.printf("Received %d bytes from %s, port %d", packetSize, Udp.remoteIP().toString().c_str(), Udp.remotePort());
    int len = Udp.read(packet, 255);
    if (len > 0)
    {
      packet[len] = 0;
    }
    Serial.printf("UDP packet contents: %s", packet);
    Serial.println();
  }
        if(strcmp(packet,"fulon") == 0){
          digitalWrite(motor1,HIGH);
        }
        else if(strcmp(packet,"fuloff") == 0){
          digitalWrite(motor1,LOW);
        }
        else if(strcmp(packet,"furon") == 0){
          digitalWrite(motor2,HIGH);
        }
        else if(strcmp(packet,"furoff") == 0){
          digitalWrite(motor2,LOW);
        }
        else if(strcmp(packet,"fllon") == 0){
          digitalWrite(motor3,HIGH);
        }
        else if(strcmp(packet,"flloff") == 0){
          digitalWrite(motor3,LOW);
        }
        else if(strcmp(packet,"flron") == 0){
          digitalWrite(motor4,HIGH);
        }
        else if(strcmp(packet,"flroff") == 0){
          digitalWrite(motor4,LOW);
        }
        else if(strcmp(packet,"bulon") == 0){
          digitalWrite(motor5,HIGH);
        }
        else if(strcmp(packet,"buloff") == 0){
          digitalWrite(motor5,LOW);
        }
        else if(strcmp(packet,"buron") == 0){
          digitalWrite(motor6,HIGH);
        }
        else if(strcmp(packet,"buroff") == 0){
          digitalWrite(motor6,LOW);
        }
        else if(strcmp(packet,"bllon") == 0){
          digitalWrite(motor7,HIGH);
        }
        else if(strcmp(packet,"blloff") == 0){
          digitalWrite(motor7,LOW);
        }
        else if(strcmp(packet,"blron") == 0){
          digitalWrite(motor8,HIGH);
        }
        else if(strcmp(packet,"blroff") == 0){
          digitalWrite(motor8,LOW);
        } 
    }else{
  server.handleClient();}
}

void handlePortal() {

  if (server.method() == HTTP_POST) {
    strncpy(user_wifi.ssid,     server.arg("ssid").c_str(),     sizeof(user_wifi.ssid) );
    strncpy(user_wifi.password, server.arg("password").c_str(), sizeof(user_wifi.password) );
    user_wifi.ssid[server.arg("ssid").length()] = user_wifi.password[server.arg("password").length()] = '\0';
    EEPROM.put(0, user_wifi);
    EEPROM.commit();
    server.send(200,   "text/html",  "<!doctype html><html lang='en'><head><meta charset='utf-8'><meta name='viewport' content='width=device-width, initial-scale=1'><title>Shudder Chest Setup</title><style>*,::after,::before{box-sizing:border-box;}body{margin:0;font-family:'Segoe UI',Roboto,'Helvetica Neue',Arial,'Noto Sans','Liberation Sans';font-size:1rem;font-weight:400;line-height:1.5;color:#212529;background-color:#f5f5f5;}.form-control{display:block;width:100%;height:calc(1.5em + .75rem + 2px);border:1px solid #ced4da;}button{border:1px solid transparent;color:#fff;background-color:#007bff;border-color:#007bff;padding:.5rem 1rem;font-size:1.25rem;line-height:1.5;border-radius:.3rem;width:100%}.form-signin{width:100%;max-width:400px;padding:15px;margin:auto;}h1,p{text-align: center}</style> </head> <body><main class='form-signin'> <h1>Shudder Chest Setup</h1> <br/> <p>Your settings have been saved successfully!<br />Please restart the device.</p></main></body></html>" );
  } else {
    server.send(200,   "text/html", "<!doctype html><html lang='en'><head><meta charset='utf-8'><meta name='viewport' content='width=device-width, initial-scale=1'><title>Shudder Chest Setup</title> <style>*,::after,::before{box-sizing:border-box;}body{margin:0;font-family:'Segoe UI',Roboto,'Helvetica Neue',Arial,'Noto Sans','Liberation Sans';font-size:1rem;font-weight:400;line-height:1.5;color:#212529;background-color:#f5f5f5;}.form-control{display:block;width:100%;height:calc(1.5em + .75rem + 2px);border:1px solid #ced4da;}button{cursor: pointer;border:1px solid transparent;color:#fff;background-color:#007bff;border-color:#007bff;padding:.5rem 1rem;font-size:1.25rem;line-height:1.5;border-radius:.3rem;width:100%}.form-signin{width:100%;max-width:400px;padding:15px;margin:auto;}h1{text-align: center}</style> </head> <body><main class='form-signin'> <form action='/' method='post'> <h1 class=''>Shudder Chest Setup</h1><br/><div class='form-floating'><label>SSID</label><input type='text' class='form-control' name='ssid'> </div><div class='form-floating'><br/><label>Password</label><input type='password' class='form-control' name='password'></div><br/><br/><button type='submit'>Save</button><p style='text-align: right'>VR Haptic Vest</p></form></main> </body></html>" );
  }
}
