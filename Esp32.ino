#include <WiFi.h>
#include <ArduinoWebsockets.h>
#include <ArduinoJson.h>

const char* ssid = "*********";  //Enter SSID
const char* password = "*********";  //Enter Password

using namespace websockets;
WebsocketsClient client;

void handleReceivedMessage(String message) {
  message.replace("From Server: ", "");
  StaticJsonDocument<200> doc;
  DeserializationError err = deserializeJson(doc, message);


  int caseType = doc["MessageType"].as<int>();
  switch (caseType) {
    case 5:  // Login
      Serial.print(message);
      break;
    case 10:  // Register
      Serial.print(message);
      break;
    case 15:  // Log add
      Serial.println("log add...");
      break;
  }
}

void onMessageCallback(WebsocketsMessage message) {
  handleReceivedMessage(message.data());
}

void onEventsCallback(WebsocketsEvent event, String data) {
  if (event == WebsocketsEvent::ConnectionOpened) {
    Serial.println("Connnection Opened");
  } else if (event == WebsocketsEvent::ConnectionClosed) {
    Serial.println("Connnection Closed");
  } else if (event == WebsocketsEvent::GotPing) {
    Serial.println("Got a Ping!");
  } else if (event == WebsocketsEvent::GotPong) {
    Serial.println("Got a Pong!");
  }
}

void setup() {
  Serial.begin(9600);
  client.onMessage(onMessageCallback);
  client.onEvent(onEventsCallback);
}

void loop() {
  if (WiFi.status() != WL_CONNECTED) {
    if (true) {
      WiFi.begin(ssid, password);
      delay(10000);

      // Serial.println("WiFi Connection...");
      for (int i = 0; i < 10 && WiFi.status() != WL_CONNECTED; i++) {
        Serial.print(".");
        delay(1000);
      }
      if (WiFi.status() != WL_CONNECTED) {
        Serial.println("WiFi Hatali Giris!");
      } else {
        // Serial.println("Client Connection...");
        client.connect("socket_address"); //  in connection, websocket
      }
    }
  } else {
    client.poll();
  }

  sendDataToWS();
}

String bufferStrJson = "";
void sendDataToWS() {
  if (Serial.available()) {

    String str = String(char(Serial.read()));
    if (str == "#") {
      bufferStrJson = "{\"";
    } else if (str == "$") {
      bufferStrJson += "\"}";
      client.send(bufferStrJson);
    } else {
      bufferStrJson += str;
    }
  }
}