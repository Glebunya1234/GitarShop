using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace WindowsFormsApp1
{
    public partial class Charact : Form
    {
        public static string name_prodCB { get; set; }
        public static int id_prod { get; set; } = 1;
        DB dB = new DB();
        public Charact()
        {
            InitializeComponent();  
        }
        private void Charact_Load(object sender, EventArgs e)
        {
            dataGridView1.Font = new Font("Century Gothic", 14, FontStyle.Bold);
            button1.FlatAppearance.BorderSize = 0;
            button1.FlatStyle = FlatStyle.Flat;
            button2.FlatAppearance.BorderSize = 0;
            button2.FlatStyle = FlatStyle.Flat;
            comboBox1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            _CreatorCol();
            _RefreshDataGridView(dataGridView1);
            _SetCombobox();
            comboBox1.SelectedIndex = 0;
        }
        private void _CreatorCol()
        {
            dataGridView1.Columns.Add("Name_charact", "Назва характеристики");
            dataGridView1.Columns.Add("Charact_value", "Значення");
        
        }


        /// <summary>
        /// Чтение строк
        /// </summary>
        private void _ReadRows(DataGridView datagr, IDataRecord record)
        {
            datagr.Rows.Add(record.GetString(0), record.GetString(1));
        }


        /// <summary>
        /// Обновление дата грид
        /// </summary>
        private void _RefreshDataGridView(DataGridView datagr)
        {
            datagr.Rows.Clear();
            string selectbd = $"select Name_charact, Charact_value from charact_product join charact_category using (id_charact) join charact_goods using (id_charact_cat) join product using (id_product) where Id_product = '{id_prod}';";
            MySqlCommand mySqlCommand = new MySqlCommand(selectbd, dB.GetConnection());
            dB.openConnection();
            MySqlDataReader reader = mySqlCommand.ExecuteReader();
            while (reader.Read())
            {
                _ReadRows(datagr, reader);
            }
            reader.Close();
            dB.closeConnection();

        }

        

        private void Charact_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.Hide();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            _RefreshDataGridView(dataGridView1);
        }
       
        public void _SetCombobox()
        {
            dB.openConnection();
            DataTable _table = new DataTable();
            MySqlDataAdapter _adapter = new MySqlDataAdapter();

            MySqlCommand cmd = new MySqlCommand("Select `Product_name` from product where State = '1'", dB.GetConnection());
            MySqlDataReader my;
            my = cmd.ExecuteReader();
            while(my.Read())
            {
                string sname = my.GetString("Product_name");
                comboBox1.Items.Add(sname);

            }
            dB.closeConnection();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            name_prodCB = comboBox1.Text;
            dB.openConnection();
            MySqlCommand comand = new MySqlCommand($"Select `Id_product` from `product` where (Product_name = '{name_prodCB}' and State = '1');", dB.GetConnection());
            comand.ExecuteNonQuery();
            id_prod = (int)comand.ExecuteScalar();
            dB.closeConnection();
            _RefreshDataGridView(dataGridView1);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
