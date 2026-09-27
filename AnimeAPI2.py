import requests

BASE_URL = "https://api.jikan.moe/v4/anime"

while True:
    page = input("\nEnter page number (0 to exit): ")
    
    if page == "0":
        print("Goodbye!")
        break
    
    #  Need number validation logic

    if not page.isdigit():
        print("Please enter a valid number.")
        continue
    
    # page lemit logic

    page = int (page)

    if page > 1186:
        print("Invalid page number. Please enter a number less than 1186.")
        continue


    # Get logic
    
    url = f"{BASE_URL}?page={int(page)}"
    response = requests.get(url)
    anime_list = response.json()["data"]

    # Logic Next Page

    print("\n=== Anime List ===")
    for i, anime in enumerate(anime_list, start=1):
        print(f"{i}. {anime['title']}")

    # List of anime in a page logic

    while True:
        choice = input("\nChoose anime (1–25) or B to go back: ").lower()

        if choice == "b":
            break

        if not choice.isdigit():
            print("Invalid input.")
            continue

        choice = int(choice)

        if choice < 1 or choice > len(anime_list):
            print("Number out of range.")
            continue

        #  Show anime details logic

        anime = anime_list[choice - 1]
        print("\n=== Anime Details ===")
        print("Anime ID:", anime["mal_id"])
        print("Title:", anime["title"])
        print("Episodes:", anime["episodes"])
        print("Status:", anime["status"])
        print("Score:", anime["score"])
        print("\nSynopsis:")
        print(anime["synopsis"])
        print("-" * 50)
