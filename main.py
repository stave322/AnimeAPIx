"""
Main Application - Combines API logic and UI
Entry point for the Anime API Explorer
"""

from api_handler import AnimeAPI
from ui import AnimeUI


def main():
    """Main application loop"""
    api = AnimeAPI()
    ui = AnimeUI()
    
    ui.print_header()
    
    while True:
        ui.print_menu()
        choice = ui.get_user_choice()
        
        if choice == "1":
            # Search anime by name
            query = ui.get_search_query()
            if not query:
                ui.print_invalid_input()
                continue
            
            ui.print_loading()
            results = api.search_anime(query)
            
            if results is None:
                ui.print_error("Could not connect to API")
            elif results:
                ui.print_anime_list(results[:5], f"Search Results for '{query}'")
            else:
                ui.print_not_found("Anime")
        
        elif choice == "2":
            # Get top 10 anime
            ui.print_loading()
            results = api.get_top_anime(limit=10)
            
            if results is None:
                ui.print_error("Could not connect to API")
            elif results:
                ui.print_anime_list(results, "🏆 Top 10 Highest Rated Anime")
            else:
                ui.print_not_found("Top anime")
        
        elif choice == "3":
            # Get anime by ID
            anime_id = ui.get_anime_id()
            if not anime_id.isdigit():
                ui.print_invalid_input()
                continue
            
            ui.print_loading()
            anime = api.get_anime_by_id(int(anime_id))
            
            if anime is None:
                ui.print_error("Could not connect to API")
            elif anime:
                ui.print_anime_details(anime)
            else:
                ui.print_not_found("Anime with this ID")
        
        elif choice == "4":
            # Exit
            ui.print_goodbye()
            break
        
        else:
            ui.print_invalid_input()


if __name__ == "__main__":
    main()
