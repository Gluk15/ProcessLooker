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
using MySql.Data.MySqlClient;
using System.Data.SqlClient;

namespace ProccesLooker
{
    public partial class Form1 : Form
    {
        System.Windows.Forms.Timer _timer;
        public int currproc = 0;
        public Timer tmrShow;
        public int timeLeft = 0;

        String connStr = @"Data Source = \\.\pipe\MSSQL$SQLEXPRESS\sql\query;
                            Initial Catalog = BD;
                            Integrated Security = False;
                            UID = root;
                            PWD = qweasd;";




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


        public void Timer_Tick(object sender, EventArgs e)
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
                String ip = System.Net.Dns.GetHostByName(System.Net.Dns.GetHostName()).AddressList[0].ToString();
                String date = DateTime.Now.ToString(".yyyy.dd.MM");
                String id = ip.Substring(ip.Length - 3) + "." + date; 
                SqlConnection conn = new SqlConnection(connStr);
                Process[] procList = UpdateProc();


                try
                {
                    conn.Open();
                    
                }
                catch (SqlException se)
                {
                    
                    throw;
                }

               
                SqlCommand cmd = new SqlCommand();

                    foreach (Process pr in procList)
                    {
                        if (pr.MainWindowTitle != "")
                        {
                            string idrs = id + "." + pr.Id.ToString();

                            cmd = new SqlCommand("IF (SELECT COUNT(*) FROM BD.dbo.main WHERE id = '"+ idrs+ "') > '0'" +
                                                "BEGIN " +
                                                    " UPDATE BD.dbo.main" +
                                                    " SET EndTime = '" + DateTime.Now.ToString() + "'" +
	                                                " WHERE (id = '" + idrs + "')"+
                                                " END"+
                                                " ELSE"+
                                                " BEGIN"+
                                                " Insert into main" +
                                                "(ID, ip, name, StartTime, EndTime) Values ('" + idrs + "', '" + ip + "', '" + pr.MainWindowTitle.ToString() + "', '" + pr.StartTime + "', '" + DateTime.Now.ToString() + "')"+
                                                "END" , conn);

                            try
                            {
                                cmd.ExecuteNonQuery();
                            }
                            catch
                            {

                                throw;
                            }

                        }
                        
                    }


                    conn.Close();
                    conn.Dispose();
               
                
                timeLeft = 30;
                
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }


    }
}
