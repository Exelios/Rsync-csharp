using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace WFA_preTPI_dougoudxa_networkchat
{
    public partial class udpClientForm : Form
    {
        //Declaration of the client attributes.
        private UdpClient g_client;
        private IPEndPoint IPClient = null;

        private bool g_continue;
        private Thread g_thListener;

        /// <summary>
        /// Constructor of the udpClientForm
        /// </summary>
        public udpClientForm()
        {
            InitializeComponent();

            //We automatically create the client that will send messages to the server.
            g_client = new UdpClient();
            g_client.Connect("127.0.0.1", 4000);

            //Initialization of the client attributes. Starting the listening thread.
            g_continue = true;
            g_thListener = new Thread(new ThreadStart(ThreadListener));
            g_thListener.Start();
        }

        /// <summary>
        /// Method designed to listen as a separate thread.
        /// </summary>
        private void ThreadListener()
        {
            //Decleration of the Socket.
            UdpClient listener = null;

            //Socket creation.
            try
            {
                listener = new UdpClient(4000);
            }
            catch
            {
                MessageBox.Show("Impossible to connect to UDP port 4000. Check your network settings.");
                return;
            }

            //Timeout definition.
            listener.Client.ReceiveTimeout = 1000;

            //Infinite loop listening to the port.
            while (g_continue)
            {
                try
                {
                    
                    byte[] data = listener.Receive(ref IPClient);

                    //Use of the AddLogEntry to write messages in the readTextBox
                    this.Invoke(new Action<string>(AddLogEntry), Encoding.Default.GetString(data));
                }
                catch
                {
                    //Nothing
                }
            }
            listener.Close();
        }

        /// <summary>
        /// Method adding text to the client log.
        /// </summary>
        /// <param name="data"></param>
        private void AddLogEntry(string data)
        {
            readTextBox.AppendText("\r\nMessage from " + IPClient.Address + ":" + IPClient.Port + " > " + data);
        }

        /// <summary>
        /// Manages when a message is sent. No need for an additionnal thread.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonSend_Click(object sender, EventArgs e)
        {
            byte[] data = Encoding.Default.GetBytes(writeTextBox.Text);
            g_client.Send(data, data.Length);

            writeTextBox.Clear();
            writeTextBox.Focus();
        }

        /// <summary>
        /// Manages the form closing process. We stop listening, close the form and wait for the listening thread to stop.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void udpClientForm_FormClosing_1(object sender, FormClosingEventArgs e)
        {
            g_continue = false;
            g_client.Close();
            g_thListener.Join();
        }

        /// <summary>
        /// Sends a message when we press enter.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void writeTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if(e.KeyChar == (char)Keys.Enter)
            {
                buttonSend.PerformClick();
            }
        }
    }
}
