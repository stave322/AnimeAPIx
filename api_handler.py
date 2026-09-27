import requests
from typing import Dict, List, Optional

API_URL = "https://api.jikan.moe/v4/anime"

class AnimeAPI:
    """Handles all API requests to Jikan Anime API"""
    
    def __init__(self):
        self.base_url = API_URL
        self.timeout = 5
    
    def search_anime(self, query: str) -> Optional[List[Dict]]:
        """
        Search for anime by name using GET request
        
        Args:
            query: Anime name to search for
            
        Returns:
            List of anime data or None if request fails
        """
        try:
            params = {"query": query, "order_by": "popularity", "sort": "asc"}
            response = requests.get(self.base_url, params=params, timeout=self.timeout)
            response.raise_for_status()
            
            data = response.json()
            return data.get("data", [])
        
        except requests.exceptions.RequestException as e:
            return None
    
    def get_top_anime(self, limit: int = 10) -> Optional[List[Dict]]:
        """
        Get top anime using GET request
        
        Args:
            limit: Number of top anime to retrieve
            
        Returns:
            List of top anime or None if request fails
        """
        try:
            params = {"order_by": "score", "sort": "desc", "limit": limit}
            response = requests.get(self.base_url, params=params, timeout=self.timeout)
            response.raise_for_status()
            
            data = response.json()
            return data.get("data", [])
        
        except requests.exceptions.RequestException as e:
            return None
    
    def get_anime_by_id(self, anime_id: int) -> Optional[Dict]:
        """
        Get specific anime by ID using GET request
        
        Args:
            anime_id: ID of the anime
            
        Returns:
            Anime data or None if request fails
        """
        try:
            url = f"{self.base_url}/{anime_id}"
            response = requests.get(url, timeout=self.timeout)
            response.raise_for_status()
            
            data = response.json()
            return data.get("data")
        
        except requests.exceptions.RequestException as e:
            return None
