using MySql.Data.MySqlClient;
using Mysqlx.Crud;
using MySqlX.XDevAPI.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
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
    public partial class EmployeeList : Form
    {
        Form _Formlast;
        Form _FormLigin;
        Form _AddLoginNPassword;

        DB dB = new DB();
        private static int RowSelecct { get; set; }
        public static string _name { get; private set; }
        private static bool flag { get; set; } = true;
        public static int id { get; private set; }
        public static string position { get; private set; }

        public EmployeeList(Form form_for_admin)
        {
            InitializeComponent();
            _Formlast = form_for_admin;
        }


        private void EmployeeList_Load(object sender, EventArgs e)
        {
            #region Стиль Комбобокс
            comboBox1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            comboBox2.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            #endregion
            #region Стиль кнопок
            button1.FlatAppearance.BorderSize = 0;
            button1.FlatStyle = FlatStyle.Flat;
            
            button3.FlatAppearance.BorderSize = 0;
            button3.FlatStyle = FlatStyle.Flat;
            button4.FlatAppearance.BorderSize = 0;
            button4.FlatStyle = FlatStyle.Flat;
            button5.FlatAppearance.BorderSize = 0;
            button5.FlatStyle = FlatStyle.Flat;
           
            button7.FlatAppearance.BorderSize = 0;
            button7.FlatStyle = FlatStyle.Flat;

            #endregion
           
            _CreatorCol();
            _RefreshDataGridView(dataGridView1);
            dataGridView1.Font = new Font("Century Gothic", 14, FontStyle.Bold);

            comboBox1.SelectedIndex = 0;
            comboBox2.SelectedIndex = 0;

            textBox2.ForeColor = Color.Gray;
            textBox2.Text = "(380)XXX-XXX-XXX";
            textBox2.MaxLength = 13;

            textBox5.MaxLength = 10;
            textBox5.ForeColor = Color.Gray;
            textBox5.Text = "рррр-мм-дд";

            textBox1.MaxLength = 150;
        }
        private void EmployeeList_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        /// <summary>
        /// Датагрид нажатие
        /// </summary>
        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            RowSelecct = e.RowIndex;
            if (e.RowIndex >= 0)
            {

                DataGridViewRow dtgr = dataGridView1.Rows[RowSelecct];
                _name = dtgr.Cells[0].Value.ToString();
            }
        }

        #region button
        
        //Кнопка назад
        private void button4_Click(object sender, EventArgs e)
        {
           this.Hide();
           _DelleteCol();
           _Formlast.Show();
        }
        
        //Кнопка добавить 1
        private void button1_Click(object sender, EventArgs e)
        {
            Animation.AnimationFormWight(this, 961, 722, 130, 700);
            button5.Visible = true;
        }

        //Кнопка добавить 2
        private void button5_Click(object sender, EventArgs e)
        {
           
            if (textBox1.Text == String.Empty)
            {
                MessageBox.Show("Співробітник повинен мати ПІБ!");
                return;
            }
            if((textBox5.Text!=String.Empty))
            {
                if (textBox5.Text != "рррр-мм-дд")
                {
                    Regex regex = new Regex(@"^(?!0{4})\d{4}-(0[1-9]|1[0-2])-([0-2][1-9]|[1-3]1)$");
                    if (!regex.IsMatch(textBox5.Text))
                    {
                        MessageBox.Show("Дата не відповідає формату YYYY-MM-DD\nабо має некоректні значения місяця та/або дня)");
                        return;
                    }
                }

            }

            if (MessageBox.Show("Ви точно бажаєте додати співробітника?", "", MessageBoxButtons.YesNo)== DialogResult.No)
                return;
            
           
            DB db = new DB();
            db.openConnection();
               
               
            #region Занос в переменные из текстбокс
            string _FIO = textBox1.Text;
            _name = _FIO;
            string _NumberPhone = "";

            if (textBox2.Text == "(380)XXX-XXX-XXX") { _NumberPhone = String.Empty; }
            else { _NumberPhone = textBox2.Text; }


            string _Sex = comboBox1.Text;
            string _Position = comboBox2.Text;
            string _Birth = textBox5.Text;
            if (textBox5.Text == "рррр-мм-дд") { _Birth = String.Empty; }
            else { _Birth = textBox5.Text; }
            #endregion
            #region Проверка и добавление пользователя + открыте новой формы для создания логина входа
            if (!_IsGetEmplyee())
            {
                string insertbd = $"INSERT INTO `employee` (`Emp_pib`, `Emp_tel`, `Emp_gender`, `Emp_position`, `Emp_birth`) VALUES('{_FIO}', '{_NumberPhone}', '{_Sex}', '{_Position}', '{_Birth}')";
                MySqlCommand _addcomand = new MySqlCommand(insertbd, db.GetConnection());

                if (_addcomand.ExecuteNonQuery() == 1) //сначало выполнится метод(он же запрос) а потом проверит == 1?
                {
                    string query = $"Select Id_employee From `employee` where `Emp_pib` = '{_FIO}'"; //Запрос
                    MySqlCommand qquery = new MySqlCommand(query, db.GetConnection()); // отправляем запрос в бд
                    qquery.ExecuteNonQuery();//выполняем запрос
                    id = (int)qquery.ExecuteScalar(); //заносим в переменную результат что получили после выполнения запроса
                    position = _Position;
                    _AddLoginNPassword ??= new AddLoginAndPassword();
                    _AddLoginNPassword.ShowDialog();

                    MessageBox.Show("Ви успішно додали");
                    _RefreshDataGridView(dataGridView1);
                    Animation.AnimationFormWight(this, 961, 722, 130, 700);
                    Animation.AnimationFormWight(this, 961, 722, 130, 700);

                }    
                else MessageBox.Show("Щось пішло не так");
            }
            else MessageBox.Show("Працівник с таким ПІБ вже є");
            #endregion
            db.closeConnection();
                
        }
       
    
        //кнопка удалить
        private void button3_Click(object sender, EventArgs e)
        {
            if (_IsGetEmplyee())
            {
                if (!_ThisUser())
                {
                    if (_CheckAdminOrEmployee())
                    {
                        if (_CountAdmin())
                        {
                            _DeletedEmployee();
                        }
                        else MessageBox.Show("Ви не можете видалити єдиного адміна");
                    }
                    else
                    {
                        _DeletedEmployee();
                    }
                }
                else MessageBox.Show("Ви не можете видалити самого себе");
            }
            else MessageBox.Show("Користувач не знайден");
            
        }

        #endregion

        #region MyMethods

        /// <summary>
        /// Создаем колонки
        /// </summary>
        private void _CreatorCol()
        {
            
            dataGridView1.Columns.Add("Emp_pib", "ПІБ");
            dataGridView1.Columns.Add("Emp_tel", "Номер телефону");
            dataGridView1.Columns.Add("Emp_gender", "Пол");
            dataGridView1.Columns.Add("Emp_position", "Посада");
            dataGridView1.Columns.Add("Emp_birth", "Дата народженя");
        }


        /// <summary>
        /// Удаляем колноки
        /// </summary>
        private void _DelleteCol()
        {
            
            dataGridView1.Columns.Remove("Emp_pib");
            dataGridView1.Columns.Remove("Emp_tel");
            dataGridView1.Columns.Remove("Emp_gender");
            dataGridView1.Columns.Remove("Emp_position");
            dataGridView1.Columns.Remove("Emp_birth");
        }


        /// <summary>
        /// Чтение с бд колонок
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
            string selectbd = $"select Emp_pib,Emp_tel,Emp_gender,Emp_position,Emp_birth from `employee` where `State` ='1'";
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
        /// Проверка на сотрудника
        /// </summary>
        /// <returns></returns>
        public bool _IsGetEmplyee()
        {
            DB dB = new DB();
            //_name = textBox1.Text;
            DataTable _table = new DataTable();
            MySqlDataAdapter _adapter = new MySqlDataAdapter();
            MySqlCommand _checkadmin = new MySqlCommand($"SELECT Emp_pib From `employee` where  (Emp_pib = '{_name}' and State = '1')", dB.GetConnection());
            _adapter.SelectCommand = _checkadmin;
            _adapter.Fill(_table);
            if (_table.Rows.Count > 0)
            {
                return true;
            }
            else return false;
        }

      
        /// <summary>
        /// Количество админов
        /// </summary>
        /// <returns></returns>
        public bool _CountAdmin()
        {
            DB dB = new DB();
            DataTable _table = new DataTable();
            MySqlDataAdapter _adapter = new MySqlDataAdapter();
            MySqlCommand _checkadmin = new MySqlCommand($"SELECT COUNT(*) From `employee` where (Emp_position = 'Адміністратор' and State = '1') ;", dB.GetConnection());
            _adapter.SelectCommand = _checkadmin;
            _adapter.Fill(_table);
            int count = Convert.ToInt32(_table.Rows[0][0]);
            if (count > 1)
            {
                return true;
            }
            else if (count < 1)
            {
                return false;
            }
            else return false;
        }


        /// <summary>
        /// Удаление сотрудников
        /// </summary>
        private void _DeletedEmployee()
        {
            if (MessageBox.Show("Ви точно бажаєте видалити", " ", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                DB db = new DB();
                db.openConnection();

                MySqlCommand command1 = new MySqlCommand($"Select `Id_employee` from `employee` where `Emp_pib` = '{_name}'", db.GetConnection());
                command1.ExecuteNonQuery();
                int idd = (int)command1.ExecuteScalar();

                MySqlCommand command2 = new MySqlCommand($"Select `login_user` from `register` where `Id_employee` = '{idd}' ;", db.GetConnection());
                command2.ExecuteNonQuery();
                string user = (string)command2.ExecuteScalar();

                MySqlCommand command3 = new MySqlCommand($"drop user '{user}'@'127.0.0.1';", db.GetConnection());
                command3.ExecuteNonQuery();

                //$"DELETE FROM `employee` WHERE (`Emp_pib` = '{_name}');"
                string command4 = $"UPDATE `employee` SET `State` = '0' WHERE(`Emp_pib` = '{_name}');";
                MySqlCommand mycomand = new MySqlCommand(command4, db.GetConnection());
                if (mycomand.ExecuteNonQuery() == 1)
                {
                    MessageBox.Show("Успішно ЗВІЛЬНЕН!");
                }
                db.closeConnection();
                _RefreshDataGridView(dataGridView1);

            }
        }


        /// <summary>
        /// Проверка админ или сотрудник
        /// </summary>
        /// <returns></returns>
        public bool _CheckAdminOrEmployee()
        {
            DB dB = new DB();
            dB.openConnection();
            MySqlCommand _check = new MySqlCommand($"SELECT Emp_position From `employee` where (Emp_pib = '{_name}' and State = '1');", dB.GetConnection());
            _check.ExecuteNonQuery();
            string _result = _check.ExecuteScalar().ToString();
            if (_result == "Адміністратор")
            {
                dB.closeConnection();
                return true;
            }
            else { dB.closeConnection(); return false; }
            
        }

        public bool _ThisUser()
        {
            DB dB = new DB();
            dB.openConnection();
            MySqlCommand _check = new MySqlCommand($"SELECT Emp_pib From `employee` join register using(Id_employee) where (login_user = '{Form1.getUser()}');", dB.GetConnection());
            _check.ExecuteNonQuery();
            string _result = _check.ExecuteScalar().ToString();
            if (_result == _name)
            {
                dB.closeConnection();
                return true;
            }
            else { dB.closeConnection(); return false; }
        }

        #endregion

        #region Animation

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
            if(textBox2.Text == "(380)XXX-XXX-XXX")
            textBox2.Text = String.Empty;
        }

        private void textBox2_MouseLeave(object sender, EventArgs e)
        {
            textBox2.BorderStyle = BorderStyle.None;
            if(textBox2.Text == String.Empty)
            {
                textBox2.ForeColor = Color.Gray;
                textBox2.Text = "(380)XXX-XXX-XXX";
            }
        }


        private void textBox5_MouseEnter(object sender, EventArgs e)
        {
            textBox5.BorderStyle = BorderStyle.Fixed3D;
            textBox5.BorderStyle = BorderStyle.Fixed3D;
            textBox5.ForeColor = Color.White;
            if (textBox5.Text == "рррр-мм-дд")
                textBox5.Text = String.Empty;
        }

        private void textBox5_MouseLeave(object sender, EventArgs e)
        {
            textBox5.BorderStyle = BorderStyle.None;
            textBox5.BorderStyle = BorderStyle.None;
            if (textBox5.Text == String.Empty)
            {
                textBox5.ForeColor = Color.Gray;
                textBox5.Text = "рррр-мм-дд";
            }
        }

      

        private void button7_Click(object sender, EventArgs e)
        {
            if (_FormLigin == null || _FormLigin.IsDisposed)
            {
                _FormLigin = new LoginList();
                _FormLigin.Show();
            }
            else _FormLigin.Activate();
        }

        private void textBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void textBox5_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && !char.IsSymbol(e.KeyChar) && !char.IsPunctuation(e.KeyChar))
            {
                e.Handled = true; // Запрещаем обработку символа
            }
        }

        #endregion

        //751; 368
        //566; 368

    }
}
