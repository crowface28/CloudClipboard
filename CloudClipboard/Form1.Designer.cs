namespace CloudClipboard
{
    partial class Form1
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            this.lblServerUrl = new System.Windows.Forms.Label();
            this.txtServerUrl = new System.Windows.Forms.TextBox();
            this.lblSyncPhrase = new System.Windows.Forms.Label();
            this.txtSyncPhrase = new System.Windows.Forms.TextBox();
            this.btnGeneratePhrase = new System.Windows.Forms.Button();
            this.btnStart = new System.Windows.Forms.Button();
            this.lblInstructions = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // lblServerUrl
            // 
            this.lblServerUrl.AutoSize = true;
            this.lblServerUrl.Location = new System.Drawing.Point(20, 25);
            this.lblServerUrl.Name = "lblServerUrl";
            this.lblServerUrl.Size = new System.Drawing.Size(78, 19);
            this.lblServerUrl.TabIndex = 0;
            this.lblServerUrl.Text = "Server URL:";
            // 
            // txtServerUrl
            // 
            this.txtServerUrl.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(47)))), ((int)(((byte)(49)))), ((int)(((byte)(54)))));
            this.txtServerUrl.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtServerUrl.ForeColor = System.Drawing.Color.White;
            this.txtServerUrl.Location = new System.Drawing.Point(120, 22);
            this.txtServerUrl.Name = "txtServerUrl";
            this.txtServerUrl.PlaceholderText = " e.g. cloudclipboard.ddns.net";
            this.txtServerUrl.Size = new System.Drawing.Size(300, 25);
            this.txtServerUrl.TabIndex = 1;
            this.txtServerUrl.Text = "cloudclipboard.ddns.net";
            // 
            // lblSyncPhrase
            // 
            this.lblSyncPhrase.AutoSize = true;
            this.lblSyncPhrase.Location = new System.Drawing.Point(20, 65);
            this.lblSyncPhrase.Name = "lblSyncPhrase";
            this.lblSyncPhrase.Size = new System.Drawing.Size(84, 19);
            this.lblSyncPhrase.TabIndex = 2;
            this.lblSyncPhrase.Text = "Sync Phrase:";
            // 
            // txtSyncPhrase
            // 
            this.txtSyncPhrase.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(47)))), ((int)(((byte)(49)))), ((int)(((byte)(54)))));
            this.txtSyncPhrase.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtSyncPhrase.ForeColor = System.Drawing.Color.White;
            this.txtSyncPhrase.Location = new System.Drawing.Point(120, 62);
            this.txtSyncPhrase.Name = "txtSyncPhrase";
            this.txtSyncPhrase.PlaceholderText = " e.g. apple-horse-battery";
            this.txtSyncPhrase.Size = new System.Drawing.Size(200, 25);
            this.txtSyncPhrase.TabIndex = 3;
            // 
            // btnGeneratePhrase
            // 
            this.btnGeneratePhrase.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(88)))), ((int)(((byte)(101)))), ((int)(((byte)(242)))));
            this.btnGeneratePhrase.FlatAppearance.BorderSize = 0;
            this.btnGeneratePhrase.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnGeneratePhrase.ForeColor = System.Drawing.Color.White;
            this.btnGeneratePhrase.Location = new System.Drawing.Point(330, 60);
            this.btnGeneratePhrase.Name = "btnGeneratePhrase";
            this.btnGeneratePhrase.Size = new System.Drawing.Size(90, 28);
            this.btnGeneratePhrase.TabIndex = 4;
            this.btnGeneratePhrase.Text = "Generate";
            this.btnGeneratePhrase.UseVisualStyleBackColor = false;
            this.btnGeneratePhrase.Click += new System.EventHandler(this.btnGeneratePhrase_Click);
            // 
            // btnStart
            // 
            this.btnStart.BackColor = System.Drawing.Color.MediumSeaGreen;
            this.btnStart.FlatAppearance.BorderSize = 0;
            this.btnStart.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnStart.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.btnStart.ForeColor = System.Drawing.Color.White;
            this.btnStart.Location = new System.Drawing.Point(20, 110);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(400, 45);
            this.btnStart.TabIndex = 5;
            this.btnStart.Text = "START SYNCING";
            this.btnStart.UseVisualStyleBackColor = false;
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // lblInstructions
            // 
            this.lblInstructions.ForeColor = System.Drawing.Color.DarkGray;
            this.lblInstructions.Location = new System.Drawing.Point(20, 170);
            this.lblInstructions.Name = "lblInstructions";
            this.lblInstructions.Size = new System.Drawing.Size(400, 45);
            this.lblInstructions.TabIndex = 6;
            this.lblInstructions.Text = "Not syncing. Click START to begin.";
            this.lblInstructions.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(34)))), ((int)(((byte)(37)))));
            this.ClientSize = new System.Drawing.Size(444, 231);
            this.Controls.Add(this.lblInstructions);
            this.Controls.Add(this.btnStart);
            this.Controls.Add(this.btnGeneratePhrase);
            this.Controls.Add(this.txtSyncPhrase);
            this.Controls.Add(this.lblSyncPhrase);
            this.Controls.Add(this.txtServerUrl);
            this.Controls.Add(this.lblServerUrl);
            this.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.ForeColor = System.Drawing.Color.White;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Cloud Clipboard";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblServerUrl;
        private System.Windows.Forms.TextBox txtServerUrl;
        private System.Windows.Forms.Label lblSyncPhrase;
        private System.Windows.Forms.TextBox txtSyncPhrase;
        private System.Windows.Forms.Button btnGeneratePhrase;
        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.Label lblInstructions;
    }
}