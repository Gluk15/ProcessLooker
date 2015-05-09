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
using System.IO;

namespace ProccesLooker
{
    public partial class Form1 : Form
    {
        System.Windows.Forms.Timer _timer;
        public int currproc = 0;
        public int timeLeft = 0;

        String connStr = @"Data Source =  192.168.1.39;
                            Initial Catalog = BD;
                            Integrated Security = False;
                            UID = sa;
                            PWD = qweasd;";
        Random rnd = new Random();




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
                String ip = System.Net.Dns.GetHostByName(System.Net.Dns.GetHostName()).AddressList[0].ToString();
                String date = DateTime.Now.ToString("dd.MM.yyyy");
                String id = ip.Substring(ip.Length - 3) + "." + date; 
                String name;
                SqlConnection conn = new SqlConnection(connStr);
                Process[] procList = UpdateProc();
                int ran = rnd.Next(10);


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
                                                "(ID, ip, name,StartDate, StartTime, EndTime) Values ('" + idrs + "', '" + ip + "', '" + pr.MainWindowTitle.ToString() + "', '" + pr.StartTime.ToString(@"dd.MM.yyyy") + "', '" + pr.StartTime.ToString(@"HH:mm:ss") + "', '" + DateTime.Now.ToString() + "')" +
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

                    if (ran>7)
                    {
                        var bmp = new Bitmap(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height);
                        Graphics graph = null;
                        graph = Graphics.FromImage(bmp);
                        graph.CopyFromScreen(0, 0, 0, 0, bmp.Size);
                        name = id +DateTime.Now.ToString(".hh.mm.ss")+".jpg";
                        

                        byte[] photo = GetPhoto(name);

                        SqlCommand command = new SqlCommand(
                                                            "INSERT INTO BD.dbo.img (id, ip, " +
                                                            "date, time, img) " +
                                                            "Values(@id, @ip, @date, " +
                                                            "@time, @img)", conn);

                        command.Parameters.Add("@id",
                           SqlDbType.NVarChar, 50).Value = name;
                        command.Parameters.Add("@ip",
                            SqlDbType.NVarChar, 50).Value = ip;
                        command.Parameters.Add("@date",
                            SqlDbType.NVarChar, 50).Value = date;
                        command.Parameters.Add("@time",
                             SqlDbType.NVarChar, 50).Value = DateTime.Now.ToString("hh.mm.ss");

                        command.Parameters.Add("@img",
                            SqlDbType.Image, photo.Length).Value = photo;

                        command.ExecuteNonQuery();

                    }
                    conn.Close();
                    conn.Dispose();
               
                

                
            }
        public static byte[] GetPhoto(string filePath)
        {
            FileStream stream = new FileStream(
                filePath, FileMode.Open, FileAccess.Read);
            BinaryReader reader = new BinaryReader(stream);

            byte[] photo = reader.ReadBytes((int)stream.Length);

            reader.Close();
            stream.Close();

            return photo;
        }
        }

        
    

    }


