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

namespace ProccesLooker
{
    public partial class Form1 : Form
    {
        System.Windows.Forms.Timer _timer;
        public int currproc = 0;
        public Timer tmrShow;
        public int timeLeft = 0;

        String tname = DateTime.Today.ToString("d-MMM-yyyy") + ":" + System.Net.Dns.GetHostByName(System.Net.Dns.GetHostName()).AddressList[0].ToString().Replace('.', '-');

         



        public DataTable createtable()
        {
            System.Data.DataTable datatab = new DataTable(DateTime.Today.ToString("d") + System.Net.Dns.GetHostByName(System.Net.Dns.GetHostName()).AddressList[0].ToString());       

            datatab.Columns.Add(new DataColumn("id", System.Type.GetType("System.Int32")));
            datatab.Columns.Add(new DataColumn("name", System.Type.GetType("System.String")));
            datatab.Columns.Add(new DataColumn("startTime", System.Type.GetType("System.String")));
            datatab.Columns.Add(new DataColumn("endTime", System.Type.GetType("System.String")));
            datatab.Columns.Add(new DataColumn("ip", System.Type.GetType("System.String")));

            DataColumn[] PrimaryKeyColumns = new DataColumn[1];
            PrimaryKeyColumns[0] = datatab.Columns["id"];
            datatab.PrimaryKey = PrimaryKeyColumns;

            return datatab;
        }


        public Form1()
        {
            InitializeComponent();
            string connect = "Server=127.0.0.1;" +
                                    "Database=plt;" +
                                    "Uid=root;" +
                                    "Pwd=1234;" +
                                    "CharSet = cp1251;";

            MySqlConnection con = new MySqlConnection(connect);
            MySqlCommand com = new MySqlCommand();
            com.Connection = con;

            try
            {
                con.Open();
                com.CommandText = @"CREATE TABLE IF NOT EXISTS `" + tname + "`  ( `id` MEDIUMINT NOT NULL, `name` TEXT NOT NULL, `StartTime` TEXT NOT NULL, `EndTime` TEXT NOT NULL, `ip` TEXT NOT NULL, PRIMARY KEY (`id`))COLLATE='utf8_general_ci' ENGINE=InnoDB;";
                com.ExecuteReader();



            }
            catch (MySqlException SSDB_Exception)
            {

                MessageBox.Show("Ошибка при создании таблицы " + tname + ":\n" + SSDB_Exception.Message);
            }
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
                DataRow row;
                DataSet DS = new DataSet();
                DataTable st = new DataTable();
                DataTable table = createtable();
                
                System.Net.IPAddress ip = System.Net.Dns.GetHostByName(System.Net.Dns.GetHostName()).AddressList[0];
                

                Process[] procList = UpdateProc();

               



                foreach (Process pr in procList)
                {
                    if (pr.MainWindowTitle != "")
                    {
                        row = table.NewRow();
                        row["id"] = pr.Id;
                        row["name"] = pr.MainWindowTitle;
                        row["startTime"] = pr.StartTime;
                        row["endTime"] = DateTime.Now.Subtract(pr.StartTime).ToString(@"d\.hh\:mm\:ss");
                        row["ip"] = ip.ToString();
                        table.Rows.Add(row);
                    }
                }




                
                   
                label1.Text = ip.ToString();
                DS.Tables.Add(table);
                dataGridView1.DataSource = DS.Tables[0];
                timeLeft = 30;
                
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }


    }
}
