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
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            int update = 1000;
            List<Procc> listProcc = new List<Procc>();
            Process[] procList = Process.GetProcesses();
            foreach (Process pr in procList)
            {
                if (pr.MainWindowTitle!="")
                {
                    listProcc.Add(new Procc(pr.Id, pr.MainWindowTitle, pr.StartTime));
                }
            }
 
		 listBox1.DataSource = listProcc;

            

        }
    }
}
