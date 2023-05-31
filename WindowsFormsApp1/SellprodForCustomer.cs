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
    public partial class SellprodForCustomer : Form
    {
        Form _Formlast;
        Form _Check;
        DB dB = new DB();
        private static int CountTextB { get; set; } = 1;
        private static double sum { get; set; }
        private static double lastsum { get; set; } = 0;
        private static int amount { get; set; }
        private static int id_employee { get; set; }
        
        
        
        private static int countprodfromlist { get; set; } = 0;
        private static string name_prodCB { get; set; }
        private static int id_sales { get; set; }
        public string fromdatagridclick { get; private set; }
        public int RowSelecct { get; private set; }

        List<MyList1> products = new List<MyList1>();
        public SellprodForCustomer(Form _LastForm)
        {
            InitializeComponent();
            _Formlast = _LastForm;
        }

        private void SellprodForCustomer_Load(object sender, EventArgs e)
        {
            #region Стиль кнопок
            
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
            #endregion


            #region Стиль лабела
            //label1.Parent = pictureBox1;
            //label1.BackColor = Color.Transparent;
            //label2.Parent = pictureBox1;
            //label2.BackColor = Color.Transparent;
            //label3.Parent = pictureBox1;
            //label3.BackColor = Color.Transparent;
            //label4.Parent = pictureBox1;
            //label4.BackColor = Color.Transparent;
            #endregion

           
            _SetCombobox();
            dataGridView1.Font = new Font("Century Gothic", 12, FontStyle.Bold);
            dataGridView1.DefaultCellStyle.ForeColor = Color.Black;
            comboBox1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
           
           
           
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Regex regex = new Regex(@"^(?!0{4})\d{4}-(0[1-9]|1[0-2])-([0-2][1-9]|[1-3]0)$");

            
           
            if (textBox11.Text == String.Empty && textBox10.Text == String.Empty)
            {
                MessageBox.Show("Заполните корзину");
                return;
            }
            if (MessageBox.Show("Вы уверенны что хотите продать?", "", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                DB db = new DB();
                db.openConnection();

               

               #region Добавление в бд
                               

               
                sales();
                saleslist();
                SetAmountInBD();
                products.Clear();

                dataGridView1.DataSource = null;                            
               
                textBox10.Text = String.Empty;
                textBox11.Text = String.Empty;
                
                lastsum = 0;
                amount = 0;
                countprodfromlist = 0;
                _DellCombobox();
                _SetCombobox();
              
                MessageBox.Show("Ви успішно закупили товар");

                if (MessageBox.Show("Бажаєте роздрукувати чек?","", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    if (_Check == null || _Check.IsDisposed)
                    {
                        _Check = new CheckForm();
                        _Check.Show();
                    }
                    else _Check.Activate();
                }
                #endregion
                db.closeConnection();
            }
          
        }
        private void sales()
        {
            dB.openConnection();
            MySqlCommand cmd = new MySqlCommand($"SELECT Id_employee From `employee` join register using (Id_employee) where login_user = '{Form1._logi}'", dB.GetConnection());
            cmd.ExecuteNonQuery();
            id_employee = int.Parse(cmd.ExecuteScalar().ToString());

            MySqlCommand cmd3 = new MySqlCommand($"INSERT INTO `sales` (`Id_employee`, `Sales_date`) VALUES ('{id_employee}', curdate());", dB.GetConnection());
            cmd3.ExecuteNonQuery();

            MySqlCommand cmd4 = new MySqlCommand($"select Max(Id_sales) from sales",dB.GetConnection());
            cmd4.ExecuteNonQuery();
            id_sales = int.Parse(cmd4.ExecuteScalar().ToString());
            dB.closeConnection();

        }

        private void saleslist()
        {
            dB.openConnection();
            int id_prod;
            for (int i = 0; i < products.Count; i++)
            {
                MySqlCommand cmd0 = new MySqlCommand($"select `Id_product` from product where `Product_name` = '{products[i].name}'", dB.GetConnection());
                cmd0.ExecuteNonQuery();
                id_prod = int.Parse(cmd0.ExecuteScalar().ToString());

                MySqlCommand cmd = new MySqlCommand($"INSERT INTO `sales_list` (`Id_sales`, `Id_product`, `Sales_amount`) VALUES ('{id_sales}', '{id_prod}', '{products[i].count}');", dB.GetConnection());
                cmd.ExecuteNonQuery();
            }
            dB.closeConnection();
        
        }
        private void SetAmountInBD()
        {
            dB.openConnection();
            for (int i = 0; i < products.Count; i++)
            {
                MySqlCommand command = new MySqlCommand($"UPDATE `product` SET `Prod_amount` = Prod_amount -'{products[i].count}' WHERE (`Product_name` = '{products[i].name}');", dB.GetConnection());
                command.ExecuteNonQuery();
                countprodfromlist = 0;
                amount = 0;
                lastsum = 0;
            }
            dB.closeConnection();
        }
        public void _DellCombobox()
        {
            comboBox1.Items.Clear();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            _DellCombobox();
            this.Hide();
            _Formlast.Show();
        }


        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            name_prodCB = comboBox1.Text;
            dB.openConnection();

            MySqlCommand command1 = new MySqlCommand($"Select `Prod_amount` from `product` where(Product_name = '{name_prodCB}') and State = '1'", dB.GetConnection());
            command1.ExecuteNonQuery();
            textBox9.Text = command1.ExecuteScalar().ToString();

            MySqlCommand command2 = new MySqlCommand($"Select `Prod_price` from `product` where(Product_name = '{name_prodCB}') and State = '1'", dB.GetConnection());
            command2.ExecuteNonQuery();
            textBox8.Text = command2.ExecuteScalar().ToString();
            sum = Convert.ToDouble(textBox8.Text) * Convert.ToDouble(CountTextB);
            textBox5.Text = sum.ToString();

            MySqlCommand command3 = new MySqlCommand($"Select `Prod_country` from `product` where(Product_name = '{name_prodCB}') and State = '1'", dB.GetConnection());
            command3.ExecuteNonQuery();
            textBox6.Text = command3.ExecuteScalar().ToString();

            int n = int.Parse(textBox9.Text);
            if(CountTextB> n)
            {
                if (n <= 10)
                {
                    CountTextB = n;
                    textBox7.Text = CountTextB.ToString();
                    sum = 0;
                    sum = CountTextB * double.Parse(textBox8.Text);
                    textBox5.Text = sum.ToString();
                }
            }
            
            dB.closeConnection();
        }

        public void _SetCombobox()
        {
            dB.openConnection();
            DataTable _table = new DataTable();
            MySqlDataAdapter _adapter = new MySqlDataAdapter();

            #region Combobox1
            MySqlCommand cmd = new MySqlCommand("Select `Product_name` from product where (State = '1' and Prod_amount > '0') ", dB.GetConnection());
            MySqlDataReader my;
            my = cmd.ExecuteReader();
            while (my.Read())
            {
                string sname = my.GetString("Product_name");
                comboBox1.Items.Add(sname);
            }
            dB.closeConnection();
            #endregion

            dB.closeConnection();
            comboBox1.SelectedIndex = 0;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            AddProd(new MyList1(name_prodCB, Convert.ToInt32(textBox7.Text), Convert.ToDouble(textBox5.Text)));
            dataGridView1.DataSource = null;
            dataGridView1.DataSource = products;
            CountCount();
            textBox11.Text = amount.ToString();
            textBox10.Text = lastsum.ToString();
        }
        /// <summary>
        /// Добавление продуктов в список
        /// </summary>
        /// <param name="myList"></param>
        private void AddProd(MyList1 myList)
        {
            int _coun_mismatch = 0;

            if (products.Count == 0)
            {
                products.Add(myList);

            }
            else
            {
                for (int i = 0; i < products.Count; i++)
                {
                    if (products[i].name == myList.name)
                    {
                        if (MessageBox.Show("Товар уже есть в корзине, желаете перезаписать?", "", MessageBoxButtons.YesNo) == DialogResult.Yes)
                        {
                            products[i].count = myList.count;
                            products[i].price = myList.price;
                            countprodfromlist = 0;
                            amount = 0;
                            lastsum = 0;
                            CountCount();
                        }
                    }
                    else
                    {
                        _coun_mismatch++;
                        if (_coun_mismatch == products.Count)
                        {
                            products.Add(myList);

                            return;
                        }
                    }

                }
            }

        }
       

        private void CountCount()
        {
            for (int i = countprodfromlist; i < products.Count; i++)
            {
                amount += products[i].count;
                lastsum += products[i].price;
                countprodfromlist++;
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            int n;
            //if (CountTextB != 10)
            //{
                n = int.Parse(textBox9.Text);
                if (CountTextB < n)
                {
                    CountTextB += 1;
                    sum += double.Parse(textBox8.Text);
                    textBox5.Text = sum.ToString();
                    textBox7.Text = CountTextB.ToString();
                }
            //}
        }

        private void button7_Click(object sender, EventArgs e)
        {
            if (CountTextB != 1)
            {
                CountTextB -= 1;
                sum -= double.Parse(textBox8.Text);
                textBox5.Text = sum.ToString();
                textBox7.Text = CountTextB.ToString();
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            DellProd(fromdatagridclick);
            dataGridView1.DataSource = null;
            dataGridView1.DataSource = products;
            
            if (amount == 0)
            {
                textBox11.Text = String.Empty;
                textBox10.Text = String.Empty;
            }
            else
            {
                textBox11.Text = amount.ToString();
                textBox10.Text = lastsum.ToString();
            }
        }

        /// <summary>
        /// Удаление продукта из списка
        /// </summary>
        /// <param name="name"></param>
        public void DellProd(string name)
        {
            for (int i = 0; i < products.Count; i++)
            {
                if (products[i].name == name)
                {
                    products.Remove(products[i]);
                    countprodfromlist = 0;
                    amount = 0;
                    lastsum = 0;
                    CountCount();
                }
            }
        }
        private void SellprodForCustomer_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
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

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
    class MyList1
    {
        public string name { get; set; }
        public int count { get; set; }
        public double price { get; set; }
        public MyList1(string name, int count, double price)
        {
            this.name = name;
            this.count = count;
            this.price = price;
        }
    }
}
