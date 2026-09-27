

import io 
import tkinter as tk 
from PIL import Image, ImageTk 
from tkinter import scrolledtext 
import requests


from UI import (
    apply_dark_theme, create_loading_overlay, show_loading, hide_loading,
    create_search_frame, create_button_frame, create_treeview
)
from Action import start_search, prev_page, next_page, clear_search, _start_load
from data_loading import _load_page, _download_and_cache, update_listbox, update_buttons

class AnimeApp:

    def __init__(self, root):
        self.root = root
        self.anime_list = []
        self.current_page = 1
  
        self.current_query = None
        self.image_cache = {}
   
        self.loading = False

        apply_dark_theme(self.root)
        self.setup_ui()

        self._start_load()

    def setup_ui(self):
        self.root.title("AniPics - Real-time Anime Info and Epsiode Tracker by Ash, Riz, and Ryan")
        self.root.geometry("1000x720")
        self.root.minsize(940, 640)

        create_search_frame(self)
        create_button_frame(self)
        create_treeview(self)
        create_loading_overlay(self)

  
    start_search = start_search
    prev_page = prev_page
    next_page = next_page
    clear_search = clear_search
    _start_load = _start_load
    _load_page = _load_page
    _download_and_cache = _download_and_cache
    update_listbox = update_listbox
    update_buttons = update_buttons
    show_loading = show_loading
    hide_loading = hide_loading

  

    def on_double_click(self, event):
        selection = self.tree.selection()
        if not selection or self.loading:
            return
        idx = self.tree.index(selection[0])
        anime = self.anime_list[idx]

        win = tk.Toplevel(self.root)
        win.title(anime.get("title", "Details"))
        win.geometry("780x680")
        win.configure(bg="#1e1e1e")

        top = tk.Frame(win, bg="#1e1e1e")
        top.pack(fill=tk.X, padx=15, pady=15)

        imgf = tk.Frame(top, bg="#1e1e1e")
        imgf.pack(side=tk.LEFT)

        url = anime.get("images", {}).get("jpg", {}).get("large_image_url")
        if url:
            try:
                
                headers = {
                    'User-Agent': 'Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36'
                }
                response = requests.get(url, timeout=12, headers=headers)
                response.raise_for_status()

                img_data = response.content
                img = Image.open(io.BytesIO(img_data))
                img = img.convert("RGB")  # Fix transparency/format issues
                img = img.resize((200, 280), Image.Resampling.LANCZOS)

                
                ph = ImageTk.PhotoImage(img)

                lbl = tk.Label(imgf, image=ph, bg="#1e1e1e")
                lbl.image = ph  
                lbl.pack()

            except requests.exceptions.RequestException: 
      
                tk.Label(imgf, text="Network error", bg="#1e1e1e", fg="#ff6666").pack(pady=20)
            
            except Exception as e:
                tk.Label(imgf, text="Image failed", bg="#1e1e1e", fg="gray").pack(pady=20)
             
        else:
            tk.Label(imgf, text="No image", bg="#1e1e1e", fg="gray").pack(pady=20)

        
        info = tk.Frame(top, bg="#1e1e1e")
        info.pack(side=tk.LEFT, fill=tk.BOTH, expand=True, padx=(20, 0))

        tk.Label(info, text=anime.get("title", "N/A"),
                 bg="#1e1e1e", fg="white", font=("Segoe UI", 15, "bold")).pack(anchor="w", pady=(0,10))

        for k, v in [
            ("Episodes", anime.get("episodes", "N/A")),
            ("Status",   anime.get("status", "N/A")),
            ("Score",    anime.get("score", "N/A")),
            ("Aired",    anime.get("aired", {}).get("string", "N/A")),
            ("Genres",   ", ".join(g["name"] for g in anime.get("genres", [])) or "N/A"),
            ("Studios",  ", ".join(s["name"] for s in anime.get("studios", [])) or "N/A"),
            ("Rating",   anime.get("rating", "N/A")),
            ("Duration", anime.get("duration", "N/A")),
        ]:
            tk.Label(info, text=f"{k}: {v}",
                     bg="#1e1e1e", fg="#d0d0d0", anchor="w").pack(anchor="w")

        bottom = tk.Frame(win, bg="#1e1e1e")
        bottom.pack(fill=tk.BOTH, expand=True, in_=win, padx=15, pady=(0,15))

        tk.Label(bottom, text="Synopsis", bg="#1e1e1e", fg="white",
                 font=("Segoe UI", 11, "bold")).pack(anchor="w", pady=(0,8))

        txt = scrolledtext.ScrolledText(bottom, wrap=tk.WORD,
                                        bg="#2a2a2a", fg="#e0e0e0",
                                        font=("Segoe UI", 10),
                                        insertbackground="white")
        txt.insert(tk.END, anime.get("synopsis", "No synopsis available."))
        txt.config(state=tk.DISABLED)
        txt.pack(fill=tk.BOTH, expand=True)




if __name__ == "__main__":
    root = tk.Tk()
    app = AnimeApp(root)
    root.mainloop()