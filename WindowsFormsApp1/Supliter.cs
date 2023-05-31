using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using TextBox = System.Windows.Forms.TextBox;

namespace WindowsFormsApp1
{
    public partial class Supliter : Form
    {
        DB dB = new DB();
        Form _LastForm;
        Form _Acceptance_list;
        private static int RowSelecct { get; set; }
        private static string _nameFirm { get; set; }
        private static bool flag { get; set; } = true;
        public Supliter(Form _LastForm)
        {
            InitializeComponent();
            this._LastForm = _LastForm;
        }
        private void Supliter_Load(object sender, EventArgs e)
        {
            dataGridView1.Font = new Font("Century Gothic", 14, FontStyle.Bold);
            #region Stylebuttons 
            button1.FlatAppearance.BorderSize = 0;
            button1.FlatStyle = FlatStyle.Flat;
          
            button3.FlatAppearance.BorderSize = 0;
            button3.FlatStyle = FlatStyle.Flat;
            button4.FlatAppearance.BorderSize = 0;
            button4.FlatStyle = FlatStyle.Flat;
            button5.FlatAppearance.BorderSize = 0;
            button5.FlatStyle = FlatStyle.Flat;
            button6.FlatAppearance.BorderSize = 0;
            button6.FlatStyle = FlatStyle.Flat;
            
            #endregion
           
            
            textBox1.MaxLength = 60;

            textBox2.MaxLength = 13;
            textBox2.ForeColor = Color.Gray;
            textBox2.Text = "(380)XXX-XXX-XXX";

            textBox3.ForeColor = Color.Gray;
            textBox3.MaxLength = 8;
            textBox3.Text = "XX-XX-XX-XX";
            
            textBox4.MaxLength = 60;

            textBox5.MaxLength = 29;

            _CreatorCol();
            _RefreshDataGridView(dataGridView1);

        }
        private void Supliter_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            RowSelecct = e.RowIndex;
            if (e.RowIndex >= 0)
            {
                DataGridViewRow dtgr = dataGridView1.Rows[RowSelecct];
                _nameFirm = dtgr.Cells[0].Value.ToString();
               
            }
        }

        

        private void textBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void textBox3_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void textBox5_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        #region Buttons
        //кнопка добавить 1
        private void button1_Click(object sender, EventArgs e)
        {

            //if (flag)
            //{
            //    this.Width = 961;
            //    flag = false;
            //    button6.Visible = true;
            //}
            //else
            //{
            //    this.Width = 722;
            //    flag = true;
            //    button6.Visible = false;
            //}
            Animation.AnimationFormWight(this, 961, 722, 130, 700);

        }

        //кнопка добавить 2
        private void button6_Click(object sender, EventArgs e)
        {

            if (textBox1.Text == String.Empty ) /*|| textBox3.Text == String.Empty || textBox5.Text == String.Empty)*/
            {
                MessageBox.Show("Постачальник повинен мати назву фірми");
                return;
            }

            if (MessageBox.Show("Ви точно бажаєте додати?", "", MessageBoxButtons.YesNo)==DialogResult.No)
                return;


            DB db = new DB();
            db.openConnection();
            #region Занос в переменные из текстбокс
            string _namefirm = textBox1.Text;
            string _NumberPhone = "";
            string _CodEDRPOU = "";
            if (textBox2.Text == "(380)XXX-XXX-XXX") {_NumberPhone = String.Empty;}
            else {_NumberPhone = textBox2.Text;}

            if (textBox3.Text == "XX-XX-XX-XX") { _CodEDRPOU = String.Empty; }
            else {  _CodEDRPOU = textBox3.Text; }
            
            string _Adress = textBox4.Text;
            string _SuplBank = textBox5.Text;
            #endregion
            #region Проверка и добавление пользователя 
            if (!_IsGetSupplier())
            {
                string insertbd = $"INSERT INTO `gitar_shop`.`supplier` (`Supl_name_firm`, `Supl_tel`, `Supl_edrpou`, `Supl_adress`, `Supl_bank`,`State`) VALUES ('{_namefirm}', '{_NumberPhone}', '{_CodEDRPOU}', '{_Adress}', '{_SuplBank}','1');";
                MySqlCommand _addcomand = new MySqlCommand(insertbd, db.GetConnection());

                if (_addcomand.ExecuteNonQuery() == 1)
                {
                    MessageBox.Show("Ви успішно додали");
                    _RefreshDataGridView(dataGridView1);

                    //this.Width = 722;
                    Animation.AnimationFormWight(this, 961, 722, 130, 700);
                    Animation.AnimationFormWight(this, 961, 722, 130, 700);
                    flag = true;
                    }
                else MessageBox.Show("Что-то пошло не так");
            }
            else MessageBox.Show("Такий постачальник вже є");
            #endregion

            db.closeConnection();
            
        }

        

        //кнопка удалить
        private void button3_Click(object sender, EventArgs e)
        {
            if (_IsGetSupplier())
            {
                if (MessageBox.Show("Ви впевненн що бажаєте видалити?", " ", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    DB db = new DB();
                    db.openConnection();
                    string command4 = $"UPDATE `gitar_shop`.`supplier` SET `State` = '0' WHERE (`Supl_name_firm` = '{_nameFirm}')";
                    MySqlCommand mycomand = new MySqlCommand(command4, db.GetConnection());
                    if (mycomand.ExecuteNonQuery() == 1)
                    {
                        MessageBox.Show("Успішно видален");
                    }
                    db.closeConnection();
                    _RefreshDataGridView(dataGridView1);

                }

            }
            else MessageBox.Show("Користувач не знайден");
        }

        //Кнопка назад
        private void button5_Click(object sender, EventArgs e)
        {
            _DelleteCol();
            this.Hide();
            _LastForm.Show();
        }

        // кнопка переход на форму список пост
        private void button4_Click(object sender, EventArgs e)
        {
            if (_Acceptance_list == null || _Acceptance_list.IsDisposed)
            {
                _Acceptance_list = new acceptance_list();
                _Acceptance_list.Show();
            }
            else _Acceptance_list.Activate();
        }
        #endregion

        #region MyMethods

        //Создани колонок
        private void _CreatorCol()
        {
            dataGridView1.Columns.Add("Supl_name_firm", "Назва фірми");
            dataGridView1.Columns.Add("Supl_tel", "Номер телефону");
            dataGridView1.Columns.Add("Supl_edrpou", "Код ЄДРПОУ");
            dataGridView1.Columns.Add("Supl_adress", "Адреса");
            dataGridView1.Columns.Add("Supl_bank", "Розрахунк номер");
        }

        //Удаление колонок
        private void _DelleteCol()
        {
            
            dataGridView1.Columns.Remove("Supl_name_firm");
            dataGridView1.Columns.Remove("Supl_tel");
            dataGridView1.Columns.Remove("Supl_edrpou");
            dataGridView1.Columns.Remove("Supl_adress");
            dataGridView1.Columns.Remove("Supl_bank");
        }

        /// <summary>
        /// Чтение строк
        /// </summary>
        private void _ReadRows(DataGridView datagr, IDataRecord record)
        {
            datagr.Rows.Add(record.GetString(0), record.GetString(1), record.GetString(2), record.GetString(3), record.GetString(4));
        }


        /// <summary>
        /// Обновление дата грид
        /// </summary>
        private void _RefreshDataGridView(DataGridView datagr)
        {
            datagr.Rows.Clear();
            string selectbd = $"select `Supl_name_firm`, `Supl_tel`, `Supl_edrpou`, `Supl_adress`, `Supl_bank` from `supplier` where State = 1;";
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


        /// <summary>
        /// Проверка на Поставщика
        /// </summary>
        /// <returns></returns>
        public bool _IsGetSupplier()
        {
            DB dB = new DB();
           
            DataTable _table = new DataTable();
            MySqlDataAdapter _adapter = new MySqlDataAdapter();
            MySqlCommand _checkadmin = new MySqlCommand($"SELECT * From `supplier` where  Supl_name_firm = '{_nameFirm}'", dB.GetConnection());
            _adapter.SelectCommand = _checkadmin;
            _adapter.Fill(_table);
            if (_table.Rows.Count > 0)
            {
                return true;
            }
            else return false;
        }

        #endregion

        #region Animations    
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
            textBox2.ForeColor = Color.White;
            if (textBox2.Text == "(380)XXX-XXX-XXX")
                textBox2.Text = String.Empty;
        }

        private void textBox2_MouseLeave(object sender, EventArgs e)
        {
            textBox2.BorderStyle = BorderStyle.None;
            if (textBox2.Text == String.Empty)
            {
                textBox2.ForeColor = Color.Gray;
                textBox2.Text = "(380)XXX-XXX-XXX";
            }
        }

        private void textBox3_MouseEnter(object sender, EventArgs e)
        {
            textBox3.BorderStyle = BorderStyle.Fixed3D;
            textBox3.ForeColor = Color.White;
            if (textBox3.Text == "XX-XX-XX-XX")
                textBox3.Text = String.Empty;
        }

        private void textBox3_MouseLeave(object sender, EventArgs e)
        {
            textBox3.BorderStyle = BorderStyle.None;
            if (textBox3.Text == String.Empty)
            {
                textBox3.ForeColor = Color.Gray;
                textBox3.Text = "XX-XX-XX-XX";
            }
        }

        private void textBox4_MouseEnter(object sender, EventArgs e)
        {
            textBox4.BorderStyle = BorderStyle.Fixed3D;
        }

        private void textBox4_MouseLeave(object sender, EventArgs e)
        {
            textBox4.BorderStyle = BorderStyle.None;
        }

        private void textBox5_MouseEnter(object sender, EventArgs e)
        {
            textBox5.BorderStyle = BorderStyle.Fixed3D;
        }

        private void textBox5_MouseLeave(object sender, EventArgs e)
        {
            textBox5.BorderStyle = BorderStyle.None;
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }


        #endregion

        //570; 378
        //764; 370
    }
}
