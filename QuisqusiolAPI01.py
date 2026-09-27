import requests

response = requests.get("https://pokeapi.co/api/v2/pokemon/pikachu")


data = response.json()

print("Name:", data["name"])
print("Height:", data["height"])
print("Weight:", data["weight"])

# response = requests.get("https://catfact.ninja/fact")
# data = response.json()
# print("")
# print("Cat Fact:", data["fact"])