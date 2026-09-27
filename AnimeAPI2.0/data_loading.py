

import threading
import requests 
import io  
from PIL import Image, ImageTk 
from concurrent.futures import ThreadPoolExecutor
from tkinter import messagebox 
import tkinter as tk 


from api import fetch_anime_page

def _load_page(self, page, query):
    anime_data, error = fetch_anime_page(page, query)

    if error:
        self.root.after(0, lambda: messagebox.showerror("Error", error))
        self.root.after(0, lambda: setattr(self, 'anime_list', []))
        self.root.after(0, self.update_listbox)
        self.root.after(0, self.hide_loading)
        return

    if not anime_data and page > 1:
        self.root.after(0, lambda: messagebox.showinfo("Info", "No more anime available on this page."))
        self.current_page -= 1
        self.root.after(0, self.hide_loading)
        return

    self.anime_list = anime_data

    def preload():
        urls = []
        for anime in self.anime_list:
            url = anime.get("images", {}).get("jpg", {}).get("image_url")
            if url and url not in self.image_cache:
                urls.append(url)

        if not urls:
            self.root.after(0, self.update_listbox)
            self.root.after(0, self.hide_loading)
            return

        with ThreadPoolExecutor(max_workers=6) as executor:
            future_to_url = {executor.submit(self._download_and_cache, url): url for url in urls}
            for future in future_to_url:
                try:
                    future.result()
                except:
                    pass

        self.root.after(0, self.update_listbox)
        self.root.after(0, self.hide_loading)

    threading.Thread(target=preload, daemon=True).start()

def _download_and_cache(self, url):
    try:
        img_data = requests.get(url, timeout=5).content
        img = Image.open(io.BytesIO(img_data)).resize((50, 70), Image.Resampling.LANCZOS)
        photo = ImageTk.PhotoImage(img)
        self.image_cache[url] = photo
    except:
        self.image_cache[url] = None

def update_listbox(self):
    self.tree.delete(*self.tree.get_children())

    for anime in self.anime_list:
        url = anime.get("images", {}).get("jpg", {}).get("image_url")
        thumb = self.image_cache.get(url)
        self.tree.insert("", tk.END,
                         image=thumb,
                         values=(
                             anime.get("title", "N/A"),
                             anime.get("score", "N/A"),
                             anime.get("status", "N/A")
                         ))

def update_buttons(self):
    self.page_label.config(text=f"Page: {self.current_page}")
    self.prev_btn.config(state=tk.NORMAL if self.current_page > 1 else tk.DISABLED)
    self.next_btn.config(state=tk.NORMAL)