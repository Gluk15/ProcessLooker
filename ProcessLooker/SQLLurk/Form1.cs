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
	        }else
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
                name = "'"+comboBox1.SelectedText.ToString()+"'";
            }

            try
            {
                conn.Open();

            }
            catch (SqlException)
            {

                label1.Text = "Ошибка подключения";
            }

            SqlDataAdapter da = new SqlDataAdapter(@"SELECT " + help + "main.* FROM dbo.main, dbo.pc  WHERE main.ip=pc.ip AND pc.Name = " + name + " AND main.StartDate LIKE '" + stime +"'", conn);
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
    }
}
