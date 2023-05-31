using MySql.Data.MySqlClient;
using Mysqlx.Crud;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.AxHost;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using TextBox = System.Windows.Forms.TextBox;

namespace WindowsFormsApp1
{
    public partial class AddProd : Form
    {
        DB dB = new DB();
        Form _Next;
        Form _Back;
        public string name_prodCB { get; private set; }
        public static int id_prodcat { get; private set; }
        public static int id_prod { get; set; }

        public AddProd(Form back)
        {
            InitializeComponent();

            _Back = back;
        }

        private void AddProd_Load(object sender, EventArgs e)
        {
            _SetComboBox();

            comboBox3.SelectedIndex = 0;
            comboBox2.SelectedIndex = 0;
            comboBox1.SelectedIndex = 0;

            comboBox1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            comboBox2.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            comboBox3.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;

            button1.FlatAppearance.BorderSize = 0;
            button1.FlatStyle = FlatStyle.Flat;
            button2.FlatAppearance.BorderSize = 0;
            button2.FlatStyle = FlatStyle.Flat;

            textBox2.KeyPress += new KeyPressEventHandler(textBox2_KeyPress);
            



        }
        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            name_prodCB = comboBox2.Text;
            dB.openConnection();
            MySqlCommand cmd = new MySqlCommand($"select `id _prod_cat` from `category_product` where(`Name_cat` = '{name_prodCB}') ", dB.GetConnection());
            cmd.ExecuteNonQuery();
            id_prodcat = int.Parse(cmd.ExecuteScalar().ToString());
            dB.closeConnection();


        }
        private void textBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }
        private void AddProd_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }


        #region MyButtons
        private void button2_Click(object sender, EventArgs e)
        {
            
            if (textBox1.Text == String.Empty || textBox2.Text == String.Empty || textBox4.Text == String.Empty)
            {
                MessageBox.Show("Заповніть поля");
                return;
            }
            if (MessageBox.Show("Ви точно бажаєте додати товар?", "", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                    if (!IsGetProduct())
                    {

                        string _prodname = textBox1.Text;
                        double _price = double.Parse(textBox2.Text);

                        string _country = textBox4.Text;
                        string _orient = comboBox1.Text;

                        dB.openConnection();
                        MySqlCommand cmd = new MySqlCommand($"INSERT INTO`product` (`id _prod_cat`, `Product_name`, `Prod_amount`, `Prod_price`, `Prod_country`, `Prod_orient`, `State`) VALUES('{id_prodcat}', '{_prodname}', '0', '{_price}', '{_country}', '{_orient}', '1')", dB.GetConnection());
                        cmd.ExecuteNonQuery();
                        dB.closeConnection();
                        dB.openConnection();
                        MySqlCommand cmd1 = new MySqlCommand($"select `Id_product` from product where `Product_name` = '{_prodname}'", dB.GetConnection());
                        cmd1.ExecuteNonQuery();
                        id_prod = int.Parse(cmd1.ExecuteScalar().ToString());
                        dB.closeConnection();
                        _Dellcombobox();
                        MessageBox.Show("Ви успішно додали товар");
                        this.Hide();
                        _Back.Show();
                    }
                    else MessageBox.Show("Гітара с таким іменем вже є");
                }
              
           
        }
        private void button1_Click(object sender, EventArgs e)
        {
            _Dellcombobox();
            this.Hide();
            _Back.Show();
        }

        #endregion

        #region MyMethod
        private void _SetComboBox()
        {
            #region Combobox2
            dB.openConnection();
            MySqlCommand cmd1 = new MySqlCommand("Select `Name_cat` from category_product", dB.GetConnection());
            MySqlDataReader my1;
            my1 = cmd1.ExecuteReader();
            while (my1.Read())
            {
                string sname2 = my1.GetString("Name_cat");
                comboBox2.Items.Add(sname2);
            }
            dB.closeConnection();
            #endregion


            #region Combobox3
            dB.openConnection();
            MySqlCommand cmd2 = new MySqlCommand("Select `Supl_name_firm` from supplier where State = 1", dB.GetConnection());
            MySqlDataReader my2;
            my2 = cmd2.ExecuteReader();
            while (my2.Read())
            {
                string sname = my2.GetString("Supl_name_firm");
                comboBox3.Items.Add(sname);
            }
            dB.closeConnection();
            #endregion

        }

        private void _Dellcombobox()
        {
            comboBox2.Items.Clear();
            comboBox3.Items.Clear();
            textBox1.Text = String.Empty;
            textBox2.Text = String.Empty;
            
            textBox4.Text = String.Empty;
        }

        private bool IsGetProduct()
        {
            DB dB = new DB();
            string _name = textBox1.Text;
            DataTable _table = new DataTable();
            MySqlDataAdapter _adapter = new MySqlDataAdapter();
            MySqlCommand _checkadmin = new MySqlCommand($"SELECT Product_name FROM product where Product_name = '{_name}'", dB.GetConnection());
            _adapter.SelectCommand = _checkadmin;
            _adapter.Fill(_table);
            if (_table.Rows.Count > 0)
            {
                return true;
            }
            else return false;
        }


        #endregion


        #region Animation

        private void textBox1_MouseLeave(object sender, EventArgs e)
        {
            textBox1.BorderStyle = BorderStyle.None;
        }

        private void textBox1_MouseEnter(object sender, EventArgs e)
        {
            textBox1.BorderStyle = BorderStyle.Fixed3D;
        }

        private void textBox4_MouseEnter(object sender, EventArgs e)
        {
            textBox4.BorderStyle = BorderStyle.Fixed3D;
        }

        private void textBox4_MouseLeave(object sender, EventArgs e)
        {
            textBox4.BorderStyle = BorderStyle.None;
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

     
       

        private void textBox4_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (char.IsLetter(e.KeyChar) == false && e.KeyChar != (char)Keys.Back)
            {
                e.Handled = true;
            }
        }
    }
}
