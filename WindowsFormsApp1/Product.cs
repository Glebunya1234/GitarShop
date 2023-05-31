using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace WindowsFormsApp1
{
    public partial class Product : Form
    {
        Form _CharactForm;
        Form _LastForm;
        Form _BuyProd;
        Form _SellProd;
        Form _AddProd;
        Form _AddCharact;
        DB dB = new DB();
        private static bool flag { get; set; } = false;
        private static bool flagCheck { get; set; } = false;
        private static int RowSelecct { get; set; }
        
        private static bool flag2 { get; set; } = false;
        public static int id_prod { get; set; } 
        private static double minprice { get; set; }
        private static double maxprice { get; set; }
        public static string _nameprod { get; private set; }
        private static int _count { get; set; }
        public static int thisheight{ get; set; }
        public static int id_prod_cat { get; private set; }
        

        public Product(Form lastForm)
        {
            InitializeComponent();
            _LastForm = lastForm;
        }

        private void Product_Load(object sender, EventArgs e)
        {
            dataGridView1.Font = new Font("Century Gothic", 13, FontStyle.Bold);
            #region Style buttons
            button1.FlatAppearance.BorderSize = 0;
            button1.FlatStyle = FlatStyle.Flat;
            button2.FlatAppearance.BorderSize = 0;
            button2.FlatStyle = FlatStyle.Flat;
            button3.FlatAppearance.BorderSize = 0;
            button3.FlatStyle = FlatStyle.Flat;
            button4.FlatAppearance.BorderSize = 0;
            button4.FlatStyle = FlatStyle.Flat;
            button5.FlatAppearance.BorderSize = 0;
            button5.FlatStyle = FlatStyle.Flat;
            button6.FlatAppearance.BorderSize = 0;
            button6.FlatStyle = FlatStyle.Flat;
            button7.FlatAppearance.BorderSize = 0;
            button7.FlatStyle = FlatStyle.Flat;
            button8.FlatAppearance.BorderSize = 0;
            button8.FlatStyle = FlatStyle.Flat;
            button9.FlatAppearance.BorderSize = 0;
            button9.FlatStyle = FlatStyle.Flat;
            button10.FlatAppearance.BorderSize = 0;
            button10.FlatStyle = FlatStyle.Flat;
            button11.FlatAppearance.BorderSize = 0;
            button11.FlatStyle = FlatStyle.Flat;
            button12.FlatAppearance.BorderSize = 0;
            button12.FlatStyle = FlatStyle.Flat;
            #endregion
            minprice = 0;
            maxprice = 500000;
            _CreatorCol();
            _SerchCheckbox(dataGridView1);
           
            comboBox1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            comboBox1.SelectedIndex = 0;
            label9.Text =String.Format("Вибраний товар: {0} ", dataGridView1.Rows[0].Cells[1].Value.ToString());
            _nameprod = dataGridView1.Rows[0].Cells[1].Value.ToString();
            _count = Convert.ToInt32(dataGridView1.Rows[0].Cells[2].Value.ToString());
            if (_Check_adm())
                button7.Visible = true;
        }
        private void Product_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }
        
        /// <summary>
        /// Добавляем колонки
        /// </summary>
        private void _CreatorCol()
        {
            dataGridView1.Columns.Add("id _prod_cat", "Категорія");
            dataGridView1.Columns.Add("Product_name", "Назва");
            dataGridView1.Columns.Add("Prod_amount", "Кол-во");
            dataGridView1.Columns.Add("Prod_price", "Ціна за 1");
           
            dataGridView1.Columns.Add("Prod_country", "Виробник");
            dataGridView1.Columns.Add("Prod_orient", "Орієнтація");
        }


        /// <summary>
        /// Удаляем колноки
        /// </summary>
        private void _DelleteCol()
        {
            dataGridView1.Columns.Remove("id _prod_cat");
            dataGridView1.Columns.Remove("Product_name");
            dataGridView1.Columns.Remove("Prod_amount");
            dataGridView1.Columns.Remove("Prod_price");
           
            dataGridView1.Columns.Remove("Prod_country");
            dataGridView1.Columns.Remove("Prod_orient");

        }

        /// <summary>
        /// Чтение строк
        /// </summary>
        private void _ReadRows(DataGridView datagr, IDataRecord record)
        {
            datagr.Rows.Add(record.GetString(0), record.GetString(1), record.GetInt32(2), record.GetDouble(3), record.GetString(4), record.GetString(5));
        }




        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
           
            RowSelecct = e.RowIndex;
            if (e.RowIndex >= 0)
            {
                DB dbb = new DB();
                dbb.openConnection();
                DataGridViewRow dtgr = dataGridView1.Rows[RowSelecct];
                label9.Text = String.Format("Вибраний товар: {0} ", dtgr.Cells[1].Value.ToString());
                _nameprod = dtgr.Cells[1].Value.ToString();
                _count = Convert.ToInt32(dtgr.Cells[2].Value.ToString());
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            _DelleteCol();
            this.Hide();
            _LastForm.Show();
        }
        private void button3_Click(object sender, EventArgs e)
        {
            Animation.AnimationFormWight(this, 1395, 1185, 130, 1000);
            //if (flag)
            //{
            //    this.Width = 1395;
            //    flag = false;
            //}
            //else
            //{
            //    this.Width = 1185;
            //    flag = true;
            //}
        }
        private void button4_Click(object sender, EventArgs e)
        {
            //DB db = new DB();
            //db.openConnection();

            if (_CharactForm == null || _CharactForm.IsDisposed)
            {
                _CharactForm = new Charact();
                _CharactForm.Show();
            }
            else _CharactForm.Activate();

        }
        private void button5_Click(object sender, EventArgs e)
        {
            _SerchCheckbox(dataGridView1);
        }


        public void _SerchCheckbox(DataGridView dataGridView)
        {
            
            string _filterCountry =" ";
            string _filterCountryOther = " ";
            string _filterTypeGitar = " ";
            string _filterOrient = " ";
            int _state = 1;
            #region Проверки полей Типа гитары
            if (checkBox1.Checked) { _filterTypeGitar += "'Акустичні',"; }
            if (checkBox2.Checked) { _filterTypeGitar += "'Класичні',"; }
            if (checkBox3.Checked) { _filterTypeGitar += "'Басс',"; }
            if (checkBox4.Checked) { _filterTypeGitar += "'Електроакустичні',"; }
            
            if (!checkBox1.Checked && !checkBox2.Checked && !checkBox3.Checked && !checkBox4.Checked)
                _filterTypeGitar = "' '";
            #endregion

            #region Проверки полей стран
            if (checkBox5.Checked) { _filterCountry += "'Індонезія' ,"; }
            if (checkBox6.Checked) { _filterCountry += "'Китай',"; }
            if (checkBox7.Checked) { _filterCountry += "'Корея',"; }
            if (checkBox8.Checked) { _filterCountryOther = "OR Prod_country NOT IN('Індонезія', 'Китай', 'Корея')"; }
            if (!checkBox5.Checked && !checkBox6.Checked && !checkBox7.Checked && checkBox8.Checked)
                _filterCountry = "' '";
            if (!checkBox5.Checked && !checkBox6.Checked && !checkBox7.Checked && !checkBox8.Checked)
            { _filterCountry = "' '"; _filterCountryOther = ""; }
            #endregion

            #region Проверки полей ориентации
            if (checkBox9.Checked) { _filterOrient += "'Правостороння',"; }
            if (checkBox10.Checked) { _filterOrient += "'Лівостороння',"; }
            if (!checkBox9.Checked && !checkBox10.Checked) { _filterOrient += "' '";  }
            #endregion

            dataGridView.Rows.Clear();
            //if (!string.IsNullOrEmpty(_filterCountry))
            //{
                _filterCountry = _filterCountry.TrimEnd(','); //Если в конце стоит запятая то удалит
                _filterTypeGitar = _filterTypeGitar.TrimEnd(',');//Если в конце стоит запятая то удалит
                _filterOrient = _filterOrient.TrimEnd(',');//Если в конце стоит запятая то удалит

                if (comboBox1.SelectedIndex == 0)  _state = 1;
                else if (comboBox1.SelectedIndex == 1) _state = 0;
                
                string selectbd = $"SELECT Name_cat, Product_name,Prod_amount,Prod_price,Prod_country,Prod_orient from product join category_product using(`id _prod_cat`) WHERE (Prod_country in({_filterCountry}) and `Name_cat` in({_filterTypeGitar}) and Prod_price BETWEEN {minprice} AND {maxprice} and Prod_orient in({_filterOrient}) and State = '{_state}') {_filterCountryOther} and `Name_cat` in ({_filterTypeGitar})and Prod_price BETWEEN {minprice} AND {maxprice} and Prod_orient in({_filterOrient}) and State = '{_state}';";
                MySqlCommand mySqlCommand = new MySqlCommand(selectbd, dB.GetConnection());
                dB.openConnection();

                MySqlDataReader reader = mySqlCommand.ExecuteReader();
                while (reader.Read())
                {
                    _ReadRows(dataGridView, reader);
                }
                reader.Close();
            //}
            dB.closeConnection();
        }
      
        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            textBox1.Text = trackBar1.Value.ToString();
        }      
        private void trackBar2_Scroll(object sender, EventArgs e)
        {
            textBox2.Text = trackBar2.Value.ToString();
        }
       
        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            minprice = double.Parse(textBox1.Text);
            _SerchCheckbox(dataGridView1);
        }
        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            maxprice = double.Parse(textBox2.Text);
            _SerchCheckbox(dataGridView1);
        }

        //1044; 613
        #region Чекбоксы с автобновлением табл
        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            _SerchCheckbox(dataGridView1);
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            _SerchCheckbox(dataGridView1);
        }

        private void checkBox4_CheckedChanged(object sender, EventArgs e)
        {
            _SerchCheckbox(dataGridView1);
        }

        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
            _SerchCheckbox(dataGridView1);
        }

        private void checkBox5_CheckedChanged(object sender, EventArgs e)
        {
            _SerchCheckbox(dataGridView1);
        }

        private void checkBox8_CheckedChanged(object sender, EventArgs e)
        {
            _SerchCheckbox(dataGridView1);
        }

        private void checkBox7_CheckedChanged(object sender, EventArgs e)
        {
            _SerchCheckbox(dataGridView1);
        }

        private void checkBox6_CheckedChanged(object sender, EventArgs e)
        {
            _SerchCheckbox(dataGridView1);
        }

        private void checkBox9_CheckedChanged(object sender, EventArgs e)
        {
            _SerchCheckbox(dataGridView1);
        }

        private void checkBox10_CheckedChanged(object sender, EventArgs e)
        {
            _SerchCheckbox(dataGridView1);
        }
        #endregion

        private void button1_Click(object sender, EventArgs e)
        {
            this.Hide();
            _SellProd ??= new SellprodForCustomer(this);
            _SellProd.ShowDialog();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            this.Hide();
            _BuyProd ??= new BuyProduct(this);
            _BuyProd.ShowDialog();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            _SerchCheckbox(dataGridView1);
        }

       

        private void button7_Click(object sender, EventArgs e)
        {
            Animation.AnimationFormHeight(this, 878, 757, 130, 1050);
            //if(flag2==false)
            // {
            //     this.Height = 878;
            //     flag2 = true;
            // }
            //else
            // {
            //     this.Height = 760;
            //     flag2=false;
            // }

            //1212; 878
            //1212; 760
        }

        private void button9_Click(object sender, EventArgs e)
        {
            dB.openConnection();
            MySqlCommand cmd = new MySqlCommand($"UPDATE `product` SET `State` = '0' WHERE (`Product_name` = '{_nameprod}');",dB.GetConnection());
            cmd.ExecuteNonQuery();
            _SerchCheckbox(dataGridView1);
            label9.Text = "Вибраний товар:";
            dB.closeConnection();
        }

        private void button10_Click(object sender, EventArgs e)
        {
            dB.openConnection();
            MySqlCommand cmd = new MySqlCommand($"UPDATE `product` SET `State` = '1' WHERE (`Product_name` = '{_nameprod}');", dB.GetConnection());
            cmd.ExecuteNonQuery();
            _SerchCheckbox(dataGridView1);
            label9.Text = "Вибраний товар:";
            dB.closeConnection();
        }

        private void button8_Click(object sender, EventArgs e)
        {
            this.Hide();
            _AddProd ??= new AddProd(this);
            _AddProd.ShowDialog();
        }

        private void button11_Click(object sender, EventArgs e)
        {
            
                //Получение айди товара и котегории 
                dB.openConnection();
                MySqlCommand cmd = new MySqlCommand($"Select Id_product from product where Product_name = '{_nameprod}'", dB.GetConnection());
                cmd.ExecuteNonQuery();
                id_prod = int.Parse(cmd.ExecuteScalar().ToString());
                MySqlCommand cmd1 = new MySqlCommand($"Select `id _prod_cat` from product where Product_name = '{_nameprod}'", dB.GetConnection());
                cmd1.ExecuteNonQuery();
                id_prod_cat = int.Parse(cmd1.ExecuteScalar().ToString());
                this.Hide();
                _AddCharact ??= new AddnewProd(this); //Должен принимать параметры айди продукта и айди категории продукта
                _AddCharact.ShowDialog();
            

        }

        private bool _Check_adm()
        {
            dB.openConnection();
            int id_empl;
            string position;
            MySqlCommand cmd0 = new MySqlCommand($"Select Id_employee from register where login_user = '{Form1._logi}';", dB.GetConnection()) ;
            cmd0.ExecuteNonQuery();
            id_empl = int.Parse(cmd0.ExecuteScalar().ToString());

            MySqlCommand cmd = new MySqlCommand($"Select Emp_position from employee where Id_employee = '{id_empl}'", dB.GetConnection());
            cmd.ExecuteNonQuery();
            position = cmd.ExecuteScalar().ToString();
            if (position == "Адміністратор")
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

        private void textBox2_MouseEnter(object sender, EventArgs e)
        {
            textBox2.BorderStyle = BorderStyle.Fixed3D;
        }

        private void textBox2_MouseLeave(object sender, EventArgs e)
        {
            textBox2.BorderStyle = BorderStyle.None;
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Back && textBox1.TextLength == 1)
            {
                e.Handled = true;
            }
        }

        private void button12_Click(object sender, EventArgs e)
        {
            if (flagCheck)
            {
                foreach (CheckBox cb in Controls.OfType<CheckBox>())
                {
                    cb.Checked = true;
                }
                flagCheck = false;
            }
            else
            {
                foreach (CheckBox cb in Controls.OfType<CheckBox>())
                {
                    cb.Checked = false;
                }
                flagCheck = true;
            }
        }
    }
}
