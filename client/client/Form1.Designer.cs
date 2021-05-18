namespace client
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.textBox_ip = new System.Windows.Forms.TextBox();
            this.textBox_port = new System.Windows.Forms.TextBox();
            this.button_connect = new System.Windows.Forms.Button();
            this.logs = new System.Windows.Forms.RichTextBox();
            this.button_send = new System.Windows.Forms.Button();
            this.label_username = new System.Windows.Forms.Label();
            this.textBox_username = new System.Windows.Forms.TextBox();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.button_browse = new System.Windows.Forms.Button();
            this.label_path = new System.Windows.Forms.Label();
            this.textBox_path = new System.Windows.Forms.TextBox();
            this.button_get_files = new System.Windows.Forms.Button();
            this.button_disconnect = new System.Windows.Forms.Button();
            this.button_copy = new System.Windows.Forms.Button();
            this.textBox_filenames = new System.Windows.Forms.TextBox();
            this.filename_label = new System.Windows.Forms.Label();
            this.download_button = new System.Windows.Forms.Button();
            this.delete_button = new System.Windows.Forms.Button();
            this.SetPath = new System.Windows.Forms.Button();
            this.getPublicList_Button = new System.Windows.Forms.Button();
            this.makepublic_button = new System.Windows.Forms.Button();
            this.button_public_download = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(71, 52);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(20, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "IP:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(62, 79);
            this.label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(29, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Port:";
            // 
            // textBox_ip
            // 
            this.textBox_ip.Location = new System.Drawing.Point(94, 50);
            this.textBox_ip.Margin = new System.Windows.Forms.Padding(2);
            this.textBox_ip.Name = "textBox_ip";
            this.textBox_ip.Size = new System.Drawing.Size(169, 20);
            this.textBox_ip.TabIndex = 2;
            // 
            // textBox_port
            // 
            this.textBox_port.Location = new System.Drawing.Point(94, 79);
            this.textBox_port.Margin = new System.Windows.Forms.Padding(2);
            this.textBox_port.Name = "textBox_port";
            this.textBox_port.Size = new System.Drawing.Size(169, 20);
            this.textBox_port.TabIndex = 3;
            // 
            // button_connect
            // 
            this.button_connect.Location = new System.Drawing.Point(94, 130);
            this.button_connect.Margin = new System.Windows.Forms.Padding(2);
            this.button_connect.Name = "button_connect";
            this.button_connect.Size = new System.Drawing.Size(78, 26);
            this.button_connect.TabIndex = 4;
            this.button_connect.Text = "Connect";
            this.button_connect.UseVisualStyleBackColor = true;
            this.button_connect.Click += new System.EventHandler(this.button_connect_Click);
            // 
            // logs
            // 
            this.logs.Location = new System.Drawing.Point(407, 37);
            this.logs.Margin = new System.Windows.Forms.Padding(2);
            this.logs.Name = "logs";
            this.logs.Size = new System.Drawing.Size(308, 261);
            this.logs.TabIndex = 5;
            this.logs.Text = "";
            // 
            // button_send
            // 
            this.button_send.Enabled = false;
            this.button_send.Location = new System.Drawing.Point(119, 211);
            this.button_send.Margin = new System.Windows.Forms.Padding(2);
            this.button_send.Name = "button_send";
            this.button_send.Size = new System.Drawing.Size(70, 28);
            this.button_send.TabIndex = 8;
            this.button_send.Text = "Send";
            this.button_send.UseVisualStyleBackColor = true;
            this.button_send.Click += new System.EventHandler(this.button_send_Click);
            // 
            // label_username
            // 
            this.label_username.AutoSize = true;
            this.label_username.Location = new System.Drawing.Point(32, 110);
            this.label_username.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label_username.Name = "label_username";
            this.label_username.Size = new System.Drawing.Size(58, 13);
            this.label_username.TabIndex = 9;
            this.label_username.Text = "Username:";
            // 
            // textBox_username
            // 
            this.textBox_username.Location = new System.Drawing.Point(94, 108);
            this.textBox_username.Margin = new System.Windows.Forms.Padding(2);
            this.textBox_username.Name = "textBox_username";
            this.textBox_username.Size = new System.Drawing.Size(169, 20);
            this.textBox_username.TabIndex = 10;
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // button_browse
            // 
            this.button_browse.Location = new System.Drawing.Point(45, 211);
            this.button_browse.Margin = new System.Windows.Forms.Padding(2);
            this.button_browse.Name = "button_browse";
            this.button_browse.Size = new System.Drawing.Size(70, 28);
            this.button_browse.TabIndex = 11;
            this.button_browse.Text = "Browse";
            this.button_browse.UseVisualStyleBackColor = true;
            this.button_browse.Click += new System.EventHandler(this.button_browse_Click);
            // 
            // label_path
            // 
            this.label_path.AutoSize = true;
            this.label_path.Location = new System.Drawing.Point(10, 186);
            this.label_path.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label_path.Name = "label_path";
            this.label_path.Size = new System.Drawing.Size(32, 13);
            this.label_path.TabIndex = 12;
            this.label_path.Text = "Path:";
            // 
            // textBox_path
            // 
            this.textBox_path.Location = new System.Drawing.Point(45, 184);
            this.textBox_path.Margin = new System.Windows.Forms.Padding(2);
            this.textBox_path.Name = "textBox_path";
            this.textBox_path.Size = new System.Drawing.Size(334, 20);
            this.textBox_path.TabIndex = 13;
            // 
            // button_get_files
            // 
            this.button_get_files.Enabled = false;
            this.button_get_files.Location = new System.Drawing.Point(407, 306);
            this.button_get_files.Margin = new System.Windows.Forms.Padding(2);
            this.button_get_files.Name = "button_get_files";
            this.button_get_files.Size = new System.Drawing.Size(70, 28);
            this.button_get_files.TabIndex = 14;
            this.button_get_files.Text = "Get File List";
            this.button_get_files.UseVisualStyleBackColor = true;
            this.button_get_files.Click += new System.EventHandler(this.button_get_files_Click);
            // 
            // button_disconnect
            // 
            this.button_disconnect.Enabled = false;
            this.button_disconnect.Location = new System.Drawing.Point(185, 130);
            this.button_disconnect.Name = "button_disconnect";
            this.button_disconnect.Size = new System.Drawing.Size(78, 26);
            this.button_disconnect.TabIndex = 15;
            this.button_disconnect.Text = "Disconnect";
            this.button_disconnect.UseVisualStyleBackColor = true;
            this.button_disconnect.Click += new System.EventHandler(this.button1_Click);
            // 
            // button_copy
            // 
            this.button_copy.Enabled = false;
            this.button_copy.Location = new System.Drawing.Point(205, 307);
            this.button_copy.Name = "button_copy";
            this.button_copy.Size = new System.Drawing.Size(70, 28);
            this.button_copy.TabIndex = 16;
            this.button_copy.Text = "Copy";
            this.button_copy.UseVisualStyleBackColor = true;
            this.button_copy.Click += new System.EventHandler(this.button_copy_Click);
            // 
            // textBox_filenames
            // 
            this.textBox_filenames.Location = new System.Drawing.Point(45, 271);
            this.textBox_filenames.Margin = new System.Windows.Forms.Padding(2);
            this.textBox_filenames.Name = "textBox_filenames";
            this.textBox_filenames.Size = new System.Drawing.Size(334, 20);
            this.textBox_filenames.TabIndex = 17;
            this.textBox_filenames.TextChanged += new System.EventHandler(this.textBox_filenames_TextChanged);
            // 
            // filename_label
            // 
            this.filename_label.AutoSize = true;
            this.filename_label.Location = new System.Drawing.Point(6, 273);
            this.filename_label.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.filename_label.Name = "filename_label";
            this.filename_label.Size = new System.Drawing.Size(26, 13);
            this.filename_label.TabIndex = 18;
            this.filename_label.Text = "File:";
            // 
            // download_button
            // 
            this.download_button.Location = new System.Drawing.Point(45, 306);
            this.download_button.Margin = new System.Windows.Forms.Padding(2);
            this.download_button.Name = "download_button";
            this.download_button.Size = new System.Drawing.Size(70, 28);
            this.download_button.TabIndex = 19;
            this.download_button.Text = "Download";
            this.download_button.UseVisualStyleBackColor = true;
            this.download_button.Click += new System.EventHandler(this.download_button_Click);
            // 
            // delete_button
            // 
            this.delete_button.Enabled = false;
            this.delete_button.Location = new System.Drawing.Point(125, 306);
            this.delete_button.Margin = new System.Windows.Forms.Padding(2);
            this.delete_button.Name = "delete_button";
            this.delete_button.Size = new System.Drawing.Size(70, 28);
            this.delete_button.TabIndex = 20;
            this.delete_button.Text = "Delete";
            this.delete_button.UseVisualStyleBackColor = true;
            this.delete_button.Click += new System.EventHandler(this.delete_button_Click);
            // 
            // SetPath
            // 
            this.SetPath.Location = new System.Drawing.Point(45, 339);
            this.SetPath.Name = "SetPath";
            this.SetPath.Size = new System.Drawing.Size(75, 28);
            this.SetPath.TabIndex = 21;
            this.SetPath.Text = "Set Path";
            this.SetPath.UseVisualStyleBackColor = true;
            this.SetPath.Click += new System.EventHandler(this.SetPath_Click);
            // 
            // getPublicList_Button
            // 
            this.getPublicList_Button.Enabled = false;
            this.getPublicList_Button.Location = new System.Drawing.Point(482, 308);
            this.getPublicList_Button.Margin = new System.Windows.Forms.Padding(2);
            this.getPublicList_Button.Name = "getPublicList_Button";
            this.getPublicList_Button.Size = new System.Drawing.Size(100, 26);
            this.getPublicList_Button.TabIndex = 22;
            this.getPublicList_Button.Text = "Get Public File List";
            this.getPublicList_Button.UseVisualStyleBackColor = true;
            this.getPublicList_Button.Click += new System.EventHandler(this.getPublicList_Button_Click);
            // 
            // makepublic_button
            // 
            this.makepublic_button.Enabled = false;
            this.makepublic_button.Location = new System.Drawing.Point(125, 338);
            this.makepublic_button.Margin = new System.Windows.Forms.Padding(2);
            this.makepublic_button.Name = "makepublic_button";
            this.makepublic_button.Size = new System.Drawing.Size(75, 28);
            this.makepublic_button.TabIndex = 23;
            this.makepublic_button.Text = "Make Public";
            this.makepublic_button.UseVisualStyleBackColor = true;
            this.makepublic_button.Click += new System.EventHandler(this.makepublic_button_Click);
            // 
            // button_public_download
            // 
            this.button_public_download.Enabled = false;
            this.button_public_download.Location = new System.Drawing.Point(205, 338);
            this.button_public_download.Name = "button_public_download";
            this.button_public_download.Size = new System.Drawing.Size(95, 28);
            this.button_public_download.TabIndex = 24;
            this.button_public_download.Text = "Public Download";
            this.button_public_download.UseVisualStyleBackColor = true;
            this.button_public_download.Click += new System.EventHandler(this.button_public_download_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(775, 391);
            this.Controls.Add(this.button_public_download);
            this.Controls.Add(this.makepublic_button);
            this.Controls.Add(this.getPublicList_Button);
            this.Controls.Add(this.SetPath);
            this.Controls.Add(this.delete_button);
            this.Controls.Add(this.download_button);
            this.Controls.Add(this.filename_label);
            this.Controls.Add(this.textBox_filenames);
            this.Controls.Add(this.button_copy);
            this.Controls.Add(this.button_disconnect);
            this.Controls.Add(this.button_get_files);
            this.Controls.Add(this.textBox_path);
            this.Controls.Add(this.label_path);
            this.Controls.Add(this.button_browse);
            this.Controls.Add(this.textBox_username);
            this.Controls.Add(this.label_username);
            this.Controls.Add(this.button_send);
            this.Controls.Add(this.logs);
            this.Controls.Add(this.button_connect);
            this.Controls.Add(this.textBox_port);
            this.Controls.Add(this.textBox_ip);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBox_ip;
        private System.Windows.Forms.TextBox textBox_port;
        private System.Windows.Forms.Button button_connect;
        private System.Windows.Forms.RichTextBox logs;
        private System.Windows.Forms.Button button_send;
        private System.Windows.Forms.Label label_username;
        private System.Windows.Forms.TextBox textBox_username;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.Button button_browse;
        private System.Windows.Forms.Label label_path;
        private System.Windows.Forms.TextBox textBox_path;
        private System.Windows.Forms.Button button_get_files;
        private System.Windows.Forms.Button button_disconnect;
        private System.Windows.Forms.Button button_copy;
        private System.Windows.Forms.TextBox textBox_filenames;
        private System.Windows.Forms.Label filename_label;
        private System.Windows.Forms.Button download_button;
        private System.Windows.Forms.Button delete_button;
        private System.Windows.Forms.Button SetPath;
        private System.Windows.Forms.Button getPublicList_Button;
        private System.Windows.Forms.Button makepublic_button;
        private System.Windows.Forms.Button button_public_download;
    }
}

