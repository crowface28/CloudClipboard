import os
import sys
import re
import time
from http.server import ThreadingHTTPServer, BaseHTTPRequestHandler

DATA_DIR = "server_data"
if not os.path.exists(DATA_DIR):
    os.makedirs(DATA_DIR)

# --- SECURITY CONSTANTS & STATE ---
VALID_PATH_RE = re.compile(r'^[a-zA-Z0-9-]+$')
MAX_UPLOAD_SIZE = 50 * 1024 * 1024       # 50 MB per file
MAX_FOLDER_SIZE = 1 * 1024 * 1024 * 1024 # 1 GB total server limit

failed_attempts = {} # Dictionary mapping IP -> {'count': int, 'last_time': float}
# ----------------------------------

def get_folder_size(folder):
    total_size = 0
    for dirpath, dirnames, filenames in os.walk(folder):
        for f in filenames:
            fp = os.path.join(dirpath, f)
            if not os.path.islink(fp):
                total_size += os.path.getsize(fp)
    return total_size

def get_file_path(phrase_id):
    # Enforce strict alphanumeric and hyphen regex
    safe_name = phrase_id.lstrip('/')
    if not VALID_PATH_RE.match(safe_name):
        return None
    return os.path.join(DATA_DIR, safe_name + ".dat")

class ClipboardHandler(BaseHTTPRequestHandler):
    def do_GET(self):
        phrase_id = self.path
        client_ip = self.client_address[0]
        
        # 1. Check Rate Limiting for Brute Force protection
        if client_ip in failed_attempts:
            info = failed_attempts[client_ip]
            if info['count'] >= 20:
                if time.time() - info['last_time'] < 300: # 5 minutes block
                    self.send_response(429)
                    self.end_headers()
                    self.wfile.write(b"Too Many Requests. Blocked for 5 minutes.")
                    return
                else:
                    # Time expired, reset their block
                    del failed_attempts[client_ip]

        # 2. Allow base paths explicitly without blocking
        if phrase_id == '/':
            self.send_response(200)
            self.send_header('Content-Type', 'text/plain')
            self.end_headers()
            self.wfile.write(b"Cloud Clipboard Server is running.\n\nVisit /download-app to download the Cloud Clipboard app!")
            return

        if phrase_id == '/download-app':
            exe_path = "CloudClipboard.exe"
            if os.path.exists(exe_path):
                with open(exe_path, 'rb') as f:
                    exe_data = f.read()
                self.send_response(200)
                self.send_header('Content-Type', 'application/x-msdownload')
                self.send_header('Content-Disposition', 'attachment; filename="CloudClipboard.exe"')
                self.send_header('Content-Length', str(len(exe_data)))
                self.end_headers()
                self.wfile.write(exe_data)
                print(f"[{client_ip}] Served the CloudClipboard.exe application!")
            else:
                self.send_response(404)
                self.end_headers()
                self.wfile.write(b"App executable not found on server.")
            return

        # 3. Handle actual clipboard download
        file_path = get_file_path(phrase_id)
        if file_path and os.path.exists(file_path):
            # Successful hit! Reset failed attempts for this IP
            if client_ip in failed_attempts:
                del failed_attempts[client_ip]
                
            with open(file_path, 'rb') as f:
                data = f.read()
            self.send_response(200)
            self.send_header('Content-Type', 'application/octet-stream')
            self.send_header('Content-Length', str(len(data)))
            self.end_headers()
            self.wfile.write(data)
            print(f"[{phrase_id}] - Downloaded {len(data)} bytes from {file_path}")
        else:
            self.send_response(404)
            self.end_headers()
            self.wfile.write(b"Not Found")
            print(f"[{phrase_id}] - Not Found")
            
            # Increment failed attempts for rate limiting
            if client_ip not in failed_attempts:
                failed_attempts[client_ip] = {'count': 1, 'last_time': time.time()}
            else:
                failed_attempts[client_ip]['count'] += 1
                failed_attempts[client_ip]['last_time'] = time.time()

    def do_POST(self):
        phrase_id = self.path
        
        content_length = int(self.headers.get('Content-Length', 0))
        
        # 1. Enforce individual file size limit (50MB)
        if content_length > MAX_UPLOAD_SIZE:
            self.send_response(413)
            self.end_headers()
            self.wfile.write(b"Payload Too Large: Maximum upload size is 50MB.")
            print(f"[{phrase_id}] - Rejected upload: {content_length} bytes exceeds 50MB limit.")
            return

        # 2. Enforce total server folder quota (1GB)
        if get_folder_size(DATA_DIR) + content_length > MAX_FOLDER_SIZE:
            self.send_response(507)
            self.end_headers()
            self.wfile.write(b"Insufficient Storage: Server quota reached.")
            print(f"[{phrase_id}] - Rejected upload: Server storage quota exceeded.")
            return

        # 3. Handle actual upload
        if content_length > 0:
            post_data = self.rfile.read(content_length)
            
            file_path = get_file_path(phrase_id)
            if file_path:
                with open(file_path, 'wb') as f:
                    f.write(post_data)
                    
                self.send_response(200)
                self.end_headers()
                self.wfile.write(b"Uploaded successfully")
                print(f"[{phrase_id}] - Uploaded {content_length} bytes and saved to {file_path}")
            else:
                self.send_response(400)
                self.end_headers()
                self.wfile.write(b"Bad Request: Invalid path format. Use alphanumeric and hyphens only.")
                print(f"[{phrase_id}] - Bad Request: Invalid path format.")
        else:
            self.send_response(400)
            self.end_headers()
            self.wfile.write(b"Bad Request: No data")
            print(f"[{phrase_id}] - Bad Request: No Data")

if __name__ == '__main__':
    port = 80
    server_address = ('0.0.0.0', port)
    # Using ThreadingHTTPServer prevents single-threaded blocking (DoS)
    httpd = ThreadingHTTPServer(server_address, ClipboardHandler)
    print(f"Starting Cloud Clipboard Server on port {port}...")
    print(f"Data will be saved to the '{os.path.abspath(DATA_DIR)}' folder.")
    print("Press Ctrl+C to stop.")
    try:
        httpd.serve_forever()
    except KeyboardInterrupt:
        pass
    httpd.server_close()
    print("\nServer stopped.")
