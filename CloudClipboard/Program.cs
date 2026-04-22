using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using System.Timers;
using System.Drawing;
using System.Net;
using System.Threading;
using System.Runtime.InteropServices;


namespace CloudClipboard
{


    internal static class Program
    {
        public static void ReadClipboardToFile(string filePath)
        {
            // Get the clipboard data.
            if (Clipboard.ContainsImage())
            {
                using (Image returnImage = Clipboard.GetImage())
                {
                    if (returnImage != null)
                    {
                        returnImage.Save(filePath, System.Drawing.Imaging.ImageFormat.Png);
                    }
                }
            }
            else if (Clipboard.ContainsText())
            {
                string clipboardData = Clipboard.GetText();
                using (var streamWriter = new StreamWriter(filePath))
                {
                    streamWriter.Write(clipboardData);
                }
            }
            else
            {
                // Create empty file if no supported data found
                File.Create(filePath).Close();
            }
        }
        public static Task UploadFileAsync(string filePath, string serverUrl)
        {
            // Create a new WebClient object.
            var webClient = new WebClient();

            // Read data in from file
            byte[] fileData = File.ReadAllBytes(filePath);

            // Upload to webserver
            webClient.UploadData(serverUrl, fileData);

            return Task.CompletedTask;
        }

        public static void SetClipboardDataFromUrl(string url)
        {
            // Create a new WebClient object.
            using (var webClient = new WebClient())
            {
                // Download the data from the URL.
                byte[] data = webClient.DownloadData(url);

                // Check if the data is binary or text data.
                if (IsImage(data))
                {
                    Image img;
                    // Set the clipboard data as binary data.
                    using (var ms = new MemoryStream(data))
                    {
                        img = Image.FromStream(ms);
                    }
                    Clipboard.SetImage(img);
                }
                else
                {
                    // Set the clipboard data as text data.
                    Clipboard.SetText(Encoding.UTF8.GetString(data));
                }
            }
        }

        // Check if the data is binary data.
        private static bool IsImage(byte[] data)
        {
            // Check if the first few bytes of the data are a known image file header.
            byte[] pngHeader = new byte[] { 0x89, 0x50, 0x4E }; // PNG header
            byte[] jpgHeader = new byte[] { 0xFF, 0xD8, 0xFF }; // JPEG
            byte[] dataBytes = new byte[3];
            Array.Copy(data, 0, dataBytes, 0, dataBytes.Length);
            if (dataBytes.SequenceEqual(pngHeader) || dataBytes.SequenceEqual(jpgHeader))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        
        [STAThread]
        static void Main()
        {
            ApplicationConfiguration.Initialize();
            Application.Run(new Form1());
        }
    }
}