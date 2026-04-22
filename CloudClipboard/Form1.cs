using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Windows;
using System.Reflection;
using System.Drawing;

namespace CloudClipboard
{
    public partial class Form1 : Form
    {
        private bool isSyncing = false;

        public Form1()
        {
            InitializeComponent();
            try { this.Icon = new Icon("sciccors.ico"); } catch { }
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            if (isSyncing)
            {
                UnregisterHotKey(Handle, 1);
                UnregisterHotKey(Handle, 2);
            }
            base.OnFormClosing(e);
        }

        protected override void WndProc(ref Message m)
        {
            // Check for hotkey messages
            if (m.Msg == WM_HOTKEY)
            {
                if (m.WParam.ToInt32() == 1)
                {
                    // Ctrl+Alt+J pressed
                    UploadClipboard();
                }
                else if (m.WParam.ToInt32() == 2)
                {
                    // Ctrl+Alt+K pressed
                    DownloadClipboard();
                }
            }

            base.WndProc(ref m);
        }

        public async void UploadClipboard()
        {
            if (string.IsNullOrWhiteSpace(txtSyncPhrase.Text))
            {
                MessageBox.Show("Please enter or generate a sync phrase first.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Release physical modifiers so SendKeys works correctly
            keybd_event(VK_CONTROL, 0, KEYEVENTF_KEYUP, UIntPtr.Zero);
            keybd_event(VK_MENU, 0, KEYEVENTF_KEYUP, UIntPtr.Zero);
            await System.Threading.Tasks.Task.Delay(50);

            // Save old clipboard and clear it
            IDataObject oldData = null;
            try { oldData = Clipboard.GetDataObject(); } catch { }
            
            try { Clipboard.Clear(); } catch { }

            // Trigger copy
            SendKeys.SendWait("^c");
            await System.Threading.Tasks.Task.Delay(100);

            // Check if Ctrl+C actually copied anything
            IDataObject newData = null;
            try { newData = Clipboard.GetDataObject(); } catch { }
            
            bool isNewEmpty = (newData == null || newData.GetFormats().Length == 0);

            if (isNewEmpty && oldData != null)
            {
                // Nothing was highlighted to copy. Restore the old clipboard data (e.g. the image they copied earlier)
                try { Clipboard.SetDataObject(oldData, true, 5, 100); } catch { }
            }

            string host = txtServerUrl.Text.Trim();
            if (!host.Contains(":"))
            {
                host += ":80";
            }

            string serverUrl = $"http://{host}/{txtSyncPhrase.Text.Trim()}";
            string filePath = System.IO.Path.GetTempFileName();
            
            try 
            {
                Program.ReadClipboardToFile(filePath);
                await Program.UploadFileAsync(filePath, serverUrl);
            }
            finally 
            {
                if (System.IO.File.Exists(filePath)) 
                {
                    System.IO.File.Delete(filePath);
                }
            }
        }

        public async void DownloadClipboard()
        {
            if (string.IsNullOrWhiteSpace(txtSyncPhrase.Text))
            {
                MessageBox.Show("Please enter or generate a sync phrase first.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string host = txtServerUrl.Text.Trim();
            if (!host.Contains(":"))
            {
                host += ":80";
            }

            string serverDownloadUrl = $"http://{host}/{txtSyncPhrase.Text.Trim()}";
            Program.SetClipboardDataFromUrl(serverDownloadUrl);

            // Release physical modifiers so SendKeys works correctly
            keybd_event(VK_CONTROL, 0, KEYEVENTF_KEYUP, UIntPtr.Zero);
            keybd_event(VK_MENU, 0, KEYEVENTF_KEYUP, UIntPtr.Zero);
            await System.Threading.Tasks.Task.Delay(50);

            // Trigger paste
            SendKeys.SendWait("^v");
        }

        private void btnGeneratePhrase_Click(object sender, EventArgs e)
        {
            string[] words = { "apple", "horse", "battery", "staple", "chair", "table", "cloud", "sun", "moon", "star",
                               "water", "fire", "earth", "wind", "tree", "leaf", "bird", "fish", "bear", "wolf",
                               "river", "ocean", "mountain", "valley", "stone", "sand", "snow", "rain", "storm", "ice",
                               "red", "blue", "green", "yellow", "black", "white", "gray", "purple", "orange", "brown",
                               "fast", "slow", "high", "low", "long", "short", "big", "small", "hot", "cold",
                               "happy", "sad", "good", "bad", "new", "old", "first", "last", "true", "false",
                               "one", "two", "three", "four", "five", "six", "seven", "eight", "nine", "ten" };
            Random rnd = new Random();
            string phrase = $"{words[rnd.Next(words.Length)]}-{words[rnd.Next(words.Length)]}-{words[rnd.Next(words.Length)]}";
            txtSyncPhrase.Text = phrase;
            MessageBox.Show("Your new sync phrase is: " + phrase + "\n\nPlease save this phrase! You will need to enter it on your other PC to sync your clipboards.", "Save Sync Phrase", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtServerUrl.Text) || string.IsNullOrWhiteSpace(txtSyncPhrase.Text))
            {
                MessageBox.Show("Please generate a sync phrase and provide a server URL.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (!isSyncing)
            {
                // Start Syncing
                RegisterHotKey(Handle, 1, MOD_CONTROL | MOD_ALT, (int)Keys.J);
                RegisterHotKey(Handle, 2, MOD_CONTROL | MOD_ALT, (int)Keys.K);
                isSyncing = true;
                btnStart.Text = "STOP SYNCING";
                btnStart.BackColor = Color.IndianRed;
                txtServerUrl.Enabled = false;
                txtSyncPhrase.Enabled = false;
                btnGeneratePhrase.Enabled = false;
                lblInstructions.Text = "Active! Highlight && press Ctrl+Alt+J to Upload.\nClick anywhere && press Ctrl+Alt+K to Download.";
            }
            else
            {
                // Stop Syncing
                UnregisterHotKey(Handle, 1);
                UnregisterHotKey(Handle, 2);
                isSyncing = false;
                btnStart.Text = "START SYNCING";
                btnStart.BackColor = Color.MediumSeaGreen;
                txtServerUrl.Enabled = true;
                txtSyncPhrase.Enabled = true;
                btnGeneratePhrase.Enabled = true;
                lblInstructions.Text = "Not syncing. Click START to begin.";
            }
        }

        // Constants for hotkey registration
        private const int WM_HOTKEY = 0x312;
        private const int MOD_CONTROL = 0x2;
        private const int MOD_ALT = 0x1;

        [DllImport("user32.dll")]
        private static extern bool RegisterHotKey(IntPtr hWnd, int id, int modifiers, int vk);

        [DllImport("user32.dll")]
        private static extern bool UnregisterHotKey(IntPtr hWnd, int id);

        [DllImport("user32.dll", SetLastError = true)]
        static extern void keybd_event(byte bVk, byte bScan, uint dwFlags, UIntPtr dwExtraInfo);

        private const int KEYEVENTF_KEYUP = 0x0002;
        private const byte VK_CONTROL = 0x11;
        private const byte VK_MENU = 0x12; // Alt
    }
}