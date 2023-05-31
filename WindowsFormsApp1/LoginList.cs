using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class LoginList : Form
    {
       
        DB dB = new DB();
        public LoginList()
        {
            InitializeComponent();     
        }

        private void LoginList_Load(object sender, EventArgs e)
        {
            CreatorCol();
            _RefreshDataGridView(dataGridView1);
            dataGridView1.Font = new Font("Century Gothic", 12, FontStyle.Bold);
            button1.FlatAppearance.BorderSize = 0;
            button1.FlatStyle = FlatStyle.Flat;
        }

        private void CreatorCol()
        {

            dataGridView1.Columns.Add("Fio", "ПІБ працівника");
            dataGridView1.Columns.Add("Login", "Логін");
            dataGridView1.Columns.Add("Passw", "Пароль");        
        }
        private void _DelleteCol()
        {

            dataGridView1.Columns.Remove("Fio");
            dataGridView1.Columns.Remove("Login");
            dataGridView1.Columns.Remove("Passw");
           
        }

        private void ReadRows(DataGridView datagr, IDataRecord record)
        {
            datagr.Rows.Add(record.GetString(0), record.GetString(1), record.GetString(2));
        }
        private void _RefreshDataGridView(DataGridView datagr)
        {
            datagr.Rows.Clear();
            string selectbd = $"select Emp_pib,login_user, password_user from register join employee using(Id_employee) where State = '1'";
            MySqlCommand mySqlCommand = new MySqlCommand(selectbd, dB.GetConnection());
            dB.openConnection();
            MySqlDataReader reader = mySqlCommand.ExecuteReader();
            while (reader.Read())
            {
                ReadRows(datagr, reader);

            }
            reader.Close();
            dB.closeConnection();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            _DelleteCol();
            this.Close();
            
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
