import tkinter as tk
from tkinter import messagebox, scrolledtext, ttk
import requests, threading

def fetch_anime_page(page, callback, query=None):
    def fetch():
        try:
            url = f"https://api.jikan.moe/v4/anime?{'q=' + query + '&' if query else ''}page={page}"
            data = requests.get(url, timeout=10).json().get("data", [])
            callback(data)
        except Exception as e:
            callback(None)
    
    threading.Thread(target=fetch, daemon=True).start()

class AnimeApp:
    def __init__(self, root):
        """Initialize the Anime Browser application and UI"""
        self.root = root
        
        # State variables for tracking current data and navigation
        self.anime_list = []
        self.current_page = 1
        self.current_query = None
        
        # Build the user interface
        self.setup_ui()
        
        # Load the first page of anime data
        self.fetch_page(1)
    
    # ============== UI DESIGN METHODS ==============
    
    def setup_ui(self):
        """Set up the main window and all UI components"""
        self.root.title("Anime Browser")
        self.root.geometry("900x500")
        
        self.create_treeview()
        self.create_search_frame()
        self.create_button_frame()
    
    def create_treeview(self):
        """Create and configure the anime list table display"""
        columns = ("Title", "Rating", "Status")
        self.tree = ttk.Treeview(self.root, columns=columns, height=15, show="headings")
        self.tree.heading("Title", text="Anime Title")
        self.tree.heading("Rating", text="Rating")
        self.tree.heading("Status", text="Status")
        self.tree.column("Title", width=550, anchor="w")
        self.tree.column("Rating", width=100, anchor="center")
        self.tree.column("Status", width=150, anchor="center")
        self.tree.pack(pady=10, fill=tk.BOTH, expand=True)
        # Bind double-click event to show anime details
        self.tree.bind("<Double-1>", self.on_double_click)
    
    def create_search_frame(self):
        """Create the search input field and buttons"""
        search_frame = tk.Frame(self.root)
        search_frame.pack(pady=5)
        tk.Label(search_frame, text="Search:").pack(side=tk.LEFT, padx=5)
        self.search_entry = tk.Entry(search_frame, width=30)
        self.search_entry.pack(side=tk.LEFT, padx=5)
        tk.Button(search_frame, text="Search", command=self.search_anime).pack(side=tk.LEFT, padx=5)
        tk.Button(search_frame, text="Clear", command=self.clear_search).pack(side=tk.LEFT, padx=5)
    
    def create_button_frame(self):
        """Create navigation and action buttons"""
        button_frame = tk.Frame(self.root)
        button_frame.pack(pady=5)
        self.page_label = tk.Label(button_frame, text="Page: 1")
        self.page_label.pack(side=tk.LEFT, padx=5)
        self.prev_btn = tk.Button(button_frame, text="Prev", command=self.prev_page, state=tk.DISABLED)
        self.prev_btn.pack(side=tk.LEFT, padx=5)
        tk.Button(button_frame, text="Next", command=self.next_page).pack(side=tk.LEFT, padx=5)
        tk.Button(button_frame, text="Exit", command=self.root.quit).pack(side=tk.LEFT, padx=5)
    
    # ============== DATA & API METHODS ==============
    
    def fetch_page(self, page):
        """Fetch anime data from API and update the display"""
        def callback(anime_list, error=None):
            if error:
                messagebox.showerror("Error", f"Failed to fetch data: {error}")
                return
            if self.current_query:
                query_lower = self.current_query.lower()
                anime_list = [a for a in anime_list if query_lower in a['title'].lower()]
                if not anime_list:
                    messagebox.showinfo("Search", f"No anime found with '{self.current_query}'")
            self.anime_list = anime_list
            self.update_listbox()
            self.update_buttons()
        fetch_anime_page(page, callback, self.current_query)
    
    # ============== UI UPDATE METHODS ==============
    
    def update_listbox(self):
        """Refresh the anime list table with current data"""
        self.tree.delete(*self.tree.get_children())
        for a in self.anime_list:
            self.tree.insert("", tk.END, values=(a['title'], a.get('score', 'N/A'), a['status']))

    def update_buttons(self):
        """Update button states based on current page number"""
        self.page_label.config(text=f"Page: {self.current_page}")
        self.prev_btn.config(state=tk.NORMAL if self.current_page > 1 else tk.DISABLED)
    
    # ============== ACTION METHODS ==============
    
    def prev_page(self):
        """Navigate to the previous page of anime"""
        if self.current_page > 1:
            self.current_page -= 1
            self.fetch_page(self.current_page)

    def next_page(self):
        """Navigate to the next page of anime"""
        self.current_page += 1
        self.fetch_page(self.current_page)

    def search_anime(self):
        """Handle anime search functionality"""
        query = self.search_entry.get().strip()
        if not query:
            messagebox.showwarning("Warning", "Please enter a search term.")
            return
        self.current_query = query
        self.current_page = 1
        self.fetch_page(1)

    def clear_search(self):
        """Reset search and reload first page of all anime"""
        self.current_query = None
        self.current_page = 1
        self.search_entry.delete(0, tk.END)
        self.fetch_page(1)

    def on_double_click(self, event):
        """Handle double-click event on anime item to show details"""
        selection = self.tree.selection()
        if not selection:
            return
        self.show_details(selection)
    
    def show_details(self, selection):
        """Display detailed information about selected anime in a new window"""
        if not selection:
            messagebox.showwarning("Warning", "Please select an anime.")
            return
        anime = self.anime_list[self.tree.index(selection[0])]
        win = tk.Toplevel(self.root)
        win.title("Anime Details")
        win.geometry("500x400")
        tk.Label(win, text=f"Title: {anime['title']}", font=("Arial", 12)).pack(pady=5)
        tk.Label(win, text=f"Episodes: {anime.get('episodes', 'Unknown')}").pack(pady=5)
        tk.Label(win, text=f"Status: {anime['status']}").pack(pady=5)
        tk.Label(win, text=f"Score: {anime.get('score', 'N/A')}").pack(pady=5)
        tk.Label(win, text="Synopsis:").pack(pady=5)
        txt = scrolledtext.ScrolledText(win, wrap=tk.WORD, height=10)
        txt.insert(tk.END, anime.get('synopsis', 'No synopsis available.'))
        txt.config(state=tk.DISABLED)
        txt.pack(pady=5, fill=tk.BOTH, expand=True)

if __name__ == "__main__":
    root = tk.Tk()
    app = AnimeApp(root)
    root.mainloop()