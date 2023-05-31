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
    public partial class acceptance_list : Form
    {
        DB dB = new DB();
        public acceptance_list()
        {
            InitializeComponent();
        }
        private void acceptance_list_Load(object sender, EventArgs e)
        {
            #region Buttons Style
            button1.FlatAppearance.BorderSize = 0;
            button1.FlatStyle = FlatStyle.Flat;
            button2.FlatAppearance.BorderSize = 0;
            button2.FlatStyle = FlatStyle.Flat;
            #endregion
            dataGridView1.Font = new Font("Century Gothic", 12, FontStyle.Bold);
            _CreatorCol();
            _RefreshDataGridView(dataGridView1);
        }

        #region Buttons
        private void button1_Click(object sender, EventArgs e)
        {
            _RefreshDataGridView(dataGridView1);
        }
        private void button2_Click(object sender, EventArgs e)
        {
            _DelleteCol();
            this.Close();
        }

        #endregion

        #region MyMethods

        /// <summary>
        /// Создаем колонки
        /// </summary>
        private void _CreatorCol()
        {
            dataGridView1.Columns.Add("Emp_pib", "Им'я працівника");
            dataGridView1.Columns.Add("Supl_name_firm", "Постачальник");
            dataGridView1.Columns.Add("Product_name", "Продукт");
            dataGridView1.Columns.Add("Amount_product", "Кіл-ть");
            dataGridView1.Columns.Add("Purchase_price", "Заг ціна");
            dataGridView1.Columns.Add("Date", "Дата");
        }


        /// <summary>
        /// Удаляем колноки
        /// </summary>
        private void _DelleteCol()
        {
            dataGridView1.Columns.Remove("Emp_pib");
            dataGridView1.Columns.Remove("Supl_name_firm");
            dataGridView1.Columns.Remove("Product_name");
            dataGridView1.Columns.Remove("Amount_product");
            dataGridView1.Columns.Remove("Purchase_price");
            dataGridView1.Columns.Remove("Date");
        }


        /// <summary>
        /// Чтение с бд колонок
        /// </summary>
        private void _ReadRows(DataGridView datagr, IDataRecord record)
        {
            datagr.Rows.Add(record.GetString(0), record.GetString(1), record.GetString(2), record.GetInt32(3), record.GetDouble(4), record.GetString(5));
        }

        /// <summary>
        /// Обновление дата грид
        /// </summary>
        private void _RefreshDataGridView(DataGridView datagr)
        {
            datagr.Rows.Clear();
            string selectbd = $"select Emp_pib, Supl_name_firm, Product_name, Amount_product,Purchase_price,`acceptance _date`  FROM acceptance_list join invoice using (`Id_ invoice`) join employee using(Id_employee) join product using (Id_product) join supplier using(id_supplier) ORDER BY `acceptance _date` DESC";
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

        #endregion

    }
}
