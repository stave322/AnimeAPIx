import tkinter as tk
from tkinter import messagebox, scrolledtext, ttk
import requests
import threading
from PIL import Image, ImageTk
import io
from concurrent.futures import ThreadPoolExecutor

# ================= API FETCH =================

def fetch_anime_page(page, query=None):
    try:
        base = "https://api.jikan.moe/v4/anime"
        params = {"page": page, "limit": 25}  # smaller page size = faster response
        if query:
            params["q"] = query
        response = requests.get(base, params=params, timeout=8)
        response.raise_for_status()
        data = response.json()
        return data.get("data", []), None
    except Exception as e:
        return [], str(e)

# ================= MAIN APP =================

class AnimeApp:
    def __init__(self, root):
        self.root = root
        self.anime_list = []
        self.current_page = 1
        self.current_query = None
        self.image_cache = {}           # url → PhotoImage (persistent)
        self.loading = False

        self.apply_dark_theme()
        self.setup_ui()

        # Load first page
        self._start_load()

    def apply_dark_theme(self):
        self.root.configure(bg="#1e1e1e")
        style = ttk.Style()
        style.theme_use("default")
        style.configure("Treeview",
                        background="#2d2d2d",
                        foreground="#e0e0e0",
                        fieldbackground="#2d2d2d",
                        rowheight=80)
        style.configure("Treeview.Heading",
                        background="#3a3a3a",
                        foreground="#ffffff")
        style.map("Treeview", background=[("selected", "#505050")])

    def setup_ui(self):
        self.root.title("Anime Browser")
        self.root.geometry("1000x720")
        self.root.minsize(940, 640)

        self.create_search_frame()
        self.create_button_frame()
        self.create_treeview()
        self.create_loading_overlay()

    def create_loading_overlay(self):
        self.loading_frame = tk.Frame(self.root, bg="#1e1e1e")
        self.loading_label = tk.Label(self.loading_frame, text="Loading...",
                                      bg="#1e1e1e", fg="#aaaaaa", font=("Segoe UI", 12, "bold"))
        self.loading_label.pack(expand=True)
        self.loading_frame.place(relx=0.5, rely=0.5, anchor="center")
        self.loading_frame.lower()  # hidden initially

    def show_loading(self):
        self.loading = True
        self.prev_btn.config(state="disabled")
        self.next_btn.config(state="disabled")
        self.loading_frame.lift()

    def hide_loading(self):
        self.loading = False
        self.loading_frame.lower()
        self.update_buttons()

    def create_search_frame(self):
        frame = tk.Frame(self.root, bg="#1e1e1e")
        frame.place(x=15, y=12)

        tk.Label(frame, text="Search:", bg="#1e1e1e", fg="#d0d0d0",
                 font=("Segoe UI", 9)).pack(side=tk.LEFT, padx=(0, 6))

        self.search_entry = tk.Entry(frame, width=25,
                                     bg="#333333", fg="#ffffff",
                                     insertbackground="white",
                                     font=("Segoe UI", 9))
        self.search_entry.pack(side=tk.LEFT, padx=4)

        tk.Button(frame, text="Go", command=self.start_search,
                  bg="#444444", fg="white", width=6,
                  relief="flat", activebackground="#666666",
                  font=("Segoe UI", 9)).pack(side=tk.LEFT, padx=4)

    def create_button_frame(self):
        container = tk.Frame(self.root, bg="#1e1e1e", height=70)
        container.pack(fill=tk.X, padx=12, pady=(0, 10), side=tk.BOTTOM)
        container.pack_propagate(False)

        frame = tk.Frame(container, bg="#1e1e1e")
        frame.pack(fill=tk.BOTH, expand=True)

        left = tk.Frame(frame, bg="#1e1e1e")
        left.pack(side=tk.LEFT, pady=6)

        self.page_label = tk.Label(left, text="Page: 1",
                                   bg="#1e1e1e", fg="#b0b0b0",
                                   font=("Segoe UI", 10, "bold"))
        self.page_label.pack(side=tk.LEFT, padx=(0, 20))

        self.prev_btn = tk.Button(left, text="◄ Prev", command=self.prev_page,
                                  state=tk.DISABLED,
                                  bg="#3a3a3a", fg="white", width=10,
                                  relief="flat")
        self.prev_btn.pack(side=tk.LEFT, padx=5)

        self.next_btn = tk.Button(left, text="Next ►", command=self.next_page,
                                  bg="#3a3a3a", fg="white", width=10,
                                  relief="flat")
        self.next_btn.pack(side=tk.LEFT, padx=5)

        right = tk.Frame(frame, bg="#1e1e1e")
        right.pack(side=tk.RIGHT, pady=6)

        tk.Button(right, text="Clear", command=self.clear_search,
                  bg="#664d1f", fg="white", width=10,
                  relief="flat").pack(side=tk.RIGHT, padx=8)

        tk.Button(right, text="Exit", command=self.root.quit,
                  bg="#7a1f1f", fg="white", width=9,
                  relief="flat").pack(side=tk.RIGHT, padx=6)

    def create_treeview(self):
        columns = ("Title", "Rating", "Status")
        self.tree = ttk.Treeview(self.root, columns=columns, show="tree headings",
                                 selectmode="browse")

        self.tree.heading("#0", text="Poster")
        self.tree.heading("Title", text="Anime Title")
        self.tree.heading("Rating", text="Score")
        self.tree.heading("Status", text="Status")

        self.tree.column("#0", width=90, anchor="center")
        self.tree.column("Title", width=550, anchor="w")
        self.tree.column("Rating", width=110, anchor="center")
        self.tree.column("Status", width=170, anchor="center")

        self.tree.pack(fill=tk.BOTH, expand=True, padx=12, pady=(50, 10))
        self.tree.bind("<Double-1>", self.on_double_click)

    # ================= ACTIONS =================

    def start_search(self):
        if self.loading: return
        query = self.search_entry.get().strip()
        if not query:
            messagebox.showwarning("Warning", "Please enter something.")
            return
        self.current_query = query
        self.current_page = 1
        self._start_load()

    def prev_page(self):
        if self.loading or self.current_page <= 1: return
        self.current_page -= 1
        self._start_load()

    def next_page(self):
        if self.loading: return
        self.current_page += 1
        self._start_load()

    def clear_search(self):
        if self.loading: return
        self.current_query = None
        self.current_page = 1
        self.search_entry.delete(0, tk.END)
        self._start_load()

    def _start_load(self):
        self.show_loading()
        threading.Thread(target=self._load_page,
                         args=(self.current_page, self.current_query),
                         daemon=True).start()

    def _load_page(self, page, query):
        # 1. Fetch anime metadata
        anime_data, error = fetch_anime_page(page, query)

        if error:
            self.root.after(0, lambda: messagebox.showerror("Error", error))
            self.root.after(0, lambda: setattr(self, 'anime_list', []))
            self.root.after(0, self.update_listbox)
            self.root.after(0, self.hide_loading)
            return

        # Check for no results on next page
        if not anime_data and page > 1:
            self.root.after(0, lambda: messagebox.showinfo("Info", "No more anime available on this page."))
            self.current_page -= 1  # Revert to previous page
            self.root.after(0, self.hide_loading)
            return

        self.anime_list = anime_data

        # 2. Preload thumbnails in background (parallel)
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
                        future.result()  # wait & cache
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
        self.next_btn.config(state=tk.NORMAL)  # usually safe to allow

    # ================= DETAILS =================

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
                data = requests.get(url, timeout=5).content
                img = Image.open(io.BytesIO(data)).resize((200, 280), Image.Resampling.LANCZOS)
                ph = ImageTk.PhotoImage(img)
                lbl = tk.Label(imgf, image=ph, bg="#1e1e1e")
                lbl.image = ph
                lbl.pack()
            except:
                tk.Label(imgf, text="Image failed", bg="#1e1e1e", fg="gray").pack()
        else:
            tk.Label(imgf, text="No image", bg="#1e1e1e", fg="gray").pack()

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
        bottom.pack(fill=tk.BOTH, expand=True, padx=15, pady=(0,15))

        tk.Label(bottom, text="Synopsis", bg="#1e1e1e", fg="white",
                 font=("Segoe UI", 11, "bold")).pack(anchor="w", pady=(0,8))

        txt = scrolledtext.ScrolledText(bottom, wrap=tk.WORD,
                                        bg="#2a2a2a", fg="#e0e0e0",
                                        font=("Segoe UI", 10),
                                        insertbackground="white")
        txt.insert(tk.END, anime.get("synopsis", "No synopsis available."))
        txt.config(state=tk.DISABLED)
        txt.pack(fill=tk.BOTH, expand=True)

# ================= RUN =================

if __name__ == "__main__":
    root = tk.Tk()
    app = AnimeApp(root)
    root.mainloop()