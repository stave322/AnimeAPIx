age = 90

# More detailed age categories (use elif for additional branches).
if age < 13:
    print("You are a child.")
elif age < 18:
    print("You are a minor.")
elif age < 25:
    print("You are a young adult.")
elif age < 40:
    print("You are an adult.")
elif age < 65:
    print("You are middle-aged.")
elif age < 80:
    print("You are a senior.")
elif age < 100:
    print("You are an elder.")
else:
    print("You are a supercentenarian.")