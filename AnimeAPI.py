import requests

BASE_URL = "https://api.jikan.moe/v4/anime"


def get_page(page):
    url = f"{BASE_URL}?page={page}"
    response = requests.get(url)
    return response.json()["data"]


def show_anime_list(anime_list):
    print("\n=== Anime List ===")
    for i, anime in enumerate(anime_list, start=1):
        print(f"{i}. {anime['title']}")


def show_anime_details(anime):
    print("\n=== Anime Details ===")
    print("Anime ID:", anime["mal_id"])
    print("Title:", anime["title"])
    print("Episodes:", anime["episodes"])
    print("Status:", anime["status"])
    print("Score:", anime["score"])
    print("\nSynopsis:")
    print(anime["synopsis"])
    print("-" * 50)


def main():
    while True:
        page = input("\nEnter page number (0 to exit): ")

        if page == "0":
            print("Goodbye!")
            break

        if not page.isdigit():
            print("Please enter a valid number.")
            continue

        anime_list = get_page(int(page))
        show_anime_list(anime_list)

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

            show_anime_details(anime_list[choice - 1])


# Program starts here
if __name__ == "__main__":
    main()