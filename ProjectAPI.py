from flask import Flask, jsonify, request

app = Flask(__name__)

# In-memory data (acts like a mini database)
coffees = [
    {"id": 1, "name": "Espresso", "price": 100},
    {"id": 2, "name": "Latte", "price": 130}
]

# Helper: find coffee by ID
def find_coffee(coffee_id):
    for coffee in coffees:
        if coffee["id"] == coffee_id:
            return coffee
    return None


# GET all coffees
@app.route("/api/coffees", methods=["GET"])
def get_coffees():
    return jsonify(coffees), 200


# GET single coffee
@app.route("/api/coffees/<int:coffee_id>", methods=["GET"])
def get_coffee(coffee_id):
    coffee = find_coffee(coffee_id)
    if coffee:
        return jsonify(coffee), 200
    return jsonify({"error": "Coffee not found"}), 404


# ADD new coffee
@app.route("/api/coffees", methods=["POST"])
def add_coffee():
    data = request.get_json()

    if not data or "name" not in data or "price" not in data:
        return jsonify({"error": "Invalid data"}), 400

    new_id = coffees[-1]["id"] + 1 if coffees else 1
    new_coffee = {
        "id": new_id,
        "name": data["name"],
        "price": data["price"]
    }

    coffees.append(new_coffee)
    return jsonify(new_coffee), 201


# UPDATE coffee
@app.route("/api/coffees/<int:coffee_id>", methods=["PUT"])
def update_coffee(coffee_id):
    coffee = find_coffee(coffee_id)
    if not coffee:
        return jsonify({"error": "Coffee not found"}), 404

    data = request.get_json()
    coffee["name"] = data.get("name", coffee["name"])
    coffee["price"] = data.get("price", coffee["price"])

    return jsonify(coffee), 200


# DELETE coffee
@app.route("/api/coffees/<int:coffee_id>", methods=["DELETE"])
def delete_coffee(coffee_id):
    coffee = find_coffee(coffee_id)
    if not coffee:
        return jsonify({"error": "Coffee not found"}), 404

    coffees.remove(coffee)
    return jsonify({"message": "Coffee deleted"}), 200


if __name__ == "__main__":
    app.run(debug=True)
