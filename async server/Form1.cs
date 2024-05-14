using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;

namespace async_server
{
    public partial class Form1 : Form
    {
        TcpListener listener;
        TcpClient client;
        int port = 12345;
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                listener = new TcpListener(IPAddress.Any, port);
                listener.Start();
            }
            catch (Exception error)
            {
                MessageBox.Show(error.Message, Text);
                return;
            }

            button1.Enabled = false;
            
            StartReceving();
            
        }

        public async Task StartReceving()
        {
                while (true)
                {
                    client = await listener.AcceptTcpClientAsync();
                    HandleClient(client);
                }
        }

        private async void HandleClient(TcpClient client)
        {
            byte[] buffer = new byte[1024];
            int n;

            try
            {
                while ((n = await client.GetStream().ReadAsync(buffer, 0, buffer.Length)) != 0)
                {
                    string message = Encoding.Unicode.GetString(buffer, 0, n);
                    LogMessage(message);
                }
            }
            catch (Exception error) { MessageBox.Show(error.Message, Text); }
            client.Close();
        }

        private void LogMessage(string message) 
        {
            if (textBox1.InvokeRequired)
            {
                textBox1.Invoke(new MethodInvoker(delegate { textBox1.AppendText(message + Environment.NewLine); }));
            }
            else 
            {
                textBox1.AppendText(message + Environment.NewLine);
            }
        }

        public async void StartReading(TcpClient k)
        {
            byte[] buffer = new byte[1024];
            int n = 0;
            try
            {
                n = await k.GetStream().ReadAsync(buffer, 0, buffer.Length);
            }
            catch (Exception error) { MessageBox.Show(error.Message, Text); return; }
            textBox1.AppendText(Encoding.Unicode.GetString(buffer, 0, n));

            StartReading(k);
        }
    }

}
