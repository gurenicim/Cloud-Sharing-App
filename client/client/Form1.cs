using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace client
{
    public partial class Form1 : Form
    {

        bool terminating = false;
        bool connected = false;
        Socket clientSocket;
        string username = "";
        string fileName = "", shortFileName = "";
        string path = "";
        string Permanent_Path = "";
        bool pathCheck = false; // ensures path is chosen before connecting
        public Form1()
        {
            Control.CheckForIllegalCrossThreadCalls = false;
            this.FormClosing += new FormClosingEventHandler(Form1_FormClosing);
            InitializeComponent();
            download_button.Enabled = false;

        }

        private void button_connect_Click(object sender, EventArgs e)
        {
            clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            string IP = textBox_ip.Text;

            int portNum;
            if (Int32.TryParse(textBox_port.Text, out portNum))
            {
                try
                {
                    clientSocket.Connect(IP, portNum);
                    
                    connected = true;
                    logs.AppendText("Connected to the server!\n");
                    
                    username = textBox_username.Text;

                    if (username != "" && username.Length <= 64)
                    {
                        Byte[] buffer = new Byte[64];
                        buffer = Encoding.Default.GetBytes(username);
                        clientSocket.Send(buffer);
                    }

                    button_connect.Enabled = false;
                    button_send.Enabled = true;
                    button_get_files.Enabled = true;
                    button_disconnect.Enabled = true;
                    getPublicList_Button.Enabled = true;
                    makepublic_button.Enabled = true;
                    delete_button.Enabled = true;
                    button_copy.Enabled = true;

                    Thread receiveThread = new Thread(Receive);
                    receiveThread.Start();

                }
                catch
                {
                    logs.AppendText("Could not connect to the server!\n");
                }
            }
            else
            {
                logs.AppendText("Check the port\n");
            }

        }

        private void Receive()
        {
            int count = 0;
            string filename2 = "";
            while (connected)
            {
                try
                {
                    Byte[] buffer = new Byte[64];
                    clientSocket.Receive(buffer);

                    string incomingMessage = Encoding.Default.GetString(buffer);
                    incomingMessage = incomingMessage.Substring(0, incomingMessage.IndexOf("\0"));

                    //logs.AppendText("Server: " + incomingMessage + "\n");

                    if (incomingMessage == "401")
                    {
                        logs.AppendText("This username already exists. Please try another one\n");
                        disconnect();
                    }
                    else if (incomingMessage.Contains("**FileList")) // if list of file is coming from the server.
                    {
                        incomingMessage = incomingMessage.Substring(10, incomingMessage.Length - 10);
                        logs.AppendText(incomingMessage);
                    }
                    else if (incomingMessage == "fatal_error")
                    {
                        disconnect();
                    }
                    else if (incomingMessage.Contains("pub_ok-"))
                    {
                        incomingMessage = incomingMessage.Substring(7, incomingMessage.Length - 7);
                        logs.AppendText(incomingMessage + " made public.\n");
                    }
                    else if (incomingMessage.Contains("**GPF")) // get the public file list from server.
                    {
                        incomingMessage = incomingMessage.Substring(5, incomingMessage.Length - 5);
                        logs.AppendText(incomingMessage);
                    }
                    else if (incomingMessage.Contains("**CC-")) // create copy confirmation.
                    {
                        incomingMessage = incomingMessage.Substring(5, incomingMessage.Length - 5);
                        logs.AppendText(incomingMessage);
                    }
                    // ACCESS REQUEST DENIED
                    else if (incomingMessage == "file_error")
                    {
                        logs.AppendText("Request denied: Either file doesn't exist or you don't have access\n");
                    }
                    // MAKE PUBLIC REQUEST DENIED
                    else if (incomingMessage == "make_public_error")
                    {
                        logs.AppendText("Request denied: Either file doesn't exist or already public\n");
                    }
                    else if (incomingMessage == "delete_success")
                    {
                        logs.AppendText("File deleted.\n");
                    }
                    // DOWNLOAD REQUEST ACCEPTED, RECEIVING FILE
                    else if (incomingMessage.Contains("**FD-"))
                    {
                        incomingMessage = incomingMessage.Substring(5, incomingMessage.Length - 5);
                        logs.AppendText("Receiving download: " + incomingMessage + "\n"); //create path
                        string filename1 = "\\" + incomingMessage;
                        path = path + filename1;

                        if (!File.Exists(path)) // filename is unique
                        {
                            // Create a file to write.
                            using (StreamWriter sw = File.CreateText(path))
                            {
                                string chunk;
                                bool end = false;
                                do // receive and write until chunks end
                                {
                                    Byte[] cbuffer = new Byte[256];
                                    clientSocket.Receive(cbuffer);
                                    chunk = Encoding.Default.GetString(cbuffer);
                                    if (chunk.IndexOf("\0") != -1)
                                    {
                                        end = true;
                                        chunk = chunk.Substring(0, chunk.IndexOf("\0"));
                                    }
                                    sw.Write(chunk);
                                } while (!end);
                            }
                            path = Permanent_Path;
                        }
                        else //filename is not unique
                        {
                            String permanent_path_without_ext = path.Substring(0, path.IndexOf(".txt"));
                            while (File.Exists(path)) // increment filename until filename is unique
                            {
                                String path_without_ext = path.Substring(0, path.IndexOf(".txt"));
                                path = permanent_path_without_ext + count.ToString() + ".txt";
                                filename2 = path.Substring(path.LastIndexOf("\\") + 1, path.IndexOf(".txt") - (path.LastIndexOf("\\") + 1));
                                filename2 = filename2 + ".txt";
                                count++;
                                path_without_ext = permanent_path_without_ext;
                            }
                            count = 0;
                            using (StreamWriter sw = File.CreateText(path))
                            {
                                string chunk;
                                bool end = false;
                                do // receive and write until chunks end
                                {
                                    Byte[] cbuffer = new Byte[256];
                                    clientSocket.Receive(cbuffer);
                                    chunk = Encoding.Default.GetString(cbuffer);
                                    if (chunk == "permission denied")
                                    {
                                        logs.AppendText("Permission Denied\n");
                                    }
                                    if (chunk.IndexOf("\0") != -1)
                                    {
                                        end = true;
                                        chunk = chunk.Substring(0, chunk.IndexOf("\0"));
                                    }
                                    sw.Write(chunk);
                                } while (!end);
                            }
                            path = Permanent_Path;
                        }
                    }
                }
                catch
                {
                    disconnect();
                }

            }
        }

        private void Form1_FormClosing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            connected = false;
            terminating = true;
            Environment.Exit(0);
        }

        private void button_send_Click(object sender, EventArgs e)
        {
            if (textBox_path.Text == "")
            {
                logs.AppendText("Please specify a path first\n");
            }
            else
            {
                //name will be sent first
                String filename = "**FL-" + Path.GetFileName(textBox_path.Text);
                String text = System.IO.File.ReadAllText(fileName);
                Byte[] buffer3 = new Byte[filename.Length];
                buffer3 = Encoding.Default.GetBytes(filename);
                clientSocket.Send(buffer3);

                // then chunks with a fixed size of 256 bytes will be sent
                StreamReader reader;
                reader = new StreamReader(fileName);
                string mes = "";
                while (!reader.EndOfStream)
                {
                    Byte[] buffer4 = new Byte[256];
                    mes = "";
                    for (int i = 0; i < 256; i++)
                    {
                        if (!reader.EndOfStream)
                        {
                            mes += (char)reader.Read();
                        }
                        else
                        {
                            mes += "\0";// ending character
                        }
                    }
                    buffer4 = Encoding.Default.GetBytes(mes);
                    clientSocket.Send(buffer4);
                }
                if (mes.IndexOf("\0") == -1) // only happens when file size is a multiple of 256
                { // null character will be the end of the sending process
                    Byte[] end_buffer = new Byte[256];// if this is the case, we need to send a buffer that will act as an ender
                    clientSocket.Send(end_buffer);
                }
                logs.AppendText("File is sent.\n");
            }
        }

        private void button_get_files_Click(object sender, EventArgs e)
        {
            String user = textBox_username.Text;
            Byte[] buffer = new Byte[32];
            buffer = Encoding.Default.GetBytes("**GF-"+user);
            clientSocket.Send(buffer);
        }

        private void makepublic_button_Click(object sender, EventArgs e)
        {
            if (textBox_filenames.Text == "")
            {
                logs.AppendText("Please specify a file name first\n");
            }
            else
            {
                String user = textBox_username.Text;
                String filename = textBox_filenames.Text;
                Byte[] buffer = new Byte[64];
                buffer = Encoding.Default.GetBytes("**MP-" + user + ":" + filename);
                clientSocket.Send(buffer);
            }


        }

        //DISCONNECT :(
        private void button1_Click(object sender, EventArgs e) 
        {
            disconnect();
        }

        private void disconnect()
        {
            if (connected)
            {
                connected = false;
                terminating = true;
                clientSocket.Disconnect(true);
                clientSocket.Close();
                logs.AppendText(username + " is disconnected.\n");
                button_connect.Enabled = true;
                button_disconnect.Enabled = false;
                button_send.Enabled = false;
                button_get_files.Enabled = false;
                getPublicList_Button.Enabled = false;
                makepublic_button.Enabled = false;
                delete_button.Enabled = false;
                button_copy.Enabled = false;
            }
        }
        private void button_copy_Click(object sender, EventArgs e)
        {
            if (textBox_filenames.Text == "")
            {
                logs.AppendText("Please specify a file name first\n");
            }
            else
            {
                String user = textBox_username.Text;
                String fileName = textBox_filenames.Text;
                Byte[] buffer = new Byte[64];
                buffer = Encoding.Default.GetBytes("**CC-" + user + ":" + fileName);
                clientSocket.Send(buffer);
            }
        }

        private void textBox_filenames_TextChanged(object sender, EventArgs e)
        {

        }

        private void download_button_Click(object sender, EventArgs e)
        {
            if (textBox_filenames.Text == "")
            {
                logs.AppendText("Please specify a file name first\n");
            }
            else if (!pathCheck || !connected)
            {
                logs.AppendText("Please specify path or connect to server.\n");
            }
            else
            {
                String user = textBox_username.Text;
                String filename = textBox_filenames.Text;
                Byte[] buffer = new Byte[64];
                buffer = Encoding.Default.GetBytes("**GD-" + user + ":" + filename);
                clientSocket.Send(buffer);
            }
     
        }

        private void delete_button_Click(object sender, EventArgs e)
        {
            if (textBox_filenames.Text == "")
            {
                logs.AppendText("Please specify a file name first\n");
            }
            else
            {
                String user = textBox_username.Text;
                String filename = textBox_filenames.Text;
                Byte[] buffer = new Byte[64];
                buffer = Encoding.Default.GetBytes("**DE-" + user + ":" + filename);
                clientSocket.Send(buffer);
            }
        }

        private void SetPath_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog folderDlg = new FolderBrowserDialog();
            folderDlg.ShowNewFolderButton = true;
            // Show the FolderBrowserDialog.  
            DialogResult result = folderDlg.ShowDialog();
            if (result == DialogResult.OK)
            {
                path = folderDlg.SelectedPath;
                Permanent_Path = folderDlg.SelectedPath;
                Environment.SpecialFolder root = folderDlg.RootFolder;
                pathCheck = true;
                download_button.Enabled = true;
                button_public_download.Enabled = true;
            }
        }

        private void getPublicList_Button_Click(object sender, EventArgs e)
        {
            Byte[] buffer = new Byte[32];
            buffer = Encoding.Default.GetBytes("**GPF-");
            clientSocket.Send(buffer);
        }

        private void button_public_download_Click(object sender, EventArgs e)
        {
            if (textBox_filenames.Text == "")
            {
                logs.AppendText("Please specify a file name first\n");
            }
            else if (!pathCheck || !connected)
            {
                logs.AppendText("Please specify path or connect to server.\n");
            }
            else
            {
                Byte[] buffer = Encoding.Default.GetBytes("**PD-" + textBox_filenames.Text);
                clientSocket.Send(buffer);
            }
        }

        private void button_browse_Click(object sender, EventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Title = "File Sharing Client";
            dlg.ShowDialog();
            textBox_path.Text = dlg.FileName;
            fileName = dlg.FileName;
            shortFileName = dlg.SafeFileName;
        }

    }
}
