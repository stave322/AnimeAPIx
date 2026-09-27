import requests

# Example location: Manila
url = "https://api.open-meteo.com/v1/forecast?latitude=10.31575&longitude=123.88549842&current_weather=true"

response = requests.get(url)
data = response.json()

weather = data["current_weather"]

print("Temperature:", weather["temperature"], "°C")
print("Wind Speed:", weather["windspeed"], "km/h")
print("Wind Direction:", weather["winddirection"], "°")
print("Weather Code:", weather["weathercode"])
