using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.IO;

namespace SQLLurk
{
    public partial class Form1 : Form
    {
        String connStr = @"Data Source =  192.168.1.39;
                            Initial Catalog = BD;
                            Integrated Security = False;
                            UID = sa;
                            PWD = qweasd;";




        public Form1()
        {
            InitializeComponent();
            SqlConnection conn = new SqlConnection(connStr);
            try
            {
                conn.Open();
            }
            catch (SqlException se)
            {
                throw;
            }
            SqlDataReader reader;

            SqlCommand cmd = new SqlCommand("SELECT * FROM pc");
            cmd.Connection = conn;
            using (reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    var MyString = reader.GetString(1);
                    comboBox1.Items.Add(MyString);
                }
            }
            conn.Close();
        }



        private void button1_Click(object sender, EventArgs e)
        {
            SqlConnection conn = new SqlConnection(connStr);
            DateTime time = dateTimePicker1.Value;
            string stime;
            if (checkBox1.Checked)
            {
                stime = "%" + time.ToString(@".MM.") + "%";
            }
            else
            {
                stime = time.ToString(@"dd.MM.yyyy");
            }
            String name = comboBox1.SelectedText;
            String help = "";
            if (name == "")
            {
                name = "pc.Name";
                help = "pc.Name, ";
            }
            else
            {
                name = "'" + comboBox1.SelectedText.ToString() + "'";
            }

            try
            {
                conn.Open();

            }
            catch (SqlException)
            {

                label1.Text = "Ошибка подключения";
            }

            SqlDataAdapter da = new SqlDataAdapter(@"SELECT " + help + "main.* FROM dbo.main, dbo.pc  WHERE main.ip=pc.ip AND pc.Name = " + name + " AND main.StartDate LIKE '" + stime + "'", conn);
            DataTable dt = new DataTable();
            da.Fill(dt);
            dataGridView1.DataSource = dt;
            conn.Close();


        }

        private void button2_Click(object sender, EventArgs e)
        {
            comboBox1.SelectedItem = null;
            checkBox1.CheckState = CheckState.Unchecked;
            dataGridView1.DataSource = null;
        }

        private void printDocument1_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            Bitmap bmp = new Bitmap(dataGridView1.Size.Width + 10, dataGridView1.Size.Height + 10);
            dataGridView1.DrawToBitmap(bmp, dataGridView1.Bounds);
            e.Graphics.DrawImage(bmp, 0, 0);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            printDocument1.Print();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            MessageBox.Show("*Открыть - показывает журнал запущенных приложений за выбранный день, за месяц, если поставили соответсвующую галочку.\n" +
            "*Скриншоты - скачивает в корневую папку скриншоты выбранного сотрудника за выбранный день, если не выбран сотрудник то скачаются все скриншоты за день.\n" +
            "*Печать - выводит в печать запрошенную таблицу.\n" +
            "*Отчистить - возвращает все поля в изначальные позиции.");
        }

        private void button4_Click(object sender, EventArgs e)
        {
            SqlConnection conn = new SqlConnection(connStr);
            DateTime time = dateTimePicker1.Value;
            string stime;
            stime = time.ToString(@"yyyy.dd.MM");
        
            String name = comboBox1.SelectedText;
            String help = "";
            if (name == "")
            {
                name = "pc.Name";
                help = "pc.Name, ";
            }
            else
            {
                name = "'" + comboBox1.SelectedText.ToString() + "'";
            }

            try
            {
                conn.Open();

            }
            catch (SqlException)
            {

                label1.Text = "Ошибка подключения";
            }
            System.IO.Directory.CreateDirectory("ScreenShots");
            SqlDataAdapter da = new SqlDataAdapter(@"SELECT " + help + " img.* FROM BD.dbo.img, BD.dbo.pc  WHERE img.ip=pc.ip AND pc.Name = " + name + " AND img.date LIKE '" + stime + "'", conn);
            DataSet dt = new DataSet();
            da.Fill(dt);
            Byte[] data = new Byte[0];
            String aId;
            String aIp;
            for (int i = 0; i < dt.Tables[0].Rows.Count; i++)
            {
                data = (Byte[])(dt.Tables[0].Rows[i]["img"]);
                aId = dt.Tables[0].Rows[i]["id"].ToString();
                aIp = dt.Tables[0].Rows[i]["ip"].ToString();
                System.IO.Directory.CreateDirectory("ScreenShots\\"+aIp);
                MemoryStream mem = new MemoryStream(data);
                var img = Image.FromStream(mem);
                img.Save(Application.StartupPath +@"\\ScreenShots\\"+ aIp+"\\"+aId+ ".bmp");
                img.Dispose();
            }
            conn.Close();
        }


    }
}
