import requests
import random

pokemon_list = ["pikachu", "charmander"]

results = []

# Get Pokémon data + roll dice
for pokemon in pokemon_list:
    response = requests.get(f"https://pokeapi.co/api/v2/pokemon/{pokemon}")
    data = response.json()

    roll = random.randint(1, 6)

    results.append({
        "name": data["name"],
        "height": data["height"],
        "weight": data["weight"],
        "ability": data["abilities"][0]["ability"]["name"],
        "roll": roll
    })

# Find highest roll
max_roll = max(p["roll"] for p in results)

# Print results
for p in results:
    status = "WIN 🏆" if p["roll"] == max_roll else "LOSE ❌"

    print("Name:", p["name"])
    print("Height:", p["height"])
    print("Weight:", p["weight"])
    print("Ability:", p["ability"])
    print("Dice Roll:", p["roll"])
    print("Result:", status)
    print("-" * 30)


# response = requests.get("https://catfact.ninja/fact")
# data = response.json()

# print("Cat Fact:", data["fact"])