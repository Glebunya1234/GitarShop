using Microsoft.Win32;
using MySql.Data.MySqlClient;
using Mysqlx.Expr;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using TextBox = System.Windows.Forms.TextBox;

namespace WindowsFormsApp1
{
    public partial class AddLoginAndPassword : Form
    {
        private int _id { get; set; }
        public AddLoginAndPassword()
        {
            InitializeComponent();
        }


        private void button1_Click(object sender, EventArgs e)
        {
            _id = EmployeeList.id;
            if(UniqLoginNpass())
            {
                MessageBox.Show("Такий логін вже зайнят!");
                return;
            }
            if (textBox1.Text != String.Empty || textBox2.Text != String.Empty)
            {
                DB db = new DB();
                db.openConnection();
                string _logi = textBox1.Text;
                string _Passw = textBox2.Text;
                #region Создание логина в регестрация
                string insertbd = $"INSERT INTO `register` (`Id_employee`, `login_user`, `password_user`) VALUES ('{_id}', '{_logi}', '{_Passw}')";
                MySqlCommand _addcomand = new MySqlCommand(insertbd, db.GetConnection());
                if (_addcomand.ExecuteNonQuery() == 1)
                {
                    MessageBox.Show("Ви успішно создали логін та пароль");
                }
                else MessageBox.Show("Щось пішло не так");
                #endregion
                
                #region Создания Юзера с правами доступа
                if (EmployeeList.position == "Адміністратор")
                {
                    //MySqlCommand command0 = new MySqlCommand($"create user '{_logi}'@'127.0.0.1' IDENTIFIED BY '{_Passw}';", db.GetConnection());
                    //command0.ExecuteNonQuery();
                    //MySqlCommand command = new MySqlCommand($"grant all on gitar_shop.* to '{_logi}'@'127.0.0.1';", db.GetConnection());
                    //command.ExecuteNonQuery();
                    MySqlCommand cmd1 = new MySqlCommand($"call createAdmons('{_logi}', '{_Passw}');", db.GetConnection());
                    cmd1.ExecuteNonQuery();
                }
                else
                {
                    #region text

                    //MySqlCommand comman = new MySqlCommand($"create user '{_logi}'@'127.0.0.1' IDENTIFIED BY '{_Passw}';", db.GetConnection());
                    //comman.ExecuteNonQuery();
                    //MySqlCommand command2 = new MySqlCommand($"GRANT UPDATE on gitar_shop.product to '{_logi}'@'127.0.0.1';" +
                    //    $"GRANT UPDATE on gitar_shop.sales to '{_logi}'@'127.0.0.1';" +
                    //    $"GRANT UPDATE on gitar_shop.customer to '{_logi}'@'127.0.0.1';" +
                    //    $"GRANT UPDATE on gitar_shop.sales_list to '{_logi}'@'127.0.0.1';", db.GetConnection());
                    //command2.ExecuteNonQuery();
                    //MySqlCommand command3 = new MySqlCommand($"GRANT INSERT on gitar_shop.product to '{_logi}'@'127.0.0.1';" +
                    //    $"GRANT INSERT on gitar_shop.customer to '{_logi}'@'127.0.0.1';" +
                    //    $"GRANT INSERT on gitar_shop.sales to '{_logi}'@'127.0.0.1';" +
                    //    $"GRANT INSERT on gitar_shop.sales_list to '{_logi}'@'127.0.0.1';" +
                    //    $"GRANT INSERT on gitar_shop.invoice to '{_logi}'@'127.0.0.1';"+
                    //    $"GRANT INSERT on gitar_shop.acceptance_list to '{_logi}'@'127.0.0.1';"+
                    //    $"GRANT INSERT on gitar_shop.supplier to '{_logi}'@'127.0.0.1';", db.GetConnection());

                    //command3.ExecuteNonQuery();
                    //MySqlCommand command4 = new MySqlCommand($"GRANT select on gitar_shop.product to '{_logi}'@'127.0.0.1';" +
                    //    $"GRANT select on gitar_shop.sales to '{_logi}'@'127.0.0.1';" +
                    //    $"GRANT select on gitar_shop.customer to '{_logi}'@'127.0.0.1';" +
                    //    $"GRANT select on gitar_shop.sales_list to '{_logi}'@'127.0.0.1';" +
                    //    $"GRANT select on gitar_shop.register to '{_logi}'@'127.0.0.1';" +
                    //    $"GRANT select on gitar_shop.employee to '{_logi}'@'127.0.0.1';" +
                    //    $"GRANT select on gitar_shop.charact_product to '{_logi}'@'127.0.0.1';" +
                    //    $"GRANT select on gitar_shop.charact_goods to '{_logi}'@'127.0.0.1';" +
                    //    $"GRANT select on gitar_shop.category_product to '{_logi}'@'127.0.0.1';" +
                    //    $"GRANT select on gitar_shop.charact_category to '{_logi}'@'127.0.0.1';" +
                    //    $"GRANT select on gitar_shop.invoice to '{_logi}'@'127.0.0.1';" +
                    //    $"GRANT select on gitar_shop.acceptance_list to '{_logi}'@'127.0.0.1';"+
                    //    $"GRANT select on gitar_shop.supplier to '{_logi}'@'127.0.0.1';",db.GetConnection());
                    //command4.ExecuteNonQuery();
                    #endregion

                    MySqlCommand cmd = new MySqlCommand($"call createEmployees('{_logi}', '{_Passw}');", db.GetConnection()) ;
                    cmd.ExecuteNonQuery();
                }

                #endregion
                db.closeConnection();
                textBox1.Text = String.Empty;
                textBox2.Text = String.Empty;
                this.Close();
            }
            else MessageBox.Show("Заполніть всі поля");
        }

        private void AddLoginAndPassword_Load(object sender, EventArgs e)
        {
            button1.FlatAppearance.BorderSize = 0;
            button1.FlatStyle = FlatStyle.Flat;
        }

        private bool UniqLoginNpass()
        {
            string login = textBox1.Text;
            DB dB = new DB();
            DataTable _table = new DataTable();
            MySqlDataAdapter _adapter = new MySqlDataAdapter();
            MySqlCommand _checkadmin = new MySqlCommand($"SELECT login_user FROM register  where login_user = '{login}'", dB.GetConnection());
            _adapter.SelectCommand = _checkadmin;
            _adapter.Fill(_table);
            if (_table.Rows.Count > 0)
            {
                return true;
            }
            else return false;
        }

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
        }

        private void textBox2_MouseLeave(object sender, EventArgs e)
        {
            textBox2.BorderStyle = BorderStyle.None;
        }
        #endregion
    }
}
