using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO.Ports;

namespace RS485Test
{
    public partial class Form1 : Form
    {
        public static Form1 fm;
        public Form1()
        {
            fm = this;
            InitializeComponent();
        }
        delegate void SetRichTextValue(object sender, SerialDataReceivedEventArgs e);
        private void button1_Click(object sender, EventArgs e)
        {
            SerialPort sp = new SerialPort();
            sp.PortName = textBox1.Text.ToString().Trim();
            sp.Open();
            // check port is open or not
            if (sp.IsOpen == true)
                Console.WriteLine("Port is open");
            // set the port parameters
            sp.BaudRate = 9600;

            sp.DtrEnable = true;
            sp.Handshake = Handshake.None;
            sp.Parity = Parity.None;
            sp.StopBits = StopBits.One;
            sp.DataBits = 8;
            // write some command on port
            //sp.WriteLine("hello baby");
            // make DataReceived event handler
            sp.DataReceived += new SerialDataReceivedEventHandler(sp_datarecived);

           

        }
        public static void sp_datarecived(object sender, SerialDataReceivedEventArgs e)
        {
           
            SerialPort sPort = (SerialPort)sender;
            //String data = sPort.ReadExisting();
            int readByte = sPort.BytesToRead;
            byte[] buffer = new byte[readByte];

            try
            {
                sPort.Read(buffer, 0, readByte);
            }
            catch (Exception ex) {
                readByte = 0;
            }
            foreach (byte ch in buffer)
            {
                if (fm.richTextBox1.InvokeRequired)
                {

                    SetRichTextValue st = new SetRichTextValue(sp_datarecived);
                    fm.Invoke(st, new object[] { sender, e });

                }
                else {
                    fm.richTextBox1.Text += String.Format("{0:X2}",ch)+" ";
                }
               
            }
        }


    }
}
