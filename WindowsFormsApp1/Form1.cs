using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        public static string _logi { get; set; }
        public static string _passw { get; set; }
        public static bool fl { get; set; } = true;

        Form _Form2_For_Admin;
        Form _Product;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {          
            button1.FlatAppearance.BorderSize = 0;
            button1.FlatStyle = FlatStyle.Flat;
            textBox1.BorderStyle = BorderStyle.None;
            textBox2.BorderStyle = BorderStyle.None;
            label2.Parent = pictureBox2;
            label2.BackColor = Color.Transparent;
            label3.Parent = pictureBox2;
            label3.BackColor = Color.Transparent;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            _logi = textBox1.Text;
            _passw = textBox2.Text;
            #region Проверка на пользователя
            try
            {
                DB dB = new DB();

                DataTable table = new DataTable();

                MySqlDataAdapter adapter = new MySqlDataAdapter();

                MySqlCommand mySqlCommand = new MySqlCommand($"SELECT * From `register` where `login_user` = '{_logi}' AND `password_user` = '{_passw}'", dB.GetConnection());

                adapter.SelectCommand = mySqlCommand;

                adapter.Fill(table);

                if (table.Rows.Count > 0)
                {  
                    if (IsGetAministation()) 
                    {
                      this.Hide();
                      _Form2_For_Admin ??= new Form2_for_admin(this);
                      _Form2_For_Admin.ShowDialog();
                    }
                    else if (IsGetEmplyee()) 
                    {
                      this.Hide();
                      _Product ??= new Product(this);
                      _Product.ShowDialog(); 
                    }
                }
                else MessageBox.Show("Щось пішло не так!");
            }
            catch { MessageBox.Show("Користувач не знайден!"); }
            #endregion
        }
        public static string getUser() { return _logi ; }

        public static string getPassword() { return _passw; }

        public bool IsGetAministation() 
        {
            DB dB = new DB();

            DataTable _table = new DataTable();
            MySqlDataAdapter _adapter = new MySqlDataAdapter();
            MySqlCommand _checkadmin = new MySqlCommand($"SELECT * From `employee` join register using (Id_employee) where `Emp_position` = 'Адміністратор' and login_user = '{_logi}'", dB.GetConnection());
            _adapter.SelectCommand = _checkadmin;
            _adapter.Fill(_table);
            if (_table.Rows.Count > 0)
            {
                return true;
            }
            else return false;
        }

        public bool IsGetEmplyee()
        {
            DB dB = new DB();

            DataTable _table = new DataTable();
            MySqlDataAdapter _adapter = new MySqlDataAdapter();
            MySqlCommand _checkadmin = new MySqlCommand($"SELECT * From `employee` join register using (Id_employee) where `Emp_position` = 'Співробітник' and login_user = '{_logi}'", dB.GetConnection());
            _adapter.SelectCommand = _checkadmin;
            _adapter.Fill(_table);
            if (_table.Rows.Count > 0)
            {
                return true;
            }
            else return false;
        }

        #region animation
        private void pictureBox1_Click(object sender, EventArgs e)
        {
            if(fl == true) 
            {
                textBox2.UseSystemPasswordChar = false;
                fl = false;          
            }
            else if(fl == false)
            {
                textBox2.UseSystemPasswordChar = true;
                fl = true;
            }
        }

        private void textBox1_MouseEnter(object sender, EventArgs e)
        {
            textBox1.BorderStyle = BorderStyle.Fixed3D;
 
        }

        private void textBox1_MouseLeave(object sender, EventArgs e)
        {
            textBox1.BorderStyle = BorderStyle.None;      
        }

        private void textBox2_MouseEnter(object sender, EventArgs e)
        {
            textBox2.BorderStyle = BorderStyle.Fixed3D;
        }

        private void textBox2_MouseLeave(object sender, EventArgs e)
        {
            textBox2.BorderStyle = BorderStyle.None;
        }
        #endregion

    }
}
