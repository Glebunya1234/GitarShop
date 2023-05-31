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
    public partial class AddCharacteristic : Form
    {
         DB dB = new DB();
        private static int id_cat_prod { get; set; }
        private static string _name { get; set; }
        public AddCharacteristic(int id_category_prod)
        {
            InitializeComponent();
            id_cat_prod = id_category_prod;
        }

        private void AddCharacteristic_Load(object sender, EventArgs e)
        {
            button1.FlatAppearance.BorderSize = 0;
            button1.FlatStyle = FlatStyle.Flat;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text != String.Empty)
            {
                if (MessageBox.Show("Ви точно бажаєте додати?", " ", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    if (!IsGetCharact())// проверка на то есть ли уже зарактеристика
                    //если нет то добавит и найдет последнее айди если есть то найдет айди по имени 
                    {
                        dB.openConnection();
                        MySqlCommand cmd = new MySqlCommand($"INSERT INTO `charact_product` (`Name_charact`) VALUES ('{textBox1.Text}')", dB.GetConnection());
                        cmd.ExecuteNonQuery();
                        dB.closeConnection();
                    }
                    if (!ForCategory())
                    {
                        dB.openConnection();
                        MySqlCommand cmd11 = new MySqlCommand($"SELECT Id_charact from charact_product where Name_charact = '{textBox1.Text}'", dB.GetConnection());
                        cmd11.ExecuteNonQuery();
                        int id_charact_found = int.Parse(cmd11.ExecuteScalar().ToString());

                        MySqlCommand cmd22 = new MySqlCommand($"INSERT INTO `charact_category` (`Id_charact`, `id _prod_cat`) VALUES ('{id_charact_found}', '{Product.id_prod_cat}');", dB.GetConnection());
                        cmd22.ExecuteNonQuery();
                        dB.closeConnection();
                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show("Така характеристика вже є");
                    }

                }
            }
            else MessageBox.Show("Поле не може бути пустим");
        }
        public bool IsGetCharact()
        {
            DB dB = new DB();
            _name = textBox1.Text;
            DataTable _table = new DataTable();
            MySqlDataAdapter _adapter = new MySqlDataAdapter();
            MySqlCommand _checkadmin = new MySqlCommand($"SELECT * FROM charact_product where  (Name_charact = '{_name}')", dB.GetConnection());
            _adapter.SelectCommand = _checkadmin;
            _adapter.Fill(_table);
            if (_table.Rows.Count > 0)
            {
                return true;
            }
            else return false;
        }
       
        public bool ForCategory()
        {
            DB dB = new DB();
            _name = textBox1.Text;
            DataTable _table = new DataTable();
            MySqlDataAdapter _adapter = new MySqlDataAdapter();
            MySqlCommand _checkadmin = new MySqlCommand($"SELECT Name_charact FROM charact_product join charact_category using(Id_charact)  where  (`id _prod_cat`= {Product.id_prod_cat} and Name_charact = '{_name}')", dB.GetConnection());
            _adapter.SelectCommand = _checkadmin;
            _adapter.Fill(_table);
            if (_table.Rows.Count > 0)
            {
                return true;
            }
            else return false;
        }

        private void textBox1_MouseEnter(object sender, EventArgs e)
        {
            textBox1.BorderStyle = BorderStyle.Fixed3D;
        }

        private void textBox1_MouseLeave(object sender, EventArgs e)
        {
            textBox1.BorderStyle = BorderStyle.None;
        }
    }
}
