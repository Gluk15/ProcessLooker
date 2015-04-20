using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;

namespace ProccesLooker
{
    public partial class Form1 : Form
    {
        System.Windows.Forms.Timer _timer;
        public int currproc = 0;
        public Timer tmrShow;
        public int timeLeft = 0;
        


        public Form1()
        {
            InitializeComponent();
            _timer = new System.Windows.Forms.Timer();
            _timer.Tick += new EventHandler(Timer_Tick);
            _timer.Interval = 500;
            _timer.Enabled = true;
        }
        

        //Возвращает обновленный массив процессов.
        public Process[] UpdateProc()
        {
             Process[] proceList = Process.GetProcesses();
             return proceList;
        }


        private void Timer_Tick(object sender, EventArgs e)
        {
            
            if (timeLeft > 0)
            {
                timeLeft = timeLeft - 1;
                int minutes = timeLeft / 60;
                int seconds = timeLeft - (minutes * 60);
                if (seconds < 10)
                {
                    lblTimer.Text = minutes + ":0" + seconds;
                }
                else
                {
                    lblTimer.Text = minutes + ":" + seconds;
                }
            }
            else
            {
                    List<Procc> listProcc = new List<Procc>();
                    String host = System.Net.Dns.GetHostName();
                    System.Net.IPAddress ip = System.Net.Dns.GetHostByName(host).AddressList[0];
                    PCinfo pc = new PCinfo(host, ip.ToString());
                    currproc = 0;
                    Process[] procList = UpdateProc();
                    foreach (Process pr in procList)
                    {
                        if (pr.MainWindowTitle != "")
                        {
                            currproc++;
                            listProcc.Add(new Procc(pr.Id, pr.MainWindowTitle, pr.StartTime));
                        }
                    }
                    label1.Text = pc.ToString();
                    listBox1.DataSource = listProcc;
                    timeLeft = 19;
                
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }


    }
}
