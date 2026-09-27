

import tkinter as tk 
from tkinter import messagebox
import threading 

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