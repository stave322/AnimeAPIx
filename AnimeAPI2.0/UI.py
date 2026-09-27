

import tkinter as tk 
from tkinter import ttk 

def apply_dark_theme(root):
    root.configure(bg="#1e1e1e")
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

def create_loading_overlay(self):
    self.loading_frame = tk.Frame(self.root, bg="#1e1e1e")
    self.loading_label = tk.Label(self.loading_frame, text="Loading...",
                                  bg="#1e1e1e", fg="#aaaaaa", font=("Segoe UI", 12, "bold"))
    self.loading_label.pack(expand=True)
    self.loading_frame.place(relx=0.5, rely=0.5, anchor="center")
    self.loading_frame.lower()

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