
#include <ArduinoJson.h>

#include "MFRC522.h"
MFRC522 rfid(10, 9);

bool isRegister = false;
void setup() {
  Serial.begin(9600);
  delay(1000);

  SPI.begin();
  rfid.PCD_Init();
}

void loop() {
  if (rfid.PICC_IsNewCardPresent()) {
    sendRFID();
    delay(2000);
    readDataFromESP32();
  }
}

void sendRFID() {
  rfid.PICC_ReadCardSerial();
  MFRC522::PICC_Type piccType = rfid.PICC_GetType(rfid.uid.sak);

  if (!isRegister) {
    sendDataJsonToESP32("5", cardID());
  } else {
    sendDataJsonToESP32("10", cardID());
    isRegister = false;
  }

  rfid.PICC_HaltA();  // Kart ile iletişimi sonlandıralım
  rfid.PCD_StopCrypto1();
}

String cardID() {
  String text = "";
  for (int x = 0; x < rfid.uid.size; x++) {
    text += String(rfid.uid.uidByte[x], HEX);
    if (x < rfid.uid.size - 1) text += ":";
    text.toUpperCase();
  }
  
  StaticJsonDocument<200> doc;
  String jsonCardId;
  doc["cId"] = text;
  serializeJson(doc, jsonCardId);

  return jsonCardId;
}

String bufferStrJson = "";
void readDataFromESP32() {
  while (Serial.available()) {
    String str = String(char(Serial.read()));
    bufferStrJson += str;
  }
  parseReadData(bufferStrJson);
}

void parseReadData(String data) {
  StaticJsonDocument<200> doc;
  DeserializationError err = deserializeJson(doc, data);

  int caseType = doc["MessageType"].as<int>();
  switch (caseType) {    
    case 5:
      //Kapı açılacak
      break;
    case 10:
      //Yeni kayıt Register
      isRegister = true;
      break;
    case 15:
      //log add
      break;
  }
}

void sendDataJsonToESP32(String messageType, String data) {
  StaticJsonDocument<200> doc;
  String json;
  doc["#d"] = "[" + data + "]";
  doc["MessageType"] = messageType + "$";
  serializeJson(doc, json);
  json.replace("\\","");  
  Serial.println(json);
}