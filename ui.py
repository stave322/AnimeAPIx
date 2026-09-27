"""
UI Module - Handles all user interface messages and display
"""

class AnimeUI:
    """Handles all UI messages and output formatting"""
    
    @staticmethod
    def print_header():
        """Display application header"""
        print("\n" + "=" * 60)
        print(" " * 15 + "🎌 ANIME API EXPLORER 🎌")
        print("=" * 60 + "\n")
    
    @staticmethod
    def print_menu():
        """Display main menu options"""
        print("\n--- MENU ---")
        print("1. Search anime by name")
        print("2. Get top 10 anime")
        print("3. Get anime details by ID")
        print("4. Exit")
        print("-" * 40)
    
    @staticmethod
    def get_user_choice() -> str:
        """Get user's menu choice"""
        return input("Enter your choice (1-4): ").strip()
    
    @staticmethod
    def get_search_query() -> str:
        """Get search query from user"""
        return input("Enter anime name to search: ").strip()
    
    @staticmethod
    def get_anime_id() -> str:
        """Get anime ID from user"""
        return input("Enter anime ID: ").strip()
    
    @staticmethod
    def print_loading():
        """Display loading message"""
        print("\n⏳ Fetching data from API...")
    
    @staticmethod
    def print_error(message: str = "Failed to fetch data"):
        """Display error message"""
        print(f"\n❌ ERROR: {message}")
    
    @staticmethod
    def print_not_found(item: str = "Anime"):
        """Display not found message"""
        print(f"\n⚠️  {item} not found!")
    
    @staticmethod
    def print_anime_details(anime: dict):
        """
        Display anime information
        
        Args:
            anime: Anime data dictionary
        """
        print("\n" + "-" * 60)
        print(f"Title: {anime.get('title', 'N/A')}")
        print(f"ID: {anime.get('mal_id', 'N/A')}")
        print(f"Year: {anime.get('year', 'N/A')}")
        print(f"Type: {anime.get('type', 'N/A')}")
        print(f"Episodes: {anime.get('episodes', 'N/A')}")
        print(f"Score: {anime.get('score', 'N/A')}/10")
        print(f"Status: {anime.get('status', 'N/A')}")
        
        synopsis = anime.get('synopsis', 'N/A')
        if synopsis and len(synopsis) > 150:
            synopsis = synopsis[:150] + "..."
        print(f"Synopsis: {synopsis}")
        print("-" * 60)
    
    @staticmethod
    def print_anime_list(animes: list, title: str = "Results"):
        """
        Display list of anime
        
        Args:
            animes: List of anime data
            title: List title
        """
        if not animes:
            print(f"\n📭 No {title.lower()} found")
            return
        
        print(f"\n📺 {title}:\n")
        for idx, anime in enumerate(animes, 1):
            mal_id = anime.get('mal_id', 'N/A')
            title_name = anime.get('title', 'N/A')
            year = anime.get('year', 'N/A')
            score = anime.get('score', 'N/A')
            print(f"{idx}. [{mal_id}] {title_name} ({year}) - Score: {score}")
        print()
    
    @staticmethod
    def print_goodbye():
        """Display goodbye message"""
        print("\n👋 Thank you for using Anime API Explorer!")
        print("=" * 60 + "\n")
    
    @staticmethod
    def print_invalid_input():
        """Display invalid input message"""
        print("\n⚠️  Invalid input. Please try again.")
