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
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Button;
using CheckBox = System.Windows.Forms.CheckBox;

namespace WindowsFormsApp1
{
    public partial class Sales : Form
    {
        List<string> employeeNames = new List<string>();
        Form _LastForm;
        DB dB = new DB();
        public static string _filter = " ";
        private static bool flag2 = false;
        private static bool allUnchecked = true;
        private static bool flag { get; set; } = true;
        private static bool flagDate { get; set; } = false;
        private string select { get; set; } = $"select Emp_pib, Product_name, Sales_amount, Prod_price * Sales_amount AS total_sales, Sales_date FROM sales_list join sales using(Id_sales) join employee using(Id_employee) join product using(Id_product) ORDER BY Sales_date DESC";
        private string selectAmount { get; set; } = $"select Sum(Sales_amount) FROM sales_list join sales using(Id_sales) join employee using(Id_employee) join product using(Id_product) ORDER BY Sales_date DESC";
        private string selectSum { get; set; } = $"select Sum(Prod_price * Sales_amount)  FROM sales_list join sales using(Id_sales) join employee using(Id_employee) join product using(Id_product) ORDER BY Sales_date DESC";

        public Sales(Form _LastForm)
        {
            InitializeComponent();
            this._LastForm = _LastForm;
        }

        private void Sales_Load(object sender, EventArgs e)
        {
            dataGridView1.Font = new Font("Century Gothic", 12, FontStyle.Bold);
            _CreatorCol();
            _RefreshDataGridView(dataGridView1, select);
            _RefreshTextBox(selectSum, selectAmount);
            #region Buttons
            button1.FlatAppearance.BorderSize = 0;
            button1.FlatStyle = FlatStyle.Flat;
            button2.FlatAppearance.BorderSize = 0;
            button2.FlatStyle = FlatStyle.Flat;
            button3.FlatAppearance.BorderSize = 0;
            button3.FlatStyle = FlatStyle.Flat;
            button4.FlatAppearance.BorderSize = 0;
            button4.FlatStyle = FlatStyle.Flat;
            addcheckbox();
            #endregion

        }

        #region Buttons
        private void button1_Click(object sender, EventArgs e)
        {

            employeeNames.Clear();
            panel1.Controls.Clear();
            _DelleteCol();
            this.Hide();
            _LastForm.Show();
        }
        private void button2_Click(object sender, EventArgs e)
        {
            _RefreshDataGridView(dataGridView1, select);

        }
        #endregion

        #region MyMethods

        /// <summary>
        /// Создаем колонки
        /// </summary>
        private void _CreatorCol()
        {

            dataGridView1.Columns.Add("Emp_pib", "ФИО сотрудника");
            dataGridView1.Columns.Add("Product_name", "Название продукта");
            dataGridView1.Columns.Add("Sales_amount", "Кол-во");
            dataGridView1.Columns.Add("total_sales", "Сумма");
            dataGridView1.Columns.Add("Sales_date", "Дата");
        }


        /// <summary>
        /// Удаляем колноки
        /// </summary>
        private void _DelleteCol()
        {

            dataGridView1.Columns.Remove("Sales_date");
            dataGridView1.Columns.Remove("Product_name");
            dataGridView1.Columns.Remove("Sales_amount");
            dataGridView1.Columns.Remove("total_sales");
            dataGridView1.Columns.Remove("Emp_pib");
        }


        /// <summary>
        /// Чтение с бд колонок
        /// </summary>
        private void _ReadRows(DataGridView datagr, IDataRecord record)
        {
            datagr.Rows.Add(record.GetString(0), record.GetString(1), record.GetInt32(2), record.GetString(3), record.GetString(4));
        }

        /// <summary>
        /// Обновление дата грид
        /// </summary>
        private void _RefreshDataGridView(DataGridView datagr, string selectbd)
        {
            datagr.Rows.Clear();
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

        private void _RefreshTextBox(string selectSum, string selectAmount)
        {
            dB.openConnection();
            MySqlCommand cmd = new MySqlCommand(selectSum, dB.GetConnection());
            cmd.ExecuteNonQuery();
            textBox1.Text = cmd.ExecuteScalar().ToString();

            MySqlCommand cmd1 = new MySqlCommand(selectAmount, dB.GetConnection());
            cmd1.ExecuteNonQuery();
            textBox2.Text = cmd1.ExecuteScalar().ToString();
            dB.closeConnection();
        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {

            IsDataSelect();
            _RefreshDataGridView(dataGridView1, select);
            _RefreshTextBox(selectSum, selectAmount);

        }

        private void Sales_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }
        private void addcheckbox()
        {
            GetNameEmp();
            dB.openConnection();
            for (int i = 0; i < employeeNames.Count; i++)
            {
                CheckBox checkBox = new CheckBox();
                checkBox.Font = new Font("Century Gothic", 12, FontStyle.Bold);
                checkBox.ForeColor = Color.White;
                checkBox.Name = "checkBox" + i.ToString();
                checkBox.Text = $"{employeeNames[i]}";
                checkBox.Top = 10 + i * 25;
                checkBox.CheckedChanged += CheckBox_CheckedChanged;
                panel1.Controls.Add(checkBox);
                checkBox.Checked = true;
            }
            flag2 = true;
            _filter = _filter.TrimEnd(',');
            IsDataSelect();
            _RefreshDataGridView(dataGridView1, select);
            _RefreshTextBox(selectSum, selectAmount);
            dB.closeConnection();
        }
        private void CheckBox_CheckedChanged(object sender, EventArgs e)
        {

            CheckBox checkBox = (CheckBox)sender; // получаем ссылку на чекбокс, который вызвал событие
            if (checkBox.Checked) // если чекбокс отмечен
            {
                if (flag2)
                {
                    _filter = "";
                    foreach (Control control in panel1.Controls)
                    {
                        CheckBox checkBox1 = (CheckBox)control;
                        if (control is CheckBox)
                        {
                            if (checkBox1.Checked)
                            {
                                _filter += $"'{checkBox1.Text}' ,";
                                _RefreshTextBox(selectSum, selectAmount);
                            }
                        }

                    }
                    _filter = _filter.TrimEnd(',');
                    IsDataSelect();
                    _RefreshDataGridView(dataGridView1, select);
                    _RefreshTextBox(selectSum, selectAmount);
                }
                else
                {
                    _filter += $"'{checkBox.Text}' ,";
                    _RefreshTextBox(selectSum, selectAmount);
                }
            }
            if (!checkBox.Checked)
            {
                foreach (Control control in panel1.Controls)
                {
                    if (control is CheckBox checkBox1)
                    {
                        if (checkBox1.Checked)
                        {
                            allUnchecked = false;
                            break;
                        }
                    }
                }
                if (allUnchecked)
                {
                    _filter = "' '";
                    IsDataSelect();
                   _RefreshDataGridView(dataGridView1, select);
                    _RefreshTextBox(selectSum, selectAmount);
                }
                else
                {
                    _filter = "";
                    foreach (Control control in panel1.Controls)
                    {
                        CheckBox checkBox1 = (CheckBox)control;
                        if (control is CheckBox)
                        {
                            if (checkBox1.Checked)
                            {
                                _filter += $"'{checkBox1.Text}' ,";
                                _RefreshTextBox(selectSum, selectAmount);
                            }
                        }

                    }
                    _filter = _filter.TrimEnd(',');
                    IsDataSelect();
                    _RefreshDataGridView(dataGridView1, select);
                    _RefreshTextBox(selectSum, selectAmount);
                    allUnchecked = true;
                }
            }

        }
        private void GetNameEmp()
        {
            dB.openConnection();
            MySqlCommand getname = new MySqlCommand("select Emp_pib from employee where State = 1", dB.GetConnection());
            MySqlDataReader reader = getname.ExecuteReader();
            while (reader.Read())
            {
                employeeNames.Add(reader.GetString(0));
            }
            dB.closeConnection();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (flag)
            {
                foreach (Control control in panel1.Controls)
                    if (control is CheckBox)
                    {
                        CheckBox checkBox = (CheckBox)control;
                        checkBox.Checked = false;
                    }
                flag = false;
            }
            else
            {
                foreach (Control control in panel1.Controls)
                    if (control is CheckBox)
                    {
                        CheckBox checkBox = (CheckBox)control;
                        checkBox.Checked = true;
                    }
                flag = true;
            }
            _filter = _filter.TrimEnd(',');
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (flagDate)
                flagDate = false;
            else flagDate = true;
            IsDataSelect();
            _RefreshDataGridView(dataGridView1, select);
            _RefreshTextBox(selectSum, selectAmount);
        }
        private void IsDataSelect()
        {
            if (flagDate)
            {
                DateTime dateTime = dateTimePicker1.Value;
                select = $"select Emp_pib, Product_name, Sales_amount, Prod_price * Sales_amount AS total_sales, Sales_date FROM sales_list join sales using(Id_sales) join employee using(Id_employee) join product using(Id_product) where Emp_pib in ({_filter}) and Date(Sales_date) = '{dateTime.ToString("yyyy-MM-dd")}' ORDER BY Sales_date DESC";
                selectSum = $"select Sum(Prod_price * Sales_amount) AS total_sales FROM sales_list join sales using(Id_sales) join employee using(Id_employee) join product using(Id_product) where Emp_pib in ({_filter}) and Date(Sales_date) = '{dateTime.ToString("yyyy-MM-dd")}' ORDER BY Sales_date DESC";
                selectAmount = $"select Sum(Sales_amount) AS total_sales FROM sales_list join sales using(Id_sales) join employee using(Id_employee) join product using(Id_product) where Emp_pib in ({_filter}) and Date(Sales_date) = '{dateTime.ToString("yyyy-MM-dd")}' ORDER BY Sales_date DESC";
            }
            else
            {
                select = $"select Emp_pib, Product_name, Sales_amount, Prod_price * Sales_amount AS total_sales, Sales_date FROM sales_list join sales using(Id_sales) join employee using(Id_employee) join product using(Id_product) where Emp_pib in ({_filter}) ORDER BY Sales_date DESC";
                selectAmount = $"select Sum(Sales_amount) AS total_sales FROM sales_list join sales using(Id_sales) join employee using(Id_employee) join product using(Id_product) where Emp_pib in ({_filter}) ORDER BY Sales_date DESC";
                selectSum = $"select  Sum(Prod_price * Sales_amount) AS total_sales FROM sales_list join sales using(Id_sales) join employee using(Id_employee) join product using(Id_product) where Emp_pib in ({_filter}) ORDER BY Sales_date DESC";
            }
        }


        #endregion
        //1086; 588
        //string selectbd = $"select Emp_pib, Product_name, Sales_amount, Sales_date FROM sales_list join sales using(Id_sales) join employee using(Id_employee) join product using(Id_product)";
    }
}
