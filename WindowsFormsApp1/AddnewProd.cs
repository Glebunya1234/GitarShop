using MySql.Data.MySqlClient;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlTypes;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Input;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace WindowsFormsApp1
{
    public partial class AddnewProd : Form
    {
        DB dB = new DB();
        Form last;
        Form _AddCharact;
        private static List<listCharact> _list = new List<listCharact>();
        private static string name_charactCB { get; set; }
        private static int id_charactcat { get; set; }
        private static int id_prod_cat { get; set; }
        public static int indexCombobox3 { get; private set; }
        public static string fromdatagridclick { get; private set; }
        private static int RowSelecct { get; set; }

        public AddnewProd(Form last)
        {
            InitializeComponent();
            this.last = last;   
        }

        private void AddnewProd_Load(object sender, EventArgs e)
        {
            
            label2.Text = Product._nameprod.Length > 25 ? String.Format("Обран товар:\n{0:0.##}...", Product._nameprod.Substring(0, 25)) : $"Обран товар::\n{Product._nameprod}";
            _SetComboBox();
            comboBox3.SelectedIndex = 0;
            dataGridView1.Font = new Font("Century Gothic", 12, FontStyle.Bold);
            comboBox3.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;


            button1.FlatAppearance.BorderSize = 0;
            button1.FlatStyle = FlatStyle.Flat;
            button2.FlatAppearance.BorderSize = 0;
            button2.FlatStyle = FlatStyle.Flat;
            
            button4.FlatAppearance.BorderSize = 0;
            button4.FlatStyle = FlatStyle.Flat;
            button5.FlatAppearance.BorderSize = 0;
            button5.FlatStyle = FlatStyle.Flat;
            button3.FlatAppearance.BorderSize = 0;
            button3.FlatStyle = FlatStyle.Flat;
        }
        private void AddnewProd_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }


        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            name_charactCB = comboBox3.Text;
            dB.openConnection();
            MySqlCommand cmd = new MySqlCommand($"select `Id_charact_cat` from charact_category  join charact_product using (Id_charact) join category_product using(`id _prod_cat`) where Name_charact = '{name_charactCB}' and `id _prod_cat` = {Product.id_prod_cat} ", dB.GetConnection());
            cmd.ExecuteNonQuery();
            id_charactcat = int.Parse(cmd.ExecuteScalar().ToString());
            indexCombobox3 = comboBox3.SelectedIndex;
            dB.closeConnection();
        }
        
        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            
            RowSelecct = e.RowIndex;
            if (e.RowIndex >= 0)
            {
                DataGridViewRow dtgr = dataGridView1.Rows[RowSelecct];
                fromdatagridclick = dtgr.Cells[0].Value.ToString();
              
            }
        }

        #region Buttons
        //Отмена
        private void button5_Click(object sender, EventArgs e)
        {
            _list.Clear();
            comboBox3.Items.Clear();
            this.Hide();
            Form2_for_admin._Product.Show();
        }

        //Добавить характеристику
        private void button3_Click(object sender, EventArgs e)
        {
            if (_AddCharact == null || _AddCharact.IsDisposed)
            {
                _AddCharact = new AddCharacteristic(id_prod_cat);
                _AddCharact.Show();
                
            }
            else _AddCharact.Activate();
            
        }

        //Удалить
        private void button4_Click(object sender, EventArgs e)
        {
            for(int i = 0; i<_list.Count; i++)
            {
                if (_list[i].namecharact == fromdatagridclick)
                {
                    _list.Remove(_list[i]);
                    comboBox3.Items.Add(fromdatagridclick);
                    comboBox3.SelectedIndex = 0;
                    dataGridView1.DataSource = null;
                    dataGridView1.DataSource = _list;
                }
            }
        }

        //Сохранить
        private void button2_Click(object sender, EventArgs e)
        {
            dB.openConnection();
            if (_list.Count != 0)
            {


                if (MessageBox.Show("Ви точно бажаєте зберегти?", "", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {

                    for (int i = 0; i < _list.Count; i++)
                    {
                        MySqlCommand cmd = new MySqlCommand($"INSERT INTO `charact_goods` (`Id_charact_cat`, `Id_product`, `Charact_value`) VALUES ('{_list[i].id}', '{Product.id_prod}', '{_list[i].value}');", dB.GetConnection());
                        cmd.ExecuteNonQuery();

                    }
                    dB.closeConnection();
                    dataGridView1.DataSource = null;
                    _list.Clear();
                    comboBox3.Items.Clear();
                    this.Hide();
                    Form2_for_admin._Product.Show();
                }
            }
            else MessageBox.Show("Ви повинні додати хоча б одну характеристику!");
        }

        //Добавить
        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox2.Text != String.Empty)
            {
                if (!checkcharact())
                {
                    AddProduct(new listCharact(id_charactcat, name_charactCB, textBox2.Text));
                    dataGridView1.DataSource = null;
                    dataGridView1.DataSource = _list;
                    textBox2.Text = String.Empty;

                    comboBox3.Items.RemoveAt(indexCombobox3);
                    comboBox3.SelectedIndex = 0;
                }
                else MessageBox.Show("Така характеристика вже є");
            }
            else MessageBox.Show("Ви повинні указати хначення характеристики");
        }
        #endregion

        #region MyMethod
        //добавление в список
        private void AddProduct(listCharact myList)
        {
            int _coun_mismatch = 0;

            if (_list.Count == 0)
            {
                _list.Add(myList);

            }
            else
            {
                for (int i = 0; i < _list.Count; i++)
                {
                    if (_list[i].namecharact == myList.namecharact)
                    {
                        if (MessageBox.Show("Такая характеристика уже есть, желаете перезаписать?", "", MessageBoxButtons.YesNo) == DialogResult.Yes)
                        {
                            _list[i].value= myList.value;
                        }
                    }
                    else
                    {
                        _coun_mismatch++;
                        if (_coun_mismatch == _list.Count)
                        {
                            _list.Add(myList);

                            return;
                        }
                    }

                }
            }
        }
        //проверка есть ли уже характеристика у продукта
        private bool checkcharact()
        {
            DB dB = new DB();
            string _name = comboBox3.Text;
            DataTable _table = new DataTable();
            MySqlDataAdapter _adapter = new MySqlDataAdapter();
            MySqlCommand _checkadmin = new MySqlCommand($"select Name_charact from charact_product join charact_category using (id_charact) join charact_goods using (id_charact_cat) join product using (id_product) where Id_product = '{Product.id_prod}' and Name_charact = '{_name}';" , dB.GetConnection());
            _adapter.SelectCommand = _checkadmin;
            _adapter.Fill(_table);
            if (_table.Rows.Count > 0)
            {
                return true;
            }
            else return false;
        }
        //занос с бд в комбобокс
        private void _SetComboBox()
        {
    
            #region Combobox3
            dB.openConnection();
            MySqlCommand cmd = new MySqlCommand($"Select `Name_charact` from charact_category  join charact_product using (Id_charact) join category_product using(`id _prod_cat`) where (`id _prod_cat` = {Product.id_prod_cat})", dB.GetConnection());
            MySqlDataReader my;
            my = cmd.ExecuteReader();
            while (my.Read())
            {
                string sname = my.GetString("Name_charact");
                comboBox3.Items.Add(sname);
            }
            dB.closeConnection();
            #endregion

        }

        #endregion

        #region Animation

        #endregion
        private void textBox2_MouseEnter(object sender, EventArgs e)
        {
            textBox2.BorderStyle = BorderStyle.Fixed3D;
        }

        private void textBox2_MouseLeave(object sender, EventArgs e)
        {
            textBox2.BorderStyle = BorderStyle.None;
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
    class listCharact
    {
        public string namecharact { get; set; }
        public string value { get; set; }
        public int id { get; set; }
        public listCharact(int id,string namecharact, string value)
        {
            this.id = id;
            this.namecharact = namecharact;
            this.value = value;
        }
    }
}
