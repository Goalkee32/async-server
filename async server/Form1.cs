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
        public async void StartReceving()
        {
            try
            {
                client = await listener.AcceptTcpClientAsync();
            }
            catch( Exception error)
            {
                MessageBox.Show(error.Message, Text);
                return;
            }
            StartReading(client);
        }

        public async void StartReading( TcpClient k )
        {
            byte[] buff = new byte[1024];
            int n = 0;
            try
            {
                n = await k.GetStream().ReadAsync(buff, 0, buff.Length);
            }
            catch(Exception error) 
            { 
                MessageBox.Show(error.Message, Text);
            }

            textBox1.AppendText(Encoding.Unicode.GetString(buff, 0, n));
            textBox1.AppendText(Environment.NewLine);

            StartReading(k);
        }
    }

}
