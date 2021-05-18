using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApplication3
{
    public partial class Server : Form
    {
        Socket serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        public Hashtable clientsList = new Hashtable();


        bool terminating = false;
        bool listening = false;

        bool checkFile = false;
        bool pathCheck = false; // ensures path is chosen before listening

        String path = "";
        String Permanent_Path="";
        String database = "";
        public Server()
        {
            Control.CheckForIllegalCrossThreadCalls = false;
            this.FormClosing += new FormClosingEventHandler(Server_FormClosing);
            InitializeComponent();
        }
        private void portBox_TextChanged(object sender, EventArgs e)
        {

        }
        private void portLabel_Click(object sender, EventArgs e)
        {

        }
        private void listenButton_Click(object sender, EventArgs e)
        {
            int serverPort;

            if (Int32.TryParse(portBox.Text, out serverPort) && pathCheck)
            {
                IPEndPoint endPoint = new IPEndPoint(IPAddress.Any, serverPort);
                serverSocket.Bind(endPoint);
                serverSocket.Listen(3);

                listening = true;
                listenButton.Enabled = false;
               
                Thread acceptThread = new Thread(Accept);
                acceptThread.Start();

                logs.AppendText("Started listening on port: " + serverPort + "\n");
                // initialize db
                database = Permanent_Path + "\\" + "database.txt";
                if (!File.Exists(database))
                {
                    using (StreamWriter sw = File.CreateText(database))
                    {
                        sw.WriteLine("DATABASE");
                    }
                }
            }
            else
            {
                logs.AppendText("Please check port number or specify path \n");
            }
        }
        private void Accept()
        {
            while (listening)
            {
                try
                {
                    Socket newClient = serverSocket.Accept();

                    Byte[] buffer = new Byte[64];
                    newClient.Receive(buffer);

                    string incomingMessage = Encoding.Default.GetString(buffer);
                    incomingMessage = incomingMessage.Substring(0, incomingMessage.IndexOf("\0"));

                    if (clientsList.Contains(incomingMessage))
                    {
                        sameUserNameHandler(newClient);
                    }
                    else
                    {
                        clientsList.Add(incomingMessage, newClient);
                        logs.AppendText(incomingMessage + ": connected to server.\n");
                        Thread receiveThread = new Thread(() => Receive(newClient, incomingMessage)); // updated
                        receiveThread.Start();
                    }
                }
                catch
                {
                    if (terminating)
                    {
                        listening = false;
                    }
                    else
                    {
                        logs.AppendText("The socket stopped working.\n");
                    }
                }
            }
        }
        private void Receive(Socket thisClient, String User) // updated
        {
            bool connected = true;
            String filename1="";
            String filename2 = "";
            int count = 0;
            while (connected && !terminating)
            {
                try
                {
                    Byte[] name_buffer = new Byte[256]; //receive file name
                    if (SocketConnected(thisClient) == false) // this statement checks for client socket disconnections and throws exception
                    {
                        throw new Exception("Disconnection");
                    }
                    else
                    {
                        thisClient.Receive(name_buffer);
                        filename1 = Encoding.Default.GetString(name_buffer);
                        filename1 = filename1.Substring(0, filename1.IndexOf("\0"));

                        // FILE LIST REQUEST
                        if (filename1.IndexOf("**GF-") != -1)
                        {
                            int startindex = filename1.IndexOf("**GF-") + 5;
                            String usernameToSearch = filename1.Substring(startindex, filename1.Length - (startindex));
                            foreach (string line in File.ReadLines(database))
                            {
                                if (line.Contains(usernameToSearch + ":"))
                                {
                                    int startindex2 = line.IndexOf("\t") + 2 + usernameToSearch.Length; // to not include the prefix from the uniqueness
                                    String FileNameAndTime = line.Substring(startindex2, line.Length - startindex2); // Get the filename and time
                                    FileNameAndTime = "**FileList" + FileNameAndTime + "\n";
                                    Thread.Sleep(10);
                                    Byte[] bufferx = new Byte[FileNameAndTime.Length + 1];
                                    bufferx = Encoding.Default.GetBytes(FileNameAndTime);
                                    thisClient.Send(bufferx);
                                }
                            }
                        }
                        // GET PUBLIC FILE LIST
                        else if (filename1.IndexOf("**GPF-") != -1)
                        {
                           foreach (string line in File.ReadLines(database))
                            {
                                if (line.Contains("publicfile"))
                                {
                                    string result = "**GPF" + line.Replace("publicfile", "") + "\n";
                                    Thread.Sleep(10);
                                    Byte[] bufferx = new Byte[result.Length + 1];
                                    bufferx = Encoding.Default.GetBytes(result);
                                    thisClient.Send(bufferx);
                                }
                            }
                        }
                        //  MAKE FILE PUBLIC
                        else if (filename1.IndexOf("**MP-") != -1)
                        {
                            int startindex = filename1.IndexOf("**MP-") + 5;
                            int user_end_index = filename1.IndexOf(":");
                            string filename = filename1.Substring(user_end_index + 1, filename1.Length - (user_end_index) - 1);
                            string initialname = filename;
                            bool exists = false;
                            bool already = false;
                            string username = filename1.Substring(startindex, user_end_index - (startindex));
                            logs.AppendText("filename to make public: " + filename + " from the user: " + username + "\n");
                            string[] arrLine = File.ReadAllLines(database);
                            int myIndex = 0;
                            foreach (string line in File.ReadLines(database))
                            {
                                if (line.Contains(User + "-" + filename))
                                {
                                    if (line.Contains("publicfile"))
                                    {
                                        already = true;
                                        break;
                                    }
                                    else
                                    {
                                        filename = "\\" + username + "-" + filename;
                                        arrLine[myIndex] = line + "\t" + "publicfile";
                                        exists = true;
                                        break;
                                    }
                                }
                                myIndex = myIndex + 1;
                            }
                            if (exists)
                            {
                                File.WriteAllLines(database, arrLine);
                                logs.AppendText(username + "'s file: " + filename + " became public.");
                                Byte[] msg = Encoding.Default.GetBytes("pub_ok-" + initialname);
                                thisClient.Send(msg);
                            }
                            else if(already)
                            {
                                logs.AppendText("File is already public.\n");
                                Byte[] err = Encoding.Default.GetBytes("make_public_error");
                                thisClient.Send(err);
                            }
                            else
                            {
                                logs.AppendText("File not found.\n");
                                Byte[] err = Encoding.Default.GetBytes("make_public_error");
                                thisClient.Send(err);
                            }
                        }
                        /////////////////
                        // CREATE COPY REQUEST
                        else if (filename1.IndexOf("**CC-") != -1)
                        {
                            int startindex = filename1.IndexOf("**CC-") + 5;
                            int user_end_index = filename1.IndexOf(":");
                            string filename = filename1.Substring(user_end_index + 1, filename1.Length - (user_end_index) - 1);
                            string initialname = filename;
                            bool exists = false;
                            bool isPublic = false;
                            string username = filename1.Substring(startindex, user_end_index - (startindex));
                            logs.AppendText("filename to create copy: " + filename + " from the user: " + username + "\n");
                            if (File.Exists(Permanent_Path + "\\" + User + "-" + filename))
                            {
                                path = path + "\\" + User + "-" + filename;
                                string source_path = path;
                                foreach (string line in File.ReadLines(database))
                                {
                                    if (line.Contains(User + "-" + filename))
                                    {
                                        if (line.Contains("publicfile"))
                                        {
                                            isPublic = true;
                                        }
                                        else
                                        {
                                            isPublic = false;
                                        }
                                        break;
                                    }
                                }
                                count = 0;
                                String permanent_path_without_ext = path.Substring(0, path.IndexOf(".txt"));
                                while (File.Exists(path)) // increment filename until filename is unique
                                {
                                    String path_without_ext = path.Substring(0, path.IndexOf(".txt"));
                                    path = permanent_path_without_ext + "(" + count.ToString() + ")" + ".txt";
                                    filename2 = path.Substring(path.LastIndexOf("\\") + 1, path.IndexOf(".txt") - (path.LastIndexOf("\\") + 1));
                                    filename2 = filename2 + ".txt";
                                    count++;
                                    path_without_ext = permanent_path_without_ext;
                                }
                                // Will not overwrite if the destination file already exists.
                                File.Copy(source_path, path);

                                
                                using (StreamWriter sw = File.AppendText(database))
                                { // update database
                                    DateTime now = DateTime.Now;
                                    if (isPublic)
                                    { 
                                        sw.WriteLine(User + ":\t" + filename2 + "\t" + now + "\t" + "publicfile"); 
                                    }
                                    else
                                    {
                                        
                                        sw.WriteLine(User + ":\t" + filename2 + "\t" + now);
                                    }
                                }
                                path = Permanent_Path;

                                string copied = "**CC-Server copied the file named: " + initialname + "\n";
                                Byte[] copied_buffer = new Byte[64];
                                copied_buffer = Encoding.Default.GetBytes(copied);
                                thisClient.Send(copied_buffer);
                            }
                            else
                            {
                                logs.AppendText("No such file in database with name: " + initialname + "\n");
                                Byte[] err = Encoding.Default.GetBytes("file_error");
                                thisClient.Send(err);
                            }
                        }
                        // LOCAL DOWNLOAD REQUEST
                        else if (filename1.IndexOf("**GD-") != -1)
                        {
                            // server will send file in chunks just as client sends it
                            int user_end_index = filename1.IndexOf(":");
                            string filename = filename1.Substring(user_end_index + 1, filename1.Length - (user_end_index) - 1);
                            logs.AppendText("Request to download file: " + filename + " from: " + User + "\n");
                            // Code from Client modified
                            
                            StreamReader reader;
                            try
                            {
                                reader = new StreamReader(Permanent_Path + "\\" + User + "-" + filename);
                                //name will be sent first
                                Byte[] buffer3 = new Byte[filename.Length];
                                buffer3 = Encoding.Default.GetBytes("**FD-" + filename);
                                thisClient.Send(buffer3);

                                // then chunks with a fixed size of 256 bytes will be sent
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
                                    thisClient.Send(buffer4);
                                }
                                if (mes.IndexOf("\0") == -1) // only happens when file size is a multiple of 256
                                { // null character will be the end of the sending process
                                    Byte[] end_buffer = new Byte[256];// if this is the case, we need to send a buffer that will act as an ender
                                    thisClient.Send(end_buffer);
                                }
                                logs.AppendText("File is sent.\n");
                            }
                            catch
                            {
                                logs.AppendText("Request from: " + User + " Declined!\n");
                                Byte[] err = Encoding.Default.GetBytes("file_error");
                                thisClient.Send(err);
                            }

                        }
                        // PUBLIC DOWNLOAD REQUEST
                        else if (filename1.Contains("**PD-"))
                        {
                            bool isPublic = false;
                            filename1 = filename1.Substring(5, filename1.Length - 5);
                            foreach (string line in File.ReadLines(database))
                            {
                                if (line.Contains(filename1))
                                {
                                    if (line.Contains("publicfile"))
                                    {
                                        isPublic = true;
                                    }
                                    else
                                    {
                                        isPublic = false;
                                    }
                                    break;
                                }
                            }
                            if (isPublic)
                            {
                                StreamReader reader;
                                try
                                {
                                    reader = new StreamReader(Permanent_Path + "\\" + filename1);
                                    //name will be sent first
                                    Byte[] buffer3 = new Byte[filename1.Length];
                                    buffer3 = Encoding.Default.GetBytes("**FD-" + filename1);
                                    thisClient.Send(buffer3);

                                    // then chunks with a fixed size of 256 bytes will be sent
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
                                        thisClient.Send(buffer4);
                                    }
                                }
                                catch
                                {
                                    logs.AppendText("Request from: " + User + " Declined!\n");
                                    Byte[] buffer = Encoding.Default.GetBytes("file_error");
                                    thisClient.Send(buffer);
                                }
                                
                            }
                            else
                            {
                                Byte[] buffer = Encoding.Default.GetBytes("file_error");
                                thisClient.Send(buffer);
                            }
                        }
                        // FILE DELETE REQUEST
                        else if (filename1.Contains("**DE-"))
                        {
                            int user_end_index = filename1.IndexOf(":");
                            string filename = filename1.Substring(user_end_index + 1, filename1.Length - (user_end_index) - 1);
                            logs.AppendText("Request to delete file: " + filename + " from: " + User + "\n");
                            if (File.Exists(Permanent_Path + "\\" + User + "-" + filename))
                            {
                                File.Delete(Permanent_Path + "\\" + User + "-" + filename);

                                string delete = "";
                                //update database
                                foreach (string line in File.ReadLines(database))
                                {
                                    if (line.Contains(User + "-" + filename))
                                    {
                                        delete = line;
                                    }
                                }
                                var tempFile = Path.GetTempFileName();
                                var linesToKeep = File.ReadLines(database).Where(l => l != delete);

                                File.WriteAllLines(tempFile, linesToKeep);

                                File.Delete(database);
                                File.Move(tempFile, database);

                                logs.AppendText("File deleted successfully.\n");
                                Byte[] msg = Encoding.Default.GetBytes("delete_success");
                                thisClient.Send(msg);
                            }
                            else
                            {
                                logs.AppendText("Request from: " + User + " Declined!\n");
                                Byte[] err = Encoding.Default.GetBytes("file_error");
                                thisClient.Send(err);
                            }
                        }
                        // RECEIVE FILE
                        else if (filename1.Contains("**FL-"))
                        {
                            filename1 = filename1.Substring(5);
                            logs.AppendText(User + ": " + filename1 + "\n"); //create path
                            filename1 = "\\" + User + "-" + filename1;
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
                                        Byte[] buffer = new Byte[256];
                                        thisClient.Receive(buffer);
                                        chunk = Encoding.Default.GetString(buffer);
                                        if (chunk.IndexOf("\0") != -1)
                                        {
                                            end = true;
                                            chunk = chunk.Substring(0, chunk.IndexOf("\0"));
                                        }
                                        sw.Write(chunk);
                                    } while (!end);
                                }
                                using (StreamWriter sw = File.AppendText(database))
                                { //update database
                                    DateTime now = DateTime.Now;
                                    String fileName1 = filename1.Substring(1, filename1.Length - 1);
                                    sw.WriteLine(User + ":\t" + fileName1 + "\t" + now);
                                }
                                path = Permanent_Path;
                            }
                            else //filename is not unique
                            {
                                String permanent_path_without_ext = path.Substring(0, path.IndexOf(".txt"));
                                while (File.Exists(path)) // increment filename until filename is unique
                                {
                                    String path_without_ext = path.Substring(0, path.IndexOf(".txt"));
                                    path = permanent_path_without_ext + "(" + count.ToString() + ")" + ".txt";
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
                                        Byte[] buffer = new Byte[256];
                                        thisClient.Receive(buffer);
                                        chunk = Encoding.Default.GetString(buffer);
                                        if (chunk.IndexOf("\0") != -1)
                                        {
                                            end = true;
                                            chunk = chunk.Substring(0, chunk.IndexOf("\0"));
                                        }
                                        sw.Write(chunk);
                                    } while (!end);
                                }
                                using (StreamWriter sw = File.AppendText(database))
                                { // update database
                                    DateTime now = DateTime.Now;
                                    sw.WriteLine(User + ":\t" + filename2 + "\t" + now);
                                }
                                path = Permanent_Path;
                            }
                        }
                        else if(filename1 == "exp")
                        {
                            throw new Exception();
                        }
                    }
                }
                catch
                {
                    if (!terminating)
                    {
                        logs.AppendText(User + ": client has disconnected\n");
                    }
                    else if (terminating)
                    {
                        Byte[] err = Encoding.Default.GetBytes("fatal_error");
                        thisClient.Send(err);
                    }
                    
                    thisClient.Close();
                    clientsList.Remove(User);
                    connected = false;
                    
                }
            }
        }

        private void Server_FormClosing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            listening = false;
            terminating = true;
            Environment.Exit(0);
        }

        bool SocketConnected(Socket s) // for poll every 1000 msc to check if socket is connected
        {
            bool part1 = s.Poll(1000, SelectMode.SelectRead);
            bool part2 = (s.Available == 0);
            if (part1 && part2)
                return false;
            else
                return true;
        }

        private void sameUserNameHandler(Socket thisClient) 
        {
            logs.AppendText("There is already an user in that name!\n");
            String errorCode = "401";
            Byte[] errorbuffer = new Byte[64];
            errorbuffer = Encoding.Default.GetBytes(errorCode);
            try
            {
                thisClient.Send(errorbuffer);
            }
            catch
            {
                logs.AppendText("There is a problem! Check the connection...\n");
                terminating = true;
            }
        }
        
        private void button_browse_Click(object sender, EventArgs e)
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
            }
        }
    }
}
