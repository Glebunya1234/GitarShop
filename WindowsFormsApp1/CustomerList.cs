using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ToolBar;
using TextBox = System.Windows.Forms.TextBox;

namespace WindowsFormsApp1
{
    public partial class CustomerList : Form
    {
        Form _Formlast;
        DB dB = new DB();

       
      

        public CustomerList(Form formlast)
        {
            InitializeComponent();
            _Formlast = formlast;
        }

        private void CustomerList_Load(object sender, EventArgs e)
        {
            #region Стиль кнопок
            button2.FlatAppearance.BorderSize = 0;
            button2.FlatStyle = FlatStyle.Flat;
            #endregion
            dataGridView1.Font = new Font("Comic Sans MS", 8, FontStyle.Bold);

           

            CreatorCol();
            _RefreshDataGridView(dataGridView1);
        }

        private void CustomerList_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            _DelleteCol();
            this.Hide();
            _Formlast.Show();
        }

        private void CreatorCol() 
        {
            
            dataGridView1.Columns.Add("Cust_tel", "Номер телефона");
            dataGridView1.Columns.Add("Cust_name", "Имя");
            dataGridView1.Columns.Add("Cust_sur_perent", "Фамилия");
            dataGridView1.Columns.Add("Cust_birth", "Дата рождения");
        }
        private void _DelleteCol()
        {
          
            dataGridView1.Columns.Remove("Cust_tel");
            dataGridView1.Columns.Remove("Cust_name");
            dataGridView1.Columns.Remove("Cust_sur_perent"); 
            dataGridView1.Columns.Remove("Cust_birth");
        }

        private void ReadRows(DataGridView datagr, IDataRecord record)
        {
            datagr.Rows.Add(record.GetString(1), record.GetString(2), record.GetString(3), record.GetString(4));
        }
        private void _RefreshDataGridView(DataGridView datagr)
        {
            datagr.Rows.Clear();
            string selectbd = $"select * from `customer`";
            MySqlCommand mySqlCommand = new MySqlCommand(selectbd, dB.GetConnection());
            dB.openConnection();
            MySqlDataReader reader = mySqlCommand.ExecuteReader();
            while (reader.Read()) 
            {
                ReadRows(datagr, reader);

            }
            reader.Close();
        }

    }

}
