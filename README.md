# ✂️ Cloud Clipboard

**Cloud Clipboard** is a lightweight, cross-PC clipboard synchronization tool. It bridges the gap between multiple computers, allowing you to instantly copy text or images on one PC and seamlessly paste them on another.

It consists of a Python backend server and a C# Windows desktop application.

---

## ✨ Features

- **Hotkeys**: Highlight text or copy an image and press `Ctrl+Alt+J`. The app instantly copies it, uploads it to the server, and safely holds it. On your other PC, click where you want it and press `Ctrl+Alt+K` to instantly download and paste!
- **Image Support**: It handles image copying (e.g., right-clicking an image on the web or using the Snipping Tool).
- **Sync Phrases**: Easy-to-type, 3-word dictionary combinations (like `apple-horse-battery`) link your clipboards together.
- **Optional Self-Hosting**: You can host the server yourself if desired. The server script is provided and ready to use.
- **Built-in App Distribution**: Download the Windows `.exe` directly from your server to any Windows PC.

---

## 🚀 Setting Up the Server

The backend is a lightweight Python script (`server.py`). It requires no external dependencies—just standard Python 3.

1. Upload `server.py` to your server.
2. Ensure you have Python 3 installed.
3. Open a terminal and run the server:
   ```bash
   python server.py
   ```
4. The server will start listening on port `80` by default and create a `server_data/` folder to store the clipboard snippets.

*(Note: If you are running this over the open internet, it is strongly recommended to place the server behind a reverse proxy like Nginx with SSL/HTTPS enabled to secure your clipboard contents in transit!)*

---

## 💻 Using the Windows App

The Windows client is a standalone `.exe` bundled with everything it needs.

### 1. Preparing and Downloading the App      
Before the server can distribute the app, place the compiled Windows executable (`CloudClipboard.exe`) in the exact same folder as your `server.py` script. You can either compile it yourself or download a pre-compiled version from the GitHub Releases page.

Once the `.exe` is sitting next to `server.py` and your server is running, you can download it onto any Windows PC by simply visiting your server's URL in a web browser:
`http://<your-server-ip>:80/download-app`

### 2. Linking Your PCs
1. Open `CloudClipboard.exe` on **PC 1**.
2. Enter your server's address in the **Server URL**. 
   - *Tip: If you don't type a port, it automatically assumes `:80`! If you're using a custom port or a reverse proxy, you can type it explicitly (e.g., `myserver.com:8080`).*
3. Click **Generate** to create a secure 3-word Sync Phrase, or enter your own (Alphanumeric and hyphens only).
4. Click **START SYNCING**. The app will run in the background and start listening.
5. On **PC 2**, open the app, enter the *exact same Server URL and Sync Phrase*, and hit **START SYNCING**.

### 3. Copying and Pasting
- **To Copy/Upload**: Highlight any text or copy an image, then press **`Ctrl+Alt+J`**.
- **To Paste/Download**: Click your cursor in an input field or select a location, then press **`Ctrl+Alt+K`**.
