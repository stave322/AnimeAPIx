import requests 


def fetch_anime_page(page, query=None):
    try:
        base = "https://api.jikan.moe/v4/anime"
        params = {"page": page, "limit": 10} 
        if query:
            params["q"] = query
        response = requests.get(base, params=params, timeout=8)
        response.raise_for_status()
        data = response.json()
        return data.get("data", []), None
    except Exception as e:
        return [], str(e)
